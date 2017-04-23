using System;
using Saferide.Helpers;
using Saferide.Models;
using Saferide.Views;
using System.Windows.Input;
using Saferide.Ressources;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;

        public ICommand LoginClickedCommand => new Command(VerifyLogs);
        public ICommand RegisterClickedCommand => new Command(() =>
        {
            Application.Current.MainPage = new NavigationPage(new GenericWebPageView(Constants.RegisterWebsiteUrl));
        });
        public ICommand ForgotPasswordCommand => new Command(() =>
        {
            Application.Current.MainPage = new NavigationPage(new GenericWebPageView(Constants.ResetPasswordUrl));
        });
        public string Username
        {
            set
            {
                if (_username != value)
                {
                    _username = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _username;
            }
        }

        public string Password
        {
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _password;
            }
        }
        public async void VerifyLogs()
        {
            var user = new LoginUser()
            {
                grant_type = "password",
                Username = _username,
                Password = _password
            };

            Constants.Password = _password;
            Constants.Username = _username;

            if (Username == null || Password == null)
            {
                XFToast.LongMessage(AppTexts.BothFields);
            }
            else
            {
                XFToast.ShowLoading();
                string result = await App.LoginManager.Authenticate(user);
                XFToast.HideLoading();
                switch (result)
                {
                    case "Success":
                        XFToast.LongMessage(AppTexts.Welcome);
                        Application.Current.MainPage = new MasterDetailPageView();
                        break;
                    case "Invalid":
                        XFToast.LongMessage(AppTexts.WrongLogins);
                        break;
                    case "Error":
                        XFToast.ShortErrorMessage();
                        break;
                }
            }
        }
    }
}
