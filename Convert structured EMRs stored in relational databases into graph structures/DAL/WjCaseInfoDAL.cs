using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using WebApi.Models;

namespace DAL
{
    public class WjCaseInfoDAL
    {



        //    public static long AddCaseInfo(WjCaseInfoModels model)
        //    {

        //        string sql = "";
        //        sql += @"insert into wj_case_info(CASE_NAME,SEX ,CONTACT , RACE,MARRIAGE,JOB,EDU,ADDRESS,TYPE_ID) values('"
        // + model.CASE_NAME + "'," + model.SEX + ",'" + model.CONTACT + "'," + "'"

        //+ model.RACE + "','" + model.MARRIAGE + "','" + model.JOB + "','" + model.EDU + "','" + model.ADDRESS + "'," + model.TYPE_ID + ")";


        //        return DbSql.AddandLastId("cc_sys", sql);
        //    }

        #region 获取wj_case_info表总数据条数

        public static int DataSumCountCaseInfoWj(int typeId, int status, string caseName)
        {
            string sql = "select count(*) from wj_case_info  where 1=1";



            sql += " and TYPE_ID=" + typeId;


            if (!string.IsNullOrEmpty(caseName))
            {
                sql += "  and CASE_NAME='" + caseName + "'";


            }


            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }

        #endregion



        #region  眼科病和近视的wj_case_info分页 
        public static IList<WjCaseInfoModels> GetAllPageCaseInfoWj(int typeId, int status, string caseName, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from wj_case_info where 1=1";

            sql += " and TYPE_ID=" + typeId;

         
            if (!string.IsNullOrEmpty(caseName))
            {
                sql += "  and CASE_NAME='" + caseName + "'";


            }

    
            sql += " and STATUS=" + status;

            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<WjCaseInfoModels> list = ListConvertToModel(dt);

            return list;

        }
        #endregion

        #region 获取WjCaseInfoDAL数据模型

        public static IList<WjCaseInfoModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<WjCaseInfoModels> ts = new List<WjCaseInfoModels>();
            // 获得此模型的类型   
            Type type = typeof(WjCaseInfoModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                WjCaseInfoModels t = new WjCaseInfoModels();
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

        #endregion


        #region


        //public static IList<WjCaseInfoDAL> TranToUse(IList<WjCaseInfoDAL> org, string resumeName)
        //{

        //    IList<WjCaseInfoDAL> res = new List<WjCaseInfoDAL>();
        //    for (int i = 0; i < org.Count; i++)
        //    {
        //        WjCaseInfoDAL temp = new WjCaseInfoDAL();

        //        long caseId = org[i].ID;
        //        IList<BioImageModels> imagelist = BioImageDAL.GetAllImages(caseId);

        //        temp.MedicalTypes = new List<string>();

        //        temp.CASE_INFO = org[i];

        //        temp.PUBLISH = PriUserDAL.GetNicknameById(org[i].PUBLISHER_ID);
        //        temp.PASS = PriUserDAL.GetNicknameById(org[i].PASS_ID);

        //        temp.ResumeImage = 0;

        //        for (int j = 0; j < imagelist.Count; j++)
        //        {
        //            if (imagelist[j].TYPE_NAME == resumeName)
        //            {
        //                temp.ResumeImage = 1;


        //            }
        //            else
        //            {
        //                temp.MedicalTypes.Add(imagelist[j].TYPE_NAME);


        //            }



        //        }
        //        //HashSet<string> hs = new HashSet<string>(temp.MedicalTypes);

        //        if (temp.MedicalTypes.Count > 0)
        //        {
        //            temp.MedicalTypes = temp.MedicalTypes.Distinct().ToList(); //去重复，绑定数据后面要加ToList()

        //        }




        //        res.Add(temp);

        //    }




        //    return res;

        //}

        #endregion

        #region 获取caseinfowj所有信息

        public static WjCaseInfoModels GetFirst(long id)
        {
            string sql = "select * from wj_case_info where ID=" + id;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            WjCaseInfoModels model = new WjCaseInfoModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }


        #endregion

    }
}
