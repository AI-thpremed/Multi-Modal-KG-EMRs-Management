
import random

# from utils.common import convert_graph2sum_vec, pop_labels
import json
import numpy as np
from collections import Counter

def del_line_from(file, del_line):  # del_line 行号从1开始
    with open(file, 'r', encoding="utf-8") as old_file:
        with open(file, 'r+', encoding="utf-8") as new_file:
            current_line = 0
            # 定位到需要删除的行
            while current_line < (del_line - 1):
                old_file.readline()
                current_line += 1
            # 当前光标在被删除行的行首，记录该位置
            seek_point = old_file.tell()
            # 设置光标位置
            new_file.seek(seek_point, 0)
            # 读需要删除的行，光标移到下一行行首
            del_line_content = old_file.readline()
            # 被删除行的下一行读给 next_line
            next_line = old_file.readline()
            # 连续覆盖剩余行，后面所有行上移一行
            while next_line:
                new_file.write(next_line)
                next_line = old_file.readline()
            # 写完最后一行后截断文件，因为删除操作，文件整体少了一行，原文件最后一行需要去掉
            new_file.truncate()
    return del_line_content # 剪切的行的内容



def load_samples(file):
    samples = []
    with open('/data/gaowh/data/' + file, 'r', encoding='utf-8') as f:
        for id,line in f.readlines():
            #示例
            a=random.randint(1,100)
            # print(a)
            if a<=10:
                samples.append(json.loads(line.strip()))
                del_line_from('/data/gaowh/data/' + file,id)

    return samples

xxx=load_samples("g_sim_valid.json")
fp = open('/data/gaowh/data/eval.json', 'w', encoding='utf-8')

for p in xxx:
    # p, n = a.get_graph_by_patient_id(p_id)
    # for i in p:
    #     i[1] = i[1][0]
    #     i[-1] = i[-1][0]
    # data = {"links": p, "nodes": n}
    fp.write(p + '\n')