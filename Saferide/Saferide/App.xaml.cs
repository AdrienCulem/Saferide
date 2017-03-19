using Saferide.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saferide.Data;
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
            MainPage = new MasterDetailPageView();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
