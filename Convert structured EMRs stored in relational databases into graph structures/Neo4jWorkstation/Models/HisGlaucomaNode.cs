﻿using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC.Admin.Models
{
    public class HisGlaucomaNode
    {

        public string DisplayName { get; set; }

        public long CaseId { get; set; }

        //public LocalDate FistDia { get; set; }

        public string RmkQt { get; set; }

        public string Lr { get; set; }

    }
}