using System.Web.Mvc;

namespace TalentGoManagerWebApp.Areas.ex
{
    public class exAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ex";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ex_default",
                "ex/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}