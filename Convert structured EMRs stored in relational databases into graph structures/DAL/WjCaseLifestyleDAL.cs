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
    public class WjCaseLifestyleDAL
    {


        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="model"></param>
        //        public static int AddCaseLifestyle(CaseLiftstyleWjModels model)
        //        {
        //            string sql = "";

        //            sql = @"insert into wj_case_lifestyle(CASE_ID, SMOKE_TYPE,SMOKE_INFO,SMOKE_QUIT_CON,SMOKE_QUIT, DRINK_TYPE, DRINK_INFO,DRINK_FRE,DRINK_OVER) values("
        //+ model.CASE_ID + "," + model.SMOKE_TYPE + ",'" + model.SMOKE_INFO + "'," + model.SMOKE_QUIT_CON + ",'" + model.SMOKE_QUIT + "'," + model.DRINK_TYPE +
        //",'" + model.DRINK_INFO + "','" + model.DRINK_FRE + "','" + model.DRINK_OVER + "')";

        //            return DbSql.AddOrUpdOrDel("cc_sys", sql);

        //        }


        #region  获取caselifestylewj所有信息
        public static WjCaseLifestyleModels GetFirst(long caseid)
        {
            string sql = "select * from wj_case_lifestyle where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            WjCaseLifestyleModels model = new WjCaseLifestyleModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }

        #endregion

        #region  获取caselife
        public static IList<WjCaseLifestyleModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<WjCaseLifestyleModels> ts = new List<WjCaseLifestyleModels>();
            // 获得此模型的类型   
            Type type = typeof(WjCaseLifestyleModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                WjCaseLifestyleModels t = new WjCaseLifestyleModels();
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
