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
            var person = this.CurrentUser();

            if (!this.User.Identity.IsAuthenticated)
                return PartialView("BeforeAuthPartial");

            if (!person.WhenRealIdCommited.HasValue || string.IsNullOrEmpty(person.IDCardFrontFile) || string.IsNullOrEmpty(person.IDCardBackFile))
            {
                //实名信息未提交，或身份证照片未传送。
                return PartialView("RealIdRequired");
            }
            else
            {
                if (!person.RealIdValid.HasValue)
                    return PartialView("RealIdValidating");
                else
                {
                    if (!person.RealIdValid.Value)
                        return PartialView("RealIdValidFailed"); //验证失败。
                    else
                        return PartialView("FormsAuthPartial");

                }
            }
        }

    }
}