﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity.Owin;
using Saferide.Web.Repository;

namespace Saferide.Web.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        protected UnitOfWork UnitOfWork { get; private set; }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            UnitOfWork = HttpContext.Current.GetOwinContext().Get<UnitOfWork>();
            return base.ExecuteAsync(controllerContext, cancellationToken);
        }
    }
}