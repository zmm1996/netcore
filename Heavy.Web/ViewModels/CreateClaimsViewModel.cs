using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class CreateClaimsViewModel
    {
        public CreateClaimsViewModel()
        {
            AllClaims = new List<string>();
        }
        
        [Required]
        public string UserId { get; set; }
        public string ClaimId { get; set; }

        public List<string> AllClaims { get; set; }
    }
}
