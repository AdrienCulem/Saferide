using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPageView : ContentPage
    {
        public SettingsPageView()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }
    }
}