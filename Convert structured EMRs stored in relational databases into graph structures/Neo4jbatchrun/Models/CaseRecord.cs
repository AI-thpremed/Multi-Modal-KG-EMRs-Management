using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC.Admin.Models
{
    public class CaseRecord
    {


        public string DisplayName { get; set; }

        public long CaseId { get; set; }
        public string UserName{ get; set; }


        public string HopId { get; set; }

        public int HopAge { get; set; }



        //public string HopDate { get; set; }
        public string TypeId { get; set; }




        public string Race{ get; set; }
        public string Marriage{ get; set; }
        public string Job { get; set; }
        public string Edu { get; set; }







    }
}