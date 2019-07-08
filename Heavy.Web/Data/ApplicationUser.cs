using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(18)]
        public string  IdCardNo { get; set; }

     
        public DateTime Birthday { get; set; }
    }
}
