import json
import random
from copy import deepcopy
from collections import Counter
import numpy as np

"""
该文件用以构建病历相似度数据
"""


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


def compute_two_sim_score(l_sample, r_sample, l_pre=[], r_pre=[]):
    """
    病例样本构建
    """

    # 1.获取诊断结果，并以诊断结果为核心去计算关联性
    def get_labels(sample):
        son_labels, father_labels = [], []
        for triplet in sample['links']:
            if triplet[3] in ['诊断结果', '诊断属于']:
                for node in [triplet[:2], triplet[-2:]]:
                    if node[1] == 'ResultSon':
                        son_labels.append(node[0])
                    if node[1] == 'ResultFather':
                        father_labels.append(node[0])
        return list(set(son_labels)), list(set(father_labels))

    l_son_labels, l_father_labels = get_labels(l_sample)
    r_son_labels, r_father_labels = get_labels(r_sample)

    l_son_labels += l_pre
    r_son_labels += r_pre
    l_father_labels += l_pre
    r_father_labels += r_pre

    sim_score = (get_f1(l_son_labels, r_son_labels) * 0.2 + get_f1(l_father_labels, r_father_labels) * 0.8) * 2. - 1.

    # 2.随机丢弃图里的诊断结果信息，从而防止信息过量泄露；特征过于明显，导致模型对其他部分的把握会弱
    def random_drop_labels(sample, father_labels, son_labels):
        drop_nodes = [i for i in father_labels + son_labels if random.random() > 0.3]
        for drop_node in drop_nodes:
            if str(drop_node) in sample['nodes']:
                sample['nodes'].pop(str(drop_node))
        sample['links'] = [triplet for triplet in sample['links'] if
                           triplet[0] not in drop_nodes and triplet[-2] not in drop_nodes]
        return sample

    return sim_score, random_drop_labels(l_sample, l_father_labels, l_son_labels), random_drop_labels(r_sample,
                                                                                                      r_father_labels,
                                                                                                      r_son_labels)


def get_graphs():
    """
    读取图数据
    """
    fp = open('../data/clean_kg.json', 'w', encoding='utf-8')
    with open('../data/patient_kg.json', 'r', encoding='utf-8') as f:
        for line in f.readlines():
            graph = json.loads(line.strip())
            new_links = []
            drop_nodes = []
            for triplet in graph['links']:
                if triplet[-1] not in ['YD', 'YBQJ', "Amd0", "Amd1", "Bj0", "Bj1", "Dr0", "Dr1", "Dr2", "Dr3", "Dr4"]:
                    # new_links.append(triplet)
                    if triplet[-2] != 672 or triplet[1] != 'EyeNode':
                        new_links.append(triplet)
                else:
                    drop_nodes.append(triplet[-2])
            graph['links'] = new_links
            for drop_node in drop_nodes:
                if str(drop_node) in graph['nodes']:
                    graph['nodes'].pop(str(drop_node))
            fp.write(json.dumps(graph, ensure_ascii=False) + '\n')
    fp.close()

    graphs = []
    result2graph, graph2results = {}, {}
    g_id = 0
    with open('../data/clean_kg.json', 'r', encoding='utf-8') as f:
        for line in f.readlines():
            graph = json.loads(line.strip())

            results = []
            count = 0
            for triplet in graph['links']:
                if triplet[3] in ['诊断结果', '诊断属于']:
                    count += 1
                    for node in [triplet[:2], triplet[-2:]]:
                        if node[1] == 'ResultSon':
                            results.append('Son_' + str(node[0]))

                        if node[1] == 'ResultFather':
                            results.append('Father_' + str(node[0]))
            if count == 0:
                continue
            if len(results) == 0:
                results = ['空']
            graph2results[g_id] = list(set(results))
            for result in results:
                result2graph[result] = result2graph.get(result, []) + [(g_id, graph)]

            graphs.append(graph)
            g_id += 1
    return graphs, result2graph, graph2results


def pos_samples(sample, mode):
    """
    sim score 0.85~1；需要确保二者不完全一致
    """
    sample = deepcopy(sample)
    # 获取样本的节点到类型
    node2type = {}
    for triplet in sample['links']:
        node2type[triplet[0]] = triplet[1]
        node2type[triplet[-2]] = triplet[-1]

    # 1.删除节点及其关联边；普通节点15%的概率丢弃，结果节点50%的概率丢弃
    if mode == 'drop_node':
        nodes = [k for k, v in node2type.items() if v in ['CaseRecord', 'EyeNode']]
        super_drop_nodes = [k for k, v in node2type.items() if v in ['EyeDiagnoseNode', 'ResultSon', 'ResultFather']]
        normal_drop_nodes = [k for k, v in node2type.items() if k not in nodes + super_drop_nodes]
        nodes += [i for i in normal_drop_nodes if random.random() > 0.15] + [i for i in super_drop_nodes if
                                                                             random.random() > 0.5]
        sample['links'] = [triplet for triplet in sample['links'] if triplet[0] in nodes and triplet[-2] in nodes]
        if len(nodes) == len(node2type):
            return sample, 1.
        return sample, 0.9
    # 2.随机删除一些边;15%的概率删除边
    elif mode == 'drop_edge':
        sample['links'] = [triplet for triplet in sample['links'] if triplet[3] == '属于' or random.random() > 0.15]
        return sample, 1.
    # TODO 3.遮掩属性值;前期未利用到
    return sample, 1.


def has_diagnose(sample):
    """
    检查是否存在其它类的节点；并返回列表，将此类节点记为序号-1
    """
    other_points = []
    for triplet in sample['links']:
        if triplet[3] in ['诊断结果', '诊断属于']:
            for node in [triplet[:2], triplet[-2:]]:
                if node[1] == 'EyeDiagnoseNode':
                    if sample['nodes'][str(node[0])].get('DisplayName', '') == '其它':
                        other_points.append(-1)
    if other_points:
        return True, other_points
    return False, []


def build_graph_dataset():
    """
    获取图数据
    """
    graphs, result2graph, graph2results = get_graphs()
    eval_ids = [21192, 21199, 21207, 21211, 21237, 21247, 21256, 21258, 21265, 21293, 21296,
                21308, 21311, 21315, 21325, 21329, 21334, 21338, 21342, 21347, 21370, 21375,
                21388, 21400, 21433, 21441, 21442, 21447, 21463, 21471, 21502, 21511, 21513,
                21518, 21519, 21523, 21535, 21541, 21576, 21584, 21593, 21640, 21645, 21651,
                21656, 21668, 21669, 21672, 21686, 21691, 21693, 21697, 21698, 21704, 21712,
                21721, 21734, 21739, 21744, 21755, 21772, 21777, 21779, 21784, 21796, 21816,
                21819, 21823, 21831, 21851, 21863, 21870, 21880, 21888, 21891, 21894, 21909,
                21922, 21926, 21933, 21941, 21978, 21983, 21989, 22011, 22013, 22015, 22018,
                22019, 22028, 22033, 22058, 22064, 22065, 22070, 22071, 22077, 22087, 22108,
                22113, 22115, 22118, 22121, 22126, 22128, 22143, 22145, 22151, 22175, 22177,
                22183, 22184, 22185, 22188, 22246, 22250, 22265, 22284, 22297, 22298, 22305,
                22317, 22318, 22321, 22354, 22400, 22438, 22444, 22458, 22495, 22507, 22517,
                22553, 22554, 22583, 22587, 22628, 22663, 22675, 22687, 22688, 22698, 22709,
                29416, 29417, 29418, 29440, 29449, 29453, 29466, 29468, 29482, 29486, 29490,
                29493, 29503, 29513, 29519, 29536, 29565, 29578, 29596, 29601, 29607, 29610,
                29627, 29630, 29641, 29647, 29653, 29665, 29671, 29674, 29688, 29703, 29706,
                29721, 29722, 29730, 29731, 29749, 29751, 29756, 29774, 29775, 29782, 29791,
                29795, 29796, 29797, 29798, 29799, 29808, 29813, 29819, 29824]
    g_train_fp = open('../data/g_sim_train.json', 'w', encoding='utf-8')
    g_valid_fp = open('../data/g_sim_valid.json', 'w', encoding='utf-8')

    n = 0

    def write_line(a, b, score, fp):
        if b['links'][0][0] not in eval_ids:
            data = {"example_a": a, "example_b": b, "sim_score": score}
            fp.write(json.dumps(data, ensure_ascii=False) + '\n')

    scores = []
    for g_id, graph in enumerate(graphs):
        if graph['links'][0][0] not in eval_ids:
            g_fp = g_train_fp
        else:
            g_fp = g_valid_fp
        # 正样本构建
        for mode in ['drop_node', 'drop_edge']:
            example_a = deepcopy(graph)
            example_b, sim_score = pos_samples(example_a, mode)
            n += 1
            write_line(example_a, example_b, sim_score, g_fp)
            scores.append(sim_score)
        neg_samples = graphs[g_id + 1:]
        # 负样本构建
        neg_num = min(20, len(neg_samples))
        for neg_sample in random.sample(neg_samples, neg_num):
            example_a = deepcopy(graph)
            example_b = deepcopy(neg_sample)
            trigger_a, pre_a = has_diagnose(example_a)
            trigger_b, pre_b = has_diagnose(example_b)
            # 不具备可比较性
            if trigger_a and trigger_b:
                continue
            sim_score, example_a, example_b = compute_two_sim_score(example_a, example_b, pre_a, pre_b)
            n += 1
            write_line(example_a, example_b, sim_score, g_fp)
            scores.append(sim_score)
        # 高相关负样本构建
        results = graph2results[g_id]
        choose_type = random.random()
        for result in results:
            if result != '空':
                if choose_type > 0.5:
                    if not result.startswith('Son'):
                        continue
                else:
                    if not result.startswith('Father'):
                        continue
            # 防泄漏
            neg_samples = [i[1] for i in result2graph[result] if i[0] > g_id]
            neg_num = min(10, len(neg_samples))
            for neg_sample in random.sample(neg_samples, neg_num):
                example_a = deepcopy(graph)
                example_b = deepcopy(neg_sample)
                trigger_a, pre_a = has_diagnose(example_a)
                trigger_b, pre_b = has_diagnose(example_b)
                # 不具备可比较性
                if trigger_a and trigger_b:
                    continue
                sim_score, example_a, example_b = compute_two_sim_score(example_a, example_b, pre_a, pre_b)
                n += 1
                write_line(example_a, example_b, sim_score, g_fp)
                scores.append(sim_score)
    print('相似性均值', np.mean(scores))
    print('病例对数量', n)


build_graph_dataset()
