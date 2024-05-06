using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Reflection;

namespace DAL
{
    public class BasDictionaryDAL
    {


        #region   更改允许学生评价


        public static int changeAbleStatus()
        {
            BasDictionaryModels  model = BasDictionaryDAL.GetFirstByDictName("ablAssStatus");

            string sql = "";
            if (model.DICT_VALUE=="0")
            {
                sql += "update bas_dictionary set DICT_VALUE=1,UPDATE_DATE='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'  where DICT_NAME='ablAssStatus'";
            }
            else
            {
                sql += "update bas_dictionary set DICT_VALUE=0 ,UPDATE_DATE='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'  where DICT_NAME='ablAssStatus'";
            }
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }


        #endregion

        #region 根据key 更新value
        public static int updValue(string key, string content)
        {

            string sql = "";
            sql += "update bas_dictionary set DICT_VALUE='" + content + "' where DICT_NAME='"+key+"'";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);

        }
        #endregion



        public static BasDictionaryModels GetFirstByDictName(string dictName)
        {
            BasDictionaryModels model = new BasDictionaryModels();

            string sql = "select * from bas_dictionary where 1=1";
            if (!string.IsNullOrEmpty(dictName))
            {
                sql+= " and DICT_NAME='"+dictName+"'";
            }

            DataTable dt = DbSql.GetAll("cc_sys", sql);

            if (dt.Rows.Count > 0)
            {
                model= ListConvertToModel(dt)[0];
            }

            return model;
        }


        public static IList<BasDictionaryModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<BasDictionaryModels> ts = new List<BasDictionaryModels>();
            // 获得此模型的类型   
            Type type = typeof(BasDictionaryModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                BasDictionaryModels t = new BasDictionaryModels();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite)
                            continue;
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
