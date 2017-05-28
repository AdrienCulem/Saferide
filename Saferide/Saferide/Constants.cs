using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;
using Saferide.Ressources;
using Xamarin.Forms;

namespace Saferide
{
    public static class Constants
    {
        public static bool IsConnected;
        public static bool IsBeingAskedToConfirm;
        public static bool VoiceAlreadyInit;
        public static string BearerToken;
        public static string Username;
        public static string Password;
        public static string Email;
        public static string MetricSystem;
        public static Color SaferideBlue = Color.FromHex("#4682B4");
        public static DateTime TokenValidity;
        public static List<Incident> NearestIncidents = new List<Incident>();
        public static string GetTokenUrl = "https://safe-ride.azurewebsites.net/token";
        public static string IncidentUrl = "https://safe-ride.azurewebsites.net/api/incidents";
        public static string RegisterUrl = "https://safe-ride.azurewebsites.net/api/register";
        public static string GetIncidentsUrl = "https://safe-ride.azurewebsites.net/api/getincidents";
        public static string RegisterWebsiteUrl = "https://safe-ride.azurewebsites.net/Account/Register";
        public static string IsTokenValidUrl = "https://safe-ride.azurewebsites.net/api/istokenvalid";
        public static string ResetPasswordUrl = "https://safe-ride.azurewebsites.net/Account/ForgotPassword";

        public static void LogOff()
        {
            IsConnected = false;
            BearerToken = null;
            Username = null;
            Password = null;
            Email = null;
            TokenValidity = DateTime.Now;
            NearestIncidents = null;
        }

    }
}

