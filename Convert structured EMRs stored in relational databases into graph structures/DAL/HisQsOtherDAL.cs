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
    public class HisQsOtherDAL
    {


        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int AddHisQsOther(HisQsOtherModels model)
        {
            string sql = "";

            sql = @"insert into his_qs_other(CASE_ID,STATUS,RMK ) values("
+ model.CASE_ID + "," + model.STATUS + ",'" + model.RMK + "')";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }

        //这个系列都是，添加，修改，获得
        public static HisQsOtherModels GetFirst(long caseid)
        {
            string sql = "select * from his_qs_other where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            HisQsOtherModels model = new HisQsOtherModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }
      

        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdHisQsOther(HisQsOtherModels model)
        {

            string sql = "";

            sql = "update his_qs_other set RMK = '" + model.RMK+"', STATUS = " + model.STATUS + ", UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where CASE_ID=" + model.CASE_ID;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }

      


        //这个系列都是，添加，修改，获得

        public static IList<HisQsOtherModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<HisQsOtherModels> ts = new List<HisQsOtherModels>();
            // 获得此模型的类型   
            Type type = typeof(HisQsOtherModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                HisQsOtherModels t = new HisQsOtherModels();
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


    }
}
