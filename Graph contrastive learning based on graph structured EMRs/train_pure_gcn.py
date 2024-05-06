# -*- coding: utf-8 -*-
from torch.utils.data import DataLoader
from utils.dataset_graph import PatientGraphDataset
from transformers import AutoTokenizer
from argparse import ArgumentParser
from gat_model_pure import BertGAT_pure
from gcn_model import BertGCN
import torch
from tqdm import tqdm
import numpy as np
from gensim.models import Word2Vec
import os

# from torch.nn import DataParallel

os.environ["CUDA_VISIBLE_DEVICES"] = "3"


def build_dist_loaders(args, tokenizer):
    """
    获取数据加载
    """
    node_type_table = Word2Vec.load(args.node_emb_path).wv
    embed_matrix = node_type_table.vectors

    train_dataset = PatientGraphDataset(tokenizer, args.train_path, node_type_table)
    valid_dataset = PatientGraphDataset(tokenizer, args.valid_path, node_type_table)

    train_loader = DataLoader(train_dataset,
                              collate_fn=train_dataset.collate,
                              pin_memory=(args.device == "cuda"),
                              num_workers=args.num_workers,
                              sampler=None,
                              batch_size=args.train_batch_size,
                              shuffle=True)
    valid_loader = DataLoader(valid_dataset,
                              collate_fn=valid_dataset.collate,
                              pin_memory=(args.device == "cuda"),
                              num_workers=args.num_workers,
                              sampler=None,
                              batch_size=args.valid_batch_size,
                              shuffle=False)
    return train_loader, valid_loader, embed_matrix


def train(model, text_optimizer, graph_optimizer, train_loader, args, gradient_accumulate_steps=16):
    """
    训练一轮模型
    """
    model.train()
    mse_results = []
    for idx, (token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
              scores) in tqdm(enumerate(train_loader)):
        loss, mse_result = model(token_ids.to(args.device), mask_ids.to(args.device), token_type_ids.to(args.device),
                                 node_type_ids.to(args.device), node_vectors.to(args.device),
                                 all_subgraph.to(args.device),
                                 all_adj_matrix.to(args.device), scores.to(args.device), is_training=True)
        loss.backward()
        mse_results.append(mse_result.detach().cpu().numpy())
        if (idx + 1) % gradient_accumulate_steps == 0:
            torch.nn.utils.clip_grad_norm_(model.parameters(), args.max_norm)
            text_optimizer.step()
            text_optimizer.zero_grad()
            graph_optimizer.step()
            graph_optimizer.zero_grad()
    mse_results = np.concatenate(mse_results, axis=0)
    print('train mse::: ', np.mean(mse_results))


def evaluate(model, val_loader, args):
    """
    评估模型
    """
    model.eval()
    mse_results = []
    with torch.no_grad():
        for idx, (token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
                  scores) in tqdm(enumerate(val_loader)):
            mse_result = model(token_ids.to(args.device), mask_ids.to(args.device), token_type_ids.to(args.device),
                               node_type_ids.to(args.device), node_vectors.to(args.device),
                               all_subgraph.to(args.device),
                               all_adj_matrix.to(args.device), scores.to(args.device))
            mse_results.append(mse_result.detach().cpu().numpy())
    mse_results = np.concatenate(mse_results, axis=0)
    print('eval mse::: ', np.mean(mse_results))


def main():
    parser = ArgumentParser()
    parser.add_argument("--train_path", type=str, default="/data/gaowh/data/g_sim_train.json",
                        help="Path of the train dataset for dist dataset. ")
    parser.add_argument("--valid_path", type=str, default="/data/gaowh/data/g_sim_valid.json",
                        help="train batch size.")
    parser.add_argument("--node_emb_path", type=str, default="/data/gaowh/data/node2vec.model",
                        help="node pretrain emb path.")
    parser.add_argument("--save_dir", type=str, default="/data/gaowh/data/ckpt_gcn_pure/",
                        help="model save dir.")
    parser.add_argument("--train_batch_size", type=int, default=16,
                        help="Path of the valid dataset for dist dataset. ")
    parser.add_argument("--valid_batch_size", type=int, default=8,
                        help="valid batch size.")
    parser.add_argument("--num_workers", type=int, default=4,
                        help="num workers.")
    parser.add_argument("--epochs", type=int, default=50,
                        help="num epoch.")
    parser.add_argument("--device", type=str, default='cuda',
                        help="device.")
    parser.add_argument("--bert_type", type=str, default="hfl/chinese-macbert-base",
                        help="pretrain language model.")
    parser.add_argument("--model_type", type=str, default="gcn",
                        help="model type.")
    parser.add_argument("--lr", type=float, default=1e-2,
                        help="learning rate.")
    parser.add_argument("--max_norm", type=float, default=0.3,
                        help="clip norm.")
    parser.add_argument("--dropout", type=float, default=0.1,
                        help="dropout rate.")
    parser.add_argument("--num_feature", type=int, default=11,
                        help="number of feature.")

    if not os.path.exists('/data/gaowh/data/ckpt_gcn_pure'):
        os.mkdir('/data/gaowh/data/ckpt_gcn_pure')

    args = parser.parse_args()
    tokenizer = AutoTokenizer.from_pretrained(args.bert_type)
    train_loader, val_loader, embed_matrix = build_dist_loaders(args, tokenizer)
    if args.model_type == 'gat':
        print('载入gat模型...')
        model = BertGAT_pure(args, embed_matrix)
    elif args.model_type == 'gcn':
        print('载入gcn模型...')
        model = BertGCN(args, embed_matrix)
    else:
        print('不存在的模型类型，退出训练')
        return
    print('-------随机初始--------')






    evaluate(model, val_loader, args)
    # if torch.cuda.device_count() > 1:
    #     model = torch.nn.DataParallel(model, device_ids=[0,1,2,3,4])

    text_optimizer, graph_optimizer = model.get_optimizer(args)
    for epoch_id in range(args.epochs):
        print('-------第' + str(epoch_id + 1) + '轮--------')
        train(model, text_optimizer, graph_optimizer, train_loader, args)
        evaluate(model, val_loader, args)
        torch.save(model.state_dict(), args.save_dir + 'g_sim_model_epoch' + str(epoch_id + 1) + '.pt')


if __name__ == "__main__":
    main()
