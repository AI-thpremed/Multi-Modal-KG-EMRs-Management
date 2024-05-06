using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neo4jSocial.Models
{
    public class Comment
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string User { get; set; }
        public string Avatar { get; set; }
        public LocalDateTime Time { get; set; }

    }
}