using CC.Admin.DAO;
using CC.Admin.Models;
using CsvHelper;
using DAL;
using Model;
using Neo4jWorkstation.Link;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jWorkstation
{
    public class DoJsKu
    {





        #region   结构





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



      
        public static void dojs()
        {


            ///这个逻辑现在做的是三秒创建一个病历的neo4j节点，现在的搞法就是还是会断，我觉得要最终还是做成csv导出来。估计只能这样了。tmd
            ///

            //所以简单地说。这个就是废了。



            try
            {

                IList<long> all = CaseInfoDAL.GetAllPassData(1, 2);


                 

                for (int i = 0; i < all.Count; i++)
                {


                    Task<long> xx = testJs(all[i]);


                    Console.WriteLine(i+"/"+ all.Count+"   "+ all[i]);


 

                }




                using (var writer = new StreamWriter("caserecords.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {

                        csv.WriteHeader<CaseRecord>();


                        foreach (var record in records)
                        {
                            csv.WriteRecord(record);
                        }
                    }
                }


                using (var writer = new StreamWriter("eyes.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {

                        csv.WriteHeader<EyeNode>();


                        foreach (var eye in eyes)
                        {
                            csv.WriteRecord(eye);
                        }
                    }
                }



                using (var writer = new StreamWriter("links.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        foreach (var line in createLink)
                        {
                            csv.WriteRecord(line);
                        }
                    }
                }






                using (var writer = new StreamWriter("dialist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<DiaNode>();

                        foreach (var line4 in dialist)
                        {
                            csv.WriteRecord(line4);
                        }
                    }
                }





                using (var writer = new StreamWriter("gxylist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<GxyNode>();

                        foreach (var line5 in gxylist)
                        {
                            csv.WriteRecord(line5);
                        }
                    }
                }





                using (var writer = new StreamWriter("gxzlist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {

                        csv.WriteHeader<GxzNode>();


                        foreach (var line7 in gxzlist)
                        {
                            csv.WriteRecord(line7);
                        }
                    }
                }





                using (var writer = new StreamWriter("gxblist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<GxbNode>();

                        foreach (var line8 in gxblist)
                        {
                            csv.WriteRecord(line8);
                        }
                    }
                }




                using (var writer = new StreamWriter("ngslist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<NgsNode>();

                        foreach (var line9 in ngslist)
                        {
                            csv.WriteRecord(line9);
                        }
                    }
                }




                using (var writer = new StreamWriter("qsotherlist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<QsOtherNode>();


                        foreach (var line11 in qsotherlist)
                        {
                            csv.WriteRecord(line11);
                        }
                    }
                }




                using (var writer = new StreamWriter("cataractlist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<HisCataractNode>();


                        foreach (var line12 in cataractlist)
                        {
                            csv.WriteRecord(line12);
                        }
                    }
                }





                using (var writer = new StreamWriter("diaretinlist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<HisDiaRetinNode>();


                        foreach (var line13 in diaretinlist)
                        {
                            csv.WriteRecord(line13);
                        }
                    }
                }







                using (var writer = new StreamWriter("eyeotherlist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<HisEyeOtherNode>();


                        foreach (var line14 in eyeotherlist)
                        {
                            csv.WriteRecord(line14);
                        }
                    }
                }




                using (var writer = new StreamWriter("eyewsslist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<HisEyeWssNode>();


                        foreach (var line15 in eyewsslist)
                        {
                            csv.WriteRecord(line15);
                        }
                    }
                }



                using (var writer = new StreamWriter("glaucomalist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<HisGlaucomaNode>();


                        foreach (var line16 in glaucomalist)
                        {
                            csv.WriteRecord(line16);
                        }
                    }
                }


                using (var writer = new StreamWriter("bltlist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {

                        csv.WriteHeader<HisBltNode>();

                        foreach (var line17 in bltlist)
                        {
                            csv.WriteRecord(line17);
                        }
                    }
                }




                using (var writer = new StreamWriter("sllist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<SLCheckNode>();


                        foreach (var line21 in sllist)
                        {
                            csv.WriteRecord(line21);
                        }
                    }
                }



                using (var writer = new StreamWriter("yylist.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteHeader<YYCheckNode>();


                        foreach (var line22 in yylist)
                        {
                            csv.WriteRecord(line22);
                        }
                    }
                }






                //using (var writer = new StreamWriter("zhusulist.csv"))
                //{
                //    using (var csv = new CsvWriter(writer))
                //    {
                //        csv.WriteHeader<ZhusuNode>();


                //        foreach (var line2 in zhusulist)
                //        {
                //            csv.WriteRecord(line2);
                //        }
                //    }
                //}



                //using (var writer = new StreamWriter("eyediagnose.csv"))
                //{
                //    using (var csv = new CsvWriter(writer))
                //    {
                //        csv.WriteHeader<EyeDiagnoseNode>();

                //        foreach (var line3 in eyediagnose)
                //        {
                //            csv.WriteRecord(line3);
                //        }
                //    }
                //}





                 







                //using (var writer = new StreamWriter("jztlist.csv"))
                //{
                //    using (var csv = new CsvWriter(writer))
                //    {
                //        csv.WriteHeader<EyeJztNode>();


                //        foreach (var line18 in jztlist)
                //        {
                //            csv.WriteRecord(line18);
                //        }
                //    }
                //}






                //using (var writer = new StreamWriter("eyecancerlist.csv"))
                //{
                //    using (var csv = new CsvWriter(writer))
                //    {
                //        csv.WriteHeader<EyeCancerNode>();


                //        foreach (var line19 in eyecancerlist)
                //        {
                //            csv.WriteRecord(line19);
                //        }
                //    }
                //}


                //using (var writer = new StreamWriter("eyespeciallist.csv"))
                //{
                //    using (var csv = new CsvWriter(writer))
                //    {
                //        csv.WriteHeader<EyeSpecialNode>();


                //        foreach (var line20 in eyespeciallist)
                //        {
                //            csv.WriteRecord(line20);
                //        }
                //    }
                //}







                Console.WriteLine("结束");


            }



            catch
            {

                Console.WriteLine("提交失败，请稍后再试");

            }





        }











        //眼底病现在通过创建csv的方式搞得节点
        public static async Task<long> testJs(long id)
        {



            CaseInfoModels thiscase = CaseInfoDAL.GetFirst(id);

            CaseRecord record = new CaseRecord();


            record.DisplayName = "病历";

            record.CaseId = thiscase.ID;
            record.HopId = thiscase.HOP_ID;
            record.UserName = "病人" + thiscase.ID;
            record.HopAge = thiscase.HOP_AGE;
            //record.HopDate = new LocalDate(thiscase.HOP_DATE);

            //record.HopDate = thiscase.HOP_DATE.ToString("yyyy-MM-dd");

            record.Race = thiscase.RACE;
            record.Marriage = thiscase.MARRIAGE;
            record.Job = thiscase.JOB;
            record.Edu = thiscase.EDU;

            record.TypeId = "1";



            CaseRecordDAO dao = new CaseRecordDAO();

            records.Add(record);



            //await dao.Add(record);




            EyeCheckDAO eyecheckdao = new EyeCheckDAO();


            EyeNode lefteye = new EyeNode();
            lefteye.DisplayName = "左眼";
            lefteye.CaseId = id;

            EyeNode righteye = new EyeNode();
            righteye.DisplayName = "右眼";
            righteye.CaseId = id;


            eyes.Add(lefteye);
            eyes.Add(righteye);

            Link link1 = new Link();
            link1.name = @"match(a:CaseRecord{CaseId:'" + thiscase.ID + "'}),(m:EyeNode{CaseId:'" + thiscase.ID + "'})CREATE (a)-[f:属于]->(m);";

            createLink.Add(link1);








            #region  关联诊断结果






             

            //前七项   没有子节点，就是搞得直连父节点呢  从eye直接干到父节点搞出来
            List<int> leftreslinkfather = new List<int>();
            List<int> rightreslinkfather = new List<int>();


            leftreslinkfather.Add(6);


            rightreslinkfather.Add(6);



            Link link11 = new Link();
            link11.name = LinkStation.AddResultLinkToFatherDirect(thiscase.ID, leftreslinkfather, "左眼");

            createLink.Add(link11);

             

                
            Link link2 = new Link();

            link2.name = LinkStation.AddResultLinkToFatherDirect(thiscase.ID, rightreslinkfather, "右眼");
                
            createLink.Add(link2);
















            #endregion






            #region  关联年龄和性别

            Link linksex = new Link();
            linksex.name = LinkStation.AddSexLink(thiscase.ID, thiscase.SEX);


            createLink.Add(linksex);



            int ageid = thiscase.HOP_AGE / 10 + 1;


            Link linkage = new Link();
            linkage.name = LinkStation.AddAgeLink(thiscase.ID, ageid);



            createLink.Add(linkage);


            #endregion




















            //#region  左右眼主诉病情node

            //CaseYkbZsModels zhusu = CaseYkbZsDAL.GetFirst(id);

            ////视物遮挡* 中央, 其它*右眼中央黑影遮挡3月余IVR术后1月自觉中央黑影较前好转
            ////主诉必须要有节点，没有节点左眼右眼和一些信息区分不开。

            //if (!string.IsNullOrEmpty(zhusu.INFO_L))
            //{
            //    List<string> targetZhusu = new List<string>();

            //    ZhusuNode result = new ZhusuNode();


            //    result.DisplayName = "主诉症状";

            //    result.CaseId = thiscase.ID;
            //    result.Name = zhusu.INFO_L;
            //    result.Lr = "左眼";

            //    string[] info = zhusu.INFO_L.Split(',');

            //    for (int i = 0; i < info.Length; i++)
            //    {
            //        targetZhusu.Add(info[i].Split('*')[0].Trim());

            //    }






            //    zhusulist.Add(result);









            //    Link linkzhusu1 = new Link();
            //    linkzhusu1.name = LinkStation.AddZhusuLink(thiscase.ID, targetZhusu, "左眼");



            //    createLink.Add(linkzhusu1);





            //    Link linkzhusu2 = new Link();
            //    linkzhusu2.name = LinkStation.AddToZhusuLink(thiscase.ID, "左眼");


            //    createLink.Add(linkzhusu2);



            //}


            //if (!string.IsNullOrEmpty(zhusu.INFO_R))
            //{
            //    List<string> targetZhusu = new List<string>();

            //    ZhusuNode result = new ZhusuNode();


            //    result.DisplayName = "主诉症状";

            //    result.CaseId = thiscase.ID;
            //    result.Name = zhusu.INFO_R;
            //    result.Lr = "右眼";

            //    string[] info = zhusu.INFO_R.Split(',');

            //    for (int i = 0; i < info.Length; i++)
            //    {
            //        targetZhusu.Add(info[i].Split('*')[0].Trim());

            //    }
            //    zhusulist.Add(result);










            //    Link linkzhusu1 = new Link();
            //    linkzhusu1.name = LinkStation.AddZhusuLink(thiscase.ID, targetZhusu, "右眼");



            //    createLink.Add(linkzhusu1);





            //    Link linkzhusu2 = new Link();
            //    linkzhusu2.name = LinkStation.AddToZhusuLink(thiscase.ID, "右眼");


            //    createLink.Add(linkzhusu2);





            //}



            //#endregion




            #region  todo 全身病 肿瘤暂时不用。眼科病搞起来









            QsNodeDAO qsdao = new QsNodeDAO();





            HisQsDiaModels hisdia = HisQsDiaDAL.GetFirst(id);
            HisQsGxyModels hisgxy = HisQsGxyDAL.GetFirst(id);
            HisQsGxzModels hisgxz = HisQsGxzDAL.GetFirst(id);
            HisQsGxbModels hisgxb = HisQsGxbDAL.GetFirst(id);
            HisQsNgsModels hisngs = HisQsNgsDAL.GetFirst(id);


            //HisQsMianyiModels hisqsmy = HisQsMianyiDAL.GetFirst(id);



            HisQsOtherModels hisother = HisQsOtherDAL.GetFirst(id);

            if (hisdia.STATUS == 1)
            {

                DiaNode dia = new DiaNode();


                dia.DisplayName = "糖尿病";

                dia.CaseId = thiscase.ID;
                //dia.FistDia = new LocalDate(CheckYearToStandard(hisdia.FIST_DIA));
                dia.RmkQt = hisdia.RMK_QT;





                dialist.Add(dia);



                Link linkdia = new Link();
                linkdia.name = LinkStation.AddDiaLink(thiscase.ID);



                createLink.Add(linkdia);






            }



            if (hisgxy.STATUS == 1)
            {

                GxyNode gxy = new GxyNode();
                gxy.DisplayName = "高血压";

                gxy.CaseId = thiscase.ID;
                //gxy.FistDia = new LocalDate(CheckYearToStandard(hisgxy.FIST_DIA));
                gxy.RmkQt = hisgxy.RMK_QT;







                gxylist.Add(gxy);



                Link linkgxy = new Link();
                linkgxy.name = LinkStation.AddGxyLink(thiscase.ID);



                createLink.Add(linkgxy);



            }


            if (hisgxz.STATUS == 1)
            {

                GxzNode gxz = new GxzNode();
                gxz.DisplayName = "高血脂";

                gxz.CaseId = thiscase.ID;
                //gxz.FistDia = new LocalDate(CheckYearToStandard(hisgxz.FIST_DIA));
                gxz.RmkQt = hisgxz.RMK_QT;








                gxzlist.Add(gxz);



                Link linkgxz = new Link();
                linkgxz.name = LinkStation.AddGxzLink(thiscase.ID);



                createLink.Add(linkgxz);


            }




            if (hisngs.STATUS == 1)
            {

                NgsNode ngs = new NgsNode();
                ngs.DisplayName = "脑梗塞";

                ngs.CaseId = thiscase.ID;
                //ngs.FistDia = new LocalDate(CheckYearToStandard(hisngs.FIST_DIA));
                ngs.RmkQt = hisngs.RMK_QT;




                ngslist.Add(ngs);



                Link linkngs = new Link();
                linkngs.name = LinkStation.AddNgsLink(thiscase.ID);



                createLink.Add(linkngs);
            }



            if (hisgxb.STATUS == 1)
            {

                GxbNode gxb = new GxbNode();
                gxb.DisplayName = "冠心病";

                gxb.CaseId = thiscase.ID;
                //gxb.FistDia = new LocalDate(CheckYearToStandard(hisgxb.FIST_DIA));
                gxb.RmkQt = hisgxb.RMK_QT;



                gxblist.Add(gxb);



                Link linkgxb = new Link();
                linkgxb.name = LinkStation.AddGxbLink(thiscase.ID);



                createLink.Add(linkgxb);
            }



            //if (hisqsmy.STATUS == 1)
            //{

            //    QsMianyiNode qsmianyi = new QsMianyiNode();
            //    qsmianyi.DisplayName = "全身免疫类疾病";

            //    qsmianyi.CaseId = thiscase.ID;

            //    qsmianyi.RmkQt = hisqsmy.RMK_QT;



            //    qsmylist.Add(qsmianyi);



            //    Link linkqsmy = new Link();
            //    linkqsmy.name = LinkStation.AddQsMianyiLink(thiscase.ID);



            //    createLink.Add(linkqsmy);





            //}



            if (hisother.STATUS == 1)
            {

                QsOtherNode qsother = new QsOtherNode();
                qsother.DisplayName = "其他全身疾病";

                qsother.CaseId = thiscase.ID;

                qsother.RmkQt = hisother.RMK;



                qsotherlist.Add(qsother);



                Link linkqsother = new Link();
                linkqsother.name = LinkStation.AddQsOtherLink(thiscase.ID);




                createLink.Add(linkqsother);


            }











            #endregion


            #region  todo 眼科病





            IList<HisEyeModels> hiseye = HisEyeDAL.GetBoth(id);





            HisEyeModels left = new HisEyeModels();

            HisEyeModels right = new HisEyeModels();


            for (int i = 0; i < hiseye.Count; i++)
            {
                if (hiseye[i].L_R == 1)
                    left = hiseye[i];
                else
                    right = hiseye[i];
            }



            #region  白内障



            if (left.CATARACT == 1)
            {

                HisCataractNode cataract = new HisCataractNode();

                cataract.DisplayName = "白内障";


                cataract.CaseId = thiscase.ID;



                cataract.RmkQt = left.CATARACT_RMK;

                cataract.Lr = "左眼";








                cataractlist.Add(cataract);


                Link linkcataract = new Link();
                linkcataract.name = LinkStation.AddHisCataractLink(thiscase.ID, cataract.Lr);




                createLink.Add(linkcataract);







                //await qsdao.AddHisCataract(cataract);
                //await qsdao.AddHisCataractLink(thiscase.ID, cataract.Lr);

            }


            if (right.CATARACT == 1)
            {

                HisCataractNode cataract = new HisCataractNode();
                cataract.DisplayName = "白内障";

                cataract.CaseId = thiscase.ID;



                cataract.RmkQt = left.CATARACT_RMK;

                cataract.Lr = "右眼";





                cataractlist.Add(cataract);


                Link linkcataract = new Link();
                linkcataract.name = LinkStation.AddHisCataractLink(thiscase.ID, cataract.Lr);




                createLink.Add(linkcataract);







            }






            #endregion



            #region  青光眼




            if (left.GLAUCOMA == 1)
            {

                HisGlaucomaNode glaucoma = new HisGlaucomaNode();
                glaucoma.DisplayName = "青光眼";

                glaucoma.CaseId = thiscase.ID;

                //glaucoma.FistDia = new LocalDate(CheckYearToStandard(left.GLAUCOMA_DATE));


                glaucoma.RmkQt = left.GLAUCOMA_RMK;

                glaucoma.Lr = "左眼";










                glaucomalist.Add(glaucoma);


                Link linkglaucoma = new Link();
                linkglaucoma.name = LinkStation.AddHisGlaucomaLink(thiscase.ID, glaucoma.Lr);




                createLink.Add(linkglaucoma);




                //await qsdao.AddHisGlaucoma(glaucoma);
                //await qsdao.AddHisGlaucomaLink(thiscase.ID, glaucoma.Lr);

            }


            if (right.GLAUCOMA == 1)
            {
                HisGlaucomaNode glaucoma = new HisGlaucomaNode();
                glaucoma.DisplayName = "青光眼";

                glaucoma.CaseId = thiscase.ID;

                //glaucoma.FistDia = new LocalDate(CheckYearToStandard(left.GLAUCOMA_DATE));


                glaucoma.RmkQt = left.GLAUCOMA_RMK;

                glaucoma.Lr = "右眼";




                glaucomalist.Add(glaucoma);


                Link linkglaucoma = new Link();
                linkglaucoma.name = LinkStation.AddHisGlaucomaLink(thiscase.ID, glaucoma.Lr);




                createLink.Add(linkglaucoma);




            }



            #endregion




            #region  糖尿病视网膜病变




            if (left.BLTSWM.Split('▲')[0] != "")
            {

                HisDiaRetinNode diaretin = new HisDiaRetinNode();
                diaretin.DisplayName = "糖尿病视网膜病变";

                diaretin.CaseId = thiscase.ID;

                //diaretin.FistDia = new LocalDate(Convert.ToDateTime(BasicHelper.checkDate(left.BLTSWM.Split('▲')[1])));


                diaretin.Name = left.BLTSWM.Split('▲')[0];

                diaretin.Lr = "左眼";






                diaretinlist.Add(diaretin);


                Link linkdiaretin = new Link();
                linkdiaretin.name = LinkStation.AddHisDiaRetinLink(thiscase.ID, diaretin.Lr);




                createLink.Add(linkdiaretin);


            }


            if (right.BLTSWM.Split('▲')[0] != "")
            {
                HisDiaRetinNode diaretin = new HisDiaRetinNode();
                diaretin.DisplayName = "糖尿病视网膜病变";

                diaretin.CaseId = thiscase.ID;

                //diaretin.FistDia = new LocalDate(Convert.ToDateTime(BasicHelper.checkDate(right.BLTSWM.Split('▲')[1])));


                diaretin.Name = right.BLTSWM.Split('▲')[0];

                diaretin.Lr = "右眼";




                diaretinlist.Add(diaretin);


                Link linkdiaretin = new Link();
                linkdiaretin.name = LinkStation.AddHisDiaRetinLink(thiscase.ID, diaretin.Lr);




                createLink.Add(linkdiaretin);




            }



            #endregion



            //todo其他 外伤史 和其他。。。


            #region  其他玻璃体视网膜病变




            if (left.BLTSWM.Split('▲')[2] != "" || left.BLTSWM.Split('▲')[4] != "")
            {

                HisBltNode blt = new HisBltNode();
                blt.DisplayName = "其他玻璃体视网膜病变";

                blt.CaseId = thiscase.ID;

                //blt.FistDia = BasicHelper.checkDate(left.BLTSWM.Split('▲')[3]);


                blt.Name = left.BLTSWM.Split('▲')[2];
                blt.RmkQt = left.BLTSWM.Split('▲')[4];

                blt.Lr = "左眼";




                bltlist.Add(blt);


                Link linkblt = new Link();
                linkblt.name = LinkStation.AddHisBltLink(thiscase.ID, blt.Lr);




                createLink.Add(linkblt);




            }


            if (right.BLTSWM.Split('▲')[2] != "" || right.BLTSWM.Split('▲')[4] != "")
            {

                HisBltNode blt = new HisBltNode();
                blt.DisplayName = "其他玻璃体视网膜病变";

                blt.CaseId = thiscase.ID;

                //blt.FistDia = new LocalDate(Convert.ToDateTime(BasicHelper.checkDate(right.BLTSWM.Split('▲')[3])));

                //blt.FistDia =BasicHelper.checkDate(right.BLTSWM.Split('▲')[3]);

                blt.Name = right.BLTSWM.Split('▲')[2];
                blt.RmkQt = right.BLTSWM.Split('▲')[4];

                blt.Lr = "右眼";





                bltlist.Add(blt);


                Link linkblt = new Link();
                linkblt.name = LinkStation.AddHisBltLink(thiscase.ID, blt.Lr);




                createLink.Add(linkblt);



            }




            #endregion



            #region  外伤史




            if (left.WSS.Split('▲')[0] != "")
            {

                HisEyeWssNode wss = new HisEyeWssNode();
                wss.DisplayName = "眼部外伤史";

                wss.CaseId = thiscase.ID;

                //wss.FistDia = new LocalDate(Convert.ToDateTime(BasicHelper.checkDate(left.WSS.Split('▲')[1])));


                wss.RmkQt = left.WSS.Split('▲')[0];

                wss.Lr = "左眼";





                eyewsslist.Add(wss);


                Link linkwss = new Link();
                linkwss.name = LinkStation.AddHisEyeWssLink(thiscase.ID, wss.Lr);




                createLink.Add(linkwss);




            }


            if (right.WSS.Split('▲')[0] != "")
            {

                HisEyeWssNode wss = new HisEyeWssNode();
                wss.DisplayName = "眼部外伤史";

                wss.CaseId = thiscase.ID;

                //wss.FistDia = new LocalDate(Convert.ToDateTime(BasicHelper.checkDate(right.WSS.Split('▲')[1])));


                wss.RmkQt = right.WSS.Split('▲')[0];

                wss.Lr = "右眼";


                eyewsslist.Add(wss);


                Link linkwss = new Link();
                linkwss.name = LinkStation.AddHisEyeWssLink(thiscase.ID, wss.Lr);




                createLink.Add(linkwss);


            }



            #endregion




            #region  眼科其它




            if (left.OTHER != "")
            {

                HisEyeOtherNode eyehisother = new HisEyeOtherNode();


                eyehisother.DisplayName = "眼科其他疾病";
                eyehisother.CaseId = thiscase.ID;



                eyehisother.RmkQt = left.OTHER;
                eyehisother.Lr = "左眼";






                eyeotherlist.Add(eyehisother);


                Link linkeyeother = new Link();
                linkeyeother.name = LinkStation.AddHisEyeOtherLink(thiscase.ID, eyehisother.Lr);




                createLink.Add(linkeyeother);







            }

            if (right.OTHER != "")
            {

                HisEyeOtherNode eyehisother = new HisEyeOtherNode();
                eyehisother.DisplayName = "眼科其他疾病";

                eyehisother.CaseId = thiscase.ID;

                eyehisother.RmkQt = right.OTHER;
                eyehisother.Lr = "右眼";



                eyeotherlist.Add(eyehisother);


                Link linkeyeother = new Link();
                linkeyeother.name = LinkStation.AddHisEyeOtherLink(thiscase.ID, eyehisother.Lr);




                createLink.Add(linkeyeother);


            }



            #endregion




            #endregion







            #region  对接眼部检查





            IList<EyeCheckSlModels> eyesl = EyeCheckSlDAL.GetBoth(id);

            IList<EyeCheckYyModels> eyeyy = EyeCheckYyDAL.GetBoth(id);
            IList<EyeCheckOtherModels> eyeother = EyeCheckOtherDAL.GetBoth(id);
            IList<EyeCheckYdModels> eyeyd = EyeCheckYdDAL.GetBoth(id);




            EyeCheckSlModels leftSl = new EyeCheckSlModels();

            EyeCheckSlModels rightSl = new EyeCheckSlModels();


            EyeCheckYyModels leftYy = new EyeCheckYyModels();

            EyeCheckYyModels rightYy = new EyeCheckYyModels();


            EyeCheckOtherModels leftOther = new EyeCheckOtherModels();

            EyeCheckOtherModels rightOther = new EyeCheckOtherModels();


            EyeCheckYdModels leftYd = new EyeCheckYdModels();

            EyeCheckYdModels rightYd = new EyeCheckYdModels();




            for (int i = 0; i < eyesl.Count; i++)
            {
                if (eyesl[i].L_R == 1)
                    leftSl = eyesl[i];
                else
                    rightSl = eyesl[i];
            }



            for (int i = 0; i < eyeyy.Count; i++)
            {
                if (eyesl[i].L_R == 1)
                    leftYy = eyeyy[i];
                else
                    rightYy = eyeyy[i];
            }


            for (int i = 0; i < eyeother.Count; i++)
            {
                if (eyeother[i].L_R == 1)
                    leftOther = eyeother[i];
                else
                    rightOther = eyeother[i];
            }


            for (int i = 0; i < eyeyd.Count; i++)
            {
                if (eyeyd[i].L_R == 1)
                    leftYd = eyeyd[i];
                else
                    rightYd = eyeyd[i];
            }


            #region  视力




            if (leftSl.STATUS == 1)
            {

                SLCheckNode temp = new SLCheckNode();


                temp.DisplayName = "视力检查";


                temp.CaseId = thiscase.ID;




                temp.Lr = LrTran(leftSl.L_R);


                temp.Jiaozheng = leftSl.JIAOZHENG;
                temp.Luoyan = leftSl.LUOYAN;
                temp.XianranDS = leftSl.XIANRAN_DS;
                temp.XianranSL = leftSl.XIANRAN_SL;
                temp.XianranZJDS = leftSl.XIANRAN_ZJDS;
                temp.XianranZX = leftSl.XIANRAN_ZX;






                sllist.Add(temp);


                Link linksl = new Link();
                linksl.name = LinkStation.AddSlLink(thiscase.ID, temp.Lr);





                createLink.Add(linksl);





            }

            if (rightSl.STATUS == 1)
            {

                SLCheckNode temp = new SLCheckNode();


                temp.DisplayName = "视力检查";


                temp.CaseId = thiscase.ID;




                temp.Lr = LrTran(rightSl.L_R);


                temp.Jiaozheng = rightSl.JIAOZHENG;
                temp.Luoyan = rightSl.LUOYAN;
                temp.XianranDS = rightSl.XIANRAN_DS;
                temp.XianranSL = rightSl.XIANRAN_SL;
                temp.XianranZJDS = rightSl.XIANRAN_ZJDS;
                temp.XianranZX = rightSl.XIANRAN_ZX;

                sllist.Add(temp);


                Link linksl = new Link();
                linksl.name = LinkStation.AddSlLink(thiscase.ID, temp.Lr);





                createLink.Add(linksl);

            }




            #endregion


            #region  眼压


            if (leftYy.STATUS == 1)
            {

                YYCheckNode temp = new YYCheckNode();


                temp.DisplayName = "眼压检查";


                temp.CaseId = thiscase.ID;




                temp.Lr = LrTran(leftYy.L_R);


                temp.Info = leftYy.INFO;




                yylist.Add(temp);


                Link linkyy = new Link();
                linkyy.name = LinkStation.AddYyLink(thiscase.ID, temp.Lr);





                createLink.Add(linkyy);




            }

            if (rightYy.STATUS == 1)
            {
                YYCheckNode temp = new YYCheckNode();
                temp.DisplayName = "眼压检查";
                temp.CaseId = thiscase.ID;
                temp.Lr = LrTran(rightYy.L_R);
                temp.Info = rightYy.INFO;








                yylist.Add(temp);


                Link linkyy = new Link();
                linkyy.name = LinkStation.AddYyLink(thiscase.ID, temp.Lr);





                createLink.Add(linkyy);









            }

            //await eyecheckdao.AddYyLink(thiscase.ID);

            #endregion

            //#region  眼部前节


            //if (leftOther.STATUS == 2)
            //{
            //    List<string> tar_left = new List<string>();
            //    if (leftOther.JMCX != 0)
            //        tar_left.Add("结膜充血");
            //    if (leftOther.JMTM != 0)
            //        tar_left.Add("角膜不透明");
            //    if (leftOther.KP != 0)
            //        tar_left.Add("KP");
            //    if (leftOther.TYN != 0)
            //        tar_left.Add("Tyn");
            //    if (leftOther.TK != 0)
            //        tar_left.Add("瞳孔不圆");
            //    if (leftOther.TKGF != 0)
            //        tar_left.Add("瞳孔对光反射消失");
            //    if (leftOther.HMYS != 0)
            //        tar_left.Add("虹膜异色");
            //    if (leftOther.HMXG != 0)
            //        tar_left.Add("虹膜新生血管");
            //    if (leftOther.RGJT != 0)
            //        tar_left.Add("人工晶体");
            //    if (leftOther.BLT != 0)
            //        tar_left.Add("玻璃体浑浊");
            //    //if (leftOther.JZT != 0)
            //    //    tar_left.Add("晶状体浑浊");

            //    if (tar_left.Count > 0)
            //    {


            //        Link linkybqj = new Link();
            //        linkybqj.name = LinkStation.AddYbqjLink(thiscase.ID, tar_left, "左眼");





            //        createLink.Add(linkybqj);
            //        //await eyecheckdao.AddYbqjLink(thiscase.ID, tar_left, "左眼");


            //    }

            //}


            //if (rightOther.STATUS == 2)
            //{
            //    List<string> tar_right = new List<string>();
            //    if (rightOther.JMCX != 0)
            //        tar_right.Add("结膜充血");
            //    if (rightOther.JMTM != 0)
            //        tar_right.Add("角膜不透明");
            //    if (rightOther.KP != 0)
            //        tar_right.Add("KP");
            //    if (rightOther.TYN != 0)
            //        tar_right.Add("Tyn");
            //    if (rightOther.TK != 0)
            //        tar_right.Add("瞳孔不圆");
            //    if (rightOther.TKGF != 0)
            //        tar_right.Add("瞳孔对光反射消失");
            //    if (rightOther.HMYS != 0)
            //        tar_right.Add("虹膜异色");
            //    if (rightOther.HMXG != 0)
            //        tar_right.Add("虹膜新生血管");
            //    if (rightOther.RGJT != 0)
            //        tar_right.Add("人工晶体");
            //    if (rightOther.BLT != 0)
            //        tar_right.Add("玻璃体浑浊");
            //    //if (rightOther.JZT != 0)
            //    //    tar_right.Add("晶状体浑浊");

            //    if (tar_right.Count > 0)
            //    {


            //        Link linkybqj = new Link();
            //        linkybqj.name = LinkStation.AddYbqjLink(thiscase.ID, tar_right, "右眼");





            //        createLink.Add(linkybqj);
            //    }


            //}
            //#endregion

            //#region  眼底



            //if (leftYd.STATUS == 2)
            //{
            //    List<string> tar_left = new List<string>();
            //    if (leftYd.SPXR != 0)
            //        tar_left.Add("结膜充血");
            //    if (leftYd.SPYS == 1)
            //        tar_left.Add("视盘色淡");
            //    if (leftYd.SPYS == 2)
            //        tar_left.Add("视盘苍白");
            //    if (leftYd.SPBJQX != 0)
            //        tar_left.Add("视盘边界不清晰");
            //    if (leftYd.SPSZ != 0)
            //        tar_left.Add("视盘水肿");
            //    if (leftYd.BWZ != 0)
            //        tar_left.Add("豹纹状眼底");
            //    if (leftYd.HGMPTZ != 0)
            //        tar_left.Add("后巩膜葡萄肿");
            //    if (leftYd.BLTCX != 0)
            //        tar_left.Add("玻璃体积血");
            //    if (leftYd.SWMQCX != 0)
            //        tar_left.Add("视网膜前出血");
            //    if (leftYd.SWMTL != 0)
            //        tar_left.Add("视网膜脱离");
            //    if (leftYd.SWMXY != 0)
            //        tar_left.Add("视网膜下液");
            //    if (leftYd.SWMCX != 0)
            //        tar_left.Add("视网膜出血");
            //    if (leftYd.SWMSC != 0)
            //        tar_left.Add("视网膜渗出");
            //    if (leftYd.SWMXSXG != 0)
            //        tar_left.Add("视网膜新生血管");
            //    if (leftYd.HBBX != 0)
            //        tar_left.Add("黄斑变性");
            //    if (leftYd.HBLK != 0)
            //        tar_left.Add("黄斑裂孔");
            //    if (leftYd.HBQM != 0)
            //        tar_left.Add("黄斑前膜");
            //    if (leftYd.HBSZ != 0)
            //        tar_left.Add("黄斑水肿");
            //    if (leftYd.HBCNV != 0)
            //        tar_left.Add("黄斑CNV");
            //    if (tar_left.Count > 0)
            //    {


            //        Link linkyd = new Link();
            //        linkyd.name = LinkStation.AddYdLink(thiscase.ID, tar_left, "左眼");





            //        createLink.Add(linkyd);

            //    }

            //}

            //if (rightYd.STATUS == 2)
            //{
            //    List<string> tar_right = new List<string>();
            //    if (rightYd.SPXR != 0)
            //        tar_right.Add("结膜充血");
            //    if (rightYd.SPYS == 1)
            //        tar_right.Add("视盘色淡");
            //    if (rightYd.SPYS == 2)
            //        tar_right.Add("视盘苍白");
            //    if (rightYd.SPBJQX != 0)
            //        tar_right.Add("视盘边界不清晰");
            //    if (rightYd.SPSZ != 0)
            //        tar_right.Add("视盘水肿");
            //    if (rightYd.BWZ != 0)
            //        tar_right.Add("豹纹状眼底");
            //    if (rightYd.HGMPTZ != 0)
            //        tar_right.Add("后巩膜葡萄肿");
            //    if (rightYd.BLTCX != 0)
            //        tar_right.Add("玻璃体积血");
            //    if (rightYd.SWMQCX != 0)
            //        tar_right.Add("视网膜前出血");
            //    if (rightYd.SWMTL != 0)
            //        tar_right.Add("视网膜脱离");
            //    if (rightYd.SWMXY != 0)
            //        tar_right.Add("视网膜下液");
            //    if (rightYd.SWMCX != 0)
            //        tar_right.Add("视网膜出血");
            //    if (rightYd.SWMSC != 0)
            //        tar_right.Add("视网膜渗出");
            //    if (rightYd.SWMXSXG != 0)
            //        tar_right.Add("视网膜新生血管");
            //    if (rightYd.HBBX != 0)
            //        tar_right.Add("黄斑变性");
            //    if (rightYd.HBLK != 0)
            //        tar_right.Add("黄斑裂孔");
            //    if (rightYd.HBQM != 0)
            //        tar_right.Add("黄斑前膜");
            //    if (rightYd.HBSZ != 0)
            //        tar_right.Add("黄斑水肿");
            //    if (rightYd.HBCNV != 0)
            //        tar_right.Add("黄斑CNV");

            //    if (tar_right.Count > 0)
            //    {


            //        Link linkyd = new Link();
            //        linkyd.name = LinkStation.AddYdLink(thiscase.ID, tar_right, "右眼");





            //        createLink.Add(linkyd);
            //    }

            //}





            //#endregion


            //#region  特殊节点



            ////眼肿瘤和晶体混浊
            //if (leftOther.JZT != 0)
            //{
            //    EyeJztNode temp = new EyeJztNode();


            //    temp.DisplayName = "晶状体混浊";


            //    temp.CaseId = thiscase.ID;

            //    temp.Lr = "左眼";



            //    temp.Rmk = "PSC后囊下白内障" + BasicHelper.StatusCcNcPsc(1, leftOther.PSC) + ";CC皮质白内障" + BasicHelper.StatusCcNcPsc(1, leftOther.CC) + ";NC核性白内障" + BasicHelper.StatusCcNcPsc(1, leftOther.NC) + ";位置不详" + BasicHelper.HisStatusTran(leftOther.WZBX);



            //    temp.Vector = leftOther.PSC + "," + leftOther.CC + "," + leftOther.NC + "," + leftOther.WZBX;


            //    jztlist.Add(temp);

            //    Link linkjzt = new Link();
            //    linkjzt.name = LinkStation.AddEyeJztLink(thiscase.ID, "左眼");





            //    createLink.Add(linkjzt);


            //}
            //if (rightOther.JZT != 0)
            //{

            //    EyeJztNode temp = new EyeJztNode();


            //    temp.DisplayName = "晶状体混浊";


            //    temp.CaseId = thiscase.ID;

            //    temp.Lr = "右眼";



            //    temp.Rmk = "PSC后囊下白内障" + BasicHelper.StatusCcNcPsc(1, leftOther.PSC) + ";CC皮质白内障" + BasicHelper.StatusCcNcPsc(1, leftOther.CC) + ";NC核性白内障" + BasicHelper.StatusCcNcPsc(1, leftOther.NC) + ";位置不详" + BasicHelper.HisStatusTran(leftOther.WZBX);


            //    temp.Vector = leftOther.PSC + "," + leftOther.CC + "," + leftOther.NC + "," + leftOther.WZBX;





            //    jztlist.Add(temp);

            //    Link linkjzt = new Link();
            //    linkjzt.name = LinkStation.AddEyeJztLink(thiscase.ID, "右眼");





            //    createLink.Add(linkjzt);



            //}






            //if (leftYd.CANCER_STATUS != 0)
            //{
            //    EyeCancerNode temp = new EyeCancerNode();


            //    temp.DisplayName = "眼底检查眼内肿瘤";


            //    temp.CaseId = thiscase.ID;


            //    var CANCER_INFO_ZLWZ = leftYd.CANCER_INFO.Split('▲')[0];//拆分的肿瘤位置
            //    var CANCER_INFO_LTXZ = leftYd.CANCER_INFO.Split('▲')[1];//拆分的瘤体形态
            //    var CANCER_INFO_BMSS = leftYd.CANCER_INFO.Split('▲')[2];//拆分的表面色素


            //    temp.Rmk = "肿瘤位置:" + CANCER_INFO_ZLWZ + ";瘤体形态:" + CANCER_INFO_LTXZ + ";表面色素:" + CANCER_INFO_BMSS;


            //    temp.Lr = "左眼";



            //    eyecancerlist.Add(temp);

            //    Link linkeyecancer = new Link();
            //    linkeyecancer.name = LinkStation.AddEyeCancerLink(thiscase.ID, "左眼");





            //    createLink.Add(linkeyecancer);




            //}
            //if (rightYd.CANCER_STATUS != 0)
            //{

            //    EyeCancerNode temp = new EyeCancerNode();


            //    temp.DisplayName = "眼底检查眼内肿瘤";


            //    temp.CaseId = thiscase.ID;


            //    var CANCER_INFO_ZLWZ = rightYd.CANCER_INFO.Split('▲')[0];//拆分的肿瘤位置
            //    var CANCER_INFO_LTXZ = rightYd.CANCER_INFO.Split('▲')[1];//拆分的瘤体形态
            //    var CANCER_INFO_BMSS = rightYd.CANCER_INFO.Split('▲')[2];//拆分的表面色素


            //    temp.Rmk = "肿瘤位置:" + CANCER_INFO_ZLWZ + ";瘤体形态:" + CANCER_INFO_LTXZ + ";表面色素:" + CANCER_INFO_BMSS;

            //    temp.Lr = "右眼";




            //    eyecancerlist.Add(temp);

            //    Link linkeyecancer = new Link();
            //    linkeyecancer.name = LinkStation.AddEyeCancerLink(thiscase.ID, "右眼");





            //    createLink.Add(linkeyecancer);





            //}








            //bool doleft = false;

            //bool doright = false;




            //if (leftOther.RMK.Trim() != "")
            //{

            //    EyeSpecialNode temp = new EyeSpecialNode();


            //    temp.DisplayName = "眼部前节检查其他异常";


            //    temp.CaseId = thiscase.ID;

            //    temp.Lr = "左眼";



            //    temp.Rmk = leftOther.RMK.Trim();





            //    eyespeciallist.Add(temp);


            //    //await eyecheckdao.AddEyeSpecial(temp);

            //    doleft = true;


            //}
            //if (rightOther.RMK.Trim() != "")
            //{
            //    EyeSpecialNode temp = new EyeSpecialNode();


            //    temp.DisplayName = "眼部前节检查其他异常";


            //    temp.CaseId = thiscase.ID;

            //    temp.Lr = "右眼";



            //    temp.Rmk = rightOther.RMK.Trim();




            //    eyespeciallist.Add(temp);



            //    //await eyecheckdao.AddEyeSpecial(temp);

            //    doright = true;

            //}




















            //if (leftYd.INFO.Trim() != "")
            //{

            //    EyeSpecialNode temp = new EyeSpecialNode();


            //    temp.DisplayName = "眼底检查其他";


            //    temp.CaseId = thiscase.ID;




            //    temp.Rmk = leftYd.INFO;


            //    temp.Lr = "左眼";



            //    eyespeciallist.Add(temp);


            //    //await eyecheckdao.AddEyeSpecial(temp);

            //    doleft = true;







            //}
            //if (rightYd.INFO.Trim() != "")
            //{
            //    EyeSpecialNode temp = new EyeSpecialNode();


            //    temp.DisplayName = "眼底检查其他";


            //    temp.CaseId = thiscase.ID;




            //    temp.Rmk = rightYd.INFO;


            //    temp.Lr = "右眼";



            //    eyespeciallist.Add(temp);


            //    //await eyecheckdao.AddEyeSpecial(temp);

            //    doright = true;


            //}




            //if (doleft)
            //{

            //    Link linkeyecancer = new Link();
            //    linkeyecancer.name = LinkStation.AddEyeSpecialLink(thiscase.ID, "左眼");





            //    createLink.Add(linkeyecancer);



            //    //await eyecheckdao.AddEyeSpecialLink(thiscase.ID, "左眼");



            //}

            //if (doright)
            //{

            //    Link linkeyecancer = new Link();
            //    linkeyecancer.name = LinkStation.AddEyeSpecialLink(thiscase.ID, "右眼");





            //    createLink.Add(linkeyecancer);
            //    //await eyecheckdao.AddEyeSpecialLink(thiscase.ID, "右眼");


            //}


            //#endregion


            #endregion

            return id;




        }


        public static string LrTran(int lr)
        {


            if (lr == 0)
            {

                return "右眼";
            }
            else if (lr == 1)
            {

                return "左眼";

            }
            else
            {
                return "未选定";


            }
        }









    }
}
