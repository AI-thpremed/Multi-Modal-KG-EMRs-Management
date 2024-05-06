using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HisEyeModels
    {



        public long ID { get; set; }

        public long CASE_ID { get; set; }

        public int L_R { get; set; }
        public int STATUS { get; set; }

        public int CATARACT { get; set; }

        public string CATARACT_INFO { get; set; }
        public string CATARACT_RMK { get; set; }


        public int GLAUCOMA { get; set; }
        public int GLAUCOMA_T_STATUS { get; set; }

        public DateTime GLAUCOMA_DATE { get; set; }

        public string GLAUCOMA_SUR{ get; set; }
        public string GLAUCOMA_RMK { get; set; }
        public string GLAUCOMA_TRE{ get; set; }
        public string BLTSWM { get; set; }
        public string WSS { get; set; }


      
        public string RMK { get; set; }
        public string OTHER { get; set; }
        public long UPDATE_ID { get; set; }

        public DateTime UPDATE_DATE { get; set; }






    }
}
