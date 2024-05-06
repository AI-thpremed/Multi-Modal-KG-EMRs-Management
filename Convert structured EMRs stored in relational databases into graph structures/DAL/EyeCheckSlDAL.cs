﻿using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EyeCheckSlDAL
    {
		
		
		
		
		
		
		
		  //这个系列都是，添加，修改，获得




        public static IList<EyeCheckSlModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<EyeCheckSlModels> ts = new List<EyeCheckSlModels>();
            // 获得此模型的类型   
            Type type = typeof(EyeCheckSlModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                EyeCheckSlModels t = new EyeCheckSlModels();
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
        public static int AddEyeCheckSl(EyeCheckSlModels model)
        {
            string sql = "";

            sql = @"insert into eye_check_sl(CASE_ID, L_R,STATUS ,TYPE,  INFO,XIANRAN_DS,XIANRAN_ZJDS,XIANRAN_ZX,LUOYAN,JIAOZHENG,XIANRAN_SL , RMK,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.L_R + "," + model.STATUS+ "," + model.TYPE + ",'" + model.INFO + "','" + model.XIANRAN_DS + "','" + model.XIANRAN_ZJDS + "','" + model.XIANRAN_ZX + "','"
+ model.LUOYAN + "','" + model.JIAOZHENG + "','" + model.XIANRAN_SL + "','"

+ model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdEyeCheckSl(EyeCheckSlModels model)
        {

            string sql = "";

            sql = "update eye_check_sl set   STATUS = " + model.STATUS  +", INFO = '" + model.INFO + "', TYPE = " + model.TYPE
                + ", XIANRAN_SL = '" + model.XIANRAN_SL
                + "', LUOYAN = '" + model.LUOYAN
                + "', JIAOZHENG = '" + model.JIAOZHENG
                + "', XIANRAN_DS = '" + model.XIANRAN_DS
                + "', XIANRAN_ZJDS = '" + model.XIANRAN_ZJDS
                + "', XIANRAN_ZX = '" + model.XIANRAN_ZX



                + "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID + " and L_R=" + model.L_R;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }





        public static IList<EyeCheckSlModels> GetBoth(long caseid)
        {
            string sql = "select * from eye_check_sl where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<EyeCheckSlModels> list = ListConvertToModel(dt);

            return list;

        }


         


        public static EyeCheckSlModels GetFirst(long caseid, int lr)
        {
            string sql = "select * from eye_check_sl where  L_R=" + lr + " and CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            EyeCheckSlModels model = new EyeCheckSlModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }










    }
}
