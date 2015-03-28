using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Cloud.WebApi.Startup))]

namespace Cloud.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
