using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class AddUserToRoleViewModel
    {

        public AddUserToRoleViewModel()
        {
            Users = new List<IdentityUser>();

        }
        [Required]
        [Display(Name ="角色id")]
        public string  RoleId { get; set; }
        [Required]
        [Display(Name ="用户名")]
        public string UserId { get; set; }

        public List<IdentityUser> Users { get; set; }
    }
}
