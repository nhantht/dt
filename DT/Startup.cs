using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DT.Startup))]
namespace DT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
