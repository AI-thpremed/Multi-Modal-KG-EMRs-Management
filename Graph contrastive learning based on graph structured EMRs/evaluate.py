import json
import requests
import random
from collections import Counter
import numpy as np
from sklearn.metrics import precision_score


def pop_labels(sample):
    """
    获取疾病标签，并删除
    :param sample:
    :return:
    """
    son_labels, father_labels = [], []
    for triplet in sample['links']:
        if triplet[3] in ['诊断结果', '诊断属于']:
            for node in [triplet[:2], triplet[-2:]]:
                if node[1] == 'ResultSon':
                    son_labels.append(node[0])
                if node[1] == 'ResultFather':
                    father_labels.append(node[0])
    son_labels, father_labels = list(set(son_labels)), list(set(father_labels))
    drop_nodes = [i for i in father_labels + son_labels if random.random() > 0.3]
    for drop_node in drop_nodes:
        if str(drop_node) in sample['nodes']:
            sample['nodes'].pop(str(drop_node))
    sample['links'] = [triplet for triplet in sample['links'] if
                       triplet[0] not in drop_nodes and triplet[-2] not in drop_nodes]
    return sample, son_labels, father_labels


def get_f1(pre, ref):
    """
    计算关联性
    """
    if len(pre) == 0 and len(ref) == 0:
        return 1.

    if len(pre) == 0 or len(ref) == 0:
        return 0.
    common = Counter(pre) & Counter(ref)
    overlap = sum(common.values())
    recall, precision = overlap / len(ref), overlap / len(pre)
    return (2 * recall * precision) / (recall + precision + 1e-12)


def main(k=5):
    actual=[]
    predicted=[]
    allrightcount=0
    print("运行召回："+ str(k))
    file.write("运行召回："+ str(k)+ '\n')

    s_f1, f_f1 = [], []

    # with open('/data/gaowh/data/g_sim_valid.json', 'r', encoding='utf-8') as f:

    with open('/data/gaowh/data/eval.json', 'r', encoding='utf-8') as f:
        for line in f.readlines():
            graph = json.loads(line.strip())

            # 删掉YD和YBQJ；过滤来自屈光不正库无疾病患者的疾病信息
            new_links = []
            drop_nodes = []
            for triplet in graph['links']:
                if triplet[-1] not in ['YD', 'YBQJ']:
                    # 二者为真时，为屈光不正库无疾病患者的疾病信息
                    if triplet[-2] != 672 or triplet[1] != 'EyeNode':
                        new_links.append(triplet)
                else:
                    drop_nodes.append(triplet[-2])
            graph['links'] = new_links
            for drop_node in drop_nodes:
                if str(drop_node) in graph['nodes']:
                    graph['nodes'].pop(str(drop_node))

            # 获取疾病标签，并删除
            graph, son_labels, father_labels = pop_labels(graph)

            # 请求
            res = requests.post("http://192.168.2.241:12088/raw_sim_patient_retrieval", data=json.dumps(graph))
            res = json.loads(res.text)
            # print(res)
            
            rightflag=True
            for i in res[:k]:
                i = i[1]
                _, r_son_labels, r_father_labels = pop_labels(i)
                
                s_f1.append(get_f1(son_labels, r_son_labels))
                f_f1.append(get_f1(father_labels, r_father_labels))

                if father_labels.count!=0:
                    actual.append(father_labels)
                    predicted.append(r_father_labels)
                file.write(str(father_labels)+" result:  "+str(r_father_labels) + '\n')

                if father_labels != r_father_labels:
                    rightflag=False
   
   
            if rightflag==True:
                allrightcount=allrightcount+1
    
    
    # print(actual)
    # print(predicted)
    calc(actual,predicted)
                    

    # print(f_f1)
    
    print(allrightcount)
    print('-' * 10 + 'top ' + str(k) + '-' * 10)

    print('result son F1', np.mean(s_f1))
    print('result father F1', np.mean(f_f1))
    s_overlap = [1 if i > 0 else 0 for i in s_f1]
    f_overlap = [1 if i > 0 else 0 for i in f_f1]
    # print(f_overlap)
    print('result son overlap rate', np.mean(s_overlap))
    print('result father overlap rate', np.mean(f_overlap))



def calc(actual,predicted):
    actual_binary = [[1 if i in labels else 0 for i in range(32, 40)] for labels in actual]
    predicted_binary = [[1 if i in labels else 0 for i in range(32, 40)] for labels in predicted]

    # 计算每个样本的精确度
    precisions = [precision_score(actual_binary[i], predicted_binary[i]) for i in range(len(actual_binary))]

    # 计算平均精确度
    mean_precision = sum(precisions) / len(precisions)

    print("平均精确度：", mean_precision)

if __name__ == '__main__':
    file = open('res.txt', 'a')

    for k_num in [1, 5, 10,20]:
        main(k_num)