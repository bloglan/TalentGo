using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.EntityFramework;
using TalentGo.Web;

namespace TalentGoWebApp.Controllers
{
    public class NoticeController : Controller
	{
		NoticeManager manager;

        public NoticeController(NoticeManager manager)
        {
            this.manager = manager;
        }

		// GET: Article
		public ActionResult Index()
		{
			return View();
		}



		public ActionResult Detail(int id)
		{
			var article = this.manager.FindByID(id);
			if (article.WhenPublished.HasValue)
            {
				return View(article);
            }

            //其他的所有情况均显示404
            return HttpNotFound();
		}
	}
}