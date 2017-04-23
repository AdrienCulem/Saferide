using Saferide.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageView : ContentPage
    {
        public HomePageView()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            var element = (Button)sender;
            await element.ScaleTo(1.2, 100, Easing.BounceIn);
            await element.ScaleTo(1, 100, Easing.SinIn);
        }
    }
}
