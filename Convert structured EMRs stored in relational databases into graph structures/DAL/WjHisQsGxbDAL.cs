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
    public class WjHisQsGxbDAL
    {


        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="model"></param>
        //        public static int AddHisQsGxb(WjHisQsGxbModels model)
        //        {
        //            string sql = "";
        //            /*
        //                        sql = @"insert into wj_his_qs_gxb(CASE_ID,STATUS,TYPE,FIST_DIA,RS_INFO,DMJR_INFO,MED,MED_INFO,RMK,UPDATE_ID,UPDATE_DATE ) values("
        //            + model.CASE_ID + "," + model.STATUS + ",'" + model.TYPE + "','" + model.FIST_DIA.ToString("yyyy-MM-dd") + "','" + model.RS_INFO + "','" + model.DMJR_INFO + "'," + model.MED + ",'" + model.MED_INFO + "','" + model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
        //            */

        //            sql = @"insert into wj_his_qs_gxb(CASE_ID,STATUS,TYPE,FIST_DIA,RS_INFO,DMJR_INFO,MED,RMK) values("
        //+ model.CASE_ID + "," + model.STATUS + ",'" + model.TYPE + "','" + model.FIST_DIA.ToString("yyyy-MM-dd") + "','" + model.RS_INFO + "','" + model.DMJR_INFO + "'," + model.MED + ",'" + model.RMK + "')";

        //            return DbSql.AddOrUpdOrDel("cc_sys", sql);

        //        }
        #region  获取肿瘤所有信息
        public static WjHisQsGxbModels GetFirst(long caseid)
        {
            string sql = "select * from wj_his_qs_gxb where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            WjHisQsGxbModels model = new WjHisQsGxbModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }
        #endregion


        #region 获取高血脂数据包装
        public static IList<WjHisQsGxbModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<WjHisQsGxbModels> ts = new List<WjHisQsGxbModels>();
            // 获得此模型的类型   
            Type type = typeof(WjHisQsGxbModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                WjHisQsGxbModels t = new WjHisQsGxbModels();
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
