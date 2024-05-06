import logging
import sys

from neo4j import GraphDatabase
import json
import os

"""
该文件用以从neo4j中读取病历图
"""


class NeoSearch:
    """
    neo4j查询类，调用接口以查询图谱信息
    """

    def __init__(self):
        uri = "bolt://192.168.2.240/neo4j"
        user = "neo4j"
        password = "sigsrfj@neo4j"
        self.driver = GraphDatabase.driver(uri, auth=(user, password))
        self.enable_log(logging.INFO, sys.stdout)

        self.stop_entity_types = ["Zhusu", "Sex", "YYCheckNode", "SLCheckNode", "YBQJ", "YD", "HistoryEye", "ResultSon",
                                  "ResultFather", "HistoryBody", "Amd0", "Amd1", "Bj0", "Bj1", "Dr0", "Dr1", "Dr2",
                                  "Dr3", "Dr4", "Age"]

    def close(self):
        # Don't forget to close the driver connection when you are finished with it
        self.driver.close()

    @staticmethod
    def enable_log(level, output_stream):
        handler = logging.StreamHandler(output_stream)
        handler.setLevel(level)
        logging.getLogger("neo4j").addHandler(handler)
        logging.getLogger("neo4j").setLevel(level)

    @staticmethod
    def get_triplets(tx, _node_id, not_o_id):
        """
        查询三元组
        :param tx:
        :param _node_id: 头节点id
        :param not_o_id: 不被查询的尾节点id
        :return:
        """
        return [row for row in tx.run("MATCH (s)-[r]-(o) WHERE id(s) = " + str(_node_id) +
                                      " AND id(o) <> " + str(not_o_id) +
                                      " RETURN [id(s),labels(s),id(r),type(r),id(o),labels(o)]").data()]

    @staticmethod
    def get_result_father_triplets(tx, _node_id):
        """
        查询三元组
        :param tx:
        :param _node_id: 头节点id
        :return:
        """
        return [row for row in tx.run("MATCH (s)-[r]-(o:ResultFather) WHERE id(s) = " + str(_node_id) +
                                      " RETURN [id(s),labels(s),id(r),type(r),id(o),labels(o)]").data()]

    @staticmethod
    def get_node_by_id(tx, _node_id):
        """
        查询节点
        :param tx:
        :param _node_id: 节点id
        :return:
        """
        return [row for row in tx.run("MATCH (s) WHERE id(s) = " + str(_node_id) + " RETURN s").data()]

    def get_all_patient_ids(self):
        """
        获取图谱中所有的病人id
        :return:
        """
        with self.driver.session() as session:
            # neo查询语句
            def get_all_patient_ids(tx):
                return [row for row in tx.run("MATCH (n:CaseRecord) RETURN id(n)").data()]

            all_patient_ids = session.execute_read(get_all_patient_ids)
            all_patient_ids = [patient_id['id(n)'] for patient_id in all_patient_ids]
            return all_patient_ids

    def get_graph_by_patient_id(self, patient_id, max_num=50):
        """
        通过病人id获取病历图
        :param patient_id:
        :param max_num:每一层的最大查询数量
        :return:
        """
        # 节点邻接表：[[s_id, s_types, r_id, r_type, o_id, o_types]]
        all_triplets = []
        layer_node_ids = [(patient_id, patient_id)]
        with self.driver.session() as session:
            # 利用节点id一层层查询邻接关系; 根据内在逻辑，最深不超过三层
            for _ in range(3):
                next_layer_node_ids = []
                for so_node_id in layer_node_ids[:max_num]:
                    triplets = session.execute_read(self.get_triplets, so_node_id[0], so_node_id[1])
                    for triplet in triplets:
                        for v in triplet.values():
                            all_triplets.append(v)
                            # 如o_id不在停止扩散的列表中，则加入到下一层查询节点列表
                            trigger = sum([1 if o_type in self.stop_entity_types else 0 for o_type in v[-1]])
                            if trigger == 0:
                                next_layer_node_ids.append((v[-2], v[0]))
                    layer_node_ids = next_layer_node_ids
            # 用三元组内的所有ResultSon来获取ResultFather节点
            result_sons = []
            for t in all_triplets:
                if 'ResultSon' in t[1]: result_sons.append(t[0])
                if 'ResultSon' in t[-1]: result_sons.append(t[-2])
            for result_son in list(set(result_sons)):
                triplets = session.execute_read(self.get_result_father_triplets, result_son)
                for triplet in triplets:
                    for v in triplet.values():
                        all_triplets.append(v)

            id2node = {}
            for triplet in all_triplets:
                # 遍历头尾节点id
                for node_id in [triplet[0], triplet[-2]]:
                    if node_id not in id2node:
                        node = session.execute_read(self.get_node_by_id, node_id)
                        id2node[node_id] = {k: v for k, v in node[0]['s'].items() if k not in ['HopDate', 'FistDia']}

        return all_triplets, id2node


def get_graphs():
    """
    获取所有的病例图
    """
    # if not os.path.exists('../data'):
    #     os.mkdir('../data')

    fp = open('/data/gaowh/data/patient_kg.json', 'w', encoding='utf-8')
    a = NeoSearch()
    for p_id in a.get_all_patient_ids():
        p, n = a.get_graph_by_patient_id(p_id)
        for i in p:
            i[1] = i[1][0]
            i[-1] = i[-1][0]
        data = {"links": p, "nodes": n}
        fp.write(json.dumps(data, ensure_ascii=False) + '\n')



if __name__ == '__main__':
    get_graphs()
