using System.Web.Mvc;

namespace TalentGoManagerWebApp.Controllers
{
	[Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人,QJYC\\企业域管理员")]
	public class MaintenanceController : Controller
    {
        // GET: Mgmt/Maintenance
        public ActionResult Index()
        {
            return View();
        }
    }
}