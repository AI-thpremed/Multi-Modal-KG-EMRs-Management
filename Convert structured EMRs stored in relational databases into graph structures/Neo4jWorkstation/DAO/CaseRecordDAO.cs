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
    public class CaseRecordDAO: BaseDAO
    {


        //   #region  同一个线程创建病历并且回调节点ID  必须带上caseid因为住院id和姓名都不是唯一项目   注意这个特点是同一个tx



        //   public async Task<long> AddCaseAsync(CaseRecord caserecord)
        //   {
        //       var session = Driver.AsyncSession();
        //       try
        //       {
        //           await session.ExecuteWriteAsync(async tx => await CreatePersonNodeAsync(tx, caserecord));

        //           return await session.ExecuteReadAsync(tx => MatchPersonNodeAsync(tx, caserecord));
        //       }
        //       finally
        //       {
        //           await session.CloseAsync();
        //       }
        //   }


        //   private static Task CreatePersonNodeAsync(IAsyncTransaction tx, CaseRecord caserecord)
        //   {

        //       var query = "CREATE (a:CaseRecord{UserName:$UserName, CaseId:$CaseId,HopId: $HopId,HopAge: $HopAge,HopDate: $HopDate,Race: $Race" +
        //            " Marriage: $Marriage,Job: $Job,Edu: $Edu})";

        //       return tx.RunAsync(query, new
        //{caserecord.UserName,
        //           caserecord.CaseId,
        //           caserecord.HopId,
        //           caserecord.HopAge,
        //           caserecord.HopDate,
        //           caserecord.Race,
        //           caserecord.Marriage,
        //           caserecord.Job,
        //           caserecord.Edu
        //       });



        //       //return tx.RunAsync("CREATE (a:CaseRecord) Set {a.UserName: $UserName,a.CaseId: $CaseId,a.HopId: $HopId,a.HopAge: $HopAge,a.HopDate: $HopDate,a.Race: $Race" +
        //       //     "a.Marriage: $Marriage,a.Job: $Job,a.Edu: $Edu})", new {  caserecord.UserName,




        //   }







        //   //private static async Task<long> MatchPersonNodeAsync(IAsyncTransaction tx, CaseRecord caserecord)
        //   //{
        //   //    var result = await tx.RunAsync("MATCH (a:CaseRecord {CaseId: $CaseId}) RETURN id(a)", new { CaseId=caserecord.CaseId });


        //   //    return (await result.SingleAsync())[0].As<long>();
        //   //}



        //   #endregion









        //创建病人左右眼到其主诉的方法

        public async Task AddLink(string link)
        {
            var query = link;



            await WriteAsync(query);

        }







        public  async Task Add(CaseRecord caserecord)
        {
            var query = "CREATE (a:CaseRecord{DisplayName:$DisplayName,UserName:$UserName,CaseId:$CaseId,HopId:$HopId,HopAge:$HopAge,Race:$Race,Marriage:$Marriage,Job:$Job,Edu: $Edu})";
            await WriteAsync(query, new
            {
                caserecord.DisplayName,

                caserecord.UserName,
               caserecord.CaseId,
               caserecord.HopId,
                caserecord.HopAge,
                //caserecord.HopDate,
                caserecord.Race,
                caserecord.Marriage,
                caserecord.Job,
                caserecord.Edu
            });
            
        }











        public async Task<CaseRecord> GetByCaseId(long caseid)
        {
            var query = "MATCH (a:CaseRecord {CaseId:" + caseid + "}) RETURN a";

            var session = Driver.AsyncSession();
            var results = await session.ExecuteReadAsync(async tx =>
            {

                var cursor = await tx.RunAsync(query);

                //var cursor = await tx.RunAsync(query, new { title });
                var fetched = await cursor.FetchAsync();

                while (fetched)
                {

                    var node = cursor.Current["a"].As<INode>();
                    var movie = ConvertToMovie(node);
                    return movie;
                }

                return null;
            });

            return results;
        }









        public async Task<long> GetIdByCaseId(long caseid)
        {
            var query = "MATCH (a:CaseRecord {CaseId:" + caseid + "}) RETURN Id(a) AS Id";

            var session = Driver.AsyncSession();
            var results = await session.ExecuteReadAsync(async tx =>
            {

                var cursor = await tx.RunAsync(query);

                var fetched = await cursor.FetchAsync();

                while (fetched)
                {
                    var id = cursor.Current["Id"].As<long>();
                    return id;
                }

                return 0;
            });

            return results;
        }














        private static CaseRecord ConvertToMovie(INode node)
        {
            return new CaseRecord
            {





                CaseId = node.Properties["CaseId"].As<long>(),

                UserName = node.Properties.ContainsKey("UserName") ? node.Properties["UserName"].As<string>() : null,

                HopId = node.Properties.ContainsKey("HopId") ? node.Properties["HopId"].As<string>() : null

                ////We do this as there are some Movies that don't have taglines (and indeed released years)
                //Tagline = node.Properties.ContainsKey("tagline") ? node.Properties["tagline"].As<string>() : null,
                //Released = node.Properties.ContainsKey("released") ? node.Properties["released"]?.As<int?>() : null
            };
        }










        #region  添加 主诉症状节点

        public async Task AddZhusuNode(ZhusuNode ZhusuNode)
        {
            var query = "CREATE (a:ZhusuNode{DisplayName:$DisplayName,Name:$Name,CaseId:$CaseId,Lr:$Lr})";

            
        await WriteAsync(query, new
            {

            ZhusuNode.DisplayName,
            ZhusuNode.Name,
            ZhusuNode.CaseId,
            ZhusuNode.Lr
            });

        }



        //创建左右眼主诉症状到症状的节点
        public async Task AddZhusuLink(long caseId, List<string> targetZhusu, string Lr)
        {
            var query = "match";
            var match = "";
            var create = "create";


            for (int i = 0; i < targetZhusu.Count; i++)
            {

                match += " (a" + i + ":ZhusuNode{Lr:'" + Lr + "',CaseId:"+caseId+"}),(m"+i+":Zhusu{name:'" + targetZhusu[i] + "'}),";
                create += "(a" + i + ")-[:主诉症状]->(m" + i + "),";

            }
            query += match.Substring(0, match.Length - 1);
            query += create.Substring(0, create.Length - 1);
            await WriteAsync(query);

        }



        //创建病人左右眼到其主诉的方法

        public async Task AddToZhusuLink(long caseId, string Lr)
        {
            var query = "match (a:EyeNode{CaseId:" + caseId + ", DisplayName:'"+Lr+"'}),(m:ZhusuNode{Lr:'" + Lr + "',CaseId:" + caseId + "}) CREATE (a)-[f:主诉症状]->(m)";



            await WriteAsync(query);

        }

        //public async Task AddToZhusuLink(long caseId, string Lr)
        //{
        //    var query = "match (a:CaseRecord{CaseId:" + caseId + "}),(m:ZhusuNode{Lr:'" + Lr + "',CaseId:" + caseId + "}) CREATE (a)-[f:主诉症状]->(m)";

            

        //    await WriteAsync(query);

        //}


        #endregion












        #region  添加 从病历到诊断结果的链接




    //    public async Task AddYkbResult(IList<YkbResult> me)
    //    {

    //        var query = "CREATE ";

    //        for (int i = 0; i < me.Count; i++)
    //        {
    //            query += "(a" + i + ":YkbResult{DisplayName:'"+ me[i].DisplayName+ "',CaseId:" + me[i].CaseId + ",Name:'" + me[i].Name + "',Lr:'" + me[i].Lr + "', FatherId:" + me[i].FatherId + "}),";

    //        }
    //        query = query.Substring(0, query.Length - 1);

    //        await WriteAsync(query);

    //    }



    //    public async Task AddResultLink(long caseId)
    //    {
    //        var query = "match (a:CaseRecord{CaseId:" + caseId + "}),(m:YkbResult{CaseId:" + caseId + "}) CREATE (a)-[f:诊断]->(m)";
    //        await WriteAsync(query);

    //    }



    ////    match(a1:YkbResult{ CaseId: 1594,FatherId: 9}),(m1:ResultFather{id:'9'}) 
    ////,(a0:YkbResult{CaseId:1594,FatherId:8}),(m0:ResultFather{id:'8'}) 
    ////CREATE
    
    ////(a0)-[f0: 属于]->(m0),(a1)-[f1: 属于]->(m1)

    //    public async Task AddResultLinkToFather(long caseId, IList<YkbResult> me)
    //    {
    //        var query = "match";
    //        var match = "";
    //        var create = "create";
    //        for (int i = 0; i < me.Count; i++)
    //        {
    //            match += "(a" + i + ":YkbResult{CaseId:" + caseId + ",FatherId:" + me[i].FatherId + ",Lr:'" + me[i].Lr + "'}),(m" + i + ":ResultFather{id:'" + me[i].FatherId + "'}),";
    //            create += "(a" + i + ")-[:诊断结果属于]->(m" + i + "),";
    //        }

    //        query += match.Substring(0, match.Length - 1);
    //        query += create.Substring(0, create.Length - 1);
    //        await WriteAsync(query);
    //    }






        //前七项直接连接子节点从eye搞出来


        public async Task AddResultLinkToSon(long caseId, IList<string> me, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ",DisplayName:'" + lr + "'}),";
            var match = "";
            var create = "create";
            for (int i = 0; i < me.Count; i++)
            {
                match += "(m" + i + ":ResultSon{name:'" + me[i] + "'}),";
                create += "(a)-[:诊断结果]->(m" + i + "),";
            }

            query += match.Substring(0, match.Length - 1);
            query += create.Substring(0, create.Length - 1);
            await WriteAsync(query);
        }






        //public string DisplayName { get; set; }

        //public long CaseId { get; set; }


        //public string Info { get; set; }


        public async Task AddEyeDiagnoseNode(long caseId, Dictionary<string,string> me, string lr)
        {

            var query = "CREATE ";

            int i = 0;
            foreach (KeyValuePair<string,string> x in me)
            {

                query += "(a" + i + ":EyeDiagnoseNode{DisplayName:'" + x.Key + "',CaseId:" + caseId + ",Info:'" + x.Value + "',Lr:'"+lr+"'}),";
                i++;
            }
            query = query.Substring(0, query.Length - 1);

            await WriteAsync(query);

        }


        public async Task AddResultLinkFromEye(long caseId, string lr)
        {
            var query = "match(a:EyeNode{CaseId:" + caseId + ",DisplayName:'" + lr + "'}),(m:EyeDiagnoseNode{CaseId:" + caseId + ",Lr:'" + lr + "'})create(a)-[:诊断结果]->(m)";
            

            await WriteAsync(query);
        }




        public async Task AddResultLinkToFather(long caseId, Dictionary<string, string> me, string lr)
        {
            var query = "match";
            var match = "";
            var create = "create";
            int i = 0;
            foreach (KeyValuePair<string, string> x in me)
            {
                match += "(a" + i + ":EyeDiagnoseNode{DisplayName:'" + x.Key + "',CaseId:" + caseId + ",Info:'" + x.Value + "',Lr:'" + lr + "'}),(m" + i + ":ResultFather{name:'" + x.Key + "'}),";
                create += "(a"+i+")-[:诊断结果]->(m" + i + "),";

                i++;
            }

            query += match.Substring(0, match.Length - 1);
            query += create.Substring(0, create.Length - 1);
            await WriteAsync(query);
        }


        #endregion



















        #region  添加 从病历到性别链接

        public async Task AddSexLink(long caseId,int sex)
        {
            var query = "match (a:CaseRecord{CaseId:" + caseId + "}),(m:Sex{id:'" + sex + "'}) CREATE (a)-[f:性别]->(m)";
            await WriteAsync(query);

        }

        #endregion



        #region  添加 从病历到年龄段链接

        public async Task AddAgeLink(long caseId, int ageid)
        {
            var query = "match (a:CaseRecord{CaseId:" + caseId + "}),(m:Age{id:'" + ageid+ "'}) CREATE (a)-[f:年龄段]->(m)";
            await WriteAsync(query);

        }

        #endregion
















        #region   暂时用不到



        //这段程序也可以跑。就是出现重复项直接不返回。这个做法是等待有结果，但是可能没有结果
        //var getid = await dao.CountPersonAsync(record);

        public async Task<long> CountPersonAsync(CaseRecord caserecord)
        {
            var session = Driver.AsyncSession();
            try
            {
                return await session.ExecuteReadAsync(async tx =>
                {
                    IResultCursor result =
                        await tx.RunAsync("MATCH (a:CaseRecord {CaseId:" + caserecord.CaseId + "}) RETURN id(a)");

                    return (await result.SingleAsync())[0].As<long>();
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }


        #endregion











        //public void AddResult(string user, string me)
        //{
        //    using (var session = Driver.Session())
        //    {
        //        session.Run("MATCH (u:Profile {User:'" + user + "'}),(m:Profile {User:'" + me + "'}) CREATE (m)-[f:FRIEND]->(u)");
        //    }
        //}


    }
}