using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manager.Entities
{
    public class Account
    {
        public int IdAccount { get; set; }
        [Required]
        public string WebAccountName { get; set; }
        public string UserAccount { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
