using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Threading.Tasks;
using Saferide.Web.Models;
using Saferide.Web.Repository;

namespace CyberHelp.MVC.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private IncidentRepository _incidentRepository;


        private UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public static UnitOfWork Create(IdentityFactoryOptions<UnitOfWork> factory, IOwinContext context)
        {
            return new UnitOfWork(context.Get<ApplicationDbContext>());
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IncidentRepository Incidents => _incidentRepository ?? (_incidentRepository = new IncidentRepository(_context));
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}