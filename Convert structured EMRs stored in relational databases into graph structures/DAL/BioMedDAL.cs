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
    public class BioMedDAL
    {







        #region  分页
        public static IList<BioMedModels> GetAllPage(string caseId, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from bio_med where 1=1";

            if (!string.IsNullOrEmpty(caseId))
            {
                sql += " and CASE_ID=" + Convert.ToInt64(caseId);


            }



            sql += " and IS_DELETE=0";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<BioMedModels> list = ListConvertToModel(dt);

            return list;

        }




        public static int DataSumCount(string setStatus)
        {
            string sql = "select count(*) from bio_med  where 1=1";




            sql += " and IS_DELETE=0";

            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }


        #endregion



        public static IList<BioMedModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<BioMedModels> ts = new List<BioMedModels>();
            // 获得此模型的类型   
            Type type = typeof(BioMedModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                BioMedModels t = new BioMedModels();
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



        public static BioMedModels GetFirst(int id)
        {
            string sql = "select * from bio_med where ID=" + id;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            BioMedModels model = new BioMedModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }



         


        /// <summary>
        /// 删除不显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteBioMed(int id)
        {
            string sql = "update bio_med set IS_DELETE =1,  UPDATE_DATE ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }







        /////// <summary>
        /////// 修改或者添加数据
        /////// </summary>
        /////// <param name="model"></param>
        //public static int AddOrUpdBioMed(BioMedModels model)
        //{
        //    string sql = "";
        //    if (model.ID > 0)
        //    {
        //        sql = "update bio_med set NAME = '" + model.NAME + "',RMK = '" + model.RMK + "',   UPDATE_DATE = '" + DateTime.Now + "' where ID=" + model.ID;

        //    }
        //    else
        //    {


        //        sql = @"insert into bio_med(NAME  ,RMK  ,IS_DELETE  ,UPDATE_DATE  ) values('" + model.NAME + "','" + model.RMK + "'," + model.IS_DELETE + ",'" + DateTime.Now + "')";
        //    }
        //    return DbSql.AddOrUpdOrDel("cc_sys", sql);
        //}































    }
}
