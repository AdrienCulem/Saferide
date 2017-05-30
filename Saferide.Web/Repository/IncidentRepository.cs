using System.Data.Entity;
using System.Threading.Tasks;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Repository
{
    public class IncidentRepository : BaseModelRepository<Incident>
    {
        public IncidentRepository(ApplicationDbContext context): base(context)
        {

        }

        public async Task<Incident> GetByUserId(string id)
        {
            return await Context.Incidents.Include(n => n.User).FirstOrDefaultAsync(n => n.UserId == id);
        }

        //public async Task<Incident> GetByIdAsync(int id)
        //{
        //    return await Context.Incidents
        //        .FirstOrDefaultAsync(f => f.IncidentId == id);
        //}
    }
}