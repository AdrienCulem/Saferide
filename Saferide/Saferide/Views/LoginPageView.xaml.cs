using Saferide.ViewModels;
using Xamarin.Forms;

namespace Saferide.Views
{
    public partial class LoginPageView : ContentPage
    {
        public LoginPageView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}
