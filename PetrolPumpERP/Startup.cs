using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PetrolPumpERP.Startup))]
namespace PetrolPumpERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
