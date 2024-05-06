using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jWorkstation.Link
{
    public class LinkStation
    {







        public static string AddResultLinkToSon(long caseId, IList<string> me, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "',DisplayName:'" + lr + "'}),";
            var match = "";
            var create = "create";
            for (int i = 0; i < me.Count; i++)
            {
                match += "(m" + i + ":ResultSon{name:'" + me[i] + "'}),";
                create += "(a)-[:诊断结果]->(m" + i + "),";
            }

            query += match.Substring(0, match.Length - 1);
            query += create.Substring(0, create.Length - 1);

            return query+";";


        }




        public static string AddResultLinkToFatherDirect(long caseId, IList<int> me, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "',DisplayName:'" + lr + "'}),";
            var match = "";
            var create = "create";
            for (int i = 0; i < me.Count; i++)
            {
                match += "(m" + i + ":ResultFather{id:'" + me[i] + "'}),";
                create += "(a)-[:诊断结果]->(m" + i + "),";
            }

            query += match.Substring(0, match.Length - 1);
            query += create.Substring(0, create.Length - 1);

            return query + ";";


        }






        public static string AddResultLinkToFather(long caseId, Dictionary<string, string> me, string lr)
        {
            var query = "match";
            var match = "";
            var create = "create";
            int i = 0;
            foreach (KeyValuePair<string, string> x in me)
            {
                match += "(a" + i + ":EyeDiagnoseNode{DisplayName:'" + x.Key + "',CaseId:'" + caseId + "',Info:'" + x.Value + "',Lr:'" + lr + "'}),(m" + i + ":ResultFather{name:'" + x.Key + "'}),";
                create += "(a" + i + ")-[:诊断结果]->(m" + i + "),";

                i++;
            }

            query += match.Substring(0, match.Length - 1);
            query += create.Substring(0, create.Length - 1);
            return query + ";";
        }







        public static string AddResultLinkFromEye(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "',DisplayName:'" + lr + "'}),(m:EyeDiagnoseNode{CaseId:'" + caseId + "',Lr:'" + lr + "'})create(a)-[:诊断结果]->(m)";


            return query + ";";
        }






        #region  添加 从病历到性别链接

        public static string AddSexLink(long caseId, int sex)
        {
            var query = "match (a:CaseRecord{CaseId:'" + caseId + "'}),(m:Sex{id:'" + sex + "'}) CREATE (a)-[f:性别]->(m)";
            return query + ";";

        }

        #endregion



        #region  添加 从病历到年龄段链接

        public static string AddAgeLink(long caseId, int ageid)
        {
            var query = "match (a:CaseRecord{CaseId:'" + caseId + "'}),(m:Age{id:'" + ageid + "'}) CREATE (a)-[f:年龄段]->(m)";
            return query + ";";

        }

        #endregion



        //创建左右眼主诉症状到症状的节点
        public static string AddZhusuLink(long caseId, List<string> targetZhusu, string Lr)
        {
            var query = "match";
            var match = "";
            var create = "create";


            for (int i = 0; i < targetZhusu.Count; i++)
            {

                match += " (a" + i + ":ZhusuNode{Lr:'" + Lr + "',CaseId:'" + caseId + "'}),(m" + i + ":Zhusu{name:'" + targetZhusu[i] + "'}),";
                create += "(a" + i + ")-[:主诉症状]->(m" + i + "),";

            }
            query += match.Substring(0, match.Length - 1);
            query += create.Substring(0, create.Length - 1);
            return query + ";";

        }



        //创建病人左右眼到其主诉的方法

        public static string AddToZhusuLink(long caseId, string Lr)
        {
            var query = "match (a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + Lr + "'}),(m:ZhusuNode{Lr:'" + Lr + "',CaseId:'" + caseId + "'}) CREATE (a)-[f:主诉症状]->(m)";



            return query + ";";

        }













        //创建病历到病史的连接和病史到父节点的连接
        public static string AddDiaLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:'" + caseId + "'}),(m:DiaNode{CaseId:'" + caseId + "'}),(n:HistoryBody{name:'糖尿病'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";



            return query + ";";

        }




        public static string AddGxyLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:'" + caseId + "'}),(m:GxyNode{CaseId:'" + caseId + "'}),(n:HistoryBody{name:'高血压'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            return query + ";";

        }





        public static string AddGxzLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:'" + caseId + "'}),(m:GxzNode{CaseId:'" + caseId + "'}),(n:HistoryBody{name:'高血脂'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            return query + ";";

        }






        public static string AddNgsLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:'" + caseId + "'}),(m:NgsNode{CaseId:'" + caseId + "'}),(n:HistoryBody{name:'脑梗塞'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            return query + ";";

        }



        public static string AddGxbLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:'" + caseId + "'}),(m:GxbNode{CaseId:'" + caseId + "'}),(n:HistoryBody{name:'冠心病'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            return query + ";";

        }



        public static string AddQsMianyiLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:'" + caseId + "'}),(m:QsMianyiNode{CaseId:'" + caseId + "'}),(n:HistoryBody{name:'全身免疫相关疾病'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            return query + ";";

        }



        public static string AddQsOtherLink(long caseId)
        {
            var query = "match(a:CaseRecord{CaseId:'" + caseId + "'}),(m:QsOtherNode{CaseId:'" + caseId + "'}),(n:HistoryBody{name:'其他'}) CREATE (a)-[f:全身病历史]->(m),(m)-[g:全身病]->(n)";


            return query + ";";

        }









        #region   眼科病史




        public static string AddHisCataractLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "',DisplayName:'" + lr + "'}),(m:HisCataractNode{CaseId:'" + caseId + "',Lr:'" + lr + "'}),(n:HistoryEye{name:'白内障'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            return query + ";";

        }






        public static string AddHisGlaucomaLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:HisGlaucomaNode{CaseId:'" + caseId + "', Lr:'" + lr + "'}),(n:HistoryEye{name:'青光眼'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            return query + ";";

        }







        public static string AddHisDiaRetinLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "',DisplayName:'" + lr + "'}),(m:HisDiaRetinNode{CaseId:'" + caseId + "', Lr:'" + lr + "'}),(n:HistoryEye{name:'糖尿病视网膜病变'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            return query + ";";

        }





        public static string AddHisBltLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:HisBltNode{CaseId:'" + caseId + "', Lr:'" + lr + "'}),(n:HistoryEye{name:'其他玻璃体视网膜疾病'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            return query + ";";

        }




        public static string AddHisEyeWssLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:HisEyeWssNode{CaseId:'" + caseId + "', Lr:'" + lr + "'  }),(n:HistoryEye{name:'外伤史'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            return query + ";";

        }









        public static string AddHisEyeOtherLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:HisEyeOtherNode{CaseId:'" + caseId + "', Lr:'" + lr + "' }),(n:HistoryEye{name:'其他'}) CREATE (a)-[f:眼科病历史]->(m),(m)-[g:眼科病]->(n)";


            return query + ";";

        }






        #endregion








        #region   眼科检查



        //创建病历到病史的连接和病史到父节点的连接
        public static string AddSlLink(long caseId, string lr)
        {

            var query = "";




            query = "match(a:EyeNode{CaseId:'" + caseId + "',DisplayName:'" + lr + "' }),(m:SLCheckNode{CaseId:'" + caseId + "', Lr:'" + lr + "'}) CREATE (a)-[f:视力检查]->(m)";


            return query + ";";
        }


        public static string AddYyLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:YYCheckNode{CaseId:'" + caseId + "', Lr:'" + lr + "'}) CREATE (a)-[f:眼压检查]->(m)";
            return query + ";";
        }


        #region  眼部前节检查异常


        public static string AddYbqjLink(long caseId, List<string> target, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "',  DisplayName:'" + lr + "'})";
            var match = "";
            var create = "create";


            for (int i = 0; i < target.Count; i++)
            {
                match += ",(m" + i + ":YBQJ{name:'" + target[i] + "'})";
                create += "(a)-[:眼部前节检查异常]->(m" + i + "),";

            }
            query += match;
            query += create.Substring(0, create.Length - 1);

            return query + ";";
        }




        ////创建左右眼主诉症状到症状的节点
        //public async Task AddZhusuLink(long caseId, List<string> targetZhusu, string Lr)
        //{
        //    var query = "match";
        //    var match = "";
        //    var create = "create";


        //    for (int i = 0; i < targetZhusu.Count; i++)
        //    {

        //        match += " (a" + i + ":ZhusuNode{Lr:'" + Lr + "',CaseId:'" + caseId + "'}),(m" + i + ":Zhusu{name:'" + targetZhusu[i] + "'}),";
        //        create += "(a" + i + ")-[:主诉症状属于]->(m" + i + "),";

        //    }
        //    query += match.Substring(0, match.Length - 1);
        //    query += create.Substring(0, create.Length - 1);
        //    await WriteAsync(query);

        //}

        #endregion



        #region  眼底检查异常


        public static string AddYdLink(long caseId, List<string> target, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'})";
            var match = "";
            var create = "create";


            for (int i = 0; i < target.Count; i++)
            {
                match += ",(m" + i + ":YD{name:'" + target[i] + "'})";
                create += "(a)-[:眼底检查异常]->(m" + i + "),";

            }
            query += match;
            query += create.Substring(0, create.Length - 1);

            return query + ";";
        }





        #endregion

        public static string AddEyeSpecialLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:EyeSpecialNode{CaseId:'" + caseId + "',Lr:'" + lr + "'}) CREATE (a)-[f:眼科检查特殊项]->(m)";
            return query + ";";
        }

        public static string AddEyeJztLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:EyeJztNode{CaseId:'" + caseId + "',Lr:'" + lr + "'}),(n:YBQJ{name:'晶状体浑浊'}) CREATE (a)-[f:眼科检查特殊项]->(m),(m)-[g:属于]->(n)";
            return query + ";";
        }


        public static string AddEyeCancerLink(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:'" + caseId + "', DisplayName:'" + lr + "'}),(m:EyeCancerNode{CaseId:'" + caseId + "',Lr:'" + lr + "'}),(n:YD{name:'眼内肿瘤'}) CREATE (a)-[f:眼科检查特殊项]->(m),(m)-[g:属于]->(n)";
            return query + ";";
        }





        #endregion




    }
}
