using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    class ConfirmIncidentViewModel
    {
        public ICommand Confirm { get; set; }
        private Incident _incident;
        public ConfirmIncidentViewModel(Incident incident)
        {
            _incident = incident;
            Confirm = new Command<string>(ConfirmIncidentOrNot);
        }

        private async void ConfirmIncidentOrNot(string confirmation)
        {
            _incident.Confirmed = confirmation == "yes";
            Constants.NearestIncidents
                .FirstOrDefault(a => a.IncidentId == _incident.IncidentId).Confirmed = _incident.Confirmed;
            var mainpage = Application.Current.MainPage as MasterDetailPage;
            if (mainpage != null)
            {
                await mainpage.Detail.Navigation.PopModalAsync();
            }
            await App.IncidentManager.ConfirmIncident(_incident);
        }
    }
}
