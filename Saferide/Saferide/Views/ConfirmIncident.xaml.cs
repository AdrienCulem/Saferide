using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;
using Saferide.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmIncident : ContentPage
    {
        public ConfirmIncident(Incident incident)
        {
            InitializeComponent();
            BindingContext = new ConfirmIncidentViewModel(incident);
        }
    }
}