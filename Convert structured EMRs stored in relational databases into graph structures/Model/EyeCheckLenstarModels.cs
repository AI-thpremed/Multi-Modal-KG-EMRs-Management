using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
     public class EyeCheckLenstarModels
    {




        public long ID { get; set; }

        public long CASE_ID { get; set; }

        public int L_R { get; set; }

        public int STATUS { get; set; }

        public string MODE { get; set; }
        public string AL { get; set; }
        public string CCT { get; set; }
        public string AD { get; set; }
        public string LT { get; set; }
        public string RT { get; set; }
        public string K1 { get; set; }
        public string K2 { get; set; }
        public string AST { get; set; }
        public string KERI { get; set; }
        public string WTW { get; set; }
        public string IC { get; set; }
        public string PD { get; set; }
        public string PC { get; set; }





        public string RMK { get; set; }
        public long UPDATE_ID { get; set; }

        public DateTime UPDATE_DATE { get; set; }

    }
}
