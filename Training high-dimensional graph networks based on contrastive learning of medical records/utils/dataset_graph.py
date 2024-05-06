# -*- coding: utf-8 -*-

import random
import torch
import json
from torch.utils.data import Dataset
import numpy as np
from torch.nn.utils.rnn import pad_sequence

SPECIAL_TOKEN = ["[CLS]"]
# 有序特征字段列表
FEATURE_ATTRS = ['XianranZJDS', 'Jiaozheng', 'Luoyan', 'XianranDS', 'XianranZX', 'XianranSL', 'HopAge', 'Vector']
# 有序文本字段列表
TEXT_ATTRS = ['Lr', 'DisplayName', 'name', 'Name', 'Rmk', 'RmkQt', 'Info']
# 区分独立、公共节点类型，形成id映射；
REUSE_NODE_TYPES = ['ResultSon', 'ResultFather', 'YD', 'YBQJ', 'Zhusu', 'HistoryEye', 'HistoryBody', 'Sex', 'Age']

#文本信息
def text_convert_func(node):
    """
    将节点的文本信息，按先后续拼接
    :return:
    """
    return [i for i in '，'.join([node.get(t, '') for t in TEXT_ATTRS if len(node.get(t, '')) > 0])]

#数值信息
def feature_convert_func(node, sp_value=-500.):
    """
    将特征转化
    :return:
    """
    feature = []
    for i in FEATURE_ATTRS[:-1]:
        try:
            feature.append(float(node.get(i)))
        except:
            feature.append(sp_value)
    try:
        vec = [float(j) for j in node.get(FEATURE_ATTRS[-1], '-500,-500,-500,-500').split(',')]
        assert len(vec) == 4
        feature += vec
    except:
        feature += [sp_value] * 4
    feature = [i if -499. < i < 500 else sp_value for i in feature]
    assert len(feature) == 11
    feature = [i / 500. for i in feature]
    try:
        feature.append(float(node.get('Percentage')))
    except:
        feature.append(1.)
    return feature


class PatientGraphDataset(Dataset):
    def __init__(self, tokenizer, data_path, node_type_table, is_inference=False):
        """
        病例图数据集
        """
        self.tokenizer = tokenizer
        self.data_path = data_path
        self.node_type_table = node_type_table
        self.num_type = len(node_type_table)
        self.node_id2type_id = {}
        self.node_id2node_info = {}
        self.data_len = 0
        self.max_len = 48
        self.samples = []
        if not is_inference:
            self.load_data()

    def load_data(self):
        """
        读入数据
        :return:
        """
        with open(self.data_path, 'r', encoding='utf-8') as f:
            for line in f.readlines():
                max_lens = []
                sample = json.loads(line.strip())
                real_sample = {'score': sample['sim_score'], 'example_a': [], 'example_b': []}
                # 读取，并形成每个node的id
                for g in ['example_a', 'example_b']:
                    for triplet in sample[g]['links']:
                        real_sample[g].append([triplet[0], triplet[-2]])
                        for node in [triplet[:2], triplet[-2:]]:
                            n, n_t = node[0], node[1]
                            # 2.形成id到类型的映射
                            if n_t in REUSE_NODE_TYPES:
                                self.node_id2type_id[n] = self.node_type_table.get_index(n_t + '_' + str(n),
                                                                                         self.num_type)
                            else:
                                self.node_id2type_id[n] = self.node_type_table.get_index(n_t, self.num_type)
                    # 将节点读入
                    for node_id, node_info in sample[g]['nodes'].items():
                        node_id = int(node_id)
                        if node_id not in self.node_id2type_id or node_id in self.node_id2node_info:
                            continue
                        type_id = self.node_id2type_id[node_id]


                        #文本拼接
                        token_ids = self.tokenizer.convert_tokens_to_ids(SPECIAL_TOKEN + text_convert_func(node_info))
                        max_lens.append(len(token_ids))

                        #数值信息
                        features = feature_convert_func(node_info)
                        self.node_id2node_info[node_id] = [type_id, token_ids, features]

                self.samples.append(real_sample)
        random.shuffle(self.samples)
        self.data_len = len(self.samples)
        print('node num: ', len(self.node_id2node_info))

    def __len__(self):
        return self.data_len

    def __getitem__(self, index):
        return self.samples[index]









    def get_text_tensors(self, node_text_token_ids, pad_value=0):
        """
        填充文本部分
        """
        node_text_token_ids = [i[:self.max_len] for i in node_text_token_ids]
        mask_ids = [[1] * len(i) for i in node_text_token_ids]
        max_len = max([len(i) for i in node_text_token_ids])
        for n_id, text_token_ids in enumerate(node_text_token_ids):
            while len(text_token_ids) < max_len:
                text_token_ids.append(pad_value)
                mask_ids[n_id].append(0)
        token_type_ids = [[0] * len(i) for i in node_text_token_ids]
        token_ids = torch.tensor(node_text_token_ids, dtype=torch.long)
        mask_ids = torch.tensor(mask_ids, dtype=torch.long)
        token_type_ids = torch.tensor(token_type_ids, dtype=torch.long)
        return token_ids, mask_ids, token_type_ids








        

    def collate(self, batch):
        """
        整理数据
        """
        node_id2idx = {}
        # n X ([type id, token id seq], vec)
        node_type_ids = []
        node_text_token_ids = []
        node_vectors = []
        # batch X 2 , time
        all_subgraph = []
        # batch X 2 , time , time
        all_adj = []
        # batch
        scores = []
        for sample in batch:
            scores.append(sample['score'])
            for g in ['example_a', 'example_b']:
                subgraph = []
                adj = []
                for n_pair in sample[g]:
                    for n in n_pair:
                        if n not in node_id2idx:
                            node_id2idx[n] = len(node_id2idx)
                            node_type_ids.append(self.node_id2node_info[n][0])
                            node_text_token_ids.append(
                                [i for i in self.node_id2node_info[n][1] if random.random() > 0.25])
                            node_vectors.append(self.node_id2node_info[n][2])
                        if node_id2idx[n] not in subgraph:
                            subgraph.append(node_id2idx[n])
                    assert len(n_pair) == 2
                    adj.append([subgraph.index(node_id2idx[n_pair[0]]), subgraph.index(node_id2idx[n_pair[1]])])
                all_subgraph.append(subgraph)
                all_adj.append(adj)

        # 节点信息
        token_ids, mask_ids, token_type_ids = self.get_text_tensors(node_text_token_ids)
        node_type_ids = torch.tensor(node_type_ids, dtype=torch.long)
        node_vectors = torch.tensor(node_vectors, dtype=torch.float)

        # 子图信息; NOTE 第一个位置是病人id
        max_subg_len = max([len(i) for i in all_subgraph])
        pad_node_id = len(node_id2idx)
        for subgraph in all_subgraph:
            while len(subgraph) < max_subg_len:
                subgraph.append(pad_node_id)
        all_subgraph = torch.tensor(all_subgraph, dtype=torch.long)

        # 邻接矩阵
        all_adj_matrix = []
        for adj in all_adj:
            adj_matrix = np.identity(max_subg_len)
            # TODO 考虑现在先转权重？；后期如果用GAT，则不需要转
            for edge in adj:
                adj_matrix[edge[0], edge[1]] = 1.
                adj_matrix[edge[1], edge[0]] = 1.
            all_adj_matrix.append(adj_matrix)
        all_adj_matrix = np.array(all_adj_matrix)
        all_adj_matrix = torch.tensor(all_adj_matrix, dtype=torch.float)

        # 分值
        scores = torch.tensor(scores, dtype=torch.float)

        return token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix, scores











    def convert_patient_graph2tensor(self, sample):
        node_id2type_id = {}
        real_sample = []
        for triplet in sample['links']:
            real_sample.append([triplet[0], triplet[-2]])
            for node in [triplet[:2], triplet[-2:]]:
                n, n_t = node[0], node[1]
                if n_t in REUSE_NODE_TYPES:
                    node_id2type_id[n] = self.node_type_table.get_index(n_t + '_' + str(n), self.num_type)
                else:
                    node_id2type_id[n] = self.node_type_table.get_index(n_t, self.num_type)
        # 将节点读入
        node_id2node_info = {}
        for node_id, node_info in sample['nodes'].items():
            node_id = int(node_id)
            type_id = node_id2type_id.get(node_id, self.num_type)
            token_ids = self.tokenizer.convert_tokens_to_ids(SPECIAL_TOKEN + text_convert_func(node_info))
            features = feature_convert_func(node_info)
            node_id2node_info[node_id] = [type_id, token_ids, features]
        #
        node_id2idx = {}
        # n X ([type id, token id seq], vec)
        node_type_ids = []
        node_text_token_ids = []
        node_vectors = []
        # batch
        scores = [0.]

        # subgraph
        subgraph = []
        adj = []
        for n_pair in real_sample:
            for n in n_pair:
                if n not in node_id2idx:
                    node_id2idx[n] = len(node_id2idx)
                    node_type_ids.append(node_id2node_info[n][0])
                    node_text_token_ids.append(node_id2node_info[n][1])
                    node_vectors.append(node_id2node_info[n][2])
                if node_id2idx[n] not in subgraph:
                    subgraph.append(node_id2idx[n])
            assert len(n_pair) == 2
            adj.append([subgraph.index(node_id2idx[n_pair[0]]), subgraph.index(node_id2idx[n_pair[1]])])
        all_subgraph = [subgraph]
        all_adj = [adj]

        # 节点信息
        token_ids, mask_ids, token_type_ids = self.get_text_tensors(node_text_token_ids)
        node_type_ids = torch.tensor(node_type_ids, dtype=torch.long)
        node_vectors = torch.tensor(node_vectors, dtype=torch.float)

        # 子图信息; NOTE 第一个位置是病人id
        max_subg_len = max([len(i) for i in all_subgraph])
        pad_node_id = len(node_id2idx)
        for subgraph in all_subgraph:
            while len(subgraph) < max_subg_len:
                subgraph.append(pad_node_id)
        all_subgraph = torch.tensor(all_subgraph, dtype=torch.long)

        # 邻接矩阵
        all_adj_matrix = []
        for adj in all_adj:
            adj_matrix = np.identity(max_subg_len)
            for edge in adj:
                adj_matrix[edge[0], edge[1]] = 1.
                adj_matrix[edge[1], edge[0]] = 1.
            all_adj_matrix.append(adj_matrix)
        all_adj_matrix = np.array(all_adj_matrix)
        all_adj_matrix = torch.tensor(all_adj_matrix, dtype=torch.float)

        scores = torch.tensor(scores, dtype=torch.float)
        return token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix, scores
