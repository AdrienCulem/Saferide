using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Saferide.Web.Controllers.API
{
    [Authorize]
    public class IsTokenValidController : ApiController
    {
        public bool GetIsTokenValid()
        {
            return true;
        }
    }
}
