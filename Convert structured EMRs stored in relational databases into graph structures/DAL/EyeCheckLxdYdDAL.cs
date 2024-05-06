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
    public class EyeCheckLxdYdDAL
    {
		
		
		
		
		
		  //这个系列都是，添加，修改，获得




        public static IList<EyeCheckLxdYdModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<EyeCheckLxdYdModels> ts = new List<EyeCheckLxdYdModels>();
            // 获得此模型的类型   
            Type type = typeof(EyeCheckLxdYdModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                EyeCheckLxdYdModels t = new EyeCheckLxdYdModels();
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
        public static int AddEyeCheckLxdYd(EyeCheckLxdYdModels model)
        {
            string sql = "";

            sql = @"insert into eye_check_lxd_yd(CASE_ID, L_R, STATUS, SPXR,BWZ,HGMPTZ,NMSWM , BPSWM,HBWS, FUCHS, MLMXG ,XLW, YDCX , SWMTL ,HBLK , HBQM , INFO, RMK,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.L_R + "," + model.STATUS+ "," + model.SPXR + "," + model.BWZ +
"," + model.HGMPTZ + "," + model.NMSWM + "," + model.BPSWM + "," + model.HBWS + "," + model.FUCHS 
+ "," + model.MLMXG + "," + model.XLW + "," + model.YDCX + "," + model.SWMTL +"," + model.HBLK
+ "," + model.HBQM + ",'"+model.INFO+"','"
+ model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdEyeCheckLxdYd(EyeCheckLxdYdModels model)
        {

            string sql = "";

            sql = "update eye_check_lxd_yd set  L_R = " + model.L_R + ", STATUS = " + model.STATUS + ", SPXR = " + model.SPXR
                + ", BWZ = " + model.BWZ
                + ", HGMPTZ = " + model.HGMPTZ+ ", NMSWM = " + model.NMSWM+ ", BPSWM = " + model.BPSWM+ ", HBWS = " + model.HBWS+
				", FUCHS = " + model.FUCHS+ ", MLMXG = " + model.MLMXG+ ", XLW = " + model.XLW+ ", YDCX = " + model.YDCX+ ", SWMTL = " 
                + model.SWMTL+ ", HBLK = " + model.HBLK+ ", HBQM = " + model.HBQM+ ", INFO = '" + model.INFO+"', RMK = '" + model.RMK+ "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where  CASE_ID=" + model.CASE_ID + " and L_R=" + model.L_R;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        public static EyeCheckLxdYdModels GetFirst(long caseid, int lr)
        {
            string sql = "select * from eye_check_lxd_yd where CASE_ID=" + caseid+" and L_R="+lr;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            EyeCheckLxdYdModels model = new EyeCheckLxdYdModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }





        public static IList<EyeCheckLxdYdModels> GetBoth(long caseid)
        {
            string sql = "select * from eye_check_lxd_yd where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<EyeCheckLxdYdModels> list = ListConvertToModel(dt);

            return list;

        }












    }
}
