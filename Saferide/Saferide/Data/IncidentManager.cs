using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;

namespace Saferide.Data
{
    public class IncidentManager
    {
        IRestService _restService;
        public IncidentManager(IRestService service)
        {
            _restService = service;
        }
        /// <summary>
        /// Send a new incident to the server
        /// </summary>
        /// <param name="incident">
        /// The incident to save
        /// </param>
        /// <returns>
        /// A string that can be "Success", "Invalid" if password isn't right and "Error" if and exception has been thrown
        /// </returns>
        public async Task<String> NewIncident(Incident incident)
        {
            return await _restService.NewIncident(incident);
        }
        /// <summary>
        /// Getting the incidents in a certain radius arround the user
        /// </summary>
        /// <param name="pos">
        /// The position of the user
        /// </param>
        /// <returns>
        /// A list of Incident
        /// </returns>
        public async Task<List<Incident>> GetIncidents(Position pos)
        {
            return await _restService.GetIncidents(pos);
        }

        public async Task<String> ConfirmIncident(Incident inc)
        {
            return await _restService.ConfirmIncident(inc);
        }
    }
}
