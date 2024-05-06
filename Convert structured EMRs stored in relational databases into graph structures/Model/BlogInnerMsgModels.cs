using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BlogInnerMsgModels
    {


        public int ID { get; set; }
        public string TITLE { get; set; }

        public string CONTENT { get; set; }

        public DateTime UPDATE_DATE { get; set; }
        public long UPDATE_USERID { get; set; }
        public int IS_ENABLED { get; set; }



    }
}
