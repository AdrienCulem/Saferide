using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide
{
    public static class Constants
    {
        internal static string Password;
        internal static string Username;
        internal static string StringToken;
        internal static string Firstname;
        internal static bool IsConnected;
        internal static DateTime TokenValidity;
        internal static string GetTokenUrl = "http://safe-ride.azurewebsites.net/token";
        internal static string IncidentUrl = "http://safe-ride.azurewebsites.net/api/incidents";

        public static void Disconnect()
        {
            Password = null;
            Username = null;
            IsConnected = false;
            Firstname = null;
            StringToken = null;
        }
    }
}
