using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CyberHelp.MVC.Repository;
using Microsoft.AspNet.Identity;
using Saferide.Web.Controllers.Api;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Controllers.API
{
    [Authorize]
    public class IncidentsController : BaseApiController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

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

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIncident(int id, Incident incident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var savedIncident = await UnitOfWork.Incidents.GetByIdAsync(incident.IncidentId);
            savedIncident.Trust = incident.Confirmed ? savedIncident.Trust++ : savedIncident.Trust--;
            if (savedIncident.Trust == 0)
            {
                await UnitOfWork.Incidents.Delete(savedIncident);
            }
            _dbContext.Entry(savedIncident).State = EntityState.Modified;
            await UnitOfWork.SaveChangesAsync();
            return StatusCode(HttpStatusCode.OK);
        }
    }
}
