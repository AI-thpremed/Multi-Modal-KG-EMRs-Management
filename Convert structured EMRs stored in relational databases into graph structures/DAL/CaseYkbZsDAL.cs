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
    public class CaseYkbZsDAL
    {






        //这个系列都是，添加，修改，获得




        public static IList<CaseYkbZsModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<CaseYkbZsModels> ts = new List<CaseYkbZsModels>();
            // 获得此模型的类型   
            Type type = typeof(CaseYkbZsModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                CaseYkbZsModels t = new CaseYkbZsModels();
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
        public static int AddCaseYkbZs(CaseYkbZsModels model)
        {
            string sql = "";

            sql = @"insert into case_ykb_zs(CASE_ID, FIST_DIA ,INFO_L,    INFO_R,    RMK,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + ",'" + model.FIST_DIA.ToString("yyyy-MM-dd")  + "','" + model.INFO_L  
+"','" + model.INFO_R + "','" + model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdCaseYkbZs(CaseYkbZsModels model)
        {

            string sql = "";

            sql = "update case_ykb_zs set  FIST_DIA = '" + model.FIST_DIA.ToString("yyyy-MM-dd")
        +"', INFO_L= '" +model.INFO_L+ "', INFO_R = '" + model.INFO_R + "', RMK = '" + model.RMK
                + "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where ID=" + model.ID;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static CaseYkbZsModels GetFirst(long caseid)
        {
            string sql = "select * from case_ykb_zs where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            CaseYkbZsModels model = new CaseYkbZsModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }














    }
}
