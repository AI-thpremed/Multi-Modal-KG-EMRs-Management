from sanic import Sanic
from sanic import response
from pymilvus import (
    connections,
    FieldSchema,
    CollectionSchema,
    utility,
    DataType,
    Collection,
)
from argparse import ArgumentParser
import torch
from gat_base_model import BertGAT
from gcn_base_model import BertGCN
from gensim.models import Word2Vec
from transformers import AutoTokenizer
from utils.dataset_graph import PatientGraphDataset
import time
import os
from utils.post_process import *

os.environ["CUDA_VISIBLE_DEVICES"] = "0"
app = Sanic(__name__)
parser = ArgumentParser()
parser.add_argument("--node_emb_path", type=str, default="data/node2vec.model",
                    help="node pretrain emb path.")

parser.add_argument("--sim_model_path", type=str, default="",
                    help="sim model path.")
parser.add_argument("--device", type=str, default='cuda',
                    help="device.")

parser.add_argument("--port", type=int, default=12089,
                    help="port.")
parser.add_argument("--bert_type", type=str, default="hfl/chinese-macbert-base",
                    help="pretrain language model.")
parser.add_argument("--model_type", type=str, default="gcn",
                    help="model type.")
parser.add_argument("--dropout", type=float, default=0.1,
                    help="dropout rate.")
args = parser.parse_args()


class MilvusModel:
    def __init__(self, init_mode=True):
        """
        初始化各模块
        """
        self.nlist = 128
        self.nprobe = 10
        self.id2patient_graph = {}

        # 加载模型
        node_type_table = Word2Vec.load(args.node_emb_path).wv
        embed_matrix = node_type_table.vectors
        if len(args.sim_model_path) == 0:
            if args.model_type == 'gat':
                self.sim_model = BertGAT(args, embed_matrix)
                args.sim_model_path = './data/ckpt/g_sim_gat_model_epoch11.pt'
            else:
                self.sim_model = BertGCN(args, embed_matrix)
                args.sim_model_path = './data/ckpt/g_sim_gcn_model_epoch25.pt'
        self.sim_model.load_state_dict(torch.load(args.sim_model_path))
        self.sim_model.to(args.device).eval()

        # 加载数据工具
        tokenizer = AutoTokenizer.from_pretrained(args.bert_type)
        self.kgd = PatientGraphDataset(tokenizer, None, node_type_table, is_inference=True)

        # 链接服务器
        connections.connect("default", host="localhost", port="19530")
        self.pkg = "patient_graphs_" + args.model_type
        if utility.has_collection(self.pkg):
            utility.drop_collection(self.pkg)
        # 创建集合
        fields = [
            FieldSchema(name="pk", dtype=DataType.INT64, is_primary=True, auto_id=False),
            FieldSchema(name="embeddings", dtype=DataType.FLOAT_VECTOR, dim=768)
        ]
        schema = CollectionSchema(fields, "patient graph dense retrieval.")
        self.patient_graphs = Collection(self.pkg, schema)
        print('当前病例图数量：', self.patient_graphs.num_entities)
        # 载入数据
        if init_mode:
            self.build_patient_graphs()
            print('当前病例图数量：', self.patient_graphs.num_entities)

    def encode_graph(self, patient_graph):
        """
        转化病例图的格式，并编码成向量
        """

        #把对象graph创造出来他的各种邻接矩阵之类的信息
        token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, subgraph, adj, _ = self.kgd.convert_patient_graph2tensor(
            patient_graph)


        #这些信息是input给到已经训练好的gat 或者 gcn模型里面
        graph_state = self.sim_model(token_ids.to(args.device), mask_ids.to(args.device),
                                     token_type_ids.to(args.device), node_type_ids.to(args.device),
                                     node_vectors.to(args.device), subgraph.to(args.device),
                                     adj.to(args.device), _.to(args.device), is_inference=True)


        #训练过程是通过两个example创建前面的值，然后倒数第二个是相似度。

        #调用的过程第二个直接传空这样可以吗。。。。。lol
        
        #姜维二维向量
        graph_state = torch.nn.functional.normalize(graph_state, p=2, dim=1).detach().cpu().numpy()
        return graph_state

    def build_patient_graphs(self):
        """
        将现有病例图转为向量，存入检索库
        """
        start_time = time.time()
        with open('data/clean_kg.json', 'r', encoding='utf-8') as f:
            idx = 0
            entities = [[], []]
            for line in f.readlines():
                patient_graph = json.loads(line.strip())
                self.id2patient_graph[idx] = (patient_graph['links'][0][0], patient_graph)
                entities[0].append(idx)
                vector = self.encode_graph(patient_graph)[0, :]
                entities[1].append(vector)
                if idx % 5000 == 0 and idx != 0:
                    self.insert(entities)
                    entities = [[], []]
                    print(idx)
                idx += 1
        if idx % 5000 != 0:
            self.insert(entities)
        print('病例图向量化耗时:', time.time() - start_time)
        # 建立索引
        self.index(length=idx)

    def insert(self, entities):
        """
        插入向量到集合中
        """
        insert_result = self.patient_graphs.insert(entities)
        print('insert_result')
        print(insert_result)

    def index(self, length):
        """
        创建索引
        """
        self.nlist = int(4 * (length ** 0.5))
        self.nprobe = int(4 / 32 * (length ** 0.5))
        # 为节点创建索引
        index = {
            "index_type": "IVF_FLAT",
            "metric_type": "IP",
            "params": {"nlist": self.nlist},
        }
        self.patient_graphs.create_index("embeddings", index)
        self.patient_graphs.load()

    def search(self, graph, search_num=15):
        """
        搜索的主函数
        """
        vectors = self.encode_graph(graph)
        search_params = {"metric_type": "IP",
                         "params": {"nprobe": self.nprobe}}

        case_id = [i[0] for i in graph['links'] if i[1] == 'CaseRecord']
        gid = -1
        if case_id:
            gid = case_id[0]
        # 执行向量查询：
        result = self.patient_graphs.search(vectors, "embeddings", search_params, limit=search_num + 1,
                                            output_fields=["pk"])

        sim_patients = [(j.distance, self.id2patient_graph[j.id]) for i in result for j in i]
        sim_patients = [[i[0], i[1][1]] for i in sim_patients if i[1][0] != gid][:search_num]
        return self.post_process(sim_patients)

    def raw_search(self, graph, search_num=15):
        """
        搜索的主函数
        """
        vectors = self.encode_graph(graph)

        #上面的向量化就是调用模型得到姜维空间坐标


        search_params = {"metric_type": "IP",
                         "params": {"nprobe": self.nprobe}}

        case_id = [i[0] for i in graph['links'] if i[1] == 'CaseRecord']
        gid = -1
        if case_id:
            gid = case_id[0]
        # 执行向量查询：



        #注意这里直接调用的向量搜索引擎自己的方法，在init里面已经把所有创建好的数据insert进去了。这个rawsearch相当于自己搞得一个接口
        

        result = self.patient_graphs.search(vectors, "embeddings", search_params, limit=search_num + 1,
                                            output_fields=["pk"])

        sim_patients = [self.id2patient_graph[j.id] for i in result for j in i]
        return sim_patients

    @staticmethod
    def post_process(sim_patients):
        """
        对结果进行后处理
        """
        unusual_dict, diagnose_dict = {}, {}
        p_table = []
        for s_p in sim_patients:
            p = s_p[1]
            p_info = {'base_info': get_info(p), 'treatment': get_treatment(p), 'similarity': str(s_p[0] * 100 - 5.)[:4]}
            unusual_items, unusual_str = get_unusual_items(p)
            for i in unusual_items:
                unusual_dict[i] = unusual_dict.get(i, 0) + 1

            p_info['Zhusu'] = get_single_items(p, 'Zhusu')
            p_info['YBQJ'] = get_single_items(p, 'YBQJ')
            p_info['YD'] = get_single_items(p, 'YD')
            diagnoses, diagnose_str = get_diagnoses(p)
            p_info['diagnose'] = diagnose_str
            for i in diagnoses:
                diagnose_dict[i] = diagnose_dict.get(i, 0) + 1
            p_table.append(p_info)
        unusual_dict = [{'name': k, 'value': v} for k, v in unusual_dict.items()]
        diagnose_dict = [{'name': k, 'value': v} for k, v in diagnose_dict.items()]
        return {"相似病例信息表": p_table, "主诉/异常仪表盘": unusual_dict, "诊断疾病仪表盘": diagnose_dict}


def server():
    port = args.port
    milvus_model = MilvusModel()

    @app.route("/raw_sim_patient_retrieval_" + args.model_type, methods=['POST'])
    async def sim_search(request):
        start_time = time.time()
        # try:
        if 1:
            data = request.json
            result = milvus_model.raw_search(data)
        # except:
        #     result = {}
        print('用时:', time.time() - start_time)
        return response.json(result, dumps=json.dumps, default=str)

    app.run(host='0.0.0.0', port=port, debug=False, workers=1)


if __name__ == '__main__':
    server()
