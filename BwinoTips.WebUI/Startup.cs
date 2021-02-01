using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BwinoTips.WebUI.Startup))]
namespace BwinoTips.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
