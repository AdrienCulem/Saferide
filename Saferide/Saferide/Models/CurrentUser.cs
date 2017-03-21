using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Models
{
    public class CurrentUser
    {
        public string Firstame { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BearerToken { get; set; }
        public DateTime TokenValidity { get; set; }
    }
}
