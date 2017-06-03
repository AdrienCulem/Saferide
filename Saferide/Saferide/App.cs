using Saferide.Views;
using System;
using System.Threading.Tasks;
using Saferide.Data;
using Saferide.Interfaces;
using Saferide.Models;
using Saferide.Ressources;
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

            if (Device.RuntimePlatform == "ios" || Device.RuntimePlatform == "Android")
            {
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                AppTexts.Culture = ci; // set the RESX for resource localization
                Constants.MetricSystem = ci.Name.Contains("en") ? Constants.MetricSystems.Mph : Constants.MetricSystems.Kmh;
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }
            LoadPersistedValues();
            CheckTokenValidity();
            if (Constants.IsConnected)
            {
                MainPage = new MasterDetailPageView();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPageView());
            }
        }

        protected override async void OnSleep()
        {
            Current.Properties["IsConnected"] = Constants.IsConnected;
            Current.Properties["Username"] = Constants.Username;
            Current.Properties["Password"] = Constants.Password;
            Current.Properties["Token"] = Constants.BearerToken;
            Current.Properties["TokenValidity"] = Constants.TokenValidity;
            Current.Properties["MetricSystem"] = Constants.MetricSystem.ToString();
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
            if (Current.Properties.ContainsKey("Username"))
            {
                Constants.Username = (string)Current.Properties["Username"];
            }
            if (Current.Properties.ContainsKey("Password"))
            {
                Constants.Password = (string)Current.Properties["Password"];
            }
            if (Current.Properties.ContainsKey("Token"))
            {
                Constants.BearerToken = (string)Current.Properties["Token"];
            }
            if (Current.Properties.ContainsKey("TokenValidity"))
            {
                Constants.TokenValidity = (DateTime)Current.Properties["TokenValidity"];
            }
            if (Current.Properties.ContainsKey("MetricSystem"))
            {
                var a = (string)Current.Properties["MetricSystem"];
                Constants.MetricSystem = a == "Mph" ? Constants.MetricSystems.Mph : Constants.MetricSystems.Kmh;
            }
        }

        private async void CheckTokenValidity()
        {
            if (DateTime.Now > Constants.TokenValidity)
            {
                var user = new LoginUser()
                {
                    Username = Constants.Username,
                    Password = Constants.Password,
                    grant_type = "password"
                };
                await LoginManager.Authenticate(user);
            }
        }
    }
}