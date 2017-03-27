using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;

namespace Saferide
{
    public static class Constants
    {
        internal static bool IsConnected;
        internal static string BearerToken;
        internal static string Username;
        internal static string Password;
        internal static string Email;
        internal static DateTime TokenValidity;
        internal static string GetTokenUrl = "http://safe-ride.azurewebsites.net/token";
        internal static string IncidentUrl = "http://safe-ride.azurewebsites.net/api/incidents";
        internal static string RegisterUrl = "http://safe-ride.azurewebsites.net/api/register";
    }
}

