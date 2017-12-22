using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SuccessHotelierHub.Startup))]
namespace SuccessHotelierHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
