using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElectionManagementSystem.Startup))]
namespace ElectionManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
