using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShowGuide.Startup))]
namespace ShowGuide
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
