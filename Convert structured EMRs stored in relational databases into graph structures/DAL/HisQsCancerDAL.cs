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
    public class HisQsCancerDAL
    {
		
		
		  //这个系列都是，添加，修改，获得




        public static IList<HisQsCancerModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<HisQsCancerModels> ts = new List<HisQsCancerModels>();
            // 获得此模型的类型   
            Type type = typeof(HisQsCancerModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                HisQsCancerModels t = new HisQsCancerModels();
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
        public static int AddHisQsCancer(HisQsCancerModels model)
        {
            string sql = "";

            sql = @"insert into his_qs_cancer(CASE_ID,STATUS_F,STATUS_G,STATUS_RX,STATUS_LB,STATUS_QT,FIST_DIA_F,FIST_DIA_G,FIST_DIA_RX,FIST_DIA_LB,FIST_DIA_QT,TYPE_QT,TRE_INFO_F,TRE_INFO_G,TRE_INFO_RX,TRE_INFO_LB,TRE_INFO_QT,UPDATE_ID,UPDATE_DATE ) values("
+ model.CASE_ID + "," + model.STATUS_F+","
+model.STATUS_G + ","+model.STATUS_RX + ","+model.STATUS_LB + ","+model.STATUS_QT + ",'"
+ model.FIST_DIA_F.ToString("yyyy-MM-dd") + "','" + model.FIST_DIA_G.ToString("yyyy-MM-dd") + "','" + model.FIST_DIA_RX.ToString("yyyy-MM-dd")
+ "','" + model.FIST_DIA_LB.ToString("yyyy-MM-dd") + "','" + model.FIST_DIA_QT.ToString("yyyy-MM-dd") + "','"
+ model.TYPE_QT + "','" + model.TRE_INFO_F + "','" + model.TRE_INFO_G + "','" + model.TRE_INFO_RX + "','" + model.TRE_INFO_LB + "','" + model.TRE_INFO_QT + "',"
+model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdHisQsCancer(HisQsCancerModels model)
        {

            string sql = "";

            sql = "update his_qs_cancer set  FIST_DIA_F = '" + model.FIST_DIA_F.ToString("yyyy-MM-dd") + "',FIST_DIA_G= '" 
                + model.FIST_DIA_G.ToString("yyyy-MM-dd") + "',FIST_DIA_RX= '" + model.FIST_DIA_RX.ToString("yyyy-MM-dd") 
                + "',FIST_DIA_LB= '" + model.FIST_DIA_LB.ToString("yyyy-MM-dd") + "',FIST_DIA_QT= '" + model.FIST_DIA_QT.ToString("yyyy-MM-dd")+
                "', STATUS_F = " + model.STATUS_F + ", STATUS_G = " + model.STATUS_G + ", STATUS_RX = " + model.STATUS_RX +
                 ", STATUS_LB = " + model.STATUS_LB + ", STATUS_QT = " + model.STATUS_QT +
                ", TYPE_QT = '" + model.TYPE_QT
                + "', TRE_INFO_F = '" + model.TRE_INFO_F + "', TRE_INFO_G = '" + model.TRE_INFO_G + "', TRE_INFO_RX = '" + model.TRE_INFO_RX
                + "', TRE_INFO_LB = '" + model.TRE_INFO_LB + "', TRE_INFO_QT = '" + model.TRE_INFO_QT
                + "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd")
          + "' where CASE_ID=" + model.CASE_ID;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static HisQsCancerModels GetFirst(long caseid)
        {
            string sql = "select * from his_qs_cancer where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            HisQsCancerModels model = new HisQsCancerModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }

		
		
    }
}
