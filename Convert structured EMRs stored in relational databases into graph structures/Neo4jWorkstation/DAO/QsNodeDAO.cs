using CC.Admin.Models;
using Neo4j.Driver;
using Neo4jSocial.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace CC.Admin.DAO
{
    public class QsNodeDAO : BaseDAO
    {




        #region   全身病史












        public async Task AddDia(DiaNode diarecord)
        {
            var query = "CREATE (a:DiaNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt})";
            await WriteAsync(query, new
            {
                diarecord.DisplayName,
                diarecord.CaseId,
                //diarecord.FistDia,
                diarecord.RmkQt
            });

        }






        //创建病历到病史的连接和病史到父节点的连接
        public async Task AddDiaLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:DiaNode{CaseId:" + caseId + "}),(n:HistoryBody{name:'糖尿病'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n);";

            

            await WriteAsync(query);

        }


























        public async Task AddGxy(GxyNode gxyrecord)
        {
            var query = "CREATE (a:GxyNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt})";
            await WriteAsync(query, new
            {
                gxyrecord.DisplayName,
                gxyrecord.CaseId,
                gxyrecord.RmkQt
            });

        }



        public async Task AddGxyLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:GxyNode{CaseId:" + caseId + "}),(n:HistoryBody{name:'高血压'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            await WriteAsync(query);

        }













        public async Task AddGxz(GxzNode gxzrecord)
        {
            var query = "CREATE (a:GxzNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt})";
            await WriteAsync(query, new
            {
                gxzrecord.DisplayName,
                gxzrecord.CaseId,
                gxzrecord.RmkQt
            });

        }







        public async Task AddGxzLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:GxzNode{CaseId:" + caseId + "}),(n:HistoryBody{name:'高血脂'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            await WriteAsync(query);

        }









        public async Task AddNgs(NgsNode ngsrecord)
        {
            var query = "CREATE (a:NgsNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt})";
            await WriteAsync(query, new
            {
                ngsrecord.DisplayName,
                ngsrecord.CaseId,
                ngsrecord.RmkQt
            });

        }




        public async Task AddNgsLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:NgsNode{CaseId:" + caseId + "}),(n:HistoryBody{name:'脑梗塞'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            await WriteAsync(query);

        }












         














        public async Task AddGxb(GxbNode gxbrecord)
        {
            var query = "CREATE (a:GxbNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt})";
            await WriteAsync(query, new
            {
                gxbrecord.DisplayName,
                gxbrecord.CaseId,
                gxbrecord.RmkQt
            });

        }



        public async Task AddGxbLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:GxbNode{CaseId:" + caseId + "}),(n:HistoryBody{name:'冠心病'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            await WriteAsync(query);

        }





















        public async Task AddQsMianyi(QsMianyiNode qsmianyirecord)
        {
            var query = "CREATE (a:QsMianyiNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt})";
            await WriteAsync(query, new
            {
                qsmianyirecord.DisplayName,
                qsmianyirecord.CaseId,
                qsmianyirecord.RmkQt
            });

        }


        public async Task AddQsMianyiLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:QsMianyiNode{CaseId:" + caseId + "}),(n:HistoryBody{name:'全身免疫相关疾病'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            await WriteAsync(query);

        }





















        public async Task AddQsOther(QsOtherNode qsotherrecord)
        {
            var query = "CREATE (a:QsOtherNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt})";
            await WriteAsync(query, new
            {
                qsotherrecord.DisplayName,
                qsotherrecord.CaseId,
                qsotherrecord.RmkQt
            });

        }



        public async Task AddQsOtherLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:QsOtherNode{CaseId:" + caseId + "}),(n:HistoryBody{name:'其他'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            await WriteAsync(query);

        }















        #endregion







        #region   眼科病史







        public async Task AddHisCataract(HisCataractNode hiscataract)
        {
            var query = "CREATE (a:HisCataractNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt,Lr:$Lr})";
            await WriteAsync(query, new
            {
                hiscataract.DisplayName,
                hiscataract.CaseId,
                hiscataract.RmkQt,
                hiscataract.Lr
            });

        }




        public async Task AddHisCataractLink(long caseId,string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ",DisplayName:'"+lr+"'}),(m:HisCataractNode{CaseId:" + caseId + ",Lr:'"+lr+"'}),(n:HistoryEye{name:'白内障'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            await WriteAsync(query);

        }













        public async Task AddHisGlaucoma(HisGlaucomaNode hisglaucoma)
        {
            var query = "CREATE (a:HisGlaucomaNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt,Lr:$Lr})";
            await WriteAsync(query, new
            {
                hisglaucoma.DisplayName,
                hisglaucoma.CaseId,
                //hisglaucoma.FistDia,
                hisglaucoma.RmkQt,
                hisglaucoma.Lr
            });

        }




        public async Task AddHisGlaucomaLink(long caseId,string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'"+lr+"'}),(m:HisGlaucomaNode{CaseId:" + caseId + ", Lr:'"+lr+"'}),(n:HistoryEye{name:'青光眼'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            await WriteAsync(query);

        }










        public async Task AddHisDiaRetin(HisDiaRetinNode hisdiaretin)
        {
            var query = "CREATE (a:HisDiaRetinNode{DisplayName:$DisplayName,CaseId:$CaseId,Name:$Name,Lr:$Lr})";
            await WriteAsync(query, new
            {

                hisdiaretin.DisplayName,

                hisdiaretin.CaseId,
                //hisdiaretin.FistDia,
                hisdiaretin.Name,

                hisdiaretin.Lr
            });

        }





        public async Task AddHisDiaRetinLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ",DisplayName:'"+lr+"'}),(m:HisDiaRetinNode{CaseId:" + caseId + ", Lr:'"+lr+"'}),(n:HistoryEye{name:'糖尿病视网膜病变'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            await WriteAsync(query);

        }









        public async Task AddHisBlt(HisBltNode hisblt)
        {
            var query = "CREATE (a:HisBltNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt,Lr:$Lr,Name:$Name})";
            await WriteAsync(query, new
            {

                hisblt.DisplayName,
                hisblt.CaseId,
                //hisblt.FistDia,
                hisblt.RmkQt,
                hisblt.Lr,
                hisblt.Name

            });

        }



        public async Task AddHisBltLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'"+lr+"'}),(m:HisBltNode{CaseId:" + caseId + ", Lr:'"+lr+"'}),(n:HistoryEye{name:'其他玻璃体视网膜疾病'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            await WriteAsync(query);

        }





        public async Task AddHisEyeWss(HisEyeWssNode hiseyewss)
        {
            var query = "CREATE (a:HisEyeWssNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt,Lr:$Lr})";
            await WriteAsync(query, new
            {
                hiseyewss.DisplayName,
                hiseyewss.CaseId,
                //hiseyewss.FistDia,
                hiseyewss.RmkQt,
                hiseyewss.Lr
            });

        }



        public async Task AddHisEyeWssLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'"+lr+"'}),(m:HisEyeWssNode{CaseId:" + caseId + ", Lr:'" + lr + "'  }),(n:HistoryEye{name:'外伤史'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            await WriteAsync(query);

        }



        public async Task AddHisEyeOther(HisEyeOtherNode hiseyeother)
        {
            var query = "CREATE (a:HisEyeOtherNode{DisplayName:$DisplayName,CaseId:$CaseId,RmkQt:$RmkQt,Lr:$Lr})";
            await WriteAsync(query, new
            {
                hiseyeother.DisplayName,
                hiseyeother.CaseId,
                hiseyeother.RmkQt,
                hiseyeother.Lr
            });

        }



        public async Task AddHisEyeOtherLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'" + lr + "'}),(m:HisEyeOtherNode{CaseId:" + caseId + ", Lr:'" + lr + "' }),(n:HistoryEye{name:'其他'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            await WriteAsync(query);

        }


        #endregion








        #region 医学影像节点



        public async Task AddFundus(IList<FundusNode> fundus)
        {
            //var query = "CREATE (a:FundusNode{CaseId:$CaseId,Lr:$Lr,Path:$Path})";

            var query = "CREATE";

            for (int i = 0; i < fundus.Count; i++)
            {
                query += "(a" + i + ":FundusNode{DisplayName:'"+ fundus[i].DisplayName+ "',CaseId:" + fundus[i].CaseId + ",Lr:'" + fundus[i].Lr + "',Path:'" + fundus[i].Path + "'}),";

            }
            query = query.Substring(0, query.Length - 1);

            await WriteAsync(query);

          
        }






        //创建病历到病史的连接和病史到父节点的连接
        public async Task AddFundusLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:FundusNode{CaseId:" + caseId + "}) CREATE (a)-[f:眼底图像]->(m)";
            await WriteAsync(query);
        }









        public async Task AddOct(OCTNode oct)
        {

            var query = "CREATE(a:OCTNode{DisplayName:'" + oct.DisplayName + "',CaseId:" + oct.CaseId + ",Lr:'" + oct.Lr + "',Path:'" + oct.Path + "'})";


            await WriteAsync(query);


        }






        //创建病历到病史的连接和病史到父节点的连接
        public async Task AddOctLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:OCTNode{CaseId:" + caseId + "}) CREATE (a)-[f:OCT图像]->(m)";
            await WriteAsync(query);
        }











        #endregion
















    }
}