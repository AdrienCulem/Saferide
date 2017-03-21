using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Saferide.Views;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public ICommand LoginClickedCommand
        {
            get 
            {
                return new Command(
                    async () => { await Application.Current.MainPage.Navigation.PushAsync(new LoginPageView()); });
            }
        }

        public ICommand RegisterClickedCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new RegisterPageView());
                });
            }
        }
    }
}