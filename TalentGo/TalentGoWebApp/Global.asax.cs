using Forloop.HtmlHelpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TalentGoWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


			//添加一个过滤器要求强制Https
			//除非已购买了位于受信任颁发机构的数字证书，否则不要添加此项。
			GlobalFilters.Filters.Add(new RequireHttpsAttribute());


			//If you're using the Microsoft ASP.NET Web Optimization framework,
			//you can set the ScriptPathResolver static property on ScriptContext
			//to use the System.Web.Optimization.Scripts.Render method as the function
			//for generating scripts. This should be set only once, such as in
			//Application_Start() in global.asax. For example,
			ScriptContext.ScriptPathResolver = System.Web.Optimization.Scripts.Render;


		}
	}
}
