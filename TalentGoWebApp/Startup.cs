using Microsoft.Owin;
using Owin;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(TalentGoWebApp.Startup))]
namespace TalentGoWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAutofac(app);

            




			ConfigureAuth(app);
		}
	}
}
