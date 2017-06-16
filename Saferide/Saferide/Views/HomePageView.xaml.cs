using Saferide.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.GPS;
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
            try
            {
                await element.ScaleTo(1.2, 100, Easing.BounceIn);
                await element.ScaleTo(1, 100, Easing.SinIn);
            }
            catch (Exception p)
            {
                Debug.WriteLine(p.ToString());
            }
           
        }

        private async void Heading_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                await Heading.RotateTo(0 - UserPosition.Heading, 200, Easing.SinIn);
            }
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "OnAppearing");
        }
    }
}
