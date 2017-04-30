using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.GPS;
using Saferide.Interfaces;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide.Helpers
{
    public static class GetIncident
    {
        public static async Task GetIncidents()
        {
            if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
            {
                TextToSpeech.Talk("U need to start locating first");
            }
            Position pos = new Position
            {
                Latitude = UserPosition.Latitude,
                Longitude = UserPosition.Longitude,
            };
            Constants.NearestIncidents = await App.IncidentManager.GetIncidents(pos);
        }
    }
}
