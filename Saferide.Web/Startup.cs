using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saferide.Web.Startup))]
namespace Saferide.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
