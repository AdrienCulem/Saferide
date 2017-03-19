using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Models
{
    public class User
    {
        public string grant_type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
