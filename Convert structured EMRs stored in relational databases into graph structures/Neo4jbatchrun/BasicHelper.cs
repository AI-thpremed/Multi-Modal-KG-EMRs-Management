using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Neo4jWorkstation
{
    public class BasicHelper
    {



        #region  本地方法


        //如果string位空返回1900-01-01默认日期。要不然返回本身
        public static string checkDate(string input)
        {

            if (input == "")
                return "1900-01-01";
            else
                return input;

        }






        #endregion









        /// <summary>
        /// 获取本周的周一日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetThisWeekMonday(DateTime input)
        {
            //DateTime date = DateTime.Now;
            DateTime date = input;

            DateTime firstDate = System.DateTime.Now;
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    firstDate = date;
                    break;
                case System.DayOfWeek.Tuesday:
                    firstDate = date.AddDays(-1);
                    break;
                case System.DayOfWeek.Wednesday:
                    firstDate = date.AddDays(-2);
                    break;
                case System.DayOfWeek.Thursday:
                    firstDate = date.AddDays(-3);
                    break;
                case System.DayOfWeek.Friday:
                    firstDate = date.AddDays(-4);
                    break;
                case System.DayOfWeek.Saturday:
                    firstDate = date.AddDays(-5);
                    break;
                case System.DayOfWeek.Sunday:
                    firstDate = date.AddDays(-6);
                    break;
            }
            //return firstDate.ToString("yyyy-MM-dd");


            return firstDate;

        }
















        //获得所有日期
        public static IList<string> GetAllDates(string startDate, string endDate, int pageSize, int pageIndex)
        {

            int pageIndexs = (pageIndex - 1) * pageSize;

            IList<string> dates = new List<string>();
            IList<string> res = new List<string>();

            DateTime d1 = DateTime.Parse(startDate);
            DateTime d2 = DateTime.Parse(endDate);
            for (DateTime dt = d1; dt <= d2; dt = dt.AddDays(1))
            {
                dates.Add(dt.ToString("yyyy-MM-dd"));//6、7、8、9
            }

            //if (dates.Count >= pageSize)
            //{

            res = dates.Skip(pageIndexs).Take(pageSize).ToList<string>();
            //}

            //else
            //{
            //    dates = null;
            //}



            return res;

        }




        public static int DatesCount(string startDate, string endDate)
        {

            IList<DateTime> dates = new List<DateTime>();

            DateTime d1 = DateTime.Parse(startDate);
            DateTime d2 = DateTime.Parse(endDate);
            for (DateTime dt = d1; dt <= d2; dt = dt.AddDays(1))
            {
                dates.Add(dt);//6、7、8、9
            }
            int count = dates.Count;
            return count;
        }


        //获得所有日期计数






        ////通过所有regions获得对应的regionid

        //public static string getRegionName(IList<SetRegionModels> regions, int id)
        //{


        //    string name = "";

        //    for (int i = 0; i < regions.Count; i++)

        //    {
        //        if (regions[i].ID == id)
        //        {
        //            name = regions[i].NAME;
        //            break;

        //        }



        //    }

        //    return name;
        //}






        /// <summary>
        /// 金额转换成中文大写金额
        /// </summary>
        /// <param name="LowerMoney">eg:10.74</param>
        /// <returns></returns>
        public static string MoneyToUpper(string LowerMoney)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (LowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                LowerMoney = LowerMoney.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
            if (LowerMoney.IndexOf(".") > 0)
            {
                if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
                {
                    LowerMoney = LowerMoney + "0";
                }
            }
            else
            {
                LowerMoney = LowerMoney + ".00";
            }
            strLower = LowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }





        public static string CheckYear(DateTime OrgDate)
        {

            string StrDate = OrgDate.ToString("yyyy-MM-dd");

            if (OrgDate.Year == 1900 || OrgDate.Year < 1900)
            {

                StrDate = "";
            }
            return StrDate;
        }







        /// <summary>
        /// 判断string 含小数点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }





        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="intStr">要进行转换的字符串</param>
        /// <param name="defaultValue">默认值，默认：0</param>
        /// <returns></returns>
        public static int TurnStringToNum(string intStr, int defaultValue = 0)
        {
            int parseInt;
            if (int.TryParse(intStr, out parseInt))
                return parseInt;
            return defaultValue;
        }


        /// <summary>
        /// 字符串转decimal
        /// </summary>
        /// <param name="intStr">要进行转换的字符串</param>
        /// <param name="defaultValue">默认值，默认：0</param>
        /// <returns></returns>
        public static decimal TurnStringToDeci(string intStr, decimal defaultValue = 0)
        {
            decimal parseInt;
            if (Decimal.TryParse(intStr, out parseInt))
                return parseInt;
            return defaultValue;
        }





        /// <summary>
        /// 导入的日期如果不满足1999-01-01格式的，返回1900-01-01 
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static string CheckImportDate(string dateStr, string defStr)
        {


            DateTime dtDate;
            if (DateTime.TryParse(dateStr, out dtDate))
            {
                return dtDate.ToString("yyyy-MM-dd");
            }
            else
            {

                return defStr;
            }

        }




        /// <summary>
        /// str长度限制在len以内
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CheckImportStrLen(string str, int len)
        {

            if (str.Length > len)
            {
                return str.Substring(0, len - 1);
            }
            else
            {

                return str;
            }

        }





        #region Q7: bool IsExistIndex 检测除自身外包含指定索引值的实体对象是否已存在
        /// <summary>
        /// 检测除自身外包含指定索引值的实体对象是否已存在(用在修改索引值时)
        /// </summary>
        /// <param name="dbIndexName">数据库索引字段名称</param>
        /// <param name="dbIndexValue">索引值</param>
        /// <param name="ID">对象ID</param>
        /// <returns></returns>
        //public bool IsExistIndex(string dbIndexName, string dbIndexValue, IdType ID)
        //{
        //    bool retval = false;
        //    string cmdSql = string.Format(
        //        "SELECT count(*) FROM {0} WHERE {1} <> @ID and {2} = @IndexValue",
        //        M_SqlMap.EntityKey.EntitySetName,
        //        M_SqlMap.EntityKey.PkName,
        //        dbIndexName);

        //    IDbConnection conn = null;
        //    try
        //    {
        //        conn = M_SqlMap.GetDbConnection(DbOpType.Read);
        //        conn.Open();

        //        var m = conn.Query<Int64>(cmdSql, new { ID = ID, IndexValue = dbIndexValue }).FirstOrDefault();

        //        if (m > 0)
        //        {
        //            retval = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = "IsExistIndex 出错, EntitySetName:{0}, conn:{1}, IndexValue:{2} (DbOpType.Read)";
        //        msg = string.Format(msg, M_SqlMap.EntityKey.EntitySetName, GetConnStr(conn), dbIndexValue);

        //        throw new AppEx(msg, cmdSql, ex);
        //    }
        //    finally
        //    {
        //        if (conn != null && conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //    return retval;
        //}
        #endregion










        #region  眼科项目




        ///状态的转换
        public static string StatusTran(int input)
        {

            if (input == 0)
            {
                return "未查";
            }
            else if (input == 1)
            {

                return "已查——无异常";
            }
            else
            {
                return "已查——有异常";


            }

        }


        ///状态的转换
        public static string StatusTranBasic(int input)
        {

            if (input == 0)
            {
                return "未查";
            }
            else
            {

                return "已查";
            }


        }


        ///状态的转换
        public static string StatusTranNormal(int status, int input)
        {
            if (status == 0)
            {
                return "--";

            }
            else
            {

                if (input == 0)
                {
                    return "无";
                }
                else
                {

                    return "有";
                }

            }




        }




        ///角膜透明状态的转换
        public static string StatusTranJmtm(int status, int input)
        {
            if (status == 0)
            {
                return "--";
            }
            else
            {
                if (input == 0)
                {
                    return "透明";
                }
                else
                {
                    return "不透明";
                }
            }
        }


        ///瞳孔圆不圆状态的转换
        public static string StatusTranTk(int status, int input)
        {
            if (status == 0)
            {
                return "--";
            }
            else
            {
                if (input == 0)
                {
                    return "圆";
                }
                else
                {
                    return "不圆";
                }
            }
        }
        ///黄斑中心凹反光的转换
        public static string StatusTranHbzx(int status, int input)
        {
            if (status == 0)
            {
                return "--";
            }
            else
            {
                if (input == 0)
                {
                    return "可见";
                }
                else
                {
                    return "不可见";
                }
            }
        }

        ///虹膜纹理 和 视乳头颜色
        public static string StatusTranHs(int status, int input)
        {
            if (status == 0)
            {
                return "--";
            }
            else
            {
                if (input == 0)
                {
                    return "正常";
                }
                else
                {
                    return "异常";
                }
            }
        }

        //近视库 屈光分级
        public static string YkbTypeConvert(int input)
        {

            string output = "";


            switch (input)
            {
                case 0:
                    output = "低度近视（D＜300°）";
                    break;
                case 1:
                    output = "中度近视（300°≤D＜600°）";
                    break;
                case 2:
                    output = "高度近视（D≥600°）";
                    break;
                case 3:
                    output = "远视";
                    break;
                case 4:
                    output = "正视";
                    break;

            }

            return output;
        }

        ///瞳孔对光反射状态的转换
        public static string StatusTranTkgf(int status, int input)
        {
            if (status == 0)
            {
                return "--";
            }
            else
            {
                if (input == 0)
                {
                    return "存在";
                }
                else
                {
                    //return "不存在";
                    return "消失";
                }
            }
        }

        ///人工晶体的转换
        public static string StatusTranRgjt(int input)
        {

            if (input == 0)
            {
                return "无";
            }
            else if (input == 1)
            {
                return "位正";
            }
            else
            {
                return "移位";
            }

        }

        ///晶状体透明浑浊的转换
        public static string StatusTranJzt(int status, int input)
        {
            if (status == 0)
            {
                return "--";
            }
            else
            {
                if (input == 0)
                {
                    return "透明";
                }
                else
                {
                    return "浑浊";
                }
            }
        }


        ///近视病晶状体透明浑浊的转换
        public static string StatusTranJsJzt(int status, int input)
        {
            if (status == 0)
            {
                return "--";
            }
            else
            {
                if (input == 0)
                {
                    return "透明";
                }
                else if (input == 1)

                {
                    return "皮质浑浊";
                }
                else if (input == 2)

                {
                    return "核浑浊";
                }
                else if (input == 3)

                {
                    return "囊膜浑浊";
                }
                else
                {
                    return "数据错误";

                }
            }
        }

        ///晶状体翻个部分透明状态的转换
        public static string StatusCcNcPsc(int status, int input)
        {
            if (status == 0)
            {
                return "--";

            }
            else
            {
                switch (input)
                {
                    case 0:
                        return "无";


                    case 1:
                        return "I级";

                    case 2:
                        return "II级";

                    case 3:
                        return "III级";

                    case 4:
                        return "IV级";

                    case 5:
                        return "V级";

                    case 6:
                        return "等级不详";

                    default:
                        return "--";



                }
            }




        }




        ///状态的转换
        public static string HisStatusTran(int input)
        {

            if (input == 0)
            {
                return "无";
            }
            else if (input == 1)
            {

                return "有";
            }
            else
            {
                return "不详";


            }

        }


        //视盘颜色状态转换
        public static string SpysStatusTran(int input)
        {

            if (input == 0)
            {
                return "正常";
            }
            else if (input == 1)
            {

                return "色淡";
            }
            else
            {
                return "苍白";


            }

        }


        //眼底边界清晰状态转换
        public static string BjqxStatusTran(int input)
        {

            if (input == 0)
            {
                return "是";
            }
            else if (input == 1)
            {

                return "否";
            }
            else
            {
                return "不详";


            }

        }


        ///根据是否用药转换具体用药的情况出来
        public static string HisMedDisplayTran(int input, string med_info)
        {

            if (input == 0)
            {
                return "无";
            }
            else if (input == 1)
            {

                return "有，用药名称：" + CheckStrEmpty(med_info);
            }
            else
            {
                return "不详";


            }

        }

        ///根据是否用药转换具体用药的情况出来
        public static string CancerTreDisplayTran(string tre_info)
        {


            string[] items = tre_info.Split('▲');


            string output = CheckStrEmpty(items[0]) + "；治疗开始日期：" + CheckYearYkStr(items[1]) + "；持续 " + CheckStrEmpty(items[2]) + "  单位 " + CheckStrEmpty(items[3]) + "<br>";



            return output;



        }






        //更加符合眼科系统的要求，比较美观一些
        public static string CheckYearYk(DateTime OrgDate)
        {

            string StrDate = OrgDate.ToString("yyyy-MM-dd");

            if (OrgDate.Year == 1900 || OrgDate.Year < 1900)
            {

                StrDate = "-----不详-----";
            }
            return StrDate;
        }



        //更加符合眼科系统的要求，比较美观一些
        public static string CheckYearYkStr(string date)
        {
            try
            {

                if (!string.IsNullOrEmpty(date))
                {

                    if (Convert.ToDateTime(date).Year <= 1900)
                        return "-----未记录-----";
                    else
                        return date;
                }
                else

                    return "-----未记录-----";



            }
            catch
            {
                return "-----未记录-----";



            }



        }


        //饮酒频次字段转换
        public static string DrinkFreConvert(string input)
        {

            string output = "";

            if (!string.IsNullOrEmpty(input))
            {
                switch (input)
                {
                    case "0":
                        output = "每天";
                        break;
                    case "1":
                        output = "每周2-3次";
                        break;
                    case "2":
                        output = "每周1次";
                        break;
                    case "3":
                        output = "每月2-3次";
                        break;
                    case "4":
                        output = "每月1次";
                        break;
                    case "5":
                        output = "更少或几乎不喝";
                        break;
                    case "6":
                        output = "以前喝，戒了";
                        break;
                    case "7":
                        output = "不详";
                        break;
                }
            }
            return output;
        }


        //饮酒频次字段转换
        public static string DrinkInfoConvert(string input)
        {


            string[] items_dr = input.Split('▲');
            string output = "";

            if (items_dr.Length != 4)
                return output;
            else
            {
                if (!string.IsNullOrEmpty(items_dr[0]))
                    output += "白酒 " + CheckStrEmpty(items_dr[0]) + " 两；";
                if (!string.IsNullOrEmpty(items_dr[1]))
                    output += "葡萄酒 " + CheckStrEmpty(items_dr[1]) + " 杯；";
                if (!string.IsNullOrEmpty(items_dr[2]))
                    output += "啤酒 " + CheckStrEmpty(items_dr[2]) + " ml（一瓶500ml）；";
                if (!string.IsNullOrEmpty(items_dr[3]))
                    output += "其他酒 " + CheckStrEmpty(items_dr[3]) + " 两；";




            }

            if (string.IsNullOrEmpty(output))
                return "未记录";
            else
            {
                return output;


            }


        }



        //如果未填写显示未记录
        public static string CheckStrEmpty(string input)
        {
            try
            {

                if (string.IsNullOrEmpty(input))
                {

                    return "未记录";
                }
                else

                    return input;



            }
            catch
            {
                return "未记录";



            }



        }





        //近视库 屈光分级
        public static string JsTypeConvert(int input)
        {

            string output = "";


            switch (input)
            {
                case 0:
                    output = "远视";
                    break;
                case 1:
                    output = "正视";
                    break;
                case 2:
                    output = "低度近视";
                    break;
                case 3:
                    output = "中度近视";
                    break;
                case 4:
                    output = "重度近视";
                    break;

            }

            return output;
        }




        //// 眼科主诉的分项校正，去掉最后一个字母然后输出。
        //public static string FixZhusuInfo(string input, string replace)
        //{

        //    string [] items = input.Split(',');

        //    for (int i = 0; i < items.Length; i++)
        //    {
        //        if (!string.IsNullOrEmpty(items[i]))
        //        {
        //            if (items[i].Substring(items[i].Length - 1, 1) == replace)
        //                items[i] = items[i].Substring(0, items[i].Length - 1);
        //        }

        //    }


        //    string output = String.Join(",", items);



        //    return output;
        //}





        //通过拼接获得病历的状态汇总info.TYPE_ID,info.STATUS,info.AUDIT
        public static string getCaseStatus(int type, int status)
        {
            try
            {

                string output = "";

                switch (type)
                {
                    case 0:
                        output += "眼科病库";
                        break;
                    case 1:
                        output += "近视库";
                        break;
                    case 2:
                        output += "疑难杂症库";
                        break;
                    default:
                        break;
                }

                switch (status)
                {
                    case 0:
                        output += "/临时";
                        break;
                    case 1:
                        output += "/正式";
                        break;
                    case 2:
                        output += "/标注";
                        break;
                    default:
                        break;
                }

                return output;

            }
            catch
            {
                return "";



            }



        }


        public static string getCaseKu(int type)
        {
            try
            {

                string output = "";

                switch (type)
                {
                    case 0:
                        output += "眼底病库";
                        break;
                    case 1:
                        output += "近视库";
                        break;
                    case 2:
                        output += "疑难杂症库";
                        break;
                    default:
                        break;
                }


                return output;

            }
            catch
            {
                return "";



            }



        }



        #endregion

        //通过拼接获得病历的状态汇总info.TYPE_ID,info.STATUS,info.AUDIT
        public static int getHopAge(DateTime birthday, DateTime hopdate)
        {
            try
            {

                int age = hopdate.Year - birthday.Year;

                if (birthday.Year <= 1900 || hopdate.Year <= 1900)
                {
                    return 0;
                }
                // 如果出生的月份比当前月份小，则说明还不满一岁，所以年龄减一
                else if (age <= 0)
                {
                    return 0;
                }
                else
                {
                    return age;
                }

                //// 检查出生日期是否有效
                //if (birthday > hopdate)
                //{

                //    return 0;


                //}
                //else
                //{
                //    int age = hopdate.Year - birthday.Year;

                //    // 如果出生的月份比当前月份小，则说明还不满一岁，所以年龄减一
                //    if (birthday.Month > curDate.Month)
                //    {
                //        age--;
                //    }
                //}

            }
            catch
            {
                return 0;



            }



        }






        ///晶状体翻个部分透明状态的转换
        public static string priPosTran(int priPos)
        {

            switch (priPos)
            {
                case 1:
                    return "录入员";


                case 2:
                    return "录入审核";

                case 3:
                    return "眼科录入";

                case 4:
                    return "近视录入";
                case 5:
                    return "眼科审核";

                case 6:
                    return "近视审核";

                case 10:
                    return "科研人员";

                case 11:
                    return "问卷管理员";
                case 88:
                    return "管理员";


                default:

                    return "管理员";




            }




        }






        ///眼科项目性别转换
        public static string sexTran(int sex)
        {

            switch (sex)
            {
                case 1:
                    return "男";


                case 2:
                    return "女";

                default:
                    return "未选择";




            }




        }



        //姓名泛化
        public static string nameTran(string name)
        {

            if (name.Length >= 3)
            {
                return name.Substring(0, 1) + "*" + name.Substring(name.Length - 1, 1);
            }
            else
            {
                return name.Substring(0, 1) + "*";

            }





        }





        //联系方式泛化
        public static string phoneTran(string phone)
        {

            string str = "";
            if (phone.Length > 7)
            {
                for (int i = 0; i < phone.Length - 7; i++)
                {
                    str += "*";
                }
                return phone.Substring(0, 3) + str + phone.Substring(phone.Length - 4, 4);
            }
            else
            {
                if (phone.Length >= 5 & phone.Length <= 7)
                {
                    return phone.Substring(0, 3) + "**" + phone.Substring(phone.Length - 1, 1);
                }
                else
                {
                    return phone.Substring(0, 1) + "**";
                }
            }

        }



        #region    需求 记录










        //将眼科病的分类结果的父项目从id转换为文字
        public static string resTagFather(string input)
        {

            //res_l_4  res_r_8 因为有 ▲ 不可能为空或null，但还是判断下，无伤大雅
            try
            {

                if (string.IsNullOrEmpty(input))
                {

                    return "未记录";
                }
                else
                {


                    string key = input.Split('_')[2];
                    string strUpart = "";

                    switch (key)
                    {
                        case "1":
                            strUpart = "葡萄膜黑色素瘤:";
                            break;
                        case "2":
                            strUpart = "视网膜静脉阻塞:";
                            break;
                        case "3":
                            strUpart = "年龄相关性黄斑变性:";
                            break;
                        case "4":
                            strUpart = "糖尿病视网膜病变:";
                            break;
                        case "5":
                            strUpart = "视网膜脱离:";
                            break;
                        case "6":
                            strUpart = "屈光不正:";
                            break;
                        case "7":
                            strUpart = "病理性近视:";
                            break;
                        case "8":
                            strUpart = "其它:";
                            break;
                        case "9":
                            strUpart = "手术方式:";
                            break;
                        case "10":
                            strUpart = "用药情况:";
                            break;
                        case "11":
                            strUpart = "中心性浆液性脉络膜视网膜病:";
                            break;
                        case "12":
                            strUpart = "视神经萎缩:";
                            break;
                        default:
                            strUpart = "字符串错误！";
                            break;
                    }

                    return strUpart;

                }

            }
            catch
            {
                return "未记录";



            }



        }


        //将近视的分类结果的父项目从id转换为文字
        public static string resTagFatherJs(string input)
        {

            //res_l_4  res_r_8 因为有 ▲ 不可能为空或null，但还是判断下，无伤大雅
            try
            {

                if (string.IsNullOrEmpty(input))
                {

                    return "未记录";
                }
                else
                {


                    string key = input.Split('_')[2];
                    string strUpart = "";

                    switch (key)
                    {
                        case "1":
                            strUpart = "近视:";
                            break;
                        case "2":
                            strUpart = "远视:";
                            break;
                        case "3":
                            strUpart = "散光:";
                            break;
                        case "4":
                            strUpart = "其他:";
                            break;
                        default:
                            strUpart = "字符串错误！";
                            break;
                    }

                    return strUpart;

                }

            }
            catch
            {
                return "未记录";



            }



        }





        //0三角形方法  解析成为一个可以理解的文字
        public static string HisQsLrConfig(string input)
        {
            /*
             * 传进来的糖尿病 高血压 。。。只有下面3种情况
             *   "" 这中就直接 认为糖尿病 没勾选 
             *   "1▲"
             *   "1▲填了备注"
             */
            try
            {
                string output = "";
                if (string.IsNullOrEmpty(input))
                {

                    return "未记录"; //对应上面的 "" 空的情况 直接 未记录
                }
                else
                {
                    string[] key = input.Split('▲');


                    output += "有,备注:" + CheckStrEmpty(key[1]);//对应上面后2中情况 1代表是有糖尿病，备注 走CheckStrEmpty验证



                    return output;

                }

            }
            catch
            {
                return "未记录";



            }



        }

        //将眼科病的状态 无 有 不详分出来
        public static string hiseyeState(string input)
        {

            //不可能为空或null，但还是判断下，无伤大雅
            try
            {

                if (string.IsNullOrEmpty(input))
                {

                    return "未记录";
                }
                else
                {


                    string strUpart = "";

                    switch (input)
                    {
                        case "0":
                            strUpart = "无";
                            break;
                        case "1":
                            strUpart = "有";
                            break;
                        case "2":
                            strUpart = "不详";
                            break;
                        default:
                            strUpart = "字符串错误！";
                            break;
                    }

                    return strUpart;

                }

            }
            catch
            {
                return "未记录";



            }



        }

        //眼科病史通用方法
        public static string HisEyeLrConfig(string input_0, string input)
        {
            // 因为有三角形 还是判断 空或null
            try
            {
                string output = "";
                string[] hiseyeType = { "白内障", "白内障备注", "青光眼", "青光眼备注", "糖尿病视网膜疾病", "糖尿病视网膜疾病诊断时间", "其他玻璃体视网膜疾病", "其他玻璃体视网膜疾病诊断时间", "其他", "外伤以及手术史术式", "术式诊断时间", "其他治疗史" };

                if (string.IsNullOrEmpty(input_0))
                {

                    return "未记录";
                }
                else
                {

                    for (int i = 0; i < input_0.Split('▲').Length; i++)  //遍历每一项
                    {

                        if (input_0.Split('▲')[i] != input.Split('▲')[i]) //判断新旧字符串值是否不等
                        {
                            if (i == 0) //青光眼  此处判断白内障和青光眼 左眼 和 右眼 都把 白内障、青光眼 和 其他项分为两类  白内障和青光眼都是第一、二项，有默认值
                            {
                                output += "白内障 由 " + hiseyeState(input_0.Split('▲')[i]) + " 修改为 " + hiseyeState(input.Split('▲')[i]) + "；";

                            }
                            else if (i == 2)//白内障
                            {

                                output += "青光眼 由 " + hiseyeState(input_0.Split('▲')[i]) + " 修改为 " + hiseyeState(input.Split('▲')[i]) + "；";

                            }
                            else
                            {
                                output += hiseyeType[i] + " 由 " + CheckStrEmpty(input_0.Split('▲')[i]) + " 修改为 " + CheckStrEmpty(input.Split('▲')[i]) + "；";

                            }
                        }

                    }

                    return output;

                }

            }
            catch
            {
                return "未记录";



            }



        }

        //// 眼科病史 hiseye 顺序调整方法,前台js和后台回显两者不同，就统一调整成页面效果的排列顺序
        //public static string hiseyeStr(string hiseye)
        //{


        //    hiseye = hiseye.Split('▲')[0] + "▲" + hiseye.Split('▲')[9] + "▲" + hiseye.Split('▲')[1] + "▲" + hiseye.Split('▲')[10] + "▲" + hiseye.Split('▲')[2] + "▲" +
        //    hiseye.Split('▲')[3] + "▲" + hiseye.Split('▲')[4] + "▲" + hiseye.Split('▲')[5] + "▲" + hiseye.Split('▲')[8] + "▲" + hiseye.Split('▲')[6] + "▲" + hiseye.Split('▲')[7] + "▲" + hiseye.Split('▲')[11];


        //    return hiseye;
        //}

        //// 眼科病史 hiseye_0 顺序调整方法,前台js和后台回显两者不同，就统一调整成页面效果的排列顺序
        //public static string hiseyeStr_0(string hiseye_0)
        //{


        //    hiseye_0 = hiseye_0.Split('▲')[0] + "▲" + hiseye_0.Split('▲')[9] + "▲" + hiseye_0.Split('▲')[1] + "▲" + hiseye_0.Split('▲')[10] + "▲" + hiseye_0.Split('▲')[2] + "▲" +
        //    hiseye_0.Split('▲')[3] + "▲" + hiseye_0.Split('▲')[4] + "▲" + hiseye_0.Split('▲')[5] + "▲" + hiseye_0.Split('▲')[6] + "▲" + hiseye_0.Split('▲')[7] + "▲" + hiseye_0.Split('▲')[8] + "▲" + hiseye_0.Split('▲')[11];


        //    return hiseye_0;
        //}

        // 眼科病史检查置空判断方法 这里置空也调整为和上面的顺序
        public static string hiseyeStrEmpty(string hiseye)
        {

            if (hiseye.Split('■')[2] == "0" || hiseye.Split('■')[2] == "2")
            {
                hiseye = "0▲▲0▲▲▲▲▲▲▲▲▲■0▲▲0▲▲▲▲▲▲▲▲▲■" + hiseye.Split('■')[2];
            }

            return hiseye;
        }

        // 眼科病眼部检查置空判断方法
        public static string eyecheckStrEmptyYkb(string input)
        {
            string eyecheckSl = input.Split('●')[0];
            if (eyecheckSl.Split('▲')[0] == "0")
            {
                eyecheckSl = "0▲▲▲▲▲▲";
            }
            string eyecheckYy = input.Split('●')[1];
            if (eyecheckYy.Split('▲')[0] == "0")
            {
                eyecheckYy = "0▲";
            }
            string eyecheckYbqj = input.Split('●')[2];
            if (eyecheckYbqj.Split('▲')[0] == "0" || eyecheckYbqj.Split('▲')[0] == "1")
            {
                eyecheckYbqj = eyecheckYbqj.Split('▲')[0] + "▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲";
            }
            string eyecheckYd = input.Split('●')[3];
            if (eyecheckYd.Split('▲')[0] == "0" || eyecheckYd.Split('▲')[0] == "1")
            {
                eyecheckYd = eyecheckYd.Split('▲')[0] + "▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲▲▲▲";



            }

            return eyecheckSl + "●" + eyecheckYy + "●" + eyecheckYbqj + "●" + eyecheckYd;
        }


        // 近视眼部检查置空判断方法
        public static string eyecheckStrEmptyJs(string input)
        {
            string eyecheckSl = input.Split('●')[0];
            if (eyecheckSl.Split('▲')[0] == "0")
            {

                eyecheckSl = "0▲▲▲▲▲▲▲";

            }
            string eyecheckYy = input.Split('●')[1];
            if (eyecheckYy.Split('▲')[0] == "0")
            {
                eyecheckYy = "0▲";
            }
            string eyecheckLxd = input.Split('●')[2];
            if (eyecheckLxd.Split('▲')[0] == "0" || eyecheckLxd.Split('▲')[0] == "1")
            {
                eyecheckLxd = eyecheckLxd.Split('▲')[0] + "▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲▲0▲▲▲";
            }
            string eyecheckLxdYd = input.Split('●')[3];
            if (eyecheckLxdYd.Split('▲')[0] == "0" || eyecheckLxdYd.Split('▲')[0] == "1")
            {
                eyecheckLxdYd = eyecheckLxdYd.Split('▲')[0] + "▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲0▲▲";
            }
            string eyecheckLenstar = input.Split('●')[4];
            if (eyecheckLenstar.Split('▲')[0] == "0")
            {
                eyecheckLenstar = eyecheckLenstar.Split('▲')[0] + "▲▲▲▲▲▲▲/▲/▲/▲▲▲/▲▲/";
            }
            return eyecheckSl + "●" + eyecheckYy + "●" + eyecheckLxd + "●" + eyecheckLxdYd + "●" + eyecheckLenstar;
        }


        // 眼科病解析视力方法
        public static string EyecheckSlConfig(string input)
        {
            // 不可能为 空或 null
            try
            {

                if (string.IsNullOrEmpty(input))
                {

                    return "未记录";
                }
                else
                {
                    string strUpart = "";
                    switch (input.Split('▲')[0]) //切割出来视力检查第一项 的 未查 已查 用以下面判断
                    {
                        case "0":
                            strUpart += "未查";
                            break;
                        case "1":
                            strUpart += "已查:裸眼视力 " + CheckStrEmpty(input.Split('▲')[1]) + " 矫正视力 " + CheckStrEmpty(input.Split('▲')[2]) + " 球镜度数: " + CheckStrEmpty(input.Split('▲')[3]) + " 柱镜度数: " + CheckStrEmpty(input.Split('▲')[4]) + " 轴向: " + CheckStrEmpty(input.Split('▲')[5]) + " 最佳矫正视力: " + CheckStrEmpty(input.Split('▲')[6]);
                            break;
                        default:
                            strUpart = "字符串错误！";
                            break;
                    }


                    return strUpart;

                }

            }
            catch
            {
                return "未记录";



            }



        }

        // 近视解析视力方法
        public static string EyecheckSlConfigJs(string input)
        {
            // 不可能为 空或 null
            try
            {
                //string[] str1 = { "远视", "正视", "低度近视", "中度近视", "重度近视" };
                if (string.IsNullOrEmpty(input))
                {

                    return "未记录";
                }
                else
                {
                    string strUpart = "";
                    switch (input.Split('▲')[0])
                    {
                        case "0":
                            strUpart += "未查";
                            break;
                        case "1":
                            strUpart += "已查:裸眼视力 " + CheckStrEmpty(input.Split('▲')[1]) + " 矫正视力 " + CheckStrEmpty(input.Split('▲')[2]) + " 近视力等级Jr " + CheckStrEmpty(input.Split('▲')[3]) + " 球镜度数: " + CheckStrEmpty(input.Split('▲')[4]) + " 柱镜度数: " + CheckStrEmpty(input.Split('▲')[5]) + " 轴向: " + CheckStrEmpty(input.Split('▲')[6]) + " 最佳矫正视力: " + CheckStrEmpty(input.Split('▲')[7]);
                            break;
                        default:
                            strUpart = "字符串错误！";
                            break;
                    }


                    return strUpart;

                }

            }
            catch
            {
                return "未记录";



            }



        }

        //解析眼压方法
        public static string EyecheckYyConfig(string input)
        {
            // 不可能为空 或 null
            try
            {

                if (string.IsNullOrEmpty(input))
                {

                    return "未记录";
                }
                else
                {
                    string strUpart = "";
                    switch (input.Split('▲')[0]) // 眼压 的 未查 已查
                    {
                        case "0":
                            strUpart += "未查";
                            break;
                        case "1":
                            strUpart += "已查:眼压 " + CheckStrEmpty(input.Split('▲')[1]) + " mmHg"; //眼压
                            break;
                        default:
                            strUpart = "字符串错误！";
                            break;
                    }


                    return strUpart;

                }

            }
            catch
            {
                return "未记录";



            }



        }






        #region      //眼部前节检查




        //整体状态是几种检查的父项共用的  
        static string[] eyecheckTotal = { "未查", "已查——无明显异常", "已查——有异常" };


        //把选择的名称记录下来
        static string[] eyecheckYbqj = { "整体状态","结膜充血", "角膜", "KP", "Tyn", "瞳孔", "瞳孔对光反射", "虹膜异色", "虹膜新生血管",
            "人工晶体", "玻璃体浑浊", "晶状体"  ,"皮质浑浊","核浑浊","囊膜混浊","位置不详"};


        //眼前节检查的具体情况  用字典key是名称 value是string List 做映射
        static Dictionary<string, List<string>> eyecheckYbqj_Dic =
    new Dictionary<string, List<string>> {

        { "结膜充血", new List<string> () { "无", "有" } },
        { "角膜", new List<string> () { "透明", "不透明" }},
        { "KP", new List<string> () { "无", "有" } },
        { "Tyn", new List<string> () { "无", "有" } },
        { "瞳孔", new List<string> () { "圆", "不圆" } },
        { "瞳孔对光反射", new List<string> () { "存在", "消失" } },
        { "虹膜异色", new List<string> () { "无", "有" } },
        { "虹膜新生血管", new List<string> () { "无", "有" } },
        { "人工晶体", new List<string> () { "无", "位正", "移位" } },
        { "玻璃体浑浊", new List<string> () { "无", "有" } },
        { "晶状体", new List<string> () { "透明", "浑浊", "无晶体", "人工晶体" } },
        { "皮质浑浊", new List<string> () { "无", "I级", "II级", "III级", "IV级", "V级", "等级不详" } },
        { "核浑浊", new List<string> () { "无", "I级", "II级", "III级", "IV级", "V级", "等级不详" }},
        { "囊膜混浊", new List<string> () { "无", "I级", "II级", "III级", "IV级", "V级", "等级不详" } },

        { "位置不详", new List<string> () { "未选定", "选定" } }



    };





        public static string EyecheckYbqjConfig(string check_L_0, string check_L)
        {
            // 不可能为 空 null
            try
            {


                string strUpart = "";

                string[] items_0 = check_L_0.Split('▲');
                string[] items = check_L.Split('▲');

                if (items_0[0] != items[0])
                {

                    strUpart += eyecheckTotal[int.Parse(items_0[0])] + " 修改为 " + eyecheckTotal[int.Parse(items[0])] + "；";
                }

                ArrayList dif_key = new ArrayList();

                //高伟豪。注意因为这里的业务逻辑，只要是整体从有数据到没数据。那么其他项目数据都复位为空。
                //所以如果眼部前节检查的变更后整体为0or1其他的直接不记录。


                for (int i = 1; i < items_0.Length - 8; i++)
                {

                    if (items_0[i] != items[i])
                    {
                        string name = eyecheckYbqj[i];

                        strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[i])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[i])] + "；";

                    }

                }

                // 以下是人工晶体 玻璃体浑浊 晶状体调换顺序 暂时没想到更好方法

                if (items_0[items_0.Length - 7] != items[items.Length - 7])
                {
                    string name = eyecheckYbqj[items_0.Length - 7];

                    strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[items_0.Length - 7])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[items.Length - 7])] + "；";

                }



                if (items_0[items_0.Length - 6] != items[items.Length - 6])
                {
                    string name = eyecheckYbqj[items_0.Length - 6];

                    strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[items_0.Length - 6])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[items.Length - 6])] + "；";

                }

                if (items_0[items_0.Length - 5] != items[items.Length - 5])
                {
                    string name = eyecheckYbqj[items_0.Length - 5];

                    strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[items_0.Length - 5])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[items.Length - 5])] + "；";

                }

                if (items_0[items_0.Length - 4] != items[items.Length - 4])
                {
                    string name = eyecheckYbqj[items_0.Length - 4];

                    strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[items_0.Length - 4])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[items.Length - 4])] + "；";

                }

                if (items_0[items_0.Length - 3] != items[items.Length - 3])
                {
                    string name = eyecheckYbqj[items_0.Length - 3];

                    strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[items_0.Length - 3])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[items.Length - 3])] + "；";

                }

                if (items_0[items_0.Length - 2] != items[items.Length - 2])
                {
                    string name = eyecheckYbqj[items_0.Length - 2];

                    strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[items_0.Length - 2])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[items.Length - 2])] + "；";

                }

                if (items_0[items_0.Length - 8] != items[items.Length - 8])
                {
                    string name = eyecheckYbqj[items_0.Length - 8];

                    strUpart += name + " 由 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items_0[items_0.Length - 8])] + " 修改为 " + eyecheckYbqj_Dic[name][Convert.ToInt32(items[items.Length - 8])] + "；";

                }

                if (items_0[items_0.Length - 1] != items[items.Length - 1])
                {
                    strUpart += "其他异常 由 " + CheckStrEmpty(items_0[items_0.Length - 1]) + " 修改为 " + CheckStrEmpty(items[items.Length - 1]) + "；";
                }

                return strUpart;

            }
            catch
            {
                return "未记录";



            }



        }



        #endregion



        #region      //眼底检查




        //整体状态是几种检查的父项共用的  
        static string[] eyecheckTotalYd = { "未查", "已查——无明显异常", "已查——有异常" };


        //把选择的名称记录下来
        static string[] eyecheckYd = { "整体状态", "视盘斜入", "视盘颜色", "视盘边界清晰", "视盘水肿", "豹纹状眼底", "后巩膜葡萄肿", "玻璃体积血", "视网膜前出血", "视网膜脱离", "视网膜下液", "视网膜出血", "视网膜渗出", "视网膜新生血管", "黄斑变性", "黄斑裂孔", "黄斑前膜", "黄斑水肿", "黄斑CNV(脉络膜新生血管)" };

        //眼前节检查的具体情况  用字典key是名称 value是string List 做映射
        static Dictionary<string, List<string>> eyecheckYd_Dic =
    new Dictionary<string, List<string>> {

        { "视盘斜入", new List<string> () { "无", "有" } },
        { "视盘颜色", new List<string> () { "正常", "色淡", "苍白" } },
        { "视盘边界清晰", new List<string> () { "无", "有" } },
        { "视盘水肿", new List<string> () { "是", "否" } },
        { "豹纹状眼底", new List<string> () { "无", "有" }},
        { "后巩膜葡萄肿", new List<string> () { "无", "有" } },
        { "玻璃体积血", new List<string> () { "无", "有" } },
        { "视网膜前出血", new List<string> () { "无", "有" } },
        { "视网膜脱离", new List<string> () { "无", "有" } },
        { "视网膜下液", new List<string> () { "无", "有" } },
        { "视网膜出血", new List<string> () { "无", "有" } },
        { "视网膜渗出", new List<string> () { "无", "有" } },
        { "视网膜新生血管", new List<string> () { "无", "有" } },
        { "黄斑变性", new List<string> () { "无", "有" } },
        { "黄斑裂孔", new List<string> () { "无", "有" }},
        { "黄斑前膜", new List<string> () { "无", "有" } },
        { "黄斑水肿", new List<string> () { "无", "有" } },
        { "黄斑CNV(脉络膜新生血管)", new List<string> () { "无", "有" } },
        //{ "眼内肿瘤", new List<string> () { "无", "有" } },
        //{ "肿瘤位置", new List<string> () { "无", "黄斑区", "视盘", "周边视网膜" } },
        //{ "瘤体形态", new List<string> () { "无", "蘑菇样", "其他" }},
        //{ "表面色素", new List<string> () { "无", "多量", "少量", "缺乏色素" } }

    };





        public static string EyecheckYdConfig(string check_L_0, string check_L)
        {
            // 不可能为 空 null
            try
            {


                string strUpart = "";

                string[] items_0 = check_L_0.Split('▲');
                string[] items = check_L.Split('▲');

                if (items_0[0] != items[0])
                {

                    strUpart += eyecheckTotalYd[int.Parse(items_0[0])] + " 修改为 " + eyecheckTotalYd[int.Parse(items[0])] + "；";
                }

                //高伟豪。注意因为这里的业务逻辑，只要是整体从有数据到没数据。那么其他项目数据都复位为空。
                //所以如果眼部前节检查的变更后整体为0or1其他的直接不记录。


                for (int i = 1; i < items_0.Length - 5; i++)
                {


                    if (items_0[i] != items[i])
                    {
                        string name = eyecheckYd[i];

                        strUpart += name + " 由 " + eyecheckYd_Dic[name][Convert.ToInt32(items_0[i])] + " 修改为 " + eyecheckYd_Dic[name][Convert.ToInt32(items[i])] + "；";

                    }


                }


                string YdzlStr_0 = items_0[items_0.Length - 5] + "▲" + items_0[items_0.Length - 4] + "▲" + items_0[items_0.Length - 3] + "▲" + items_0[items_0.Length - 2];
                string YdzlStr = items[items.Length - 5] + "▲" + items[items.Length - 4] + "▲" + items[items.Length - 3] + "▲" + items[items.Length - 2];




                if (YdzlStr_0 != YdzlStr)
                {

                    strUpart += "眼内肿瘤 由 " + YdzlConfig(YdzlStr_0) + " 修改为 " + YdzlConfig(YdzlStr) + "；";

                }


                if (items_0[items_0.Length - 1] != items[items.Length - 1])
                {
                    strUpart += "其他 由 " + CheckStrEmpty(items_0[items_0.Length - 1]) + " 修改为 " + CheckStrEmpty(items[items.Length - 1]) + "；";
                }




                return strUpart;

            }
            catch
            {

                return "未记录";



            }



        }

        public static string YdzlConfig(string input)
        {
            try
            {


                string strUpart = "";
                switch (input.Split('▲')[0])
                {
                    case "0":
                        strUpart += "无";
                        break;
                    case "1":
                        strUpart += "有";
                        strUpart += " 肿瘤位置:" + CheckStrEmpty(input.Split('▲')[1]) + "；瘤体形态:" + CheckStrEmpty(input.Split('▲')[2]) + "；表面色素:" + CheckStrEmpty(input.Split('▲')[3]);
                        break;
                    default:
                        strUpart = "字符串错误！";
                        break;
                }


                return strUpart;



            }
            catch
            {
                return "未记录";



            }


        }


        #endregion


        #region      //裂隙灯检查




        //整体状态是几种检查的父项共用的  
        static string[] eyecheckTotalLxd = { "未查", "已查——无明显异常", "已查——有异常" };


        //把选择的名称记录下来
        static string[] eyecheckLxd = { "整体状态", "结膜充血", "角膜", "KP", "Tyn", "瞳孔", "黄斑中心凹反光", "晶状体", "皮质浑浊", "核浑浊", "囊膜混浊", "位置不详" };


        //眼前节检查的具体情况  用字典key是名称 value是string List 做映射
        static Dictionary<string, List<string>> eyecheckLxd_Dic =
    new Dictionary<string, List<string>> {

        { "结膜充血", new List<string> () { "无", "有" } },
        { "角膜", new List<string> () { "透明", "不透明" }},
        { "KP", new List<string> () { "无", "有" } },
        { "Tyn", new List<string> () { "无", "有" } },
        { "瞳孔", new List<string> () { "圆", "不圆" } },
        { "黄斑中心凹反光", new List<string> () { "可见", "不可见" } },
        { "晶状体", new List<string> () { "透明", "浑浊" } },


        { "皮质浑浊", new List<string> () { "无", "I级", "II级", "III级", "IV级", "V级", "等级不详" } },
        { "核浑浊", new List<string> () { "无", "I级", "II级", "III级", "IV级", "V级", "等级不详" }},
        { "囊膜混浊", new List<string> () { "无", "I级", "II级", "III级", "IV级", "V级", "等级不详" } },
        { "位置不详", new List<string> () { "未选定", "选定" } }

    };





        public static string EyecheckLxdConfig(string check_L_0, string check_L)
        {
            // 不可能为 空 null
            try
            {


                string strUpart = "";

                string[] items_0 = check_L_0.Split('▲');
                string[] items = check_L.Split('▲');

                if (items_0[0] != items[0])
                {

                    strUpart += eyecheckTotalLxd[int.Parse(items_0[0])] + " 修改为 " + eyecheckTotalLxd[int.Parse(items[0])] + "；";
                }

                //高伟豪。注意因为这里的业务逻辑，只要是整体从有数据到没数据。那么其他项目数据都复位为空。
                //所以如果眼部前节检查的变更后整体为0or1其他的直接不记录。


                for (int i = 1; i < items_0.Length - 6; i++)
                {

                    if (items_0[i] != items[i])
                    {
                        string name = eyecheckLxd[i];

                        strUpart += name + " 由 " + eyecheckLxd_Dic[name][Convert.ToInt32(items_0[i])] + " 修改为 " + eyecheckLxd_Dic[name][Convert.ToInt32(items[i])] + "；";

                    }

                }


                if ((items_0[items_0.Length - 6] + items_0[items_0.Length - 5]) != (items[items.Length - 6] + items[items.Length - 5]))
                {
                    strUpart += "虹膜纹理 由 " + StatusTranHs(1, int.Parse(items_0[items_0.Length - 6])) + " " + items_0[items_0.Length - 5] + " 修改为 " + StatusTranHs(1, int.Parse(items[items.Length - 6])) + " " + items[items.Length - 5] + "；";
                }

                if ((items_0[items_0.Length - 4] + items_0[items_0.Length - 3]) != (items[items.Length - 4] + items[items.Length - 3]))
                {
                    strUpart += "视乳头颜色 由 " + StatusTranHs(1, int.Parse(items_0[items_0.Length - 4])) + " " + items_0[items_0.Length - 3] + " 修改为 " + StatusTranHs(1, int.Parse(items[items.Length - 4])) + " " + items[items.Length - 3] + "；";
                }

                if (items_0[items_0.Length - 2] != items[items.Length - 2])
                {
                    strUpart += "杯盘比C/D 由 " + CheckStrEmpty(items_0[items_0.Length - 2]) + " 修改为 " + CheckStrEmpty(items[items.Length - 2]) + "；";
                }

                if (items_0[items_0.Length - 1] != items[items.Length - 1])
                {
                    strUpart += "其他异常 由 " + CheckStrEmpty(items_0[items_0.Length - 1]) + " 修改为 " + CheckStrEmpty(items[items.Length - 1]) + "；";
                }

                return strUpart;

            }
            catch
            {
                return "未记录";



            }



        }






        #endregion






        #region      //裂隙灯眼底检查




        //整体状态是几种检查的父项共用的  
        static string[] eyecheckTotalLxdYd = { "未查", "已查——无明显异常", "已查——有异常" };


        //把选择的名称记录下来
        static string[] eyecheckLxdYd = { "整体状态","视盘斜入", "豹纹状眼底", "后巩膜葡萄肿", "弥漫性脉络膜视网膜萎缩", "斑片状脉络膜视网膜萎缩", "黄斑萎缩", "Fuchs斑", "脉络膜新生血管",
            "膝裂纹", "眼底出血", "视网膜脱离"  ,"黄斑裂孔","黄斑前膜", "视网膜变性区", "视网膜裂孔"};

        //眼前节检查的具体情况  用字典key是名称 value是string List 做映射
        static Dictionary<string, List<string>> eyecheckLxdYd_Dic =
    new Dictionary<string, List<string>> {

        { "视盘斜入", new List<string> () { "无", "有" } },
        { "豹纹状眼底", new List<string> () { "无", "有" }},
        { "后巩膜葡萄肿", new List<string> () { "无", "有" } },
        { "弥漫性脉络膜视网膜萎缩", new List<string> () { "无", "有" } },
        { "斑片状脉络膜视网膜萎缩", new List<string> () { "无", "有" } },
        { "黄斑萎缩", new List<string> () { "无", "有" } },
        { "Fuchs斑", new List<string> () { "无", "有" } },
        { "脉络膜新生血管", new List<string> () { "无", "有" } },
        { "膝裂纹", new List<string> () { "无", "有" }},
        { "眼底出血", new List<string> () { "无", "有" } },
        { "视网膜脱离", new List<string> () { "无", "有" } },
        { "黄斑裂孔", new List<string> () { "无", "有" } },
        { "黄斑前膜", new List<string> () { "无", "有" }},
        { "视网膜变性区", new List<string> () { "无", "有" }},
        { "视网膜裂孔", new List<string> () { "无", "有" }}

    };





        public static string EyecheckLxdYdConfig(string check_L_0, string check_L)
        {
            // 不可能为 空 null
            try
            {


                string strUpart = "";

                string[] items_0 = check_L_0.Split('▲');
                string[] items = check_L.Split('▲');

                if (items_0[0] != items[0])
                {

                    strUpart += eyecheckTotalLxdYd[int.Parse(items_0[0])] + " 修改为 " + eyecheckTotalLxdYd[int.Parse(items[0])] + "；";
                }

                //高伟豪。注意因为这里的业务逻辑，只要是整体从有数据到没数据。那么其他项目数据都复位为空。
                //所以如果眼部前节检查的变更后整体为0or1其他的直接不记录。

                //因为前台有个lxdyd_rmk没用字段，所以下面减2，前台lxdyd_rmk先别删，要用到的话
                for (int i = 1; i < items_0.Length - 2; i++)
                {

                    if (items_0[i] != items[i])
                    {
                        string name = eyecheckLxdYd[i];

                        strUpart += name + " 由 " + eyecheckLxdYd_Dic[name][Convert.ToInt32(items_0[i])] + " 修改为 " + eyecheckLxdYd_Dic[name][Convert.ToInt32(items[i])] + "；";
                    }

                }

                if (items_0[items_0.Length - 2] != items[items.Length - 2])
                {
                    strUpart += "其他 由 " + CheckStrEmpty(items_0[items_0.Length - 2]) + " 修改为 " + CheckStrEmpty(items[items.Length - 2]) + "；";
                }

                return strUpart;

            }
            catch
            {
                return "未记录";



            }



        }






        #endregion



        #region      //lenstar检查




        //整体状态是几种检查的父项共用的  
        static string[] eyecheckTotalLenstar = { "未查", "已查" };

        //把填写的项目名称记录下来
        public static string[,] eyecheckLenstar_Dic =
            { { "Measuring mode (Mode) ", " " } ,
            { "Axial length (AL) ", " mm" },
            { "Cornea thickness (CCT) ", " μm" } ,
            { "Aqueous depth (AD) ", " Mm" } ,
            { "Lens thickness (LT) ", " mm" } ,
            { "Retina thickness (RT) ", " μm" } ,
            { "Flat meridian (K1) ", " °" } ,
            { "Steep meridian (K2) ", " °" } ,
            { "Astigmatism (AST) ", " °" } ,
            { "Keratometric index (n) ", " " },
            { "White to White (WTW) ", " mm" } ,
            { "Iris barycenter (IC) ", " mm" } ,
            { "Pupil diameter (PD) ", " mm" } ,
            { "Pupil barycenter (PC) ", " mm" } };





        public static string EyecheckLenstar(string check_L_0, string check_L)
        {
            // 不可能为 空 null
            try
            {


                string strUpart = "";

                string[] items_0 = check_L_0.Split('▲');
                string[] items = check_L.Split('▲');

                if (items_0[0] != items[0])
                {

                    strUpart += eyecheckTotalLenstar[int.Parse(items_0[0])] + " 修改为 " + eyecheckTotalLenstar[int.Parse(items[0])] + "；";
                }


                for (int i = 1; i < items_0.Length; i++)
                {
                    if (items_0[i] != items[i])
                    {

                        strUpart += eyecheckLenstar_Dic[i - 1, 0] + "由 " + CheckStrEmpty(items_0[i]) + eyecheckLenstar_Dic[i - 1, 1] + " 修改为 " + CheckStrEmpty(items[i]) + eyecheckLenstar_Dic[i - 1, 1] + "；";

                    }
                }



                return strUpart;

            }
            catch
            {
                return "未记录";



            }



        }






        #endregion








        #region      近视现病史 主诉病情






        //把选择的名称记录下来
        static string[] jszsitems = { "视力下降年", "视力下降开始", "视力稳定持续年", "视力稳定开始",
            "是否佩戴隐形眼镜", "隐形持续年", "隐形佩戴开始","隐形佩戴停止","接触镜种类","其他" };








        public static string JsZsConfig(string check_L_0, string check_L)
        {
            // 不可能为 空 null
            try
            {


                string strUpart = "";

                string[] items_0 = check_L_0.Split('▲');
                string[] items = check_L.Split('▲');




                for (int i = 0; i < items_0.Length; i++)
                {

                    if (items_0[i] != items[i])
                    {
                        string name = jszsitems[i];

                        strUpart += name + " 由 " + items_0[i] + " 修改为 " + items[i] + "；";

                    }

                }



                return strUpart;

            }
            catch
            {
                return "未记录";



            }



        }






        #endregion









        #endregion

    }

}
