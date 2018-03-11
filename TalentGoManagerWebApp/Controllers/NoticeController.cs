using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.EntityFramework;
using TalentGo.Web;
using TalentGoManagerWebApp.Models;

namespace TalentGoManagerWebApp.Controllers
{
	public class NoticeController : Controller
	{
		NoticeManager manager;

        public NoticeController(NoticeManager manager)
        {
            this.manager = manager;
        }

        // GET: Mgmt/Article
        public ActionResult Index()
		{
			var articleSet = from article in this.manager.Articles
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
			return View();
		}

		// POST: Mgmt/Article/Create
		[HttpPost]
		public async Task<ActionResult> Create(NoticeEditViewModel model)
		{
            var notice = new Notice(model.Title, model.MainContent, model.CreatedBy)
            {
                Visible = model.Visible,
            };

			try
			{
                await this.manager.CreateAsync(notice);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				this.ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}

		// GET: Mgmt/Article/Edit/5
		public ActionResult Edit(int id)
		{
			var model = this.manager.Articles.SingleOrDefault(e => e.Id == id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		// POST: Mgmt/Article/Edit/5
		[HttpPost]
		public async Task<ActionResult> Edit(int id, NoticeEditViewModel model)
		{
            var notice = this.manager.FindByID(id);
            if (notice == null)
                return HttpNotFound();

            notice.Title = model.Title;
            notice.MainContent = model.MainContent;
            notice.CreatedBy = model.CreatedBy;
            notice.Visible = model.Visible;

			try
			{
                await this.manager.UpdateAsync(notice);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				this.ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}

		// GET: Mgmt/Article/Delete/5
		public async Task<ActionResult> Delete(int id)
		{
			var current = this.manager.Articles.SingleOrDefault(e => e.Id == id);
			if (current != null)
			{
				await this.manager.DeleteAsync(current);
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
	}
}
