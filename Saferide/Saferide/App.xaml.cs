using Saferide.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saferide.Data;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide
{
    public partial class App : Application
    {
        public static LoginManager LoginManager { get; private set; }
        public static IncidentManager IncidentManager { get; private set; }

        public App()
        {
            var service = new RestService();
            LoginManager = new LoginManager(service);
            IncidentManager = new IncidentManager(service);
            InitializeComponent();
        }

        protected override void OnStart()
        {
            LoadPersistedValues();
            if (Constants.IsConnected)
            {
                MainPage = new MasterDetailPageView();
            }
            else
            {
                MainPage = new NavigationPage(new StartPageView());
            }
        }

        protected override void OnSleep()
        {
            Current.Properties["IsConnected"] = Constants.IsConnected;
            Current.Properties["Username"] = Constants.Username;
            Current.Properties["Password"] = Constants.Password;
            Current.Properties["StringToken"] = Constants.StringToken;
            Current.Properties["TokenValidity"] = Constants.TokenValidity;
        }

        protected override void OnResume()
        {
            LoadPersistedValues();
            CheckTokenValidity();
        }

        protected void LoadPersistedValues()
        {
            if (Current.Properties.ContainsKey("IsConnected"))
            {
                Constants.IsConnected = (bool)Current.Properties["IsConnected"];
            }
            if (Current.Properties.ContainsKey("Username"))
            {
                Constants.StringToken = (string)Current.Properties["Username"];
            }
            if (Current.Properties.ContainsKey("Password"))
            {
                Constants.StringToken = (string)Current.Properties["Password"];
            }
            if (Current.Properties.ContainsKey("TokenValidity"))
            {
                Constants.TokenValidity = Convert.ToDateTime(Current.Properties["TokenValidity"]);
            }
            if (Current.Properties.ContainsKey("StringToken"))
            {
                Constants.StringToken = (string)Current.Properties["StringToken"];
            }
        }

        private async void CheckTokenValidity()
        {
            if (DateTime.Now > Constants.TokenValidity)
            {
                var user = new User
                {
                    Username = Constants.Username,
                    Password = Constants.Password
                };
                await App.LoginManager.Authenticate(user);
            }
        }

    }
}
