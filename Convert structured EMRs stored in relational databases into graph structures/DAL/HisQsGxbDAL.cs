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
    public class HisQsGxbDAL
    {
		
		
		
		
		
		  //这个系列都是，添加，修改，获得




        public static IList<HisQsGxbModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<HisQsGxbModels> ts = new List<HisQsGxbModels>();
            // 获得此模型的类型   
            Type type = typeof(HisQsGxbModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                HisQsGxbModels t = new HisQsGxbModels();
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
        public static int AddHisQsGxb(HisQsGxbModels model)
        {
            string sql = "";

            sql = @"insert into his_qs_gxb(CASE_ID,STATUS,TYPE,FIST_DIA,RS_INFO,DMJR_INFO,MED,MED_INFO,RMK,RMK_QT,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.STATUS + ",'" + model.TYPE+ "','" + model.FIST_DIA.ToString("yyyy-MM-dd") + "','" + model.RS_INFO + "','" + model.DMJR_INFO + "'," + model.MED+ ",'" + model.MED_INFO + "','" + model.RMK + "','" + model.RMK_QT + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdHisQsGxb(HisQsGxbModels model)
        {

            string sql = "";

            sql = "update his_qs_gxb set  FIST_DIA = '" + model.FIST_DIA.ToString("yyyy-MM-dd") + "', TYPE = '" + model.TYPE +
                "', STATUS = " + model.STATUS + ", RS_INFO = '" + model.RS_INFO + "', DMJR_INFO = '" + model.DMJR_INFO
                + "', MED= " + model.MED+ ", MED_INFO = '" + model.MED_INFO + "', RMK = '" + model.RMK + "', RMK_QT = '" + model.RMK_QT + "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static HisQsGxbModels GetFirst(long caseid)
        {
            string sql = "select * from his_qs_gxb where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            HisQsGxbModels model = new HisQsGxbModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }
		
		
		
		
		
		
		
		
		
    }
}
