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
    public class BioImageDAL
    {




        public static int GetNoResume(long caseId)
        {


            string sql = "select count(*) from bio_image where 1=1 and CASE_ID=" + caseId;


            sql += " and TYPE_NAME!='病历'  ";



            sql += " and IS_DELETE=0  ";

            sql += " order by ID asc";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;

        }



        public static int GetResume(long caseId)
        {


            string sql = "select count(*) from bio_image where 1=1 and CASE_ID=" + caseId;


            sql += " and TYPE_NAME='病历'  ";



            sql += " and IS_DELETE=0  ";

            sql += " order by ID asc";


            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;

        }

        //添加，修改，每个case的拉取，单一拉取




        public static IList<BioImageModels> ListConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<BioImageModels> ts = new List<BioImageModels>();
            // 获得此模型的类型   
            Type type = typeof(BioImageModels);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                BioImageModels t = new BioImageModels();
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
        public static int AddBioImage(BioImageModels model)
        {
            string sql = "";




        sql = @"insert into bio_image(CASE_ID, INS_ID ,TYPE_NAME,    INFO, L_R,   RMK,UPDATE_ID,UPDATE_DATE,CREATE_DATE ) values("
+ model.CASE_ID + ",'" + model.INS_ID + "','" + model.TYPE_NAME
+ "','" + model.INFO + "','" + model.L_R + "','" + model.RMK + "'," + model.UPDATE_ID +
",'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        ///// <summary>
        ///// 修改数据  可以改case，可以改地址，可以改左右，可以改type，可以改ins，可以改rmk
        ///// </summary>
        ///// <param name="model"></param>
        public static int UpdBioImage(BioImageModels model)
        {

            string sql = "";

            sql = "update bio_image set   TYPE_NAME = '" + model.TYPE_NAME  + "', L_R = '" + model.L_R + "', RMK = '" + model.RMK
                + "', UPDATE_ID = " + model.UPDATE_ID
                + ", UPDATE_DATE = '" + model.UPDATE_DATE.ToString("yyyy-MM-dd")
          + "' where ID=" + model.ID;

            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }




        public static BioImageModels GetFirst(long id)
        {
            string sql = "select * from bio_image where ID=" + id;
            DataTable dt = DbSql.GetAll("cc_sys", sql);
            BioImageModels model = new BioImageModels();
            if (dt.Rows.Count > 0)
            {

                model = ListConvertToModel(dt)[0];

            }
            return model;

        }


        public static IList<BioImageModels> GetAllImages(long caseId)
        {


            string sql = "select * from bio_image where 1=1 and CASE_ID="+caseId;



             

            sql += " and IS_DELETE=0  ";

            sql += " order by ID asc";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<BioImageModels> list = ListConvertToModel(dt);

            return list;

        }


        //这里只获取普通图片
        public static IList<BioImageModels> GetAllImagesLr(long caseId, int pageIndex, int pageSize)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            string sql = "select * from bio_image where 1=1 and CASE_ID=" + caseId;


            sql += " and IS_DELETE=0 and TYPE=0  ";

            sql += " order by ID asc limit " + pageIndexs + "," + pageSize + "";

            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<BioImageModels> list = ListConvertToModel(dt);

            return list;

        }

        public static int DataSumCountTempLr(long caseId)
        {


            string sql = "select count(*) from bio_image where 1=1 and CASE_ID=" + caseId;


            sql += " and IS_DELETE=0 and TYPE=0  ";



            DataTable dt = DbSql.GetAll("cc_sys", sql);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count;

        }

        public static IList<BioImageModels> GetAllImagesByName(long caseId,string typeName,string type)
        {


            string sql = "select * from bio_image where 1=1 and CASE_ID=" + caseId;



            sql += " and TYPE_NAME='"+typeName+"'";

            if (!string.IsNullOrEmpty(type))
            {
                sql += " and TYPE= " + Convert.ToInt32(type) ;

            }

            sql += " and IS_DELETE=0  ";

            sql += " order by ID asc";


            DataTable dt = DbSql.GetAll("cc_sys", sql);

            IList<BioImageModels> list = ListConvertToModel(dt);

            return list;

        }


        #region  单个删除

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteBioImage(long id)
        {
            string sql = "update bio_image set IS_DELETE =1  where ID=" + id;
            return DbSql.AddOrUpdOrDel("cc_sys", sql);
        }



        #endregion



    }
}
