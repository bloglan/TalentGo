using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;
using TalentGo.Recruitment;
using TalentGo.Utilities;

namespace TalentGoWebApp.Controllers
{
	public class ArticleController : Controller
	{
		TalentGoDbContext database;
		ArticleManager articleManager;
		// GET: Article
		public ActionResult Index()
		{
			return View();
		}

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
			this.articleManager = new ArticleManager(requestContext.HttpContext);
		}

		[ChildActionOnly]
		public async Task<ActionResult> ArticlePart(bool IsPublic, RecruitmentPlan plan, int MaxCount, string ViewName)
		{
			if (string.IsNullOrEmpty(ViewName))
				ViewName = "ArticlePart";
			var articleList = await this.articleManager.GetAvaiableArticles(IsPublic, plan);
			return PartialView(ViewName, articleList.Take(MaxCount));
		}

		public async Task<ActionResult> Detail(int id)
		{
			bool IsPublic = true;
			if (this.User.Identity.IsAuthenticated && this.User.Identity is WindowsIdentity)
			{
				IsPublic = false; //如果已验证身份，且Identity是WidowsIdetity类型，则非公开。
			}

			var article = this.articleManager.FindByID(id);

			//如果关联了plan，则以Plan的IsPublic为准来显示。
			if (article.RelatedPlan.HasValue)
			{
				if (article.RecruitmentPlan.IsPublic == IsPublic)
					return View(article);
			}

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