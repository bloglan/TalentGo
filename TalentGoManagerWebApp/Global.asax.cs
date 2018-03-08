using Forloop.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TalentGoManagerWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //If you're using the Microsoft ASP.NET Web Optimization framework, 
            //you can set the ScriptPathResolver static property on ScriptContext 
            //to use the System.Web.Optimization.Scripts.Render method as the function 
            //for generating scripts. This should be set only once, such as in 
            //Application_Start() in global.asax. For example,
            ScriptContext.ScriptPathResolver = System.Web.Optimization.Scripts.Render;

        }
    }
}
