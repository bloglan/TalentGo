using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.EntityFramework;
using TalentGo.Utilities;

namespace TalentGoWebApp.Areas.Mgmt.Controllers
{
    [Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人")]
	public class ArticleController : Controller
	{
		ArticleManager articleManager;
        RecruitmentPlanManager recruitmentPlanManager;

        public ArticleController(ArticleManager articleManager, RecruitmentPlanManager recruitmentPlanManager)
        {
            this.articleManager = articleManager;
            this.recruitmentPlanManager = recruitmentPlanManager;
        }

        // GET: Mgmt/Article
        public ActionResult Index()
		{
			var articleSet = from article in this.articleManager.Articles
							 orderby article.WhenChanged descending
							 select article;


			return View(articleSet);
		}

		// GET: Mgmt/Article/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Mgmt/Article/Create
		public ActionResult Create()
		{
			this.PrepareViewData();
			Article model = new Article()
			{
				Visible = true,
				CreatedBy = "云南省烟草公司曲靖市公司"
			};

			return View(model);
		}

		// POST: Mgmt/Article/Create
		[HttpPost]
		public async Task<ActionResult> Create(Article model)
		{
			try
			{
				// TODO: Add insert logic here
				//
				model.WhenChanged = DateTime.Now;
				model.Visible = true;

                await this.articleManager.CreateArticle(model);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				this.PrepareViewData();
				this.ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}

		// GET: Mgmt/Article/Edit/5
		public ActionResult Edit(int id)
		{
			var model = this.articleManager.Articles.SingleOrDefault(e => e.id == id);
			if (model == null)
				return RedirectToAction("Index");

			this.PrepareViewData();
			return View(model);
		}

		// POST: Mgmt/Article/Edit/5
		[HttpPost]
		public async Task<ActionResult> Edit(int id, Article model)
		{
			try
			{
                await this.articleManager.UpdateArticle(model);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				this.ModelState.AddModelError("", ex.Message);
				this.PrepareViewData();
				return View(model);
			}
		}

		// GET: Mgmt/Article/Delete/5
		public async Task<ActionResult> Delete(int id)
		{
			var current = this.articleManager.Articles.SingleOrDefault(e => e.id == id);
			if (current != null)
			{
				await this.articleManager.RemoveArticle(current);
			}
			return RedirectToAction("Index");
		}

		// POST: Mgmt/Article/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		void PrepareViewData()
		{
			DateTime now = DateTime.Now;
			ViewData["RecruitmentPlanList"] = from recruitment in this.recruitmentPlanManager.AvailableRecruitmentPlans
											  where recruitment.ExpirationDate > now
											  select new SelectListItem()
											  {
												  Text = recruitment.Title,
												  Value = recruitment.id.ToString()
											  };
		}
	}
}
