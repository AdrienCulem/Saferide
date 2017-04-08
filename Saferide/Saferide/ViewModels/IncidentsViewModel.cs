using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.GPS;
using Saferide.Models;

namespace Saferide.ViewModels
{
    public class IncidentsViewModel : BaseViewModel
    {
        public List<Incident> ListContent { get; set; }

        public IncidentsViewModel()
        {
            GetIncidents();
        }

        private async void GetIncidents()
        {
            Position pos = new Position
            {
                Latitude = UserPosition.Latitude,
                Longitude = UserPosition.Longitude,
                RadLatitude = UserPosition.RadLat,
                RadLongitude = UserPosition.RadLon
            };
            ListContent = await App.IncidentManager.GetIncidents(pos);
            RaisePropertyChanged("ListContent");
        }
    }
}
