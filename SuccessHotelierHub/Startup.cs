using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using SuccessHotelierHub.Repository;

[assembly: OwinStartupAttribute(typeof(SuccessHotelierHub.Startup))]
namespace SuccessHotelierHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage(System.Configuration.ConfigurationManager.ConnectionStrings["Hangfire_Connection"].ConnectionString);

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
