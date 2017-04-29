using System;
using System.Threading.Tasks;
using Saferide.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Saferide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageView : ContentPage
    {
        public LoginPageView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            var element = (Button) sender;
            await element.ScaleTo(1.2, 100, Easing.BounceIn);
            await element.ScaleTo(1, 100, Easing.SinIn);
        }
    }
}
