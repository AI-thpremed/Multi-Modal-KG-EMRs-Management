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
 


    public class CaseFollowDAL
    {


        

        #region  病历下面的分页跟进信息

        public static IList<CaseFollowModels> GetAllFollow(long CASE_ID, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from case_follow where 1=1";

            sql += " and CASE_ID=" + CASE_ID;
            sql += " and STATUS=0 ";

            sql += " order by ID desc limit " + pageIndexs + "," + pageSize + "";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<CaseFollowModels> list = ListConvertToModel(dt);

            return list;

        }




        public static int DataSumFollow(long CASE_ID)
        {
            string sql = "select count(*) from case_follow  where 1=1";

            sql += " and CASE_ID=" + CASE_ID;
            sql += " and STATUS=0 ";

            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;
        }







        #endregion



        public static IList<CaseFollowModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<CaseFollowModels> ts = new List<CaseFollowModels>();
            // 获得此模型的类型   
            Type type = typeof(CaseFollowModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                CaseFollowModels t = new CaseFollowModels();
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



        #region  设置follow删除
        public static int DeleteFollow(long id)
        {
            string sql = "";

            sql = "update case_follow set STATUS = 1  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion







        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="model"></param>
        public static int AddCaseFollow(CaseFollowModels model)
        {
            string sql = "";

            sql = @"insert into case_follow(CASE_ID, USER_NAME,CONTENT ,REIVIEW_TIME, ACT_HAPPEN,    STATUS ) values("
+ model.CASE_ID + ",'" + model.USER_NAME + "','"  + model.CONTENT + "','" + model.REIVIEW_TIME +
"','" + model.ACT_HAPPEN + "'," + model.STATUS  + ")";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }

         


        







    }














}
