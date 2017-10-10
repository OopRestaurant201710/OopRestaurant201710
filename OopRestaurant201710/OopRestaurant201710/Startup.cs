using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OopRestaurant201710.Startup))]
namespace OopRestaurant201710
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
