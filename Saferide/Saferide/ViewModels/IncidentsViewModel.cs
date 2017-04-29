using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Saferide.GPS;
using Saferide.Interfaces;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class IncidentsViewModel : BaseViewModel
    {
        public List<Incident> ListContent { get; set; }
        public ICommand RefreshCommand { get; set; }

        public IncidentsViewModel()
        {
            GetIncidents();
            RefreshCommand = new Command(GetIncidents);
        }

        private async void GetIncidents()
        {
            if (UserPosition.Latitude == 0 || UserPosition.Longitude == 0)
            {
                DependencyService.Get<ISpeechRecognition>().Talk("U need to start locating first");
            }
            IsBusy = true;
            Position pos = new Position
            {
                Latitude = UserPosition.Latitude,
                Longitude = UserPosition.Longitude,
            };
            ListContent = await App.IncidentManager.GetIncidents(pos);
            RaisePropertyChanged("ListContent");
            IsBusy = false;
        }
    }
}
