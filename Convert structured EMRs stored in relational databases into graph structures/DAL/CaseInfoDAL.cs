using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace DAL
{
    public class CaseInfoDAL
    {









        #region  daemon用法

        public static IList<CaseInfoModels> GetAllTag(string createDate)
        {


            string sql = "select * from case_info where 1=1";



            sql += " and CREATE_DATE>='" + Convert.ToDateTime(createDate) + "'";
      

            sql += " order by ID desc";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }




        public static int UnTag(long id,string res)
        {
            string sql = "update case_info set RESULT ='"+res+"'  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        #endregion












        #region 泛化通用方法

        //姓名泛化
        public static string nameTran(string name)
        {

            if (name.Length >= 3)
            {
                return name.Substring(0, 1) + "*" + name.Substring(name.Length - 1, 1);
            }
            else
            {
                return name.Substring(0, 1) + "*";

            }





        }


        #endregion


        #region  分页   临时库
        public static IList<CaseInfoModels> GetAllPageTemp(int typeId, int status, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }


            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }




            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumCountTemp(int typeId, int status, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }


            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }


            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion





        #region  分页 录入员  临时库


        public static IList<CaseInfoModels> GetAllPageTempLuru(int typeId, string userId,int auditS, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sql += "  and PUBLISHER_ID=" + Convert.ToInt64(userId);
            }
            else
            {
                sql += "  and PUBLISHER_ID=0";
            }

            if (auditS == 0 || auditS == 1)
            {
                sql += " and STATUS=0";

                sql += " and (AUDIT=0 OR AUDIT=3)";

            }
            else if (auditS == 2)
            {
                sql += " and STATUS=0";

                sql += " and AUDIT=1";

            }
            else {
                sql += " and STATUS=1";

                sql += " and AUDIT=2";

            }

            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumCountTempLuru(int typeId, string userId, int auditS, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }



            if (!string.IsNullOrEmpty(userId))
            {
                sql += "  and PUBLISHER_ID=" + Convert.ToInt64(userId);


            }
            else
            {
                sql += "  and PUBLISHER_ID=0";


            }


            if (auditS == 0 || auditS == 1)
            {
                sql += " and STATUS=0";

                sql += " and (AUDIT=0 OR AUDIT=3)";

            }
            else if (auditS == 2)
            {
                sql += " and STATUS=0";

                sql += " and AUDIT=1";

            }
            else
            {
                sql += " and STATUS=1";

                sql += " and AUDIT=2";

            }
            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion






        #region  分页 录入审核员  临时库


        public static IList<CaseInfoModels> GetAllPageTempShenhe(int typeId, string userId,  int auditS,string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }

            if (!string.IsNullOrEmpty(userId))
            {
                sql += "  and PASS_ID=" + Convert.ToInt64(userId);


            }
            else
            {
                sql += "  and PASS_ID=0";


            }

            if (auditS == 0 || auditS == 1)
            {

                sql += " and STATUS=0";

                sql += " and AUDIT=1";

            }
            else if (auditS == 2)
            {

                sql += " and  STATUS=1 and AUDIT=2";

            }
            else
            {
                sql += " and STATUS=0 and  AUDIT=3";

            }


            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumCountTempShenhe(int typeId, string userId, int auditS, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }


            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }



            if (!string.IsNullOrEmpty(userId))
            {
                sql += "  and PASS_ID=" + Convert.ToInt64(userId);


            }
            else
            {
                sql += "  and PASS_ID=0";


            }


            if (auditS == 0 || auditS == 1)
            {

                sql += " and STATUS=0";

                sql += " and AUDIT=1";

            }
            else if (auditS == 2)
            {

                sql += " and  STATUS=1 and AUDIT=2";

            }
            else
            {
                sql += " and STATUS=0 and  AUDIT=3";

            }

            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion









        #region  分页   正式库 
        public static IList<CaseInfoModels> GetAllPage(int typeId, int status, string sysId, string startHopDate, string endHopDate, string startUpdDate,
            string endUpdDate, string startPassDate, string endPassDate, string lrId, string shId,
            int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }
            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }

            if (!string.IsNullOrEmpty(startPassDate) && !string.IsNullOrEmpty(endPassDate))
            {
                sql += " and PASS_DATE>='" + Convert.ToDateTime(startPassDate) + "' and PASS_DATE<='" + Convert.ToDateTime(endPassDate) + "'";
            }

            if (!string.IsNullOrEmpty(lrId))
            {
                sql += "  and PUBLISHER_ID =" + Convert.ToInt64(lrId);
            }
            if (!string.IsNullOrEmpty(shId))
            {
                sql += "  and PASS_ID =" + Convert.ToInt64(shId);
            }



            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumCount(int typeId, int status, string sysId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate,
            string startPassDate, string endPassDate, string lrId, string shId)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }
            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }

            if (!string.IsNullOrEmpty(startPassDate) && !string.IsNullOrEmpty(endPassDate))
            {
                sql += " and PASS_DATE>='" + Convert.ToDateTime(startPassDate) + "' and PASS_DATE<='" + Convert.ToDateTime(endPassDate) + "'";
            }

            if (!string.IsNullOrEmpty(lrId))
            {
                sql += "  and PUBLISHER_ID =" + Convert.ToInt64(lrId);
            }
            if (!string.IsNullOrEmpty(shId))
            {
                sql += "  and PASS_ID =" + Convert.ToInt64(shId);
            }

            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion




        #region  分页   标准库
        public static IList<CaseInfoModels> GetAllFinal(int typeId, int status, string sysId, string startHopDate, string endHopDate, string startUpdDate,string endUpdDate,
            int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }
            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }



            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumFinal(int typeId, int status, string sysId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }
            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }

     

            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion




        #region  按照不同的库分类全部拉取然后做统计


        public static int DataStatCount(int typeId, int status)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;




            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #region 录入员：统计录入条数方法  统计 我作为录入员当前的数据


        public static int TongJiLuRuCount(int typeId, string auditStr, long userId)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;



            sql += " and PUBLISHER_ID=" + userId;


            if (!string.IsNullOrEmpty(auditStr))
            {
                sql += " and AUDIT=" + Convert.ToInt32(auditStr);


            }



            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion

        #region 审核员 数据
        public static int ShenHeCount(int typeId, string auditStr, long userId)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;



            sql += " and PASS_ID=" + userId;


            if (!string.IsNullOrEmpty(auditStr))
            {
                sql += " and AUDIT=" + Convert.ToInt32(auditStr);


            }



            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }

        #endregion


        #endregion





        public static IList<CaseInfoModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<CaseInfoModels> ts = new List<CaseInfoModels>();
            // 获得此模型的类型   
            Type type = typeof(CaseInfoModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                CaseInfoModels t = new CaseInfoModels();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }







        //这个方法拿了图片信息
        public static IList<CaseInfoModelsUse> TranToUse(IList<CaseInfoModels> org, string resumeName)
        {

            //第一步拿到所有可用的restag

            IList<SetResTagModels> tags = SetResTagDAL.GetAll();

            //包装成#tag_id_e


            IList<CaseInfoModelsUse> res = new List<CaseInfoModelsUse>();


            for (int i = 0; i < org.Count; i++)
            {
                CaseInfoModelsUse temp = new CaseInfoModelsUse();
                
                long caseId = org[i].ID;
                IList<BioImageModels> imagelist = BioImageDAL.GetAllImages(caseId);

                temp.MedicalTypes = new List<string>();

                temp.Tags = new List<string>();
                
                //把  #tag_17_e,#tag_16_e,#tag_5_e,#tag_4_e  split 然后一个个对 插进去


                if (org[i].RESULT != "")
                {
                    string[] resultRegs = org[i].RESULT.Split(',');



                    for (int j = 0; j < resultRegs.Length; j++)
                    {
                        string regsId = resultRegs[j].Split('_')[1];
                        for (int y = 0; y < tags.Count; y++)
                        {
                            if (int.Parse(regsId) == tags[y].ID)
                            {
                                temp.Tags.Add("#"+ tags[y].NAME);
                                break;
                            }
                        }
                    }
                }


                org[i].CASE_NAME = AESHelper.Decode(org[i].CASE_NAME);

                if (org[i].CASE_NAME != "")
                {
                    org[i].CASE_NAME = nameTran(org[i].CASE_NAME);
                }

                temp.CASE_INFO = org[i];



                temp.PUBLISH = PriUserDAL.GetNicknameById(org[i].PUBLISHER_ID);
                temp.PASS = PriUserDAL.GetNicknameById(org[i].PASS_ID);

                temp.ResumeImage = 0;

                for (int j = 0; j < imagelist.Count; j++)
                {
                    if (imagelist[j].TYPE_NAME == resumeName)
                    {
                        temp.ResumeImage = 1;


                    }
                    else
                    {
                        temp.MedicalTypes.Add(imagelist[j].TYPE_NAME);


                    }



                }
                //HashSet<string> hs = new HashSet<string>(temp.MedicalTypes);

                if (temp.MedicalTypes.Count > 0)
                {
                    temp.MedicalTypes = temp.MedicalTypes.Distinct().ToList(); //去重复，绑定数据后面要加ToList()

                }




                res.Add(temp);

            }




            return res;

        }




        //这个是不拿图片信息的trantouse
        //public static IList<CaseInfoModelsUse> TranToUseNew(IList<CaseInfoModels> org)
        //{

        //    IList<CaseInfoModelsUse> res = new List<CaseInfoModelsUse>();
        //    for (int i = 0; i < org.Count; i++)
        //    {
        //        CaseInfoModelsUse temp = new CaseInfoModelsUse();

        //        long caseId = org[i].ID;
        //        IList<BioImageModels> imagelist = BioImageDAL.GetAllImages(caseId);

        //        temp.MedicalTypes = new List<string>();


        //        //RSA rsa = new RSA();
        //        org[i].CASE_NAME = AESHelper.Decode(org[i].CASE_NAME);

        //        if (org[i].CASE_NAME != "")
        //        {
        //            org[i].CASE_NAME = nameTran(org[i].CASE_NAME);
        //        }

        //        temp.CASE_INFO = org[i];



        //        temp.PUBLISH = PriUserDAL.GetNicknameById(org[i].PUBLISHER_ID);
        //        temp.PASS = PriUserDAL.GetNicknameById(org[i].PASS_ID);

        //        temp.ResumeImage = 0;


        //        //HashSet<string> hs = new HashSet<string>(temp.MedicalTypes);

        //        if (temp.MedicalTypes.Count > 0)
        //        {
        //            temp.MedicalTypes = temp.MedicalTypes.Distinct().ToList(); //去重复，绑定数据后面要加ToList()

        //        }




        //        res.Add(temp);

        //    }




        //    return res;

        //}




        public static CaseInfoModels GetFirst(long id)
        {
            string sql = "select * from case_info where ID=" + id;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            CaseInfoModels model = new CaseInfoModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }




        #region  单个删除

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteCaseInfo(long id)
        {
            string sql = "update case_info set IS_DELETE =1  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        public static int UnDeleteCaseInfo(long id)
        {
            string sql = "update case_info set IS_DELETE =0  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion

        #region    设置批量删除




        public static int DeleteCaseBatch(string caseIdStr)
        {
            string sql = "update case_info set IS_DELETE=1  where ID in(" + caseIdStr + ")";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }





        #endregion







        #region  单个转正式

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int TranStatusSingleZs(long id, int status)
        {
            string sql = "update case_info set STATUS=" + status + "  , PASS_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "',  AUDIT=2   where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion

        #region    设置批量转正式




        public static int TranStatusBatchZs(string caseIdStr, int status)
        {
            string sql = "update case_info set STATUS=" + status + " , PASS_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "',  AUDIT=2 where ID in(" + caseIdStr + ")";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }





        #endregion





        #region  单个转标注

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int TranStatusSingleBz(long id, int status)
        {
            string sql = "update case_info set STATUS=" + status + "  , FINAL_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion

        #region    设置批量转标注




        public static int TranStatusBatchBz(string caseIdStr, int status)
        {
            string sql = "update case_info set STATUS=" + status + " , FINAL_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where ID in(" + caseIdStr + ")";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }





        #endregion


        #region   通过名称和类型码找相同的病历


        public static IList<CaseInfoModels> GetAllByName(string name, int typeid)
        {


            string sql = "select * from case_info where 1=1 ";



            if (!string.IsNullOrEmpty(name))
            {

                sql += "  and CASE_NAME= '" + name + "'";
            }


            sql += " and TYPE_ID=" + typeid;




            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<CaseInfoModels> list = ListConvertToModel(dt);


            return list;
        }





        #endregion





        #region   批量检查编辑完成audit状态




        public static int CheckAuditStatus(string caseIdStr)
        {
            string sql = "select count(*) from case_info  where 1=1";
            sql += " and ID in(" + caseIdStr + ")";


            sql += " and AUDIT!=1";




            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion




        #region   检查同名人数




        public static int CheckExictName(string caseName, string caseId)
        {
            string sql = "select count(*) from case_info  where 1=1";
            sql += " and CASE_NAME='" + caseName + "'";
            if (!string.IsNullOrEmpty(caseId))
            {
                sql += " and ID!=" + Convert.ToInt64(caseId);
            }



            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion







        #region   添加和修改



        ///// <summary>
        ///// 添加数据     只有add的时候会有enrolldate 然后  在入学的操作里面做enroll 
        ///// </summary>
        ///// <param name="model"></param>
        public static long AddCaseInfo(CaseInfoModels model)
        {

            string sql = "";




            sql = @"insert into case_info( SYS_ID,CASE_NAME, TYPE_ID , SEX , HOP_ID ,CONTACT,CONTACT_2 , BIRTHDAY,WHO_HIS , OPH_HIS , SYMPTOM  , ADDRESS, RESULT , RACE,MARRIAGE,JOB,EDU,TAG_ID_L  ,TAG_ID_R,SOURCE_ID, STATUS , IS_DELETE,RMK, PUBLISHER_ID,  CREATE_DATE ,HOP_AGE,HOP_DATE) values('"
    + model.SYS_ID + "','" + model.CASE_NAME + "'," + model.TYPE_ID + "," + model.SEX + ",'" + model.HOP_ID + "','" + model.CONTACT + "','" + model.CONTACT_2 + "','" + model.BIRTHDAY
    + "','" + model.WHO_HIS + "','" + model.OPH_HIS + "','" + model.SYMPTOM + "','" + model.ADDRESS + "','" + model.RESULT + "','"
    + model.RACE + "','" + model.MARRIAGE + "','" + model.JOB + "','" + model.EDU + "','" + model.TAG_ID_L + "','" + model.TAG_ID_R + "'," + model.SOURCE_ID + "," + model.STATUS + "," + model.IS_DELETE + ",'"
                    + model.RMK + "'," + model.PUBLISHER_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + model.HOP_AGE + ",'" + model.HOP_DATE.ToString("yyyy-MM-dd") + "')";


            return DbSql.AddandLastId("cc_sys", sql);
        }

        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdCaseInfo(CaseInfoModels model)
        {

            string sql = "";

            sql = "update case_info set CASE_NAME = '" + model.CASE_NAME + "', SEX = " + model.SEX + ", HOP_ID = '" + model.HOP_ID
                + "', CONTACT = '" + model.CONTACT
                + "', CONTACT_2 = '" + model.CONTACT_2

                + "', BIRTHDAY = '" + model.BIRTHDAY
                                + "', HOP_DATE = '" + model.HOP_DATE.ToString("yyyy-MM-dd")

                + "', WHO_HIS = '" + model.WHO_HIS
                + "', OPH_HIS = '" + model.OPH_HIS
                + "', SYMPTOM = '" + model.SYMPTOM
                + "', ADDRESS = '" + model.ADDRESS
                + "', RESULT = '" + model.RESULT

                + "', RACE = '" + model.RACE
                + "', MARRIAGE = '" + model.MARRIAGE
                + "', JOB = '" + model.JOB
                + "', EDU = '" + model.EDU
                + "', HOP_AGE = " + model.HOP_AGE
                + ", HOP_DATE = '" + model.HOP_DATE



                + "', RMK = '" + model.RMK + "' where ID=" + model.ID;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);

        }






        #endregion



        #region   诊断结果分配



        /// <summary>
        /// 给病历添加tag诊断结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static int AddTagToL(long id, string tags)
        {
            string sql = "update case_info set TAG_ID_L='" + tags + "'   where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }






        /// <summary>
        /// 给病历添加tag诊断结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static int AddTagToR(long id, string tags)
        {
            string sql = "update case_info set TAG_ID_R='" + tags + "'   where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }





        public static int AddTagTo(long id, string tagsleft, string tagsright)
        {
            string sql = "update case_info set TAG_ID_L='" + tagsleft + "', TAG_ID_R='" + tagsright + "'   where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }





        #endregion




        #region   退回审核状态


        public static int EdiAudit(long id, int audit)
        {
            string sql = "update case_info set AUDIT=" + audit + "   where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion





        #region     编辑完成提交审核


        public static int FiniEdi(long id, int audit)
        {
            string sql = "update case_info set AUDIT=" + audit + "  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion



        #region     获取编辑权限


        public static int getEditRight(long id, long userId)
        {
            string sql = "update case_info set  PUBLISHER_ID=" + userId + "  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion







        #region     获取编辑审核权限


        public static int getAuditRight(long id, long userId)
        {
            string sql = "update case_info set  PASS_ID=" + userId + "  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion






        #region  分页 随访记录        



        public static IList<CaseInfoModels> GetPageSuifang(string caseHopid, int typeId, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;


            sql += "  and HOP_ID='" + caseHopid + "'";


            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumSuifang(string caseHopid, int typeId)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;



            sql += "  and HOP_ID='" + caseHopid + "'";




            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion









        #region   管理员功能区




        #region  管理员查看已经分配了录入员和审核员的病历


        public static IList<CaseInfoModels> GetAllOccupy(int typeId, int pageId, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }

     




            if (pageId == 0 )
            {

                sql += "  and PUBLISHER_ID!=0";
                sql += "  and (AUDIT=0 OR AUDIT=3)";

            }

            else
            {

                sql += "  and PASS_ID!=0";
                sql += "  and AUDIT=1";

            }

            sql += " and STATUS=0";

            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumOccupy(int typeId, int pageId, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }




            if (pageId == 0)
            {
                //没有编辑完

                sql += "  and PUBLISHER_ID!=0";
                sql += "  and (AUDIT=0 OR AUDIT=3)";



            }

            else
            {
                //编辑完 没有审核完

                sql += "  and PASS_ID!=0";

                sql += "  and AUDIT=1";


            }

            sql += " and STATUS=0";


            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion




        #region  管理员查看所有废弃case


        public static IList<CaseInfoModels> GetAllAbandoned(int typeId, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }






            sql += " and STATUS=0";

            sql += " and IS_DELETE=1";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }



        public static int DataSumAbandoned(int typeId, string sysId, string tagId, string startHopDate, string endHopDate, string startUpdDate, string endUpdDate)
        {
            string sql = "select count(*) from case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;

            if (!string.IsNullOrEmpty(sysId))
            {
                sql += "  and HOP_ID like '%" + sysId + "%'";


            }

            if (!string.IsNullOrEmpty(tagId))
            {
                sql += "  and RESULT like '%" + tagId + "%'";


            }

            if (!string.IsNullOrEmpty(startHopDate) && !string.IsNullOrEmpty(endHopDate))
            {
                sql += " and HOP_DATE>='" + Convert.ToDateTime(startHopDate) + "' and HOP_DATE<='" + Convert.ToDateTime(endHopDate) + "'";
            }

            if (!string.IsNullOrEmpty(startUpdDate) && !string.IsNullOrEmpty(endUpdDate))
            {
                sql += " and CREATE_DATE>='" + Convert.ToDateTime(startUpdDate) + "' and CREATE_DATE<='" + Convert.ToDateTime(endUpdDate) + "'";
            }



            sql += " and STATUS=0";


            sql += " and IS_DELETE=1";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion






        #endregion











        #region   添加标签标签



        public static int AddTagToCase(long id, string tags)
        {
            string sql = "update case_info set RESULT='" + tags + "'   where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }






        #endregion





        #region  问卷匹配过程  通过姓名和两个加密联系方式字段



        public static IList<CaseInfoModels> GetAllSurvey(string name,string contact , int typeid)
        {

            string sql = "select * from case_info where 1=1 ";



            if (!string.IsNullOrEmpty(name))
            {

                sql += "  and CASE_NAME= '" + name + "'";
            }


            if (!string.IsNullOrEmpty(contact))
            {

                sql += "  and (CONTACT= '" + contact + "' or CONTACT_2= '" + contact + "')";
            }

            sql += " and TYPE_ID=" + typeid;




            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<CaseInfoModels> list = ListConvertToModel(dt);


            return list;
        }








        #endregion







        #region  230404 做相似病例 和肖戴敏




        public static IList<long> GetAllPassXdm2()
        {



            string sql = "SELECT * from case_info_xdm  where 1=1 and TYPE_ID=0  AND (STATUS=1 OR STATUS=2) and IS_DELETE=0   AND tag_id_l ='res_l_2-' ";





            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<long> list = new List<long>();
            foreach (DataRow dr in dt.Rows)
            {

                list.Add(Convert.ToInt64(dr["ID"]));

            }

            return list;

        }







        public static IList<long> GetAllPassXdm()
        {



            string sql = "SELECT * from case_info_2  where 1=1 and TYPE_ID=0  AND (STATUS=1 OR STATUS=2) and IS_DELETE=0 ";





            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<long> list = new List<long>();
            foreach (DataRow dr in dt.Rows)
            {

                list.Add(Convert.ToInt64(dr["ID"]));

            }

            return list;

        }

        #endregion



        #region  病理性近视









        public static IList<long> GetAllPassPM()
        {



            string sql = "SELECT ID from case_info  where 1=1 and TYPE_ID=0  AND (STATUS=1 OR STATUS=2) and IS_DELETE=0 AND ( tag_id_l like '%res_l_7%'  or tag_id_r like '%res_r_7%')";

     



            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<long> list = new List<long>();
            foreach (DataRow dr in dt.Rows)
            {

                list.Add(Convert.ToInt64(dr["ID"]));

            }

            return list;

        }

        #endregion




        #region  AMD









        public static IList<long> GetAllPassAMD()
        {



            string sql = "SELECT ID from case_info  where 1=1 and TYPE_ID=0  AND (STATUS=1 OR STATUS=2) and IS_DELETE=0 AND ( tag_id_l like '%res_l_3%'  or tag_id_r like '%res_r_3%')";





            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<long> list = new List<long>();
            foreach (DataRow dr in dt.Rows)
            {

                list.Add(Convert.ToInt64(dr["ID"]));

            }

            return list;

        }

        #endregion



        #region  DR 非水肿









        public static IList<long> GetAllPassDR()
        {



            string sql = "SELECT ID from case_info  where 1=1 and TYPE_ID=0  AND (STATUS=1 OR STATUS=2) and IS_DELETE=0 AND ( tag_id_l like '%res_l_4%'  or tag_id_r like '%res_r_4%') AND ( tag_id_l not like '%糖尿病性黄斑水肿%'  and tag_id_r not like '%糖尿病性黄斑水肿%')";





            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<long> list = new List<long>();
            foreach (DataRow dr in dt.Rows)
            {

                list.Add(Convert.ToInt64(dr["ID"]));

            }

            return list;

        }

        #endregion





        #region  DME









        public static IList<long> GetAllPassDME()
        {



            string sql = "SELECT ID from case_info  where 1=1 and TYPE_ID=0  AND (STATUS=1 OR STATUS=2) and IS_DELETE=0 AND ( tag_id_l = 'res_l_4-糖尿病性黄斑水肿'  and tag_id_r = 'res_r_4-糖尿病性黄斑水肿')";





            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<long> list = new List<long>();
            foreach (DataRow dr in dt.Rows)
            {

                list.Add(Convert.ToInt64(dr["ID"]));

            }

            return list;

        }

        #endregion








        #region  分页   正式库 























        public static IList<long> GetAllPassData(int typeId, int status)
        {



            string sql = "select ID from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;



            sql += " and (STATUS=1 or STATUS=2)";

            sql += " and IS_DELETE=0";

            sql += " order by ID desc  limit 2000,4000";
            //sql += " order by ID desc  ";


            DataTable dt = DbSql.GetAll("cc_sys", sql);


            IList<long> list = new List<long>();
            foreach (DataRow dr in dt.Rows)
            {

                list.Add(Convert.ToInt64(dr["ID"]));

            }

            return list;

        }

        public static DataTable GetAllPassData2(int typeId, int status)
        {



            string sql = "select * from case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;



            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit 25";


            DataTable dt = DbSql.GetAll("cc_sys", sql);


 
            return dt;

        }


        #endregion


    }
}








