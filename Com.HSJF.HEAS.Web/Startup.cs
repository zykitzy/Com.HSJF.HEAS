using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Com.HSJF.HEAS.Web.Startup))]
namespace Com.HSJF.HEAS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
