using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EPWI.Web.Startup))]
namespace EPWI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
