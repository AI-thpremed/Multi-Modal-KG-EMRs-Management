using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class WjCaseInfoModels
    {

        //改善的删了


        public long ID { get; set; }


        public string CASE_NAME { get; set; }
        public int TYPE_ID { get; set; }
        public int SEX { get; set; }

        public string CONTACT { get; set; }




        public string RACE { get; set; }
        public string MARRIAGE { get; set; }
        public string JOB { get; set; }
        public string EDU { get; set; }

        public string ADDRESS { get; set; }




        public int STATUS { get; set; }

        public int IS_DELETE { get; set; }



        public DateTime CREATE_DATE { get; set; }





        public long UPDATE_ID { get; set; }








    }
}
