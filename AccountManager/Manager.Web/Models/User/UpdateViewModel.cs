using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Web.Models.User
{
    public class UpdateViewModel
    {
        [Required]
        public int IdUser { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool act_password { get; set; } // Determinara si deseo o no actualizar el password
    }
}
