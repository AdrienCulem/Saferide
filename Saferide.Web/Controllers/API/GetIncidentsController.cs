using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Saferide.Web.Helpers;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Controllers.API
{
    public class GetIncidentsController : ApiController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        [ResponseType(typeof(Incident))]
        public async Task<List<Incident>> PostGetIncidents(Position pos)
        {
            PositionConverter pConvert = new PositionConverter(pos);
            var result = pConvert.BoundingCoordinates(20);
            List<Incident> incidentsWithinRadius;

            if (pos == null)
            {
                return null;
            }

            var minLat = result[0].Latitude;
            var minLong = result[0].Longitude;
            var maxLat = result[1].Latitude;
            var maxLong = result[1].Longitude;

            if (result[0].Longitude <= result[1].Longitude)
            {
                incidentsWithinRadius = await _dbContext.Incidents.Where(
                    i => (i.Latitude >= minLat) &&
                         (i.Latitude <= maxLat) &&
                         (i.Longitude >= minLong) &&
                         (i.Longitude <= maxLong)).ToListAsync();
            }
            else
            {
                incidentsWithinRadius = await _dbContext.Incidents.Where(
                    i => (i.Latitude >= minLat) &&
                         (i.Latitude <= maxLat) &&
                         (i.Longitude >= minLong) ||
                         (i.Longitude <= maxLong)).ToListAsync();
            }

            return incidentsWithinRadius;
        }
    }
}