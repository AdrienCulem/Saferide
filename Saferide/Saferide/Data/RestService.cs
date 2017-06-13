using Newtonsoft.Json;
using Saferide.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Saferide.Views;
using Xamarin.Forms;

namespace Saferide.Data
{
    public class RestService : IRestService
    {
        HttpClient client;

        private Token _token;

        public RestService()
        {
            client = new HttpClient { MaxResponseContentBufferSize = 256000 };
        }
        /// <summary>
        /// Authenticate the user on the sever
        /// </summary>
        /// <param name="user">
        /// The user to log
        /// </param>
        /// <returns>
        /// A string that can be "Success", "Invalid" if password isn't right and "Error" if and exception has been thrown
        /// </returns>
        public async Task<string> Authenticate(LoginUser user)
        {
            var uri = new Uri(string.Format(Constants.GetTokenUrl));

            try
            {
                var json = JsonConvert.SerializeObject(user);
                var request = string.Format("grant_type=" + user.grant_type + "&Username=" + user.Username + "&Password=" + user.Password);
                var content = new StringContent(request, Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                var statusCode = (int)response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var answer = await response.Content.ReadAsStringAsync();
                    _token = JsonConvert.DeserializeObject<Token>(answer);
                    Constants.BearerToken = _token.access_token;
                    var tokenValidtyInSeconds = _token.expires_in;
                    Constants.TokenValidity = DateTime.UtcNow.AddSeconds(tokenValidtyInSeconds);
                    Constants.IsConnected = true;
                    return "Success";
                }
                return "Invalid";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public async Task<String> NewIncident(Incident incident)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.BearerToken);
            var uri = new Uri(String.Format(Constants.IncidentUrl));
            try
            {
                var json = JsonConvert.SerializeObject(incident);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                var statusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                if (statusCode == 401)
                {
                    await TryReconnect();
                }
                return "Invalid";
            }
            catch
            {
                return "Error";
            }
        }

        public async Task<string> ConfirmIncident(Incident incident)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.BearerToken);
            var uri = new Uri(String.Format(Constants.IncidentUrl + $"/{incident.IncidentId}"));
            try
            {
                var json = JsonConvert.SerializeObject(incident);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                var statusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                if (statusCode == 401)
                {
                    await TryReconnect();
                }
                return "Invalid";
            }
            catch
            {
                return "Error";
            }
        }

        public async Task<List<Incident>> GetIncidents(Position pos)
        {
            List<Incident> incidentsList = new List<Incident>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.BearerToken);
            var stringPosLong = pos.Longitude.ToString().Replace(".", ",");
            var stringPosLat = pos.Latitude.ToString().Replace(".", ",");
            var uri = new Uri(String.Format(Constants.IncidentUrl + $"/{stringPosLong}/{stringPosLat}"));
            try
            {
                //var json = JsonConvert.SerializeObject(pos);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.GetAsync(uri);
                var statusCode = (int)response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    incidentsList = JsonConvert.DeserializeObject<List<Incident>>(result);
                }
                if (statusCode == 401)
                {
                    await TryReconnect();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return incidentsList;
        }

        /// <summary>
        /// Tries to reconnect the server by asking a new token
        /// </summary>
        /// <returns></returns>
        private async Task TryReconnect()
        {
            if (Constants.Username != null && Constants.Password != null)
            {
                var user = new LoginUser()
                {
                    Username = Constants.Username,
                    Password = Constants.Password
                };
                var result = await App.LoginManager.Authenticate(user);
                switch (result)
                {
                    case "Success": return;
                }
            }
            Application.Current.MainPage = new NavigationPage(new LoginPageView());
        }
    }
}
