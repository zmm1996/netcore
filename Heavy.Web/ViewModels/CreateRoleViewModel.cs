using Heavy.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class CreateRoleViewModel
    {
       
        [Required]
        [Display(Name ="角色名称")]
        [Remote("CheckNameExist", "Role",ErrorMessage =("角色已存在"))]
        public string RoleName { get; set; }

    }
}
