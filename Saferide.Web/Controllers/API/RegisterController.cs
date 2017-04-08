using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Saferide.Web.Models;
using Saferide.Web.Models.Poco;

namespace Saferide.Web.Controllers.API
{
    public class RegisterController : ApiController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        [ResponseType(typeof(Incident))]
        public async Task Register(RegisterViewModel)
        {
            
        }
    }
}
