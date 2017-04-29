using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Saferide.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentsPageView : ContentPage
    {
        public IncidentsPageView()
        {
            InitializeComponent();
            BindingContext = new IncidentsViewModel();
        }
    }
}
