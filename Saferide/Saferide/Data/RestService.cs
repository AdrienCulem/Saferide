using Newtonsoft.Json;
using Saferide.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Data
{
    public class RestService : IRestService
    {
        HttpClient client;

        private Token token;

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }
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

                if (response.IsSuccessStatusCode)
                {
                    var answer = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<Token>(answer);
                    Constants.BearerToken = token.access_token;
                    var tokenValidtyInSeconds = token.expires_in;
                    DateTime tokenExpires = DateTime.Now.AddSeconds(tokenValidtyInSeconds);
                    Constants.TokenValidity = tokenExpires;
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

                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                return "Error";
            }
            catch
            {
                return "Error";
            }
        }

        public async Task<string> Register(NewUser newUser)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.BearerToken);
            var uri = new Uri(String.Format(Constants.RegisterUrl));
            try
            {
                var json = JsonConvert.SerializeObject(newUser);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                return "Error";
            }
            catch
            {
                return "Error";
            }
        }

        public async Task<List<Incident>> GetIncident(Position pos)
        {
            List<Incident> incidentsList = new List<Incident>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.BearerToken);
            var uri = new Uri(String.Format(Constants.GetIncidentsUrl));
            try
            {
                var json = JsonConvert.SerializeObject(pos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    incidentsList = JsonConvert.DeserializeObject<List<Incident>>(result);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return incidentsList;
        }
    }
}
