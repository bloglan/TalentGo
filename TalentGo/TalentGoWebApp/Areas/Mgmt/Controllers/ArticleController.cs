using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;
using TalentGo.Utilities;

namespace TalentGoWebApp.Areas.Mgmt.Controllers
{
	[Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人")]
	public class ArticleController : Controller
	{
		public TalentGoDbContext database;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
		}
		// GET: Mgmt/Article
		public ActionResult Index()
		{
			var articleSet = from article in this.database.Article
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
		public ActionResult Create(Article model)
		{
			try
			{
				// TODO: Add insert logic here
				//
				model.WhenCreated = DateTime.Now;
				model.WhenChanged = DateTime.Now;
				model.Visible = true;

				this.database.Article.Add(model);
				this.database.SaveChanges();
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
			var model = this.database.Article.SingleOrDefault(e => e.id == id);
			if (model == null)
				return RedirectToAction("Index");

			this.PrepareViewData();
			return View(model);
		}

		// POST: Mgmt/Article/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, Article model)
		{
			try
			{
				// TODO: Add update logic here
				var current = this.database.Article.SingleOrDefault(e => e.id == id);
				if (current == null)
					return RedirectToAction("Index");

				model.WhenChanged = DateTime.Now;

				var currententry = this.database.Entry<Article>(current);
				currententry.CurrentValues.SetValues(model);

				this.database.SaveChanges();

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
		public ActionResult Delete(int id)
		{
			var current = this.database.Article.SingleOrDefault(e => e.id == id);
			if (current != null)
			{
				this.database.Article.Remove(current);
				this.database.SaveChanges();
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
			ViewData["RecruitmentPlanList"] = from recruitment in this.database.RecruitmentPlan
											  where recruitment.ExpirationDate > now
											  select new SelectListItem()
											  {
												  Text = recruitment.Title,
												  Value = recruitment.id.ToString()
											  };
		}
	}
}
