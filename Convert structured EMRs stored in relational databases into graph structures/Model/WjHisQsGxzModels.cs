﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class WjHisQsGxzModels
    {



        public long ID { get; set; }

        public long CASE_ID { get; set; }

        public DateTime FIST_DIA { get; set; }


        public int STATUS { get; set; }


        public int MED { get; set; }

        public string MED_INFO { get; set; }

        public string RMK { get; set; }
        public long UPDATE_ID { get; set; }

        public DateTime UPDATE_DATE { get; set; }


    }
}
