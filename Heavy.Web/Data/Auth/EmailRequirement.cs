using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data.Auth
{
    public class EmailRequirement:IAuthorizationRequirement
    {
        public EmailRequirement(string emailtype)
        {
            Emailtype = emailtype;
        }

        public string Emailtype { get; }
    }
}
