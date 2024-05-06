#这个文件容老师改了一下去了归一化，虽然pca姜维看起来更好，但是没有办法跑evaluation了。





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
from gat_model_pure import BertGAT_org

# from gat_model import BertGAT

from gcn_model import BertGCN
from gensim.models import Word2Vec
import json
from transformers import AutoTokenizer
from utils.neo_search import NeoSearch
from utils.dataset_graph import PatientGraphDataset
import time
import os
from utils.post_process import *

os.environ["CUDA_VISIBLE_DEVICES"] = "0"
app = Sanic(__name__)
parser = ArgumentParser()
parser.add_argument("--node_emb_path", type=str, default="/data/gaowh/data/node2vec.model",
                    help="node pretrain emb path.")
parser.add_argument("--sim_model_path", type=str, default="/data/gaowh/data/ckpt_gat/g_sim_model_epoch29.pt",
                    help="sim model path.")
parser.add_argument("--device", type=str, default='cuda',
                    help="device.")
parser.add_argument("--bert_type", type=str, default="hfl/chinese-macbert-base",
                    help="pretrain language model.")
parser.add_argument("--model_type", type=str, default="gat",
                    help="model type.")
parser.add_argument("--lr", type=float, default=1e-2,
                        help="learning rate.")
parser.add_argument("--max_norm", type=float, default=0.3,
                        help="clip norm.")
parser.add_argument("--dropout", type=float, default=0.1,
                        help="dropout rate.")
parser.add_argument("--num_feature", type=int, default=11,
                        help="number of feature.")
args = parser.parse_args()


def get_label(sample):
    son_labels, other_labels = [], []
    for triplet in sample['links']:
        if triplet[3] in ['诊断结果', '诊断属于']:
                if triplet[-1] == 'ResultFather':
                    son_labels.append(sample['nodes'][str(triplet[-2])]['name'])
                if triplet[-1] == 'EyeDiagnoseNode':
                    if sample['nodes'][str(triplet[-2])].get('DisplayName', '') == '其它':
                        other_labels.append('其他')
    # #、、、、
    # son_labels = [i  if i not in ["视网膜脱离", "葡萄膜黑色素瘤", "屈光不正", "中心性浆液性脉络膜视网膜病", "视神经萎缩"]  else '其他'  for i in son_labels]
    print(son_labels)
    a = list(set(son_labels+other_labels))
    if len(a) == 0:
        return '无结果'
    elif len(a) == 1:
        return a[0]
    return '多种眼疾'



class MilvusModel:
    def __init__(self, init_mode=True):
        """
        初始化各模块
        """
        self.nlist = 128
        self.nprobe = 10
        self.id2patient_graph = {}
        self.id2patient_id = {}

        # 加载模型
        node_type_table = Word2Vec.load(args.node_emb_path).wv
        embed_matrix = node_type_table.vectors
        if args.model_type == 'gat':
            self.sim_model = BertGAT_org(args, embed_matrix)
        else:
            self.sim_model = BertGCN(args, embed_matrix)
        self.sim_model.load_state_dict(torch.load(args.sim_model_path))
        self.sim_model.to(args.device).eval()

        # 加载数据工具
        tokenizer = AutoTokenizer.from_pretrained(args.bert_type)
        self.kgd = PatientGraphDataset(tokenizer, None, node_type_table, is_inference=True)

        # 检索工具
        self.neo_search = NeoSearch()

        # 链接服务器
        connections.connect("default", host="192.168.2.240", port="19530")
        if utility.has_collection("patient_graphs"):
            utility.drop_collection("patient_graphs")

        # if init_mode:
        #     utility.drop_collection("patient_graphs")
        # 创建集合
        fields = [
            FieldSchema(name="pk", dtype=DataType.INT64, is_primary=True, auto_id=False),
            FieldSchema(name="embeddings", dtype=DataType.FLOAT_VECTOR, dim=768)
        ]
        schema = CollectionSchema(fields, "patient graph dense retrieval.")
        self.patient_graphs = Collection("patient_graphs", schema)
        print('当前病例图数量：', self.patient_graphs.num_entities)
        # 载入数据
        if init_mode:
            self.build_patient_graphs()
            print('当前病例图数量：', self.patient_graphs.num_entities)

    def encode_graph(self, patient_graph):
        """
        转化病例图的格式，并编码成向量
        """
        token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, subgraph, adj, _ = self.kgd.convert_patient_graph2tensor(
            patient_graph)
        graph_state = self.sim_model(token_ids.to(args.device), mask_ids.to(args.device),
                                     token_type_ids.to(args.device), node_type_ids.to(args.device),
                                     node_vectors.to(args.device), subgraph.to(args.device),
                                     adj.to(args.device), _.to(args.device), is_inference=True).detach().cpu().numpy()
        # graph_state = torch.nn.functional.normalize(graph_state, p=2, dim=1).detach().cpu().numpy()
        return graph_state

    def build_patient_graphs(self):
        """
        将现有病例图转为向量，存入检索库
        """
        start_time = time.time()
        
        fp = open('/data/gaowh/data/patient_2d.json', 'w', encoding='utf-8')
        d_map = {}

        
        
        with open('/data/gaowh/data/clean_kg.json', 'r', encoding='utf-8') as f:
            idx = 0
            entities = [[], []]
            for line in f.readlines():


                patient_graph = json.loads(line.strip())

                self.id2patient_id[idx] = patient_graph['links'][0][0]


                self.id2patient_graph[idx] = (patient_graph['links'][0][0], patient_graph)
                entities[0].append(idx)
                vector = self.encode_graph(patient_graph)[0, :]
                entities[1].append(vector)


                label = get_label(patient_graph)
                if label in ['无结果',' 多种眼疾',  '其他']:
                    continue
                if label not in d_map:
                    d_map[label] = []
                # entities[0].append(idx)
                # vector = self.encode_graph(patient_graph)[0, :]
                # entities[1].append(vector)
                # d_map[label].append([float(vector[0]), float(vector[1]), {'id': self.id2patient_id[idx], 'name': get_info(patient_graph)}])



                tmp = [float(i) for i in vector] 
                tmp.append({'id': self.id2patient_id[idx], 'name': get_info(patient_graph)})
                d_map[label].append(tmp)

        #         if idx % 5000 == 0 and idx != 0:
        #             self.insert(entities)
        #             entities = [[], []]
        #             print(idx)
        #         idx += 1
        # if idx % 5000 != 0:
        #     self.insert(entities)
        print('病例图向量化耗时:', time.time() - start_time)
        # 建立索引
        print(1)
        # self.index(length=idx)


        ##todo 在这里对label名下的向量做

        fp.write(json.dumps(d_map, ensure_ascii=False))







# def build_patient_graphs(self):
#     """
#     将现有病例图转为向量，存入检索库
#     """
#     start_time = time.time()
    
#     d_map = {}
#     entities = [[], []]

#     with open('/data/gaowh/data/clean_kg.json', 'r', encoding='utf-8') as f:
#         idx = 0
#         for line in f.readlines():
#             patient_graph = json.loads(line.strip())

#             self.id2patient_id[idx] = patient_graph['links'][0][0]

#             self.id2patient_graph[idx] = (patient_graph['links'][0][0], patient_graph)
#             entities[0].append(idx)
#             vector = self.encode_graph(patient_graph)[0, :]
#             entities[1].append(vector)

#             label = get_label(patient_graph)
#             if label in ['无结果', '多种眼疾', '其他']:
#                 continue
#             if label not in d_map:
#                 d_map[label] = []
#             tmp = [float(i) for i in vector]
#             tmp.append({'id': self.id2patient_id[idx], 'name': get_info(patient_graph)})
#             d_map[label].append(tmp)

#             idx += 1

#     # Normalize the vectors
#     entities[1] = normalize(np.array(entities[1]))

#     print('病例图向量化耗时:', time.time() - start_time)

#     # 建立索引
#     print(1)
#     # self.index(length=idx)

#     # Write the results to a file
#     with open('/data/gaowh/data/patient_2d.json', 'w', encoding='utf-8') as fp:
#         fp.write(json.dumps(d_map, ensure_ascii=False))

























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
        print(2)
        # 为节点创建索引
        index = {
            "index_type": "IVF_FLAT",
            "metric_type": "IP",
            "params": {"nlist": self.nlist},
        }
        self.patient_graphs.create_index("embeddings", index)
        print(3)
        self.patient_graphs.load()
        print(4)

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
        search_params = {"metric_type": "IP",
                         "params": {"nprobe": self.nprobe}}

        case_id = [i[0] for i in graph['links'] if i[1] == 'CaseRecord']
        gid = -1
        if case_id:
            gid = case_id[0]
        # 执行向量查询：
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
    port = 12088
    milvus_model = MilvusModel()

    @app.route("/sim_patient_retrieval", methods=['POST'])
    async def sim_search(request):
        start_time = time.time()
        try:
            data = request.json
            result = milvus_model.search(data)
        except:
            result = {}
        print('用时:', time.time() - start_time)
        return response.json(result, dumps=json.dumps, default=str)

    @app.route("/raw_sim_patient_retrieval", methods=['POST'])
    async def sim_search(request):
        start_time = time.time()
        try:
            data = request.json
            result = milvus_model.raw_search(data)
        except:
            result = {}
        print('用时:', time.time() - start_time)
        return response.json(result, dumps=json.dumps, default=str)

    app.run(host='0.0.0.0', port=port, debug=False, workers=1)


if __name__ == '__main__':
    server()
