using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Saferide.Web.Controllers.Api;
using Saferide.Web.Helpers;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Controllers.API
{
    [Authorize]
    public class IncidentsController : BaseApiController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        [Authorize]
        [ResponseType(typeof(Incident))]
        public async Task<IHttpActionResult> PostIncident(Incident incident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            incident.Trust = 25;
            string userId = User.Identity.GetUserId();
            incident.UserId = userId;
            _dbContext.Incidents.Add(incident);
            await _dbContext.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = incident.IncidentId }, incident);
        }

        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIncident(int id, Incident incident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var savedIncident = await UnitOfWork.Incidents.GetById(id);
            savedIncident.Trust = incident.Confirmed ? savedIncident.Trust+1 : savedIncident.Trust-1;
            if (savedIncident.Trust == 0)
            {
                await UnitOfWork.Incidents.Delete(savedIncident);
            }
            _dbContext.Entry(savedIncident).State = EntityState.Modified;
            await UnitOfWork.SaveChangesAsync();
            return StatusCode(HttpStatusCode.OK);
        }

        [Authorize]
        [ResponseType(typeof(void))]
        [Route("api/incidents/{longi}/{lat}")]
        public async Task<List<Incident>> GetIncidents(string longi, string lat)
        {
            var doublePosLat = 0.0;
            var doublePosLong = 0.0;
            try
            {
                doublePosLong = Convert.ToDouble(longi.Replace(",", "."));
                doublePosLat = Convert.ToDouble(lat.Replace(",", "."));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }

            Position pos = new Position();
            pos.Latitude = doublePosLat;
            pos.Longitude = doublePosLong;
            PositionConverter pConvert = new PositionConverter(pos);
            var result = pConvert.BoundingCoordinates(10);
            List<Incident> incidentsWithinRadius;

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
