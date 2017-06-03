using System.Threading.Tasks;
using Saferide.GPS;
using Saferide.Models;
using Saferide.Ressources;

namespace Saferide.Helpers
{
    public static class GetIncident
    {
        public static async Task GetIncidents()
        {
            if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
            {
                TextToSpeech.Talk(AppTexts.StartRidingFirst);
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
