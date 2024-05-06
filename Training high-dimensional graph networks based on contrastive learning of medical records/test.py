from pymilvus import (
    connections,
    FieldSchema,
    CollectionSchema,
    utility,
    DataType,
    Collection,
)
from argparse import ArgumentParser
import torch
from gat_base_model import BertGAT
from gcn_base_model import BertGCN
from gensim.models import Word2Vec
from transformers import AutoTokenizer
from utils.dataset_graph import PatientGraphDataset
import time
import os
from utils.post_process import *

parser = ArgumentParser()
parser.add_argument("--node_emb_path", type=str, default="data/node2vec.model",
                    help="node pretrain emb path.")

parser.add_argument("--sim_model_path", type=str, default="",
                    help="sim model path.")
parser.add_argument("--device", type=str, default='cuda',
                    help="device.")

parser.add_argument("--port", type=int, default=12089,
                    help="port.")
parser.add_argument("--bert_type", type=str, default="hfl/chinese-macbert-base",
                    help="pretrain language model.")
parser.add_argument("--model_type", type=str, default="gcn",
                    help="model type.")
parser.add_argument("--dropout", type=float, default=0.1,
                    help="dropout rate.")
args = parser.parse_args()

connections.connect("default", host="192.168.2.240", port="19530")
if utility.has_collection("patient_graphs_" + args.model_type):
    print(1)
else:
    print(2)
