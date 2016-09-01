using System.Web.Mvc;

namespace TalentGoWebApp.Controllers
{
	public class HelpController : Controller
    {
        // GET: Help
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult FAQ()
		{
			return View();
		}

		public ActionResult Support()
		{
			return View();
		}

		public ActionResult Privacy()
		{
			return View();
		}

	}
}