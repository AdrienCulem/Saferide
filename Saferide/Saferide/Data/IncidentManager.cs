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
        public async Task<String> NewIncident(Incident incident)
        {
            return await _restService.NewIncident(incident);
        }

        public async Task<List<Incident>> GetIncident(Position pos)
        {
            return await _restService.GetIncident(pos);
        }
    }
}
