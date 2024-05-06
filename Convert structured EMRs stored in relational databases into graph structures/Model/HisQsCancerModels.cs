using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HisQsCancerModels
    {




        public long ID { get; set; }

        public long CASE_ID { get; set; }


        public int STATUS_F { get; set; }
        public int STATUS_G { get; set; }
        public int STATUS_RX { get; set; }
        public int STATUS_LB { get; set; }
        public int STATUS_QT { get; set; }


        public DateTime FIST_DIA_F { get; set; }

        public DateTime FIST_DIA_G { get; set; }
        public DateTime FIST_DIA_RX { get; set; }
        public DateTime FIST_DIA_LB { get; set; }
        public DateTime FIST_DIA_QT { get; set; }




        public string TYPE_QT { get; set; }


        public string TRE_INFO_F { get; set; }

        public string TRE_INFO_G { get; set; }
        public string TRE_INFO_RX { get; set; }
        public string TRE_INFO_LB { get; set; }
        public string TRE_INFO_QT { get; set; }
 





        public long UPDATE_ID { get; set; }

        public DateTime UPDATE_DATE { get; set; }






    }
}
