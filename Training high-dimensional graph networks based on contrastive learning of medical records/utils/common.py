import json
import os

import numpy as np
from gensim.models import Word2Vec

REUSE_NODE_TYPES = ['ResultSon', 'ResultFather', 'YD', 'YBQJ', 'Zhusu', 'HistoryEye', 'HistoryBody', 'Sex', 'Age']
model = Word2Vec.load("/data/gaowh/data/node2vec.model")


def get_node_types(file_dir='//data/gaowh/data/'):
    node_types = {}
    for file in ['train.json', 'eval.json']:
        with open(file_dir + file, 'r', encoding='utf-8') as f:
            for line in f.readlines():
                data = json.loads(line.strip())
                for triplet in data['links']:
                    for i in [triplet[:2], triplet[-2:]]:
                        node_types[i[1] + '_' + str(i[0]) if i[1] in REUSE_NODE_TYPES else i[1]] = 1
    return list(node_types.keys())


NODE_TYPES = get_node_types()


def convert_graph2features(g):
    features = [0] * len(NODE_TYPES)
    for triplet in g['links']:
        for i in [triplet[:2], triplet[-2:]]:
            n_type = i[1] + '_' + str(i[0]) if i[1] in REUSE_NODE_TYPES else i[1]
            features[NODE_TYPES.index(n_type)] = 1
    return features


def convert_graph2sum_vec(sample):
    seen_ids = []

    vec = np.zeros(768)
    for triplet in sample['links']:
        for i in [triplet[:2], triplet[-2:]]:
            if i[0] in seen_ids:
                continue
            seen_ids.append(i[0])
            vec += model.wv[i[1] + '_' + str(i[0]) if i[1] in REUSE_NODE_TYPES else i[1]]

    return vec


def pop_labels(sample):
    son_labels, father_labels, drop_nodes = [], [], []
    for triplet in sample['links']:
        if triplet[3] in ['诊断结果', '诊断属于']:
            for node in [triplet[:2], triplet[-2:]]:
                if node[1] == 'ResultSon':
                    son_labels.append(node[0])
                if node[1] == 'ResultFather':
                    father_labels.append(node[0])
        # for node in [triplet[:2], triplet[-2:]]:
        #     if node[1] in ["Amd0", "Amd1", "Bj0", "Bj1", "Dr0", "Dr1", "Dr2", "Dr3", "Dr4"]:
        #         drop_nodes.append(node[0])

    son_labels, father_labels = list(set(son_labels)), list(set(father_labels))
    drop_nodes = drop_nodes + father_labels + son_labels
    for drop_node in drop_nodes:
        if str(drop_node) in sample['nodes']:
            sample['nodes'].pop(str(drop_node))
    sample['links'] = [triplet for triplet in sample['links'] if
                       triplet[0] not in drop_nodes and triplet[-2] not in drop_nodes]
    return sample, son_labels, father_labels


if __name__ == '__main__':
    for file in os.listdir('//data/gaowh/data/'):
        with open('//data/gaowh/data/' + file, 'r', encoding='utf-8') as f:
            for line in f.readlines():
                data = json.loads(line.strip())
                for exp_key in ['example_a', 'example_b']:
                    convert_graph2features(data[exp_key])
