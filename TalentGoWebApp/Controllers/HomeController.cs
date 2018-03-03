using System;
using System.DirectoryServices;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo;
using TalentGo.Web;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }


        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult LoginPartial()
        {
            return PartialView("_LoginPartial");
        }

        [ChildActionOnly]
        public ActionResult HomeLoginPartial()
        {

            if (!this.User.Identity.IsAuthenticated)
            {
                return PartialView("BeforeAuthPartial");
            }
            else
            {
                return PartialView("FormsAuthPartial");
            }
        }

    }
}