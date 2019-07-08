using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<IdentityUser>();
        }

        [Required]
        [Display(Name ="id")]
        public string Id { get; set; }
        [Required]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }
        public List<IdentityUser> Users { get; set; }


    }
}
