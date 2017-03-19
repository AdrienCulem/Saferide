using Saferide.Helpers;
using Saferide.Models;
using Saferide.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string _username, _password;

        bool _isBusy;

        public ICommand LoginClickedCommand { get; set; }

        public bool isBusy
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
            LoginClickedCommand = new Command(verifyLogs);
        }

        public /*async*/ void verifyLogs()
        {
            var user = new User()
            {
                grant_type = "password",
                Username = _username,
                Password = _password
            };

            Constants.Password = _password;
            Constants.Username = _username;

            Application.Current.MainPage = new MasterDetailPageView();

            //XFToast.ShortMessage("Bienvenue!");

            //if (Username == null || Password == null)
            //{
            //    XFToast.LongMessage("Veuillez compléter les deux champs");
            //}
            //else
            //{
            //    isBusy = true;
            //    int result = await App.LoginManager.Login(user);
            //    switch (result)
            //    {
            //        case 1:
            //            XFToast.LongMessage("Bienvenue dans CyberHelp " + Constants.firstname);
            //            //Need to be improved
            //            Application.Current.MainPage = new MasterDetailPageView();
            //            break;
            //        case 2:
            //            XFToast.LongMessage("Le mot de passe ou le nom d'utilisateur est incorrect, veuillez réessayer");
            //            break;
            //        case 3:
            //            XFToast.ShortErrorMessage();
            //            break;
            //        default:
            //            break;
            //    }
            //    isBusy = false;
            //}
        }
    }
}
