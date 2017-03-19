using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Saferide.Web.Models;

namespace Saferide.Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        public async Task<ActionResult> Index()
        {
            var incidents = await db.Incidents.ToListAsync();
            return View(incidents);
        }
    }
}