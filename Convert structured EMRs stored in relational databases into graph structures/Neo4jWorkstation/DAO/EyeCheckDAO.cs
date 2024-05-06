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
    public class EyeCheckDAO : BaseDAO
    {







        public async Task AddEye(EyeNode lefteye, EyeNode righteye)
        {
            var query = "CREATE (a:EyeNode{DisplayName:'"+ lefteye.DisplayName+ "',CaseId:"+lefteye.CaseId+"}),(b:EyeNode{DisplayName:'"+righteye.DisplayName+"',CaseId:"+righteye.CaseId+"})";
            await WriteAsync(query);

        }



        public async Task AddEyeLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:EyeNode{CaseId:" + caseId + "})CREATE (a)-[f:属于]->(m)";


            await WriteAsync(query);

        }




        #region  创建视力检查和眼压检查的节点




        //public string DisplayName { get; set; }


        //public long CaseId { get; set; }


        //public string Lr { get; set; }


        //public string Jiaozheng { get; set; }
        //public string Luoyan { get; set; }

        //public string XianranDS { get; set; }

        //public string XianranZJDS { get; set; }

        //public string XianranZX { get; set; }

        //public string XianranSL { get; set; }






        public async Task AddSL(SLCheckNode slcheck)
        {
            var query = "CREATE (a:SLCheckNode{DisplayName:$DisplayName,CaseId:$CaseId,Lr:$Lr,Jiaozheng:$Jiaozheng," +
                "Luoyan:$Luoyan,XianranDS:$XianranDS,XianranZJDS:$XianranZJDS,XianranZX:$XianranZX,XianranSL:$XianranSL})";
            await WriteAsync(query, new
            {
                slcheck.DisplayName,
                slcheck.CaseId,
                slcheck.Lr,
                slcheck.Jiaozheng,
                slcheck.Luoyan,
                slcheck.XianranDS,
                slcheck.XianranZJDS,
                slcheck.XianranZX,
                slcheck.XianranSL
            });

        }


        //创建病历到病史的连接和病史到父节点的连接
        public async Task AddSlLink(long caseId, string lr)
        {

            var query = "";




            query = "match(a:EyeNode{CaseId:" + caseId + ",DisplayName:'"+lr+"' }),(m:SLCheckNode{CaseId:" + caseId + ", Lr:'"+lr+"'}) CREATE (a)-[f:视力检查]->(m)";


            await WriteAsync(query);
        }


        ////创建病历到病史的连接和病史到父节点的连接
        //public async Task AddSlLink(long caseId)
        //{
        //    var query = "match(a:CaseRecord{CaseId:" + caseId + "}),(m:SLCheckNode{CaseId:" + caseId + "}) CREATE (a)-[f:视力检查]->(m)";
        //    await WriteAsync(query);
        //}



        //public string DisplayName { get; set; }


        //public long CaseId { get; set; }


        //public string Lr { get; set; }

        //public string Info { get; set; }


        public async Task AddYY(YYCheckNode yycheck)
        {
            var query = "CREATE (a:YYCheckNode{DisplayName:$DisplayName,CaseId:$CaseId,Lr:$Lr,Info:$Info})";
            await WriteAsync(query, new
            {
                yycheck.DisplayName,
                yycheck.CaseId,
                yycheck.Lr,
                yycheck.Info
            });

        }

        public async Task AddYyLink(long caseId ,string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'"+lr+"'}),(m:YYCheckNode{CaseId:" + caseId + ", Lr:'"+lr+"'}) CREATE (a)-[f:眼压检查]->(m)";
            await WriteAsync(query);
        }



        #endregion


        #region  眼部前节检查异常


        public async Task AddYbqjLink(long caseId, List<string> target, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ",  DisplayName:'" + lr + "'})";
            var match = "";
            var create = "create";

 
            for (int i = 0; i < target.Count; i++)
            {
                match += ",(m" + i + ":YBQJ{name:'" + target[i] + "'})";
                create += "(a)-[:眼部前节检查异常]->(m" + i + "),";

            }
            query += match;
            query += create.Substring(0, create.Length - 1);

            await WriteAsync(query);
        }




        ////创建左右眼主诉症状到症状的节点
        //public async Task AddZhusuLink(long caseId, List<string> targetZhusu, string Lr)
        //{
        //    var query = "match";
        //    var match = "";
        //    var create = "create";


        //    for (int i = 0; i < targetZhusu.Count; i++)
        //    {

        //        match += " (a" + i + ":ZhusuNode{Lr:'" + Lr + "',CaseId:" + caseId + "}),(m" + i + ":Zhusu{name:'" + targetZhusu[i] + "'}),";
        //        create += "(a" + i + ")-[:主诉症状属于]->(m" + i + "),";

        //    }
        //    query += match.Substring(0, match.Length - 1);
        //    query += create.Substring(0, create.Length - 1);
        //    await WriteAsync(query);

        //}

        #endregion



        #region  眼底检查异常


        public async Task AddYdLink(long caseId, List<string> target,string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'"+lr+"'})";
            var match = "";
            var create = "create";


            for (int i = 0; i < target.Count; i++)
            {
                match += ",(m" + i + ":YD{name:'" + target[i] + "'})";
                create += "(a)-[:眼底检查异常]->(m" + i + "),";

            }
            query += match;
            query += create.Substring(0, create.Length - 1);

            await WriteAsync(query);
        }





        #endregion





        #region   眼科检查特殊节点





        public async Task AddEyeSpecial(EyeSpecialNode eyespecial)
        {
            var query = "CREATE (a:EyeSpecialNode{DisplayName:$DisplayName,CaseId:$CaseId,Rmk:$Rmk,Lr:$Lr})";
            await WriteAsync(query, new
            {
                eyespecial.DisplayName,
                eyespecial.CaseId,
                eyespecial.Rmk,
                eyespecial.Lr

            });

        }

        public async Task AddEyeSpecialLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'" + lr + "'}),(m:EyeSpecialNode{CaseId:" + caseId + ",Lr:'"+lr+"'}) CREATE (a)-[f:眼科检查特殊项]->(m)";
            await WriteAsync(query);
        }




        #endregion







        #region   眼科检查特殊节点——晶状体浑浊





        public async Task AddEyeJztNode(EyeJztNode eyejzt)
        {
            var query = "CREATE (a:EyeJztNode{DisplayName:$DisplayName,CaseId:$CaseId,Rmk:$Rmk,Lr:$Lr,Vector:$Vector})";
            await WriteAsync(query, new
            {
                eyejzt.DisplayName,
                eyejzt.CaseId,
                eyejzt.Rmk,
                eyejzt.Lr,
                eyejzt.Vector

            });

        }

        public async Task AddEyeJztLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'" + lr + "'}),(m:EyeJztNode{CaseId:" + caseId + ",Lr:'" + lr + "'}),(n:YBQJ{name:'晶状体浑浊'}) CREATE (a)-[f:眼科检查特殊项]->(m),(m)-[g:属于]->(n)";
            await WriteAsync(query);
        }




        #endregion




        #region   眼科检查特殊节点——眼科肿瘤





        public async Task AddEyeCancerNode(EyeCancerNode eyecancer)
        {
            var query = "CREATE (a:EyeCancerNode{DisplayName:$DisplayName,CaseId:$CaseId,Rmk:$Rmk,Lr:$Lr})";
            await WriteAsync(query, new
            {
                eyecancer.DisplayName,
                eyecancer.CaseId,
                eyecancer.Rmk,
                eyecancer.Lr

            });

        }

        public async Task AddEyeCancerLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ", DisplayName:'" + lr + "'}),(m:EyeCancerNode{CaseId:" + caseId + ",Lr:'" + lr + "'}),(n:YD{name:'眼内肿瘤'}) CREATE (a)-[f:眼科检查特殊项]->(m),(m)-[g:属于]->(n)";
            await WriteAsync(query);
        }




        #endregion
























    }
}