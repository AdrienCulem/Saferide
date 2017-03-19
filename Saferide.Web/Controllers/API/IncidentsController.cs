using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Controllers.API
{
    [Authorize]
    public class IncidentsController : ApiController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        [ResponseType(typeof(Incident))]
        public async Task<IHttpActionResult> PostIncident(Incident incident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.Identity.GetUserId();
            incident.UserId = userId;
            _dbContext.Incidents.Add(incident);
            await _dbContext.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = incident.IncidentId }, incident);
        }
    }
}
