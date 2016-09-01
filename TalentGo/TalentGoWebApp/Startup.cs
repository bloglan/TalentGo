using Microsoft.Owin;
using Owin;
using System.Web.Configuration;
using TalentGo.EntityFramework;

[assembly: OwinStartupAttribute(typeof(TalentGoWebApp.Startup))]
namespace TalentGoWebApp
{
	public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			//根据Web.config中关于Authentication Mode的配置，决定是否配置OWin的Auth。
			//Coded by Jonny Yang.
			app.CreatePerOwinContext(TalentGoDbContext.Create);
			var authConfig = (AuthenticationSection)WebConfigurationManager.GetSection("system.web/authentication");
			switch(authConfig.Mode)
			{
				case AuthenticationMode.None:
					ConfigureAuth(app);
					break;
				case AuthenticationMode.Windows:
					break;
				default:
					throw new System.Exception("不支持的AuthenticationMode.");
			}



			//ConfigureAuth(app);
		}
	}
}
