using System.Web.Mvc;

namespace TalentGoWebApp.Areas.Mgmt.Controllers
{
    [Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人,QJYC\\企业域管理员")]
	public class HomeController : Controller
    {
        public HomeController()
        {

        }

		// GET: Mgmt/Home
		public ActionResult Index()
        {
            return View();
        }
    }
}