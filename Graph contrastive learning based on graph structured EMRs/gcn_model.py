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


class BertGCN(nn.Module):
    """
    模型主体
    """

    def __init__(self, args, embed_matrix):
        super().__init__()
        # ENCODE PART
        self.text_encoder = BertModel.from_pretrained(args.bert_type)
        self.node_encoder = NodeEncoder(args, embed_matrix, self.text_encoder.config.hidden_size)
        self.graph_encoder = GraphEncoder(args, self.text_encoder.config.hidden_size)
        self.loss_func = nn.MSELoss()
        self.to(args.device)

    def get_optimizer(self, args):
        text_encoder_optimizer = AdamW([{'params': self.text_encoder.parameters(), 'initial_lr': 5e-6}], lr=5e-6,
                                       correct_bias=True)
        optimizer = AdamW(
            [{'params': [i for i in self.graph_encoder.parameters()] + [i for i in self.node_encoder.parameters()],
              'initial_lr': args.lr}], lr=args.lr, correct_bias=True)

        return text_encoder_optimizer, optimizer

    def forward(self, token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
                scores, is_training=False, is_inference=False):
        node_text_vec = self.text_encoder(token_ids, attention_mask=mask_ids,
                                          token_type_ids=token_type_ids).last_hidden_state[:, 0, :]
        node_embeddings = self.node_encoder(node_text_vec, node_type_ids, node_vectors)
        graph_states = self.graph_encoder(all_subgraph, all_adj_matrix, node_embeddings=node_embeddings)
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


class NodeEncoder(nn.Module):
    """
    编码节点表示
    """

    def __init__(self, args, embed_matrix, hidden_size):
        super().__init__()

        # meta path
        embed_matrix = torch.tensor(embed_matrix, dtype=torch.float)
        pad_embed = torch.zeros_like(embed_matrix[:1, :], dtype=torch.float)
        embed_matrix = torch.concat([embed_matrix, pad_embed], dim=0)
        self.meta_embeddings = nn.Embedding.from_pretrained(embed_matrix, freeze=False)

        # feature
        # self.f_weight = torch.nn.parameter.Parameter(
        #     torch.empty((1, args.num_feature), **{'device': args.device, 'dtype': torch.float}), requires_grad=True)
        # self.f_bias = torch.nn.parameter.Parameter(
        #     torch.empty((1, args.num_feature), **{'device': args.device, 'dtype': torch.float}), requires_grad=True)

        self.LayerNorm = nn.LayerNorm(hidden_size)
        self.f_dense_1 = nn.Linear(11, hidden_size)

        # attn
        self.text_focus = nn.Linear(hidden_size, 1)
        self.node_type_focus = nn.Linear(hidden_size, 1)
        self.feature_focus = nn.Linear(hidden_size, 1)

        init_weights(self.f_dense_1)
        init_weights(self.LayerNorm)
        init_weights(self.text_focus)
        init_weights(self.node_type_focus)
        init_weights(self.feature_focus)

    def forward(self, node_text_vec, node_type_ids, feature_vec):
        # type emb
        meta_vec = self.meta_embeddings(node_type_ids)

        # feature
        # feature_vec = F.relu(self.f_weight * feature_vec + self.f_bias)
        # feature_vec = self.f_dense_1(feature_vec)
        # feature_vec = self.LayerNorm(feature_vec)

        # 注意力机制融合
        # attn = torch.softmax(torch.concat([self.text_focus(node_text_vec), self.node_type_focus(meta_vec)], dim=-1),
        #                      dim=-1)
        # node_embeddings = node_text_vec * attn[:, :1] + meta_vec * attn[:, 1:]




        # pure
        attn = torch.softmax(torch.concat([self.text_focus(meta_vec), self.node_type_focus(meta_vec)], dim=-1),
                             dim=-1)
        node_embeddings = meta_vec * attn[:, :1] + meta_vec * attn[:, 1:]



        return node_embeddings


class GCNLayer(nn.Module):
    """
    图卷积层
    """

    def __init__(self, args, in_channels, out_channels):
        super().__init__()
        self.dropout = args.dropout
        self.dense = nn.Linear(in_channels, out_channels)
        self.LayerNorm = nn.LayerNorm(out_channels)
        self.dropout = nn.Dropout(args.dropout)
        init_weights(self.dense)
        init_weights(self.LayerNorm)

    def forward(self, x, adj):
        x = self.dense(x)
        x = torch.einsum('btq,bqd->btd', adj, x)
        x = self.LayerNorm(x)
        x = F.relu(x)
        x = self.dropout(x)
        return x


class GraphEncoder(nn.Module):
    """
    编码病例图表示
    """

    def __init__(self, args, hidden_size, num_layers=3):
        super().__init__()
        self.convs = torch.nn.ModuleList()
        self.pad_emb = torch.zeros([1, hidden_size], dtype=torch.float).to(args.device)
        for _ in range(num_layers):
            self.convs.append(GCNLayer(args, hidden_size, hidden_size))
        self.dropout = args.dropout
        self.dense = nn.Linear(hidden_size, hidden_size)
        init_weights(self.dense)

    def forward(self, all_subgraph, all_adj_matrix, node_embeddings):
        node_embeddings = torch.cat([node_embeddings, self.pad_emb], dim=0)
        x = F.embedding(all_subgraph, node_embeddings)
        # 邻接权重矩阵
        adj = all_adj_matrix / torch.sum(all_adj_matrix, dim=-1, keepdim=True)
        for i, conv in enumerate(self.convs):
            x = conv(x, adj)
        g_states = x[:, 0, :]
        g_states = self.dense(g_states)
        return g_states
