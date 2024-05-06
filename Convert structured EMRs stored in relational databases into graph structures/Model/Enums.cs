using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 主要记录一些枚举类型以 Dict开头
    /// 
    /// gao 枚举类型实际上是直接在models里面就改了。。。架设数据库是012。那么model类直接改成string
    /// 
    /// 
    /// </summary>
    public class Enums
    {
      
        /// <summary>
        /// 非app订单类型
        /// </summary>
        public enum DictOrderType
        {
            淘宝订单 = 0,
            微信订单 = 1,
            线下订单 = 2,
        }
        /// <summary>
        /// 订单状态 0已支付21.订单已发货，100.已收货交易完成，31.退款处理中，91.已退款交易关闭，92.交易关闭
        /// </summary>
        public enum DictOrderStatus
        {
            订单已支付 = 0,
            订单已发货 = 21,
            已收货交易完成 = 100,
            退款处理中=31,
            已退款交易关闭=91,
            交易关闭=92
        }
        public enum DictOrderFactorStatus
        {
            未制单 = 0,
            制单完成 = 1,

        }

        public enum DictOrderGramentChuanYiXiaoGuo
        {
            修身=1,
            合体=2,
            宽松=3
        }

//        0已支付21.订单已发货，100.已收货交易完成，31.退款处理中，91.已退款交易关闭，92.交易关闭
        public enum OrderStatus
        {
            订单已支付 = 0,
            订单已发货 = 21,
            已收货交易完成 = 100,
            退款处理中 = 31,
            已退款交易关闭 = 91,
            交易关闭 = 92
        }









    }
}
