using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neo4jSocial.Models
{
    public class Status
    {
        public int Id { get; set; }
        public int RelationId { get; set; }
        public string DisplayName { get; set; }
        public string Content { get; set; }
        public string UserPost { get; set; }
        public string AvatarPost { get; set; }
        public string TypeStt { get; set; }
        public LocalDateTime TimePost { get; set; }
        public LocalDateTime TimeShare { get; set; }

    }
}