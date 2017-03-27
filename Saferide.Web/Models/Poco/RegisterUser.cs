using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saferide.Web.Models.Poco
{
    public class RegisterUser
    {
        public string Firstame { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}