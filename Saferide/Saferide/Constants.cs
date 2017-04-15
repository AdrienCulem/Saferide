using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide
{
    public static class Constants
    {
        public static bool IsConnected;
        public static string BearerToken;
        public static string Username;
        public static string Password;
        public static string Email;
        public static Color SaferideBlue = Color.FromHex("#3f48cc");
        public static DateTime TokenValidity;
        public static string GetTokenUrl = "http://safe-ride.azurewebsites.net/token";
        public static string IncidentUrl = "http://safe-ride.azurewebsites.net/api/incidents";
        public static string RegisterUrl = "http://safe-ride.azurewebsites.net/api/register";
        public static string GetIncidentsUrl = "http://safe-ride.azurewebsites.net/api/getincidents";
        public static string RegisterWebsiteUrl = "http://safe-ride.azurewebsites.net/Account/Register";
    }
}

