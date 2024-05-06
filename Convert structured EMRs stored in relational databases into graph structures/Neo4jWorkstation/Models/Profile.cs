using Neo4j.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neo4jSocial.Models
{
    public class Profile
    {
        public string User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public LocalDate Birthday { get; set; }
        public string Interest { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Job { get; set; }
        public string Company { get; set; }
        public string School { get; set; }
        public string Avatar { get; set; }

    }
}