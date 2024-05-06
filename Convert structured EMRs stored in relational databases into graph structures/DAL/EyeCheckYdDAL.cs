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
    public class EyeCheckYdDAL
    {
		
		
		
		
		
		
		
		  //这个系列都是，添加，修改，获得




        public static IList<EyeCheckYdModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<EyeCheckYdModels> ts = new List<EyeCheckYdModels>();
            // 获得此模型的类型   
            Type type = typeof(EyeCheckYdModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                EyeCheckYdModels t = new EyeCheckYdModels();
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
        public static int AddEyeCheckYd(EyeCheckYdModels model)
        {
            string sql = "";

            sql = @"insert into eye_check_yd(CASE_ID, L_R,INFO,STATUS,SPXR ,SPYS,SPBJQX,SPSZ,BWZ,HGMPTZ,BLTCX, SWMCX ,SWMTL ,SWMXY,SWMQCX,SWMSC,HBBX,HBLK,HBQM,HBSZ,HBCNV,CANCER_STATUS,CANCER_INFO,RMK,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.L_R + ",'" + model.INFO + "'," + model.STATUS 
+ "," + model.SPXR + "," + model.SPYS + "," + model.SPBJQX + "," + model.SPSZ + "," + model.BWZ + "," + model.HGMPTZ+ "," + model.BLTCX + "," + model.SWMCX + "," + model.SWMTL + "," + model.SWMXY + "," + model.SWMQCX + "," + model.SWMSC + ","
 + model.HBBX + "," + model.HBLK + "," + model.HBQM + "," + model.HBSZ + "," + model.HBCNV + ","
+ model.CANCER_STATUS + ",'" + model.CANCER_INFO+ "','" + model.RMK + "'," 
+ model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdEyeCheckYd(EyeCheckYdModels model)
        {

            string sql = "";

            sql = "update eye_check_yd set   INFO = '" + model.INFO
+ "', STATUS = " + model.STATUS+
", SPXR = " + model.SPXR +
", SPYS = " + model.SPYS +
", SPBJQX = " + model.SPBJQX +
", SPSZ = " + model.SPSZ +
", BWZ = " + model.BWZ +
", HGMPTZ = " + model.HGMPTZ +
", BLTCX = " + model.BLTCX +
", SWMCX = " + model.SWMCX +
", SWMTL = " + model.SWMTL +
", SWMXY = " + model.SWMXY +
", SWMQCX = " + model.SWMQCX +
", SWMSC = " + model.SWMSC +
", HBBX = " + model.HBBX +
", HBLK = " + model.HBLK +
", HBQM = " + model.HBQM + ", HBSZ = " + model.HBSZ + ", HBCNV = " + model.HBCNV +
", CANCER_STATUS = " + model.CANCER_STATUS
+ ", CANCER_INFO = '" + model.CANCER_INFO + "', RMK = '" + model.RMK+ "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID + " and L_R=" + model.L_R;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        




        public static IList<EyeCheckYdModels> GetBoth(long caseid)
        {
            string sql = "select * from eye_check_yd where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<EyeCheckYdModels> list = ListConvertToModel(dt);

            return list;

        }


        public static EyeCheckYdModels GetFirst(long caseid, int lr)
        {
            string sql = "select * from eye_check_yd where L_R=" + lr + " and CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            EyeCheckYdModels model = new EyeCheckYdModels();

            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;
        }






    }
}
