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
            var notices = this.manager.Notices.Published().OrderByDescending(n => n.WhenPublished).Take(100);
			return View(notices);
		}



		public async Task<ActionResult> Detail(int id)
		{
			var article = await this.manager.FindByIdAsync(id);
			if (article.WhenPublished.HasValue)
            {
				return View(article);
            }

            //其他的所有情况均显示404
            return HttpNotFound();
		}
	}
}