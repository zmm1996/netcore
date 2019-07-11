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

        public ApplicationUser()
        {
            Claims = new List<IdentityUserClaim<string>>();
            Logins = new List<IdentityUserLogin<string>>();
            Tokens = new List<IdentityUserToken<string>>();
            UserRoles = new List<IdentityRoleClaim<string>>();
        }
        [MaxLength(18)]
        public string  IdCardNo { get; set; }

     
        public DateTime Birthday { get; set; }



        public  ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public  ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public  ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public  ICollection<IdentityRoleClaim<string>> UserRoles { get; set; }
    }
}
