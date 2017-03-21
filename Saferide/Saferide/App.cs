﻿using Saferide.Views;
using System;
using Saferide.Data;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide
{
    public class App : Application
    {
        public static LoginManager LoginManager { get; private set; }
        public static IncidentManager IncidentManager { get; private set; }

        public App()
        {
            var service = new RestService();
            LoginManager = new LoginManager(service);
            IncidentManager = new IncidentManager(service);
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

        protected override async void OnSleep()
        {
            Current.Properties["IsConnected"] = Constants.IsConnected;
            await Current.SavePropertiesAsync();
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
                Constants.IsConnected = (bool) Current.Properties["IsConnected"];
            }
        }

        private async void CheckTokenValidity()
        {
            if (DateTime.Now > Constants.TokenValidity)
            {
                var user = new LoginUser()
                {
                    Username = Constants.Username,
                    Password = Constants.Password
                };
                await App.LoginManager.Authenticate(user);
            }
        }
    }
}