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
    public class CaseLifestyleDAL
    {

        //这个系列都是，添加，修改，获得




        public static IList<CaseLifestyleModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<CaseLifestyleModels> ts = new List<CaseLifestyleModels>();
            // 获得此模型的类型   
            Type type = typeof(CaseLifestyleModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                CaseLifestyleModels t = new CaseLifestyleModels();
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


        


        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int AddCaseLifestyle(CaseLifestyleModels model)
        {
            string sql = "";

            sql = @"insert into case_lifestyle(CASE_ID, SMOKE_TYPE,SMOKE_INFO,SMOKE_QUIT_CON,SMOKE_QUIT, DRINK_TYPE, DRINK_INFO,DRINK_FRE,DRINK_OVER,RMK,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.SMOKE_TYPE + ",'" + model.SMOKE_INFO + "'," + model.SMOKE_QUIT_CON + ",'" + model.SMOKE_QUIT + "'," + model.DRINK_TYPE +
",'" + model.DRINK_INFO + "','" + model.DRINK_FRE + "','" + model.DRINK_OVER + "','" + model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdCaseLifestyle(CaseLifestyleModels model)
        {

            string sql = "";

            sql = "update case_lifestyle set  SMOKE_TYPE = " + model.SMOKE_TYPE + ", SMOKE_INFO = '" + model.SMOKE_INFO+ "', SMOKE_QUIT_CON = " + model.SMOKE_QUIT_CON + ", SMOKE_QUIT = '" + model.SMOKE_QUIT
                + "', DRINK_TYPE = " + model.DRINK_TYPE
                + ", DRINK_INFO = '" + model.DRINK_INFO + "', DRINK_FRE = '" + model.DRINK_FRE + "', DRINK_OVER = '" + model.DRINK_OVER + "', RMK = '" + model.RMK
                + "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static CaseLifestyleModels GetFirst(long caseid)
        {
            string sql = "select * from case_lifestyle where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            CaseLifestyleModels model = new CaseLifestyleModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }








    }
}
