using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CaseInfoModels
    {






        public long ID { get; set; }

        public string SYS_ID { get; set; }

        public string CASE_NAME { get; set; }
        public int TYPE_ID { get; set; }
        public int SEX { get; set; }
        public string HOP_ID { get; set; }
        public string CONTACT{ get; set; }

        public string CONTACT_2 { get; set; }

        public string BIRTHDAY { get; set; }
        public string WHO_HIS { get; set; }
        public string OPH_HIS { get; set; }
        public string SYMPTOM { get; set; }
        public string RESULT { get; set; }

        public string RACE { get; set; }
        public string MARRIAGE { get; set; }
        public string JOB { get; set; }
        public string EDU { get; set; }

        public string ADDRESS { get; set; }
        public string TAG_ID_L { get; set; }
        public string TAG_ID_R { get; set; }


        public int AUDIT { get; set; }

        public int SOURCE_ID { get; set; }
        public int STATUS { get; set; }

        public string RMK { get; set; }
        public int IS_DELETE { get; set; }


        public long PUBLISHER_ID { get; set; }
        public long PASS_ID { get; set; }
        public long FINAL_ID { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime HOP_DATE { get; set; }
        public DateTime PASS_DATE { get; set; }
        public DateTime FINAL_DATE { get; set; }

        public int HOP_AGE { get; set; }













    }
}
