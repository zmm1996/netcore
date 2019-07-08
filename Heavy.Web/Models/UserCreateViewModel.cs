using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Models
{
    public class UserCreateViewModel
    {

        [Required]
        [Display(Name ="用户名")]
        public string UserName { get; set; }
        [Required]
        [Display(Name ="邮箱")]
        [RegularExpression("^[a-z0-9A-Z]+[- | a-z0-9A-Z . _]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\\.)+[a-z]{2,}$")]
        public string Email { get; set; }
        [Required]
        [Display(Name ="密码")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [MaxLength(18)]
        public string IdCardNo { get; set; }


        public DateTime Birthday { get; set; }

        public string id { get; set; }
    }
}
