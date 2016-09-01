using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;

namespace TalentGoWebApp.Areas.Mgmt.Controllers
{
	[Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人,QJYC\\企业域管理员")]
	public class HomeController : Controller
    {
		TalentGoDbContext database;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
		}
		// GET: Mgmt/Home
		public ActionResult Index()
        {
			//this.database.Database.SqlQuery()
            return View();
        }
    }
}