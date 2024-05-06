import torch.nn as nn
import torch.nn.functional as F
import torch
from transformers import AdamW
from transformers import BertModel


def init_weights(module):
    """
    初始化权重
    """
    if isinstance(module, nn.Linear):
        module.weight.data.normal_(mean=0.0, std=1)
        if module.bias is not None:
            module.bias.data.zero_()
    elif isinstance(module, nn.Embedding):
        module.weight.data.normal_(mean=0.0, std=1)
        if module.padding_idx is not None:
            module.weight.data[module.padding_idx].zero_()
    elif isinstance(module, nn.LayerNorm):
        module.bias.data.zero_()
        module.weight.data.fill_(1.0)

# 定义通道注意力模块
class ChannelAttention(nn.Module):
    def __init__(self, in_channels):
        super(ChannelAttention, self).__init__()
        self.fc = nn.Sequential(
            nn.Linear(in_channels, in_channels // 2),
            nn.ReLU(inplace=True),
            nn.Linear(in_channels // 2, in_channels)
        )
    
    def forward(self, x):
        channel_att = torch.sigmoid(self.fc(x))
        return channel_att


class BertGAT_org(nn.Module):
    """
    模型主体
    """

    def __init__(self, args, embed_matrix):
        super().__init__()
        # ENCODE PART
        self.text_encoder = BertModel.from_pretrained(args.bert_type)

        #bertgat就是先bert用一些文字数值参数，然后一个gat层。这个gat完全没有卷机，就是graph跑。


        #这个hiddensize就是768
        self.graph_encoder = GraphEncoder_org(args, self.text_encoder.config.hidden_size, embed_matrix)
        self.loss_func = nn.MSELoss()
        self.to(args.device)

    def get_optimizer(self, args):
        text_optimizer = AdamW([{'params': self.text_encoder.parameters(), 'initial_lr': 5e-6}], lr=5e-6,
                               correct_bias=True)
        graph_optimizer = AdamW([{'params': self.graph_encoder.parameters(), 'initial_lr': 5e-5}], lr=5e-5,
                                correct_bias=True)
        
        return text_optimizer, graph_optimizer

    def forward(self, token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
                scores, is_training=False, is_inference=False):

        #文字编码用一些变量        
        node_text_vec = self.text_encoder(token_ids, attention_mask=mask_ids,
                                          token_type_ids=token_type_ids).last_hidden_state[:, 0, :]
        
        #graph 编码用文字结果和一些其他
        graph_states = self.graph_encoder(all_subgraph, node_text_vec, node_type_ids, node_vectors[:, -1])
        if is_inference:
            return graph_states
        l_graph_states = graph_states[0::2, :]
        r_graph_states = graph_states[1::2, :]
        preds = torch.cosine_similarity(l_graph_states, r_graph_states, dim=1)

        if is_training:
            loss = self.loss_func(preds, scores)
            return loss, (preds - scores) ** 2
        else:
            return (preds - scores) ** 2


class GraphEncoder_org(nn.Module):
    """
    编码病例图表示
    """

    def __init__(self, args, hidden_size, embed_matrix):
        super().__init__()
        # meta path
        embed_matrix = torch.tensor(embed_matrix, dtype=torch.float)
        pad_embed = torch.zeros_like(embed_matrix[:1, :], dtype=torch.float)
        embed_matrix = torch.concat([embed_matrix, pad_embed], dim=0)
        self.meta_embeddings = nn.Embedding.from_pretrained(embed_matrix, freeze=False)

        self.graph_encoder = BertModel.from_pretrained(args.bert_type)
        self.graph_encoder.encoder.layer = self.graph_encoder.encoder.layer[-3:]
        #gat有三层全部用的bert最后三层复制过来参数


        self.pad_emb = torch.zeros([1, hidden_size], dtype=torch.float).to(args.device)


        #容老师 改768为2 出二维散点图  
        self.f_dense = nn.Linear(hidden_size, 768, bias=False)
        init_weights(self.f_dense)

        self.text_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.text_dense)

        self.meta_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.meta_dense)


    def forward(self, all_subgraph, node_text_vec, node_type_ids, node_gate):
        meta_vec = self.meta_embeddings(node_type_ids)



        #联合编码
        # attn = torch.softmax(torch.concat([self.meta_dense(meta_vec), self.text_dense(node_text_vec)], dim=-1), dim=-1)


        # node_embeddings = meta_vec * attn[:, :1] + node_text_vec * attn[:, 1:]



        #单独bert编码
        attn = torch.softmax(torch.concat([self.meta_dense(node_text_vec), self.text_dense(node_text_vec)], dim=-1), dim=-1)
        node_embeddings = node_text_vec * attn[:, :1] + node_text_vec * attn[:, 1:]




        #单独编码metapath
        # attn = torch.softmax(torch.concat([self.meta_dense(meta_vec), self.meta_dense(meta_vec)], dim=-1), dim=-1)
        # node_embeddings = node_text_vec






        # node_embeddings = node_embeddings * node_gate[:, None]
        # 可见矩阵构建
        mask_ids = (all_subgraph != node_embeddings.shape[0]).long()
        extended_attention_mask = mask_ids[:, None, None, :]
        extended_attention_mask = extended_attention_mask.to(dtype=torch.float)
        extended_attention_mask = (1.0 - extended_attention_mask) * -10000.0

        # batch X 2, time, time
        # all_adj_matrix = all_adj_matrix[:, None, :, :].float()
        # extended_attention_mask = (1.0 - all_adj_matrix) * -10000.0

        # 节点向量化
        node_embeddings = torch.cat([node_embeddings, self.pad_emb], dim=0)
        x = F.embedding(all_subgraph, node_embeddings)

        graph_state = self.graph_encoder.encoder(x, attention_mask=extended_attention_mask).last_hidden_state[:, 0, :]
        graph_state = self.f_dense(graph_state)
        return graph_state



class GraphEncoder(nn.Module):
    """
    编码病例图表示
    """

    def __init__(self, args, hidden_size, embed_matrix):
        super().__init__()
        # meta path
        embed_matrix = torch.tensor(embed_matrix, dtype=torch.float)
        pad_embed = torch.zeros_like(embed_matrix[:1, :], dtype=torch.float)
        embed_matrix = torch.cat([embed_matrix, pad_embed], dim=0)
        self.meta_embeddings = nn.Embedding.from_pretrained(embed_matrix, freeze=False)

        self.graph_encoder = BertModel.from_pretrained(args.bert_type)
        self.graph_encoder.encoder.layer = self.graph_encoder.encoder.layer[-3:]

        self.pad_emb = torch.zeros([1, hidden_size], dtype=torch.float).to(args.device)

        self.f_dense = nn.Linear(hidden_size, 768, bias=False)
        init_weights(self.f_dense)

        self.text_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.text_dense)

        self.meta_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.meta_dense)

        self.channel_attention = nn.Linear(hidden_size, 1)
        init_weights(self.channel_attention)

    def forward(self, all_subgraph, node_text_vec, node_type_ids, node_gate):
        meta_vec = self.meta_embeddings(node_type_ids)

        channel_weights = torch.sigmoid(self.channel_attention(meta_vec + node_text_vec))
        channel_weights = torch.softmax(channel_weights, dim=1)

        weighted_meta_vec = torch.mul(meta_vec, channel_weights)
        weighted_node_text_vec = torch.mul(node_text_vec, channel_weights)

        node_embeddings = torch.cat([weighted_meta_vec, weighted_node_text_vec], dim=1)

        mask_ids = (all_subgraph != node_embeddings.shape[0]).long()
        extended_attention_mask = mask_ids[:, None, None, :]
        extended_attention_mask = extended_attention_mask.to(dtype=torch.float)
        extended_attention_mask = (1.0 - extended_attention_mask) * -10000.0

        node_embeddings = torch.cat([node_embeddings, self.pad_emb], dim=0)
        x = F.embedding(all_subgraph, node_embeddings)

        graph_state = self.graph_encoder.encoder(x, attention_mask=extended_attention_mask).last_hidden_state[:, 0, :]
        graph_state = self.f_dense(graph_state)
        return graph_state


#下面是最简编码

class BertGAT_pure(nn.Module):
    """
    模型主体
    """

    def __init__(self, args, embed_matrix):
        super().__init__()
        # ENCODE PART
        self.text_encoder = BertModel.from_pretrained(args.bert_type)

        #bertgat就是先bert用一些文字数值参数，然后一个gat层。这个gat完全没有卷机，就是graph跑。


        #这个hiddensize就是768
        self.graph_encoder = GraphEncoder_pure(args, self.text_encoder.config.hidden_size, embed_matrix)
        self.loss_func = nn.MSELoss()
        self.to(args.device)

    def get_optimizer(self, args):
        text_optimizer = AdamW([{'params': self.text_encoder.parameters(), 'initial_lr': 5e-6}], lr=5e-6,
                               correct_bias=True)
        graph_optimizer = AdamW([{'params': self.graph_encoder.parameters(), 'initial_lr': 5e-5}], lr=5e-5,
                                correct_bias=True)
        return text_optimizer, graph_optimizer

    def forward(self, token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
                scores, is_training=False, is_inference=False):

        #文字编码用一些变量        
        node_text_vec = self.text_encoder(token_ids, attention_mask=mask_ids,
                                          token_type_ids=token_type_ids).last_hidden_state[:, 0, :]
        
        #graph 编码用文字结果和一些其他
        graph_states = self.graph_encoder(all_subgraph, node_text_vec, node_type_ids, node_vectors[:, -1])
        if is_inference:
            return graph_states
        l_graph_states = graph_states[0::2, :]
        r_graph_states = graph_states[1::2, :]
        preds = torch.cosine_similarity(l_graph_states, r_graph_states, dim=1)

        if is_training:
            loss = self.loss_func(preds, scores)
            return loss, (preds - scores) ** 2
        else:
            return (preds - scores) ** 2


class GraphEncoder_pure(nn.Module):
    """
    编码病例图表示
    """

    def __init__(self, args, hidden_size, embed_matrix):
        super().__init__()
        # meta path
        embed_matrix = torch.tensor(embed_matrix, dtype=torch.float)
        pad_embed = torch.zeros_like(embed_matrix[:1, :], dtype=torch.float)
        embed_matrix = torch.concat([embed_matrix, pad_embed], dim=0)
        self.meta_embeddings = nn.Embedding.from_pretrained(embed_matrix, freeze=False)

        self.graph_encoder = BertModel.from_pretrained(args.bert_type)
        self.graph_encoder.encoder.layer = self.graph_encoder.encoder.layer[-3:]
        #gat有三层全部用的bert最后三层复制过来参数


        self.pad_emb = torch.zeros([1, hidden_size], dtype=torch.float).to(args.device)


        #容老师 改768为2 出二维散点图  
        self.f_dense = nn.Linear(hidden_size, 768, bias=False)
        init_weights(self.f_dense)

        self.text_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.text_dense)

        self.meta_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.meta_dense)

    def forward(self, all_subgraph, node_text_vec, node_type_ids, node_gate):
        meta_vec = self.meta_embeddings(node_type_ids)



        # #联合编码
        # attn = torch.softmax(torch.concat([self.meta_dense(meta_vec), self.text_dense(node_text_vec)], dim=-1), dim=-1)

        # node_embeddings = meta_vec * attn[:, :1] + node_text_vec * attn[:, 1:]



        #最简编码
        attn = torch.softmax(torch.concat([self.meta_dense(meta_vec), self.text_dense(meta_vec)], dim=-1), dim=-1)

        node_embeddings = meta_vec * attn[:, :1] + meta_vec * attn[:, 1:]


        # attn = torch.softmax(torch.concat([self.meta_dense(meta_vec), self.meta_dense(meta_vec)], dim=-1), dim=-1)

        # node_embeddings = node_text_vec

        # node_embeddings = node_embeddings * node_gate[:, None]
        # 可见矩阵构建
        mask_ids = (all_subgraph != node_embeddings.shape[0]).long()
        extended_attention_mask = mask_ids[:, None, None, :]
        extended_attention_mask = extended_attention_mask.to(dtype=torch.float)
        extended_attention_mask = (1.0 - extended_attention_mask) * -10000.0

        # batch X 2, time, time
        # all_adj_matrix = all_adj_matrix[:, None, :, :].float()
        # extended_attention_mask = (1.0 - all_adj_matrix) * -10000.0

        # 节点向量化
        node_embeddings = torch.cat([node_embeddings, self.pad_emb], dim=0)
        x = F.embedding(all_subgraph, node_embeddings)

        graph_state = self.graph_encoder.encoder(x, attention_mask=extended_attention_mask).last_hidden_state[:, 0, :]
        graph_state = self.f_dense(graph_state)
        return graph_state





# class BertGAT(nn.Module):
#     """
#     模型主体
#     """

#     def __init__(self, args, embed_matrix):
#         super().__init__()
#         # ENCODE PART
#         self.text_encoder = BertModel.from_pretrained(args.bert_type)

#         #bertgat就是先bert用一些文字数值参数，然后一个gat层。这个gat完全没有卷机，就是graph跑。


#         #这个hiddensize就是768
#         self.graph_encoder = GraphEncoder(args, self.text_encoder.config.hidden_size, embed_matrix)
#         self.loss_func = nn.MSELoss()
#         self.to(args.device)

#     def get_optimizer(self, args):
#         text_optimizer = AdamW([{'params': self.text_encoder.parameters(), 'initial_lr': 5e-6}], lr=5e-6,
#                                correct_bias=True)
#         graph_optimizer = AdamW([{'params': self.graph_encoder.parameters(), 'initial_lr': 5e-5}], lr=5e-5,
#                                 correct_bias=True)
#         return text_optimizer, graph_optimizer

#     def forward(self, token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
#                 scores, is_training=False, is_inference=False):

#         #文字编码用一些变量        
#         node_text_vec = self.text_encoder(token_ids, attention_mask=mask_ids,
#                                           token_type_ids=token_type_ids).last_hidden_state[:, 0, :]
        
#         #graph 编码用文字结果和一些其他
#         graph_states = self.graph_encoder(all_subgraph, node_text_vec, node_type_ids, node_vectors[:, -1])
#         if is_inference:
#             return graph_states
#         l_graph_states = graph_states[0::2, :]
#         r_graph_states = graph_states[1::2, :]
#         preds = torch.cosine_similarity(l_graph_states, r_graph_states, dim=1)

#         if is_training:
#             loss = self.loss_func(preds, scores)
#             return loss, (preds - scores) ** 2
#         else:
#             return (preds - scores) ** 2

# class GraphEncoder(nn.Module):
#     """
#     编码病例图表示
#     """

#     def __init__(self, args, hidden_size, embed_matrix):
#         super().__init__()
#         # meta path
#         embed_matrix = torch.tensor(embed_matrix, dtype=torch.float)
#         pad_embed = torch.zeros_like(embed_matrix[:1, :], dtype=torch.float)
#         embed_matrix = torch.concat([embed_matrix, pad_embed], dim=0)
#         self.meta_embeddings = nn.Embedding.from_pretrained(embed_matrix, freeze=False)

#         self.graph_encoder = BertModel.from_pretrained(args.bert_type)
#         self.graph_encoder.encoder.layer = self.graph_encoder.encoder.layer[-3:]
#         #gat有三层全部用的bert最后三层复制过来参数


#         self.pad_emb = torch.zeros([1, hidden_size], dtype=torch.float).to(args.device)


#         #容老师 改768为2 出二维散点图  
#         self.f_dense = nn.Linear(hidden_size, 2, bias=False)
#         init_weights(self.f_dense)

#         self.text_dense = nn.Linear(hidden_size, 1, bias=True)
#         init_weights(self.text_dense)

#         self.meta_dense = nn.Linear(hidden_size, 1, bias=True)
#         init_weights(self.meta_dense)

#     def forward(self, all_subgraph, node_text_vec, node_type_ids, node_gate):
#         meta_vec = self.meta_embeddings(node_type_ids)
#         # attn = torch.softmax(torch.concat([self.meta_dense(meta_vec), self.text_dense(node_text_vec)], dim=-1), dim=-1)
#         attn = torch.softmax(torch.concat([self.meta_dense(meta_vec), self.meta_dense(meta_vec)], dim=-1), dim=-1)



#         node_embeddings = meta_vec * attn[:, :1] + meta_vec * attn[:, 1:]
#         node_embeddings = node_embeddings * node_gate[:, None]
#         # 可见矩阵构建
#         mask_ids = (all_subgraph != node_embeddings.shape[0]).long()
#         extended_attention_mask = mask_ids[:, None, None, :]
#         extended_attention_mask = extended_attention_mask.to(dtype=torch.float)
#         extended_attention_mask = (1.0 - extended_attention_mask) * -10000.0

#         # batch X 2, time, time
#         # all_adj_matrix = all_adj_matrix[:, None, :, :].float()
#         # extended_attention_mask = (1.0 - all_adj_matrix) * -10000.0

#         # 节点向量化
#         node_embeddings = torch.cat([node_embeddings, self.pad_emb], dim=0)
#         x = F.embedding(all_subgraph, node_embeddings)

#         graph_state = self.graph_encoder.encoder(x, attention_mask=extended_attention_mask).last_hidden_state[:, 0, :]
#         graph_state = self.f_dense(graph_state)
#         return graph_state
