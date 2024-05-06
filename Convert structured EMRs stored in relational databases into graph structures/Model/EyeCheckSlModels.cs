using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EyeCheckSlModels
    {


        public long ID { get; set; }

        public long CASE_ID { get; set; }


        public int L_R { get; set; }
        public int STATUS { get; set; }


        public int TYPE { get; set; }

        public string INFO { get; set; }
        public string JIAOZHENG { get; set; }
        public string LUOYAN { get; set; }

        public string XIANRAN_DS { get; set; }

        public string XIANRAN_ZJDS { get; set; }

        public string XIANRAN_ZX { get; set; }

        public string XIANRAN_SL { get; set; }

        public string RMK { get; set; }
        public long UPDATE_ID { get; set; }

        public DateTime UPDATE_DATE { get; set; }






    }
}
