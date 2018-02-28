using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.EntityFramework;
using TalentGo.Web;

namespace TalentGoWebApp.Controllers
{
    public class ArticleController : Controller
	{
		ArticleManager articleManager;

        public ArticleController(ArticleManager manager)
        {
            this.articleManager = manager;
        }

		// GET: Article
		public ActionResult Index()
		{
			return View();
		}


		[ChildActionOnly]
		public async Task<ActionResult> ArticlePart(bool IsPublic, RecruitmentPlan plan, int MaxCount, string ViewName)
		{
			if (string.IsNullOrEmpty(ViewName))
				ViewName = "ArticlePart";
			var articleList = this.articleManager.GetAvaiableArticles(IsPublic, plan);
			return PartialView(ViewName, articleList.Take(MaxCount));
		}

		public ActionResult Detail(int id)
		{
			bool IsPublic = true;
			if (this.User.Identity.IsAuthenticated && this.User.Identity is WindowsIdentity)
			{
				IsPublic = false; //如果已验证身份，且Identity是WidowsIdetity类型，则非公开。
			}

			var article = this.articleManager.FindByID(id);

			//如果关联了plan，则以Plan的IsPublic为准来显示。
			//if (article.RelatedPlan.HasValue)
			//{
			//	if (article.RecruitmentPlan.IsPublic == IsPublic)
			//		return View(article);
			//}

			//未关联的情况，则根据Article.IsPublic来判断，如果没有值，或值等于IsPublic，则显示。
			if (!article.IsPublic.HasValue || article.IsPublic == IsPublic)
			{
				return View(article);
			}

			//其他的所有情况均显示404
			return HttpNotFound();

		}
	}
}