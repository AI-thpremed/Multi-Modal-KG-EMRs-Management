# utf-8
import json


def get_diagnoses(sample):
    """
    获取诊断结果
    """
    diagnoses = []
    diagnose_str = ''
    for i in ['ResultSon', 'ResultFather', 'EyeDiagnoseNode']:
        for triplet in sample['links']:
            n_i, n_t = str(triplet[-2]), triplet[-1]
            n_info = sample['nodes'][n_i]
            if n_t == 'ResultSon' == i:
                if '、' + n_info['name'] not in diagnose_str:
                    diagnose_str += '、' + n_info['name']
            if n_t == 'ResultFather' == i:
                diagnoses.append(n_info['name'])
            if n_t == 'EyeDiagnoseNode' == i:
                if n_info.get('DisplayName', '') == '其它':
                    if len(n_info.get('Info', '')) > 0 and '、【' + n_info.get('Info', '') + '】' not in diagnose_str:
                        diagnose_str += '、【' + n_info.get('Info', '') + '】'
    while diagnose_str.startswith('、'):
        diagnose_str = diagnose_str[1:]
    diagnoses = [i for i in diagnoses if len(i)]
    diagnoses = list(set(diagnoses))

    diagnose_str = diagnose_str if diagnose_str else '无'
    return diagnoses, diagnose_str


def get_treatment(sample):
    """
    获取治疗方案
    """
    treatment_str = ''
    all_items = []
    for i in ['用药情况', '手术方式']:
        treatment = []
        for triplet in sample['links']:
            n_i, n_t = str(triplet[-2]), triplet[-1]
            n_info = sample['nodes'][n_i]
            if n_t == 'EyeDiagnoseNode':
                if n_info.get('DisplayName', '') == i:
                    treatment.append(n_info.get('Info', ''))
        treatment = [i for i in treatment if len(i)]
        treatment = list(set(treatment))
        all_items += treatment
        if treatment:
            treatment_str += i[:2] + ':' + '、'.join(treatment) + '。'

    treatment_str = treatment_str if treatment_str else '无'
    return treatment_str


def get_single_items(sample, item_name='Zhusu'):
    """
    获取主诉、异常项
    """
    unusual_str = ''
    for i in [item_name]:
        items = []
        for triplet in sample['links']:
            n_i, n_t = str(triplet[-2]), triplet[-1]
            n_info = sample['nodes'][n_i]
            if n_t == i:
                items.append(n_info['name'])
        items = [i for i in items if len(i)]
        items = list(set(items))
        if items:
            unusual_str += '、'.join(items)
    unusual_str = unusual_str if unusual_str else '无'
    return unusual_str


def get_unusual_items(sample):
    """
    获取主诉、异常项
    """
    unusual_items = []
    unusual_str = ''
    for i in ['Zhusu', 'YBQJ', 'YD']:
        items = []
        for triplet in sample['links']:
            n_i, n_t = str(triplet[-2]), triplet[-1]
            n_info = sample['nodes'][n_i]
            if n_t == i:
                items.append(n_info['name'])
        items = [i for i in items if len(i)]
        items = list(set(items))
        if items:
            unusual_str += i.replace('Zhusu', '主诉病情：').replace('YBQJ', '【眼部前节异常项】') \
                               .replace('YD', '【眼底异常项】') + '、'.join(items) + '。'
        unusual_items += items
    unusual_str = unusual_str if unusual_str else '无'
    return unusual_items, unusual_str


def get_info(sample):
    """
    获取病例基本信息
    """
    info = ''
    for i in ['Sex', 'CaseRecord', 'HistoryBody', 'HistoryEye']:
        trigger = False
        disease_items = []
        for triplet in sample['links']:
            for node in [triplet[:2], triplet[-2:]]:
                if node[1] == i == 'Sex':
                    info += sample['nodes'][str(node[0])]['name']
                    trigger = True
                if node[1] == i == 'CaseRecord':
                    info += '，年龄' + str(sample['nodes'][str(node[0])]['HopAge'])
                    trigger = True
                if node[1] == i == 'HistoryBody':
                    disease_items.append(str(sample['nodes'][str(node[0])]['name']).replace('其他', '其他类'))
                if node[1] == i == 'HistoryEye':
                    disease_items.append(str(sample['nodes'][str(node[0])]['name']).replace('其他', '其他类'))
            if trigger:
                break
        if disease_items:
            disease_items = list(set(disease_items))
            disease_items.sort(key=len, reverse=True)
            if i == 'HistoryBody':
                info += '，' + '、'.join(disease_items) + '全身病史'
            if i == 'HistoryEye':
                info += '，' + '、'.join(disease_items) + '眼病史'
    return info


# def get_s_item(sample, s_item='Sex'):
#     """
#     获取病例基本信息
#     """
#     info = ''
#     for i in [s_item]:
#         trigger = False
#         disease_items = []
#         for triplet in sample['links']:
#             for node in [triplet[:2], triplet[-2:]]:
#                 if node[1][0] == i == 'Sex':
#                     info += sample['nodes'][str(node[0])]['name']
#                     trigger = True
#                 if node[1][0] == i == 'CaseRecord':
#                     if sample['nodes'][str(node[0])]['HopAge'] < 13:
#                         info += '12岁及以下'
#                     elif sample['nodes'][str(node[0])]['HopAge'] < 21:
#                         info += '13岁至20岁'
#                     elif sample['nodes'][str(node[0])]['HopAge'] < 41:
#                         info += '21岁至40岁'
#                     elif sample['nodes'][str(node[0])]['HopAge'] < 61:
#                         info += '41岁至60岁'
#                     else:
#                         info += '60岁以上'
#                     trigger = True
#                 if node[1][0] == i == 'HistoryBody':
#                     disease_items.append(str(sample['nodes'][str(node[0])]['name']).replace('其他', '其他类'))
#                 if node[1][0] == i == 'HistoryEye':
#                     disease_items.append(str(sample['nodes'][str(node[0])]['name']).replace('其他', '其他类'))
#             if trigger:
#                 break
#         if disease_items:
#             disease_items = list(set(disease_items))
#             disease_items.sort(key=len, reverse=True)
#             if i == 'HistoryBody':
#                 info += '，' + '、'.join(disease_items) + '全身病史'
#             if i == 'HistoryEye':
#                 info += '，' + '、'.join(disease_items) + '眼病史'
#     return info
#
#
# def get_ResultFather(sample):
#     """
#     获取诊断结果
#     """
#     diagnoses = []
#     for i in ['ResultFather']:
#         for triplet in sample['links']:
#             n_i, n_t = str(triplet[-2]), triplet[-1][0]
#             n_info = sample['nodes'][n_i]
#             if n_t == 'ResultFather' == i:
#                 diagnoses.append(n_info['name'])
#
#     diagnoses = [i for i in diagnoses if len(i)]
#     diagnoses = list(set(diagnoses))
#
#     return diagnoses
#
#
# def get_diagnose_clouds(sample):
#     """
#     获取诊断结果
#     """
#     diagnoses = []
#     for i in ['ResultSon', 'ResultFather', 'EyeDiagnoseNode']:
#         for triplet in sample['links']:
#             n_i, n_t = str(triplet[-2]), triplet[-1][0]
#             n_info = sample['nodes'][n_i]
#             if n_t == 'ResultSon' == i:
#                 diagnoses.append(n_info['name'])
#             if n_t == 'ResultFather' == i:
#                 diagnoses.append(n_info['name'])
#             if n_t == 'EyeDiagnoseNode' == i:
#                 if n_info.get('DisplayName', '') == '其它':
#                     diagnoses.append(n_info.get('Info', ''))
#     diagnoses = [i for i in diagnoses if len(i)]
#     diagnoses = list(set(diagnoses))
#     return diagnoses
#
#
# sex_dict = {}
# age_dict = {}
# d_dict = {}
#
# word_cloud = {}
# [
#     ['foo', 12],
#     ['bar', 6],
#     ...
# ]
# with open('../data/patient_kg.json', 'r', encoding='utf-8') as f:
#     for line in f.readlines():
#         s = json.loads(line.strip())
#         # sex
#         sex = get_s_item(s, 'Sex')
#         sex_dict[sex] = sex_dict.get(sex, 0) + 1
#
#         # age
#         age = get_s_item(s, 'CaseRecord')
#         age_dict[age] = age_dict.get(age, 0) + 1
#
#         #
#         for d in get_ResultFather(s):
#             d_dict[d] = d_dict.get(d, 0) + 1
#
#         for d in get_diagnose_clouds(s):
#             word_cloud[d] = word_cloud.get(d, 0) + 1
# print([{"value": v, "name": k} for k, v in sex_dict.items()])
# print([{"value": v, "name": k} for k, v in age_dict.items()])
# print([{"value": v, "name": k} for k, v in d_dict.items()])
# word_cloud = [[k, v] for k, v in word_cloud.items()]
#
# word_cloud.sort(key=lambda x: x[1], reverse=True)
# print(word_cloud)
