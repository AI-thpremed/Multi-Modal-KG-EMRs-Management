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
    public class WjHisQsGxyDAL
    {



        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="model"></param>
        //        public static int AddHisQsGxy(WjHisQsGxyModels model)
        //        {
        //            string sql = "";
        //            /*
        //                        sql = @"insert into wj_his_qs_gxy(CASE_ID,STATUS,FIST_DIA,MED,MED_INFO,RMK,UPDATE_ID,UPDATE_DATE ) values("
        //            + model.CASE_ID + "," + model.STATUS + ",'" + model.FIST_DIA.ToString("yyyy-MM-dd") + "'," + model.MED + ",'" + model.MED_INFO + "','" + model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
        //            */

        //            sql = @"insert into wj_his_qs_gxy(CASE_ID,STATUS,FIST_DIA,MED,RMK) values("
        //+ model.CASE_ID + "," + model.STATUS + ",'" + model.FIST_DIA.ToString("yyyy-MM-dd") + "'," + model.MED + ",'" + model.RMK + "')";


        //            return DbSql.AddOrUpdOrDel("cc_sys", sql);

        //        }
        #region  
        public static WjHisQsGxyModels GetFirst(long caseid)
        {
            string sql = "select * from wj_his_qs_gxy where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            WjHisQsGxyModels model = new WjHisQsGxyModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }
        #endregion


        #region 获取高血ya数据包装
        public static IList<WjHisQsGxyModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<WjHisQsGxyModels> ts = new List<WjHisQsGxyModels>();
            // 获得此模型的类型   
            Type type = typeof(WjHisQsGxyModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                WjHisQsGxyModels t = new WjHisQsGxyModels();
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

    }
}
