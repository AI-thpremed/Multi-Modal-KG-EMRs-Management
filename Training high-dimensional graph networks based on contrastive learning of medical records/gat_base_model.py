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


class BertGAT(nn.Module):
    """
    模型主体
    """

    def __init__(self, args, embed_matrix):
        super().__init__()


        
        # ENCODE PART
        self.graph_encoder = GraphEncoder(args, 768, embed_matrix)



        self.loss_func = nn.MSELoss()
        self.to(args.device)

    def get_optimizer(self, args):
        graph_optimizer = AdamW([{'params': self.parameters(), 'initial_lr': 1e-4}], lr=1e-4,
                                correct_bias=True)
        return graph_optimizer

    def forward(self, token_ids, mask_ids, token_type_ids, node_type_ids, node_vectors, all_subgraph, all_adj_matrix,
                scores, is_training=False, is_inference=False):
        graph_states = self.graph_encoder(all_subgraph, node_type_ids, all_adj_matrix)
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


class GraphEncoder(nn.Module):
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



        self.pad_emb = torch.zeros([1, hidden_size], dtype=torch.float).to(args.device)



        self.f_dense = nn.Linear(hidden_size, hidden_size, bias=False)
        init_weights(self.f_dense)

        self.text_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.text_dense)

        self.meta_dense = nn.Linear(hidden_size, 1, bias=True)
        init_weights(self.meta_dense)

    def forward(self, all_subgraph, node_type_ids, all_adj_matrix):
        node_embeddings = self.meta_embeddings(node_type_ids)
        # batch X 2, time, time
        all_adj_matrix = all_adj_matrix[:, None, :, :].float()
        extended_attention_mask = (1.0 - all_adj_matrix) * -10000.0

        # 节点向量化
        node_embeddings = torch.cat([node_embeddings, self.pad_emb], dim=0)
        x = F.embedding(all_subgraph, node_embeddings)

        graph_state = self.graph_encoder.encoder(x, attention_mask=extended_attention_mask).last_hidden_state[:, 0, :]
        graph_state = self.f_dense(graph_state)
        return graph_state
