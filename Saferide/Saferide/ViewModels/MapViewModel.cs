using System.Collections.Generic;
using System.Windows.Input;
using Saferide.Helpers;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public List<Incident> ListContent { get; set; }
        public ICommand RefreshCommand { get; set; }

        public MapViewModel()
        {
            GetIncidents();
            RefreshCommand = new Command(GetIncidents);
        }

        private async void GetIncidents()
        {
            await GetIncident.GetIncidents();
            ListContent = Constants.NearestIncidents;
            MessagingCenter.Send(this, "AddPins", ListContent);
        }
    }
}
