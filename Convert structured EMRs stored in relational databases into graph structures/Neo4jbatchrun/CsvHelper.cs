using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jWorkstation
{
    public class CsvHelper
    {

         







   /// <summary>
        /// Datatable数据表导出到CSV文件
        /// </summary>
        /// <param name="path">CSV文件名称</param>
        /// <param name="dt">Datatable数据表</param>
        /// <returns></returns>
        public static bool DataTableToCsv(string path, DataTable dt)
        {
            try
            {
                //添加文件路径--桌面 + 文件夹 + 日期 + xxx.csv
                string filePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ExportParameters\{DateTime.Now:yyyyMMddHHmm}";
                ////如果没有这个文件夹就创建
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                path = $"{filePath}\\{path}";
                //获取path的路径，给予创建和写入的权利
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                //允许将字符和字符串写入path，使用filestream创建streamwriter
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                var strLine = ""; //每一行行数据
                //创建列
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strLine += dt.Columns[i].ColumnName;
                    if (i < dt.Columns.Count - 1)
                    {
                        strLine += ",";
                    }
                }
                sw.WriteLine(strLine);
                //写入行
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strLine = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        strLine += dt.Rows[i][j].ToString();
                        if (j < dt.Columns.Count - 1)
                        {
                            strLine += ",";
                        }
                    }
                    //写入数据进入文件
                    sw.WriteLine(strLine);
                }
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }



  ///// <summary>
  //     /// csv文件转datatable
  //     /// </summary>
  //     /// <param name="path">文件路径</param>
  //     /// <param name="dt">传出datatable</param>
  //     /// <returns>true:成功，false：失败</returns>
  //     public static bool CsvToDataTable(string path, out DataTable dt)
  //     {
  //         try
  //         {
  //             DataTable dataTable = new DataTable();
  //             FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
  //             StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("gb2312"));
  //             string strLine = "";           //记录每次读取的一行记录
  //             string[] aryLine = null;       //每行字段内容
  //             string[] tableHead = null;     //每行字段内容
  //             int columnCount = 0;           //列数
  //             bool IsFirst = true;           //是否为第一行
  //             while ((strLine = streamReader.ReadLine()) != null)
  //             {
  //                 if (IsFirst == true)
  //                 {
  //                     tableHead = strLine.Split(',');
  //                     IsFirst = false;
  //                     columnCount = tableHead.Length;
  //                     //创建列
  //                     for (int i = 0; i < columnCount; i++)
  //                     {
  //                         DataColumn dataColumn = new DataColumn(tableHead[i]);
  //                         dataTable.Columns.Add(dataColumn);
  //                     }
  //                 }
  //                 else
  //                 {
  //                     aryLine = strLine.Split(',');
  //                     DataRow dataRow = dataTable.NewRow();
  //                     for (int j = 0; j < columnCount; j++)
  //                     {
  //                         dataRow[j] = aryLine[j];
  //                     }
  //                     dataTable.Rows.Add(dataRow);
  //                 }
  //             }
  //             if (aryLine != null && aryLine.Length > 0)
  //             {
  //                 dataTable.DefaultView.Sort = tableHead[0] + " " + "asc";
  //             }
  //             streamReader.Close();
  //             streamReader.Dispose();
  //             fileStream.Close();
  //             fileStream.Dispose();
  //             dt = dataTable;
  //             return true;
  //         }
  //         catch
  //         {
  //             dt = null;
  //             return false;
  //         }
  //     }








    }
}
