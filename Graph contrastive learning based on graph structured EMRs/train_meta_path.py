from gensim.models import Word2Vec
import json

# 区分独立、公共节点类型，形成id映射；
REUSE_NODE_TYPES = ['ResultSon', 'ResultFather', 'YD', 'YBQJ', 'Zhusu', 'HistoryEye', 'HistoryBody', 'Sex', 'Age']


def train():
    """
    训练skip-gram
    """
    sentences = []
    with open('/data/gaowh/data/meta_path.json', 'r', encoding='utf-8') as f:
        for line in f.readlines():
            data = json.loads(line)
            sentences.append([i[1] + '_' + str(i[0]) if i[1] in REUSE_NODE_TYPES else i[1] for i in data])
    print(len(sentences))
    model = Word2Vec(sentences=sentences, vector_size=768, window=5, min_count=5, workers=4, sg=1)
    model.save("/data/gaowh/data/node2vec.model")


def infer():
    """
    试用
    """
    model = Word2Vec.load("data/node2vec.model")
    print(model.wv.get_index('002', len(model.wv)))
    sims = model.wv.most_similar('Amd1', topn=10)
    print(sims)


if __name__ == '__main__':
    train()
    # infer()
