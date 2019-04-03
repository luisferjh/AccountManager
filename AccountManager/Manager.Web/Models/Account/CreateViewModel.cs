using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Web.Models.Account
{
    public class CreateViewModel
    {
        [Required]
        public string WebAccountName { get; set; }
        public string UserAccount { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
