using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DbSql
    {
        /// <summary>
        /// 查询的功用方法 xcg
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetAll(string name, string sql)
        {
            //string sqlconn = System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;//从配置文件中读取sql
            string sqlconn = "Server=47.52.41.87;Database=learnsys630;Uid=root;Pwd=shekou19881118;Connection Timeout=300;";
            //string constr = "User Id=root;Host=127.0.0.1;Database=learnsys;password=123456";

            MySql.Data.MySqlClient.MySqlConnection mycn =
                new MySql.Data.MySqlClient.MySqlConnection(sqlconn);
            //开启了数据库连接

            //MySql.Data.MySqlClient.MySqlConnection mycn = new MySql.Data.MySqlClient.MySqlConnection(sqlconn);//开启了数据库连接

            try
            {
                mycn.Open();//尝试连接数据库
            }
            catch (Exception ex)
            {
            }
            string sqlString = sql;
            MySql.Data.MySqlClient.MySqlCommand cmd = mycn.CreateCommand();//创建数据库命令
            cmd.CommandText = sqlString;//命令设定语句
            System.Data.DataTable dt = new System.Data.DataTable();//创建一个用于承载查
            MySql.Data.MySqlClient.MySqlDataAdapter dataAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd);//通过dataAdapter执行命令，返回Datatable 或 Dataset
            dataAdapter.Fill(dt);//通过dataAdapter填充datatable
            mycn.Close();//断开数据库连接
            return dt;
        }

        /// <summary>
        /// 添加修改删除的方法 xcg
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int AddOrUpdOrDel(string name, string sql)
        {
            string sqlconn = System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;//从配置文件中读取sql
            MySql.Data.MySqlClient.MySqlConnection mycn = new MySql.Data.MySqlClient.MySqlConnection(sqlconn);//开启了数据库连接
            try
            {
                mycn.Open();//尝试连接数据库
            }
            catch (Exception ex)
            {

            }
            MySql.Data.MySqlClient.MySqlCommand cmd = mycn.CreateCommand();//创建数据库命令
            string Strsql = sql;
            cmd.CommandText = Strsql;
            int i = cmd.ExecuteNonQuery();
            //long newid = cmd.LastInsertedId;
            mycn.Close();//断开数据库连接

            return i;
        }





        /// <summary>
        /// 添加修改删除的方法 xcg
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static long AddandLastId(string name, string sql)
        {
            string sqlconn = System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;//从配置文件中读取sql
            MySql.Data.MySqlClient.MySqlConnection mycn = new MySql.Data.MySqlClient.MySqlConnection(sqlconn);//开启了数据库连接
            try
            {
                mycn.Open();//尝试连接数据库
            }
            catch (Exception ex)
            {

            }
            MySql.Data.MySqlClient.MySqlCommand cmd = mycn.CreateCommand();//创建数据库命令
            string Strsql = sql;
            cmd.CommandText = Strsql;
            int i = cmd.ExecuteNonQuery();
            long newid = cmd.LastInsertedId;
            mycn.Close();//断开数据库连接

            return newid;
        }




    }





}
