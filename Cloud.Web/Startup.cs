using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cloud.Web.Startup))]
namespace Cloud.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
