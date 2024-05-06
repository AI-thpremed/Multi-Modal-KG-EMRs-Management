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
    public class WjHisEyeDAL
    {



        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="model"></param>
        //        public static long AddHisEye(WjHisEyeModels model)
        //        {
        //            string sql = "";

        //            sql = @"insert into wj_his_eye(CASE_ID,L_R,STATUS, CATARACT, CATARACT_INFO, GLAUCOMA,GLAUCOMA_T_STATUS,GLAUCOMA_DATE,GLAUCOMA_SUR,GLAUCOMA_TRE,BLTSWM,WSS,RMK) values("
        //+ model.CASE_ID + "," + model.L_R + "," + model.STATUS + "," + model.CATARACT + ",'" + model.CATARACT_INFO + "',"
        // + model.GLAUCOMA + "," + model.GLAUCOMA_T_STATUS + ",'" + model.GLAUCOMA_DATE + "','" + model.GLAUCOMA_SUR + "','" + model.GLAUCOMA_TRE + "','" + model.BLTSWM + "','" + model.WSS + "','" + model.BLTSWM + "')";
        //            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        //        }
        #region 获取hiseyewj所有信息
        public static IList<WjHisEyeModels> GetBoth(long caseid)
        {
            string sql = "select * from wj_his_eye where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<WjHisEyeModels> list = ListConvertToModel(dt);

            return list;

        }
        #endregion
        

        #region 获取hiseyewj数据包装模型
        public static IList<WjHisEyeModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<WjHisEyeModels> ts = new List<WjHisEyeModels>();
            // 获得此模型的类型   
            Type type = typeof(WjHisEyeModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                WjHisEyeModels t = new WjHisEyeModels();
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
