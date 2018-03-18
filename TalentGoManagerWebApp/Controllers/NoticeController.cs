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
            var notices = this.manager.Notices.OrderByDescending(n => n.WhenCreated);
            return View(notices);
        }

        // GET: Mgmt/Article/Details/5
        public async Task<ActionResult> Detail(int id)
        {
            var notice = await this.manager.FindByIdAsync(id);
            if (notice == null)
                return HttpNotFound();

            return View(notice);
        }

        // GET: Mgmt/Article/Create
        public ActionResult Create()
        {
            var model = new NoticeEditViewModel {
                CreatedBy = this.DomainUser().DisplayName,
            };
            return View(model);
        }

        // POST: Mgmt/Article/Create
        [HttpPost]
        public async Task<ActionResult> Create(NoticeEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var notice = new Notice(model.Title, model.MainContent, model.CreatedBy);

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
        public async Task<ActionResult> Edit(int id)
        {
            var notice = await this.manager.FindByIdAsync(id);
            if (notice == null)
                return HttpNotFound();

            var model = new NoticeEditViewModel
            {
                Title = notice.Title,
                MainContent = notice.MainContent,
                CreatedBy = notice.CreatedBy,
            };
            return View(model);
        }

        // POST: Mgmt/Article/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, NoticeEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var notice = await this.manager.FindByIdAsync(id);
            if (notice == null)
                return HttpNotFound();

            notice.Title = model.Title;
            notice.MainContent = model.MainContent;
            notice.CreatedBy = model.CreatedBy;

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
            var current = await this.manager.FindByIdAsync(id);
            if (current == null)
            {
                return HttpNotFound();
            }
            return View(current);
        }

        // POST: Mgmt/Article/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var notice = await this.manager.FindByIdAsync(id);
                if (notice != null)
                    await this.manager.DeleteAsync(notice);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Publish(int id)
        {
            var notice = await this.manager.FindByIdAsync(id);
            if (notice == null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Publish(int id, FormCollection collection)
        {
            var notice = await this.manager.FindByIdAsync(id);
            if (notice == null)
                return HttpNotFound();

            try
            {
                await this.manager.PublishAsync(notice);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View();
                throw;
            }
        }

        public async Task<ActionResult> CancelPublish(int id)
        {
            var notice = await this.manager.FindByIdAsync(id);
            if (notice == null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CancelPublish(int id, FormCollection collection)
        {
            var notice = await this.manager.FindByIdAsync(id);
            if (notice == null)
                return HttpNotFound();

            try
            {
                await this.manager.CancelPublishAsync(notice);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View();
                throw;
            }
        }

    }
}
