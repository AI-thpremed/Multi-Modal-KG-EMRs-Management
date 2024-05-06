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
    public class EyeCheckOtherDAL
    {
		
		  //这个系列都是，添加，修改，获得




        public static IList<EyeCheckOtherModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<EyeCheckOtherModels> ts = new List<EyeCheckOtherModels>();
            // 获得此模型的类型   
            Type type = typeof(EyeCheckOtherModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                EyeCheckOtherModels t = new EyeCheckOtherModels();
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
        public static int AddEyeCheckOther(EyeCheckOtherModels model)
        {
            string sql = "";

            sql = @"insert into eye_check_other(CASE_ID, L_R, STATUS,JMCX,JMTM ,KP, TYN, TK,TKGF ,HMYS,HMXG, JZT, CC,NC,PSC,WZBX,RGJT,BLT,  RMK,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.L_R + "," + model.STATUS+ "," + model.JMCX + "," + model.JMTM +
"," + model.KP + "," + model.TYN + "," + model.TK + "," + model.TKGF + "," + model.HMYS + "," + model.HMXG + "," + model.JZT + "," 
+ model.CC + "," + model.NC + "," + model.PSC + "," + model.WZBX + "," + model.RGJT+ "," + model.BLT + ",'"
+ model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdEyeCheckOther(EyeCheckOtherModels model)
        {

            string sql = "";

            sql = "update eye_check_other set STATUS = " + model.STATUS
                 +", JMCX = " + model.JMCX+ ", JMTM = " + model.JMTM
                + ", KP = " + model.KP+ ", TYN = " + model.TYN+ ", TK = " + model.TK+ ", TKGF = " + model.TKGF+
				", HMYS = " + model.HMYS+ ", HMXG = " + model.HMXG + ", JZT = " + model.JZT+ ", CC = " +
                model.CC+ ", NC = " + model.NC + ", PSC = " + model.PSC + ", WZBX = " + model.WZBX + ", RGJT = " +model.RGJT+ ", BLT = " + model.BLT +
              ", RMK = '" + model.RMK+ "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID + " and L_R=" + model.L_R;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static EyeCheckOtherModels GetFirst(long caseid, int lr)
        {
            string sql = "select * from eye_check_other where  L_R=" + lr + " and  CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            EyeCheckOtherModels model = new EyeCheckOtherModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }


        public static IList<EyeCheckOtherModels> GetBoth(long caseid)
        {
            string sql = "select * from eye_check_other where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<EyeCheckOtherModels> list = ListConvertToModel(dt);

            return list;

        }



    }
}
