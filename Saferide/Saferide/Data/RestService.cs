using Newtonsoft.Json;
using Saferide.Models;
using System;
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.StringToken);
        }
        public async Task<string> Authenticate(User user)
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
                    Constants.StringToken = token.access_token;
                    Constants.Firstname = token.Firstname;
                    var tokenValidtyInSeconds = token.expires_in;
                    DateTime tokenExpires = DateTime.Now.AddSeconds(tokenValidtyInSeconds);
                    Constants.TokenValidity = tokenExpires;
                    Constants.IsConnected = true;
                    return "Success";
                }
                return "Error";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public async Task<String> NewIncident(Incident incident)
        {
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
    }
}
