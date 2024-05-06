import random

from utils.common import convert_graph2sum_vec, pop_labels
import json
import numpy as np
from collections import Counter
from sklearn.metrics import precision_score




def calc(actual,predicted):
    actual_binary = [[1 if i in labels else 0 for i in range(32, 40)] for labels in actual]
    predicted_binary = [[1 if i in labels else 0 for i in range(32, 40)] for labels in predicted]

    # 计算每个样本的精确度
    precisions = [precision_score(actual_binary[i], predicted_binary[i]) for i in range(len(actual_binary))]

    # 计算平均精确度
    mean_precision = sum(precisions) / len(precisions)

    print("平均精确度：", mean_precision)


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


def load_samples(file):
    samples = []
    with open('/data/gaowh/data/' + file, 'r', encoding='utf-8') as f:
        for line in f.readlines():
            samples.append(json.loads(line.strip()))
    return samples


def cosine_distance(a, b):
    if a.shape != b.shape:
        raise RuntimeError("array {} shape not match {}".format(a.shape, b.shape))
    if a.ndim == 1:
        a_norm = np.linalg.norm(a)
        b_norm = np.linalg.norm(b)
    elif a.ndim == 2:
        a_norm = np.linalg.norm(a, axis=1, keepdims=True)
        b_norm = np.linalg.norm(b, axis=1, keepdims=True)
    else:
        raise RuntimeError("array dimensions {} not right".format(a.ndim))
    similiarity = np.dot(a, b.T) / (a_norm * b_norm)
    dist = 1. - similiarity
    return dist


train_samples, eval_samples = load_samples('train.json'), load_samples('eval.json')
eval_num = len(eval_samples)


id2sample = [pop_labels(i) for i in eval_samples]
#id2sample 是做出来的全量样本集

#调用现有的Word2Vec得到姜维空间
id2feature = [convert_graph2sum_vec(i[0]) for i in id2sample]
reduced_x = np.array(id2feature)



for k_num in [1, 5, 10,20]:
    s_f1, f_f1 = [], []

    actual=[]
    predicted=[]

    for idx in range(eval_num):
        
        _, son_labels, father_labels = id2sample[idx]

        #在这里余弦距离  找姜维空间的距离
        left = reduced_x[idx]
        distance = [cosine_distance(left, reduced_x[j]) for j in range(len(id2sample))]
        top_k = np.argsort(distance)[1:k_num+1]


        for k in top_k:
            _, r_son_labels, r_father_labels = id2sample[k]

            if father_labels.count!=0:
                actual.append(father_labels)
                predicted.append(r_father_labels)

            
            
            s_f1.append(get_f1(son_labels, r_son_labels))
            f_f1.append(get_f1(father_labels, r_father_labels))

    print('-' * 10 + 'top ' + str(k_num) + '-' * 10)
    f_overlap = [1 if i > 0 else 0 for i in f_f1]
    print('result father overlap rate', np.mean(f_overlap))
    print('result father F1', np.mean(f_f1))

    s_overlap = [1 if i > 0 else 0 for i in s_f1]
    print('result son overlap rate', np.mean(s_overlap))
    print('result son F1', np.mean(s_f1))
    calc(actual,predicted)
