import json
import random

"""
该文件用以采样病历节点元路径
"""

ELEMENT2TYPES = {'病人': ['CaseRecord'], '年龄': ['Age'], '性别': ['Sex'], '眼睛': ['EyeNode'],
                 '疾病史-全身': ['QsOtherNode', 'GxyNode', 'DiaNode', 'GxzNode', 'NgsNode', 'GxbNode', 'QsMianyiNode'],
                 '全身病史': ['HistoryBody'],
                 '疾病史-眼科': ['HisBltNode', 'HisEyeOtherNode', 'HisCataractNode', 'HisGlaucomaNode',
                                 'HisDiaRetinNode', 'HisEyeWssNode'],
                 '图像分类': ["Amd0", "Amd1", "Bj0", "Bj1", "Dr0", "Dr1", "Dr2", "Dr3", "Dr4", "Age"],
                 '眼病史': ['HistoryEye'],
                 '主诉症状': ['ZhusuNode'],
                 '主诉': ['Zhusu'],
                 '各项检查': ['EyeJztNode', 'EyeCancerNode', 'SLCheckNode', 'YYCheckNode', 'EyeSpecialNode'],
                 '检查异常': ['YBQJ', 'YD'],
                 '中转': ['EyeDiagnoseNode', 'ResultFather', 'ResultSon']}

# 区分独立、公共节点类型，形成id映射；
REUSE_NODE_TYPES = ['ResultSon', 'ResultFather', 'YD', 'YBQJ', 'Zhusu', 'HistoryEye', 'HistoryBody', 'Sex']


def get_adj():
    """
    节点到邻接节点的查询字典（需要包含，节点的类型）
    :return:
    """
    node2other_nodes = {}
    node_type2nodes = {}
    node2node_type = {}
    with open('/data/gaowh/data/patient_kg.json', 'r', encoding='utf-8') as f:
        for line in f.readlines():
            sample = json.loads(line.strip())
            for t in sample['links']:
                # 头节点按实体类型加入到字典-
                s_type = t[1]
                node2node_type[t[0]] = s_type
                node_type2nodes[s_type] = node_type2nodes.get(s_type, [])
                if t[0] not in node_type2nodes[s_type]:
                    node_type2nodes[s_type].append(t[0])

                # 尾节点按实体类型加入到字典
                o_type = t[-1]
                node2node_type[t[-2]] = o_type
                node_type2nodes[o_type] = node_type2nodes.get(o_type, [])
                if t[-2] not in node_type2nodes[o_type]:
                    node_type2nodes[o_type].append(t[-2])
                # 构建无向边
                node2other_nodes[t[0]] = node2other_nodes.get(t[0], []) + [t[-2]]
                node2other_nodes[t[-2]] = node2other_nodes.get(t[-2], []) + [t[0]]
    # 去重
    for k, v in node2other_nodes.items():
        node2other_nodes[k] = list(set(v))
    return node2other_nodes, node_type2nodes, node2node_type


def schema_process(n_id, e_type, schema, node2other_nodes, node2node_type):
    """
    根据定义的schema去采样形成路径
    :return:
    """
    path = [n_id]
    path_stack = [(e_type, n_id)]
    for schema_element in schema:
        e_type, walk_type = schema_element[0], schema_element[1]
        # 往前走
        if walk_type > -1:
            # 0型，大概率会不路过
            if walk_type == 0:
                if random.random() < 0.85:
                    continue
            # 寻找可走的节点，并随机迈出一步；无合适节点的两类型:1.当前节点不在正确的节点类型上；2.当前节点确实无下一步合适节点
            next_types = ELEMENT2TYPES[e_type]
            available_next_nodes = [n for n in node2other_nodes[n_id] if node2node_type[n] in next_types]
            if available_next_nodes:
                n_id = random.choice(available_next_nodes)
                path_stack.append((e_type, n_id))
                # 1型，大概率不记录
                if walk_type == 1 and random.random() < 0.85:
                    continue
                path.append(n_id)
        # 往后退；大概率不记录
        elif len(path_stack) > 1:
            if path_stack[-2][0] == e_type:
                path_stack.pop()
                n_id = path_stack[-1][1]
                if random.random() < 0.85:
                    continue
                path.append(n_id)
    return path


def transit(path, node2other_nodes, node2node_type):
    """
    借助诊断结果转移
    :return:
    """
    n_id = path[-1]
    if node2node_type[n_id] != 'ResultFather':
        if node2node_type[n_id] == 'ResultSon':
            if random.random() > 0.5:
                return True, path
        available_next_nodes = [n for n in node2other_nodes[n_id] if node2node_type[n] == 'ResultFather']
        if node2node_type[n_id] == 'ResultSon':
            1
        if not available_next_nodes:
            return False, path
        path.append(random.choice(available_next_nodes))
    return True, path


def sample_paths(num_walk=100, num_deep=5):
    """
    路径采样
    :return:
    """
    node2other_nodes, node_type2nodes, node2node_type = get_adj()


    # 这里的眼底病灶只采样一次


    fp = open('/data/gaowh/data/meta_path.json', 'w', encoding='utf-8')
    n_count = 0

    # 权重说明：2路过且记录，1路过但大概率不记录，0大概率不路过；路过会记录，-1回退大概率不记录
    path_schema_left = [('性别', 2), ('病人', -1), ('年龄', 2), ('病人', -1),
                        ('疾病史-全身', 2), ('全身病史', 0), ('疾病史-全身', -1), ('病人', -1),
                        ('眼睛', 2), ('疾病史-眼科', 2), ('眼病史', 0), ('疾病史-眼科', -1), ('眼睛', -1),
                        ('主诉症状', 1), ('主诉', 2), ('主诉症状', -1), ('眼睛', 1), ('图像分类', 2),
                        ('眼睛', -1), ('各项检查', 1), ('检查异常', 2), ('各项检查', -1), ('眼睛', -1),
                         ('检查异常', 2), ('眼睛', -1),                         
                          ('中转', 2)]
    path_schema_right = [('眼睛', 1), ('疾病史-眼科', 2), ('眼病史', 0), ('疾病史-眼科', -1), ('眼睛', -1),
                         ('病人', 1), ('疾病史-全身', 2), ('全身病史', 0), ('疾病史-全身', -1), ('病人', -1),
                         ('眼睛', 1), ('各项检查', 1), ('检查异常', 2), ('各项检查', -1), ('眼睛', -1),
                         ('检查异常', 2), ('眼睛', -1),
                         ('图像分类', 2), ('眼睛', -1), ('主诉症状', 1), ('主诉', 2), ('主诉症状', -1), ('眼睛', -1),
                         ('病人', 2), ('年龄', 2), ('病人', -1), ('性别', 2), ('病人', 2)]
    for _ in range(num_walk):
        for n_id in node_type2nodes['CaseRecord']:
            path = [n_id]
            for _ in range(num_deep):
                path += schema_process(n_id, '病人', path_schema_left, node2other_nodes, node2node_type)[1:]
                if node2node_type[path[-1]] in ELEMENT2TYPES['中转']:
                    is_transited, path = transit(path, node2other_nodes, node2node_type)
                    if is_transited:
                        path += schema_process(path[-1], '中转', path_schema_right, node2other_nodes, node2node_type)[
                                1:]
                # 是否往深层再游走
                if path[-1] == path[0] or node2node_type[path[-1]] != 'CaseRecord':
                    break

            path = [n_id for n_id in path if random.random() > 0.1]
            if len(path) >= 5:
                n_count += 1
                fp.write(json.dumps([(n_id, node2node_type[n_id]) for n_id in path], ensure_ascii=False) + '\n')
    print('总路径数', n_count)


sample_paths()
