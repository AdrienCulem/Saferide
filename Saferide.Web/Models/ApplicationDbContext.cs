using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Incident> Incidents { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}