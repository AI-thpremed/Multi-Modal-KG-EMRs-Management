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
    public class HisEyeDAL
    {
		
		
		
		
		  //这个系列都是，添加，修改，获得




        public static IList<HisEyeModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<HisEyeModels> ts = new List<HisEyeModels>();
            // 获得此模型的类型   
            Type type = typeof(HisEyeModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                HisEyeModels t = new HisEyeModels();
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
        public static int AddHisEye(HisEyeModels model)
        {
            string sql = "";

            sql = @"insert into his_eye(CASE_ID,L_R,STATUS, CATARACT, CATARACT_INFO, CATARACT_RMK,GLAUCOMA,GLAUCOMA_T_STATUS,GLAUCOMA_DATE,GLAUCOMA_SUR,GLAUCOMA_RMK,GLAUCOMA_TRE,BLTSWM,WSS,RMK,OTHER,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.L_R + "," + model.STATUS + "," + model.CATARACT + ",'" + model.CATARACT_INFO +"','"+model.CATARACT_RMK+"',"
 + model.GLAUCOMA + "," + model.GLAUCOMA_T_STATUS + ",'" + model.GLAUCOMA_DATE+ "','"  + model.GLAUCOMA_SUR + "','"+model.GLAUCOMA_RMK+"','" + model.GLAUCOMA_TRE +"','"  + model.BLTSWM +"','"  + model.WSS +"','" + model.RMK + "','"+model.OTHER + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdHisEye(HisEyeModels model)
        {

            string sql = "";

            sql = "update his_eye set  CATARACT = " + model.CATARACT + ", STATUS = " + model.STATUS + ", CATARACT_INFO = '" + model.CATARACT_INFO + "', CATARACT_RMK = '"+model.CATARACT_RMK + "', GLAUCOMA = " + model.GLAUCOMA + ", GLAUCOMA_T_STATUS = " + model.GLAUCOMA_T_STATUS 
                + ", GLAUCOMA_DATE = '" + model.GLAUCOMA_DATE+ "', GLAUCOMA_SUR = '" + model.GLAUCOMA_SUR + "', GLAUCOMA_RMK = '" + model.GLAUCOMA_RMK+"', GLAUCOMA_TRE = '" + model.GLAUCOMA_TRE+ "', BLTSWM = '" + model.BLTSWM+"', WSS = '" + model.WSS+"', RMK = '" + model.RMK+ "', OTHER = '" + model.OTHER + "', UPDATE_ID = " + model.UPDATE_ID+ ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID + " and L_R="+model.L_R;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static IList<HisEyeModels> GetBoth(long caseid)
        {
            string sql = "select * from his_eye where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<HisEyeModels> list = ListConvertToModel(dt);

            return list;

        }


         









    }
}
