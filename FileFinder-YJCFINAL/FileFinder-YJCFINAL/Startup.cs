using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileFinder_YJCFINAL.Startup))]
namespace FileFinder_YJCFINAL
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
