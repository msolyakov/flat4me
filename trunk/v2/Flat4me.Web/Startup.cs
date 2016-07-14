using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Flat4me.Web.Startup))]
namespace Flat4me.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
