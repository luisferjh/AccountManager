using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Web.Models.User
{
    public class UserViewModel
    {
        public int IdUser { get; set; }
        public string Name { get; set; }   
        public string Email { get; set; }       
        public byte[] Password_hash { get; set; } 
        public bool Condition { get; set; }
    }
}
