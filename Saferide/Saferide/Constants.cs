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
        internal static CurrentUser CurrentUser;
        internal static string GetTokenUrl = "http://safe-ride.azurewebsites.net/token";
        internal static string IncidentUrl = "http://safe-ride.azurewebsites.net/api/incidents";

        public static void Disconnect()
        {
            CurrentUser = null;
        }
    }
}
