﻿using System.Linq;
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



		public ActionResult Detail(int id)
		{
			var article = this.articleManager.FindByID(id);
			if (article.WhenPublished.HasValue)
            {
				return View(article);
            }

            //其他的所有情况均显示404
            return HttpNotFound();
		}
	}
}