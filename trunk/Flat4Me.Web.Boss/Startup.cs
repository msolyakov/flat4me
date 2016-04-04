using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Flat4Me.Web.Boss.Startup))]
namespace Flat4Me.Web.Boss
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
