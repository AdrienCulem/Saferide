using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide
{
    public static class Constants
    {
        public static string Password, Username, StringToken, Firstname;
        public static bool IsConnected;
        public static DateTime TokenValidity;
        public static string GetTokenUrl = "http://safe-ride.azurewebsites.net/token";
        public static string IncidentUrl = "http://safe-ride.azurewebsites.net/incident";
    }
}
