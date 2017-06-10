using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Saferide.Helpers;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public List<Incident> ListContent { get; set; }

        public MapViewModel()
        {
            GetIncidents();
        }

        private async Task GetIncidents()
        {
            await GetIncident.GetIncidents();
            ListContent = Constants.NearestIncidents;
            MessagingCenter.Send(this, "AddPins", ListContent);
        }
    }
}
