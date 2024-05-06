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
    public class WjHisQsNgsDAL
    {


      

        #region  获取肿瘤所有信息
        public static WjHisQsNgsModels GetFirst(long caseid)
        {
            string sql = "select * from wj_his_qs_ngs where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            WjHisQsNgsModels model = new WjHisQsNgsModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }
        #endregion


        #region 获取高血脂数据包装
        public static IList<WjHisQsNgsModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<WjHisQsNgsModels> ts = new List<WjHisQsNgsModels>();
            // 获得此模型的类型   
            Type type = typeof(WjHisQsNgsModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                WjHisQsNgsModels t = new WjHisQsNgsModels();
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
