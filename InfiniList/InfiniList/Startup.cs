using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InfiniList.Startup))]
namespace InfiniList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
