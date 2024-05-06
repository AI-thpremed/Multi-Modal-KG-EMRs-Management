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
    public class EyeCheckLenstarDAL
    {








        //这个系列都是，添加，修改，获得




        public static IList<EyeCheckLenstarModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<EyeCheckLenstarModels> ts = new List<EyeCheckLenstarModels>();
            // 获得此模型的类型   
            Type type = typeof(EyeCheckLenstarModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                EyeCheckLenstarModels t = new EyeCheckLenstarModels();
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
        public static int AddEyeCheckLenstar(EyeCheckLenstarModels model)
        {
            string sql = "";

            sql = @"insert into eye_check_lenstar(CASE_ID, L_R, STATUS,MODE,AL,CCT,AD,LT,RT,K1,K2,AST,KERI,WTW,IC,PD,PC,RMK,UPDATE_ID,UPDATE_DATE) values("
+ model.CASE_ID + "," + model.L_R + "," + model.STATUS + ",'" + model.MODE + "','" + model.AL + "','" + model.CCT + "','" + model.AD + "','" + model.LT + "','" + model.RT + "','" 
+ model.K1 + "','" + model.K2 + "','" + model.AST + "','" + model.KERI + "','" + model.WTW + "','" + model.IC + "','" + model.PD + "','" + model.PC + "','" +
model.RMK + "'," + model.UPDATE_ID + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdEyeCheckLenstar(EyeCheckLenstarModels model)
        {

            string sql = "";

            sql = "update eye_check_lenstar set   STATUS = " + model.STATUS + ",MODE='" + model.MODE + "',AL='" + model.AL + "',CCT='" + model.CCT + "',AD='" 
                + model.AD + "',LT='" + model.LT + "',RT='" + model.RT + "',K1='" + model.K1 + "',K2='" + model.K2 + "',AST='" + model.AST + "',KERI='" 
                + model.KERI + "',WTW='" + model.WTW + "',IC='" + model.IC + "',PD='" + model.PD + "',PC='" + model.PC + "',RMK='" + model.RMK + "',UPDATE_ID=" 
                + model.UPDATE_ID + ",UPDATE_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where CASE_ID=" + model.CASE_ID + " and L_R=" + model.L_R;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static EyeCheckLenstarModels GetFirst(long caseid, int lr)
        {
            string sql = "select * from eye_check_lenstar where   L_R=" + lr + " and CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            EyeCheckLenstarModels model = new EyeCheckLenstarModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }




        public static IList<EyeCheckLenstarModels> GetBoth(long caseid)
        {
            string sql = "select * from eye_check_lenstar where CASE_ID=" + caseid;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            IList<EyeCheckLenstarModels> list = ListConvertToModel(dt);

            return list;

        }
















    }
}
