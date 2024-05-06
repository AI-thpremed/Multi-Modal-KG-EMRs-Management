using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BasDictionaryModels
    {


        public long ID { get; set; }
        public string DICT_NAME { get; set; }
        public string DICT_VALUE { get; set; }
        public string RMK { get; set; }
        public DateTime UPDATE_DATE { get; set; }

    }
}
