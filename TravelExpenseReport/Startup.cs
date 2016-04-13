using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TravelExpenseReport.Startup))]
namespace TravelExpenseReport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
