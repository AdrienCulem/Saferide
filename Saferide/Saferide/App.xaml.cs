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
            Current.Properties["CurrentUser"] = Constants.CurrentUser;
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
            if (Current.Properties.ContainsKey("CurrentUser"))
            {
                Constants.CurrentUser = (CurrentUser)Current.Properties["CurrentUser"];
            }
        }

        private async void CheckTokenValidity()
        {
            if (DateTime.Now > Constants.CurrentUser.TokenValidity)
            {
                var user = new LoginUser()
                {
                    Username = Constants.CurrentUser.Username,
                    Password = Constants.CurrentUser.Password
                };
                await App.LoginManager.Authenticate(user);
            }
        }

    }
}
