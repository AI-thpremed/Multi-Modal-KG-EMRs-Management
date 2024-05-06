using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CaseLifestyleModels
    {



        public long ID { get; set; }

        public long CASE_ID { get; set; }


        public int SMOKE_TYPE { get; set; }
        public int SMOKE_QUIT_CON { get; set; }

        public string SMOKE_QUIT{ get; set; }


        public string SMOKE_INFO { get; set; }

        public int DRINK_TYPE { get; set; }

        public string DRINK_FRE { get; set; }


        public string DRINK_INFO { get; set; }
        public string DRINK_OVER { get; set; }



        public string RMK { get; set; }
        public long UPDATE_ID { get; set; }

        public DateTime UPDATE_DATE { get; set; }










    }
}
