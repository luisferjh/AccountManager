using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Web.Models.Account
{
    public class UpdateViewModel
    {
        public int IdAccount { get; set; }
        public string WebAccountName { get; set; }
        public string UserAccount { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
    }
}
