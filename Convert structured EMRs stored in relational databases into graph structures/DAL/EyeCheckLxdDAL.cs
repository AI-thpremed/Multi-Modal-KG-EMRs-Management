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
    public  class EyeCheckLxdDAL
    {
		
		
		
		
		  //这个系列都是，添加，修改，获得




        public static IList<EyeCheckLxdModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<EyeCheckLxdModels> ts = new List<EyeCheckLxdModels>();
            // 获得此模型的类型   
            Type type = typeof(EyeCheckLxdModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                EyeCheckLxdModels t = new EyeCheckLxdModels();
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
        public static int AddEyeCheckLxd(EyeCheckLxdModels model)
        {
            string sql = "";

            sql = @"insert into eye_check_lxd(CASE_ID, L_R,STATUS, JMCX,JMTM ,KP, TYN, TK, JZT, RMK,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.L_R + "," + model.STATUS + "," + model.JMCX + "," + model.JMTM +
"," + model.KP + "," + model.TYN + "," + model.TK + "," + model.JZT + ",'" 
+ model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdEyeCheckLxd(EyeCheckLxdModels model)
        {

            string sql = "";

            sql = "update eye_check_lxd set  L_R = " + model.L_R + ", STATUS = " + model.STATUS+ ", JMCX = " + model.JMCX
                + ", JMTM = " + model.JMTM
                + ", KP = " + model.KP+ ", TYN = " + model.TYN+ ", TK = " + model.TK+", JZT = " + model.JZT+
				", RMK = '" + model.RMK+ "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID + " and L_R=" + model.L_R;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static EyeCheckLxdModels GetFirst(long caseid, int lr)
        {
            string sql = "select * from eye_check_lxd where CASE_ID=" + caseid+" and L_R="+lr;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            EyeCheckLxdModels model = new EyeCheckLxdModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }





        public static IList<EyeCheckLxdModels> GetBoth(long caseid)
        {
            string sql = "select * from eye_check_lxd where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<EyeCheckLxdModels> list = ListConvertToModel(dt);

            return list;

        }












    }
}
