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

        # meta path
        # embed_matrix = torch.tensor(embed_matrix, dtype=torch.float)
        # pad_embed = torch.zeros_like(embed_matrix[:1, :], dtype=torch.float)
        # embed_matrix = torch.concat([embed_matrix, pad_embed], dim=0)
        self.meta_embeddings = nn.Embedding(500, 768)
        self.graph_encoder = GraphEncoder(args, 768)
        self.loss_func = nn.MSELoss()
        self.to(args.device)

    def get_optimizer(self, args):
        optimizer = AdamW(
            [{'params': self.parameters(),
              'initial_lr': args.lr}], lr=args.lr, correct_bias=True)

        return optimizer

    def forward(self, token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
                scores, is_training=False, is_inference=False):
        graph_states = self.graph_encoder(all_subgraph, all_adj_matrix,
                                          node_embeddings=self.meta_embeddings(node_type_ids))
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
