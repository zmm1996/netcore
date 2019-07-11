using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data
{
    public  class ClaimsType
    {
        public static List<string> ClaimType { get; set; } = new List<string> {
            "Edit album",
            "Edit user",
            "Edit role",
            "Email"
        };
        

    }
}
