using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BioImageModels
    {




        public long ID { get; set; }
        public long CASE_ID { get; set; }
        public long INS_ID { get; set; }


        public int TYPE { get; set; }

        public string TYPE_NAME { get; set; }

        public string INFO { get; set; }
        public string RMK { get; set; }

        public int L_R { get; set; }

        public int IS_DELETE { get; set; }
        public long UPDATE_ID{ get; set; }
        public DateTime UPDATE_DATE { get; set; }

        public DateTime CREATE_DATE { get; set; }








    }















}
