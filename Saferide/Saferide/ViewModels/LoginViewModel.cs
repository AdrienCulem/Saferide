using Saferide.Helpers;
using Saferide.Models;
using Saferide.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private bool _isBusy;

        public ICommand LoginClickedCommand { get; set; }

        public bool IsBusy
        {
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _isBusy;
            }
        }

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
        public LoginViewModel()
        {
            LoginClickedCommand = new Command(VerifyLogs);
        }

        public async void VerifyLogs()
        {
            var user = new User()
            {
                grant_type = "passsword",
                Username = _username,
                Password = _password
            };

            Constants.Password = _password;
            Constants.Username = _username;

            if (Username == null || Password == null)
            {
                XFToast.LongMessage("Veuillez compléter les deux champs");
            }
            else
            {
                IsBusy = true;
                string result = await App.LoginManager.Authenticate(user);
                switch (result)
                {
                    case "Success":
                        XFToast.LongMessage("Saferide vous souhaite la bienvenue!");
                        Application.Current.MainPage = new MasterDetailPageView();
                        break;
                    case "Invalid":
                        XFToast.LongMessage("Le mot de passe ou le nom d'utilisateur est incorrect, veuillez réessayer");
                        break;
                    case "Error":
                        XFToast.ShortErrorMessage();
                        break;
                }
                IsBusy = false;
            }
        }
    }
}
