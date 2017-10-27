using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JH.Startup))]
namespace JH
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
