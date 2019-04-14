using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manager.Entities
{
    public class User
    {
        public int IdUser { get; set; } 
        [Required]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Name must not have more than 100 characters or less than 3 characters")]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public byte[] Passowrd_hash { get; set; }
        [Required]
        public byte[] Password_salt { get; set; }
        public bool Condition { get; set; }
    }
}
