from sklearn.decomposition import PCA
import numpy as np
import json
data = json.load(open('/data/gaowh/data/patient_2d.json', 'r', encoding='utf-8'))

id2vector, vectors = {}, []
for k, v in data.items():
    for patient in v:
        id2vector[patient[-1]['id']] = len(id2vector)
        vectors.append(patient[:768])

pca = PCA(n_components=3,whiten=True,svd_solver='auto')
reduced_x = pca.fit_transform(np.array(vectors))

new_data = {}
n = 0
for k, v in data.items():
    new_data[k] = []
    for patient in v:
        tmp = reduced_x[id2vector[patient[-1]['id']]].tolist()
        tmp.append(patient[-1])
        new_data[k].append(tmp)
        n+=1
json.dump(new_data, open('/data/gaowh/data/patient_2d_new_gattest_3d.json', 'w', encoding='utf-8'), ensure_ascii=False)