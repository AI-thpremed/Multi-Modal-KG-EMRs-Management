# Multi-Modal-KG-EMRs-Management
This code provides a conversion from structured medical records to Neo4J graph-structured medical records.  Additionally, it offers basic functionality for assisted diagnosis through contrastive learning and self-supervision based on graph-structured EMRs, specifically for recommending similar medical records.


# EMR Graph Processing and Analysis

## Step 1: EMR Data Transformation

This C# based solution reads data from multiple tables in MySQL and transforms the information into a graph structure within Neo4j. The primary table for patient medical records is 'case_info', which is linked to all relevant forms via the 'CASE_ID'. In practical application, this can be converted into a microservice or a scheduled job.

## Step 2: Graph Contrastive Learning with CGAT-ADM Model

### Environment
- gensim==4.2.0
- torch==1.10.1
- transformers==4.18.0
- sanic==21.9.3
- neo4j==5.2.0

### Steps
1. Retrieve all case graphs from Neo4j and form JSON data. Run neo_search.py under utils.
2. Generate metapath data and train metapath2vec. Run build_metapath.py under utils, then run train_meta_path.py.
3. Generate similarity matching data for case graphs and train the model. Run build_graph_dataset.py under utils, then run train.py.
4. Deploy the service; start by deploying the Milvus vector retrieval engine.

## Step 3: Training High-Dimensional Graph Networks

Once training in Step 2 is completed, use the following code segment to perform recall validation on the test set. This project serves as an algorithm evaluation module for recommending similar medical cases.

### Environment
- gensim==4.2.0
- torch==1.10.1
- transformers==4.18.0
- sanic==21.9.3
- neo4j==5.2.0

### File Structure
- data: Related data
- utils: Toolset mainly containing modules for data preprocessing, feature construction, data loading, post-processing, etc.
- csm.py: Code covering CSM model computation and evaluation
- evaluate.py: Evaluation code for GAT and GCN
- gat_base_model.py: GAT model structure code
- gcn_base_model.py: GCN model structure code
- graph_milvus_server.py: Once GAT and GCN are trained, this code can be called for deployment, requiring definition of sim_model_path and model_type
- ours_evaluate.py: Evaluation function for our multimodal model, including different data (ours_eval.json), which includes medical imaging information
- pca.py: Code covering PCA model computation and evaluation
- train.py: Training code for GAT and GCN

### Running Instructions
The eval.json file in the data directory is not involved in any training, solely used for model evaluation. If necessary, it can be replaced with another eval.json for evaluation dataset replacement.

#### CSM and PCA Evaluation
Simply run csm.py and pca.py to obtain relevant evaluation metrics.

#### GCN, GAT, GNN Evaluation
##### Direct Evaluation (Pre-trained GCN, GAT, GNN)

1. Within evaluate.py, GCN_URL and GAT_URL are provided, which can be assigned to URL to explicitly call the desired model.
2. Run evaluate.py to obtain relevant evaluation metrics.

##### Full Process

1. Start by running train.py, where the model type can be specified using model_type.
2. After training, locate the model file address with the minimum validation loss in data/ckpt, which corresponds to sim_model_path in graph_milvus_server.py.
3. Run graph_milvus_server.py, specifying model_type and sim_model_path.
4. Modify the URL in evaluate.py to match the address in graph_milvus_server.py.
5. Run evaluate.py to obtain relevant evaluation metrics.

#### Additional Evaluation for Our Multimodal Model
Run ours_evaluate.py to obtain relevant evaluation metrics.

Special attention should be paid that contrastive learning and metapath2vec must comply with the scheme design of the graph, which must meet business requirements.



