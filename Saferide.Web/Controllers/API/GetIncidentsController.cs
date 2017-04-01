using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Controllers.API
{
    [Authorize]
    public class GetIncidentsController : ApiController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        [ResponseType(typeof(Incident))]
        public async Task<IHttpActionResult> PostGetIncidents(Position pos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            return CreatedAtRoute("DefaultApi", new { id = incident.IncidentId }, incident);
        }
    }
}