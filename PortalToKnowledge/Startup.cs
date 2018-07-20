using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortalToKnowledge.Startup))]
namespace PortalToKnowledge
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
