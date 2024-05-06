using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using CC.Admin.Models;
using Neo4j.Driver;
using CC.Admin.DAO;
using System.Threading;
using System.Data;
using static Neo4jWorkstation.CsvHelper;
using System.Globalization;
using CsvHelper;
using System.IO;
using Neo4jWorkstation.Link;
using System.Collections;
//using DocumentFormat.OpenXml.Bibliography;

using static Neo4jWorkstation.DoJsKu;
using System.Diagnostics.Metrics;


namespace Neo4jWorkstation
{
    class Program
    {




        #region   struture





        public class Link
        {

            public string name { get; set; }
        }





        #endregion





        public static List<CaseRecord> records = new List<CaseRecord>();

        public static List<EyeNode> eyes = new List<EyeNode>();

        public static List<EyeDiagnoseNode> eyediagnose = new List<EyeDiagnoseNode>();


        public static List<ZhusuNode> zhusulist = new List<ZhusuNode>();



        public static List<DiaNode> dialist = new List<DiaNode>();


        public static List<GxyNode> gxylist = new List<GxyNode>();
        public static List<GxzNode> gxzlist = new List<GxzNode>();
        public static List<GxbNode> gxblist = new List<GxbNode>();
        public static List<NgsNode> ngslist = new List<NgsNode>();
        public static List<QsMianyiNode> qsmylist = new List<QsMianyiNode>();
        public static List<QsOtherNode> qsotherlist = new List<QsOtherNode>();









        public static List<HisCataractNode> cataractlist = new List<HisCataractNode>();
        public static List<HisDiaRetinNode> diaretinlist = new List<HisDiaRetinNode>();
        public static List<HisEyeOtherNode> eyeotherlist = new List<HisEyeOtherNode>();
        public static List<HisEyeWssNode> eyewsslist = new List<HisEyeWssNode>();
        public static List<HisGlaucomaNode> glaucomalist = new List<HisGlaucomaNode>();
        public static List<HisBltNode> bltlist = new List<HisBltNode>();




        public static List<EyeJztNode> jztlist = new List<EyeJztNode>();
        public static List<EyeCancerNode> eyecancerlist = new List<EyeCancerNode>();
        public static List<EyeSpecialNode> eyespeciallist = new List<EyeSpecialNode>();
        public static List<SLCheckNode> sllist = new List<SLCheckNode>();
        public static List<YYCheckNode> yylist = new List<YYCheckNode>();


        public static List<Link> createLink = new List<Link>();



        #region   batch run

        static void Main(string[] args)
        {



            try
            {

                string filePathCsv = @"D:\work\Daemon\TopshelfDemoService-master\neo4jSetting\2375\links.csv";

                using (var reader = new StreamReader(filePathCsv, Encoding.UTF8))
                using (var csv = new CsvReader(reader))
                {
                    List<Link> records = csv.GetRecords<Link>().ToList();



                    for (int i = 0; i < records.Count; i++)
                    {

                        //string targetStr = records[i].name.Replace("\n", " ");


                        //testlink(targetStr);

                        testlink(records[i].name);


                        //Thread.Sleep(30000);
                        Thread.Sleep(300);


                        Console.WriteLine($"{records[i].name}");

                        Console.WriteLine(i + "/" + records.Count);

                    }

                }











                Console.WriteLine("finish");


            }



            catch
            {

                Console.WriteLine("fail");

            }





        }



        #endregion



        public static async void testlink(string link)
        {
            CaseRecordDAO dao = new CaseRecordDAO();

            await dao.AddLink(link);



        }















    }
}
