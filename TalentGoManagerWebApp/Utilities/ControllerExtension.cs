﻿using Autofac;
using Autofac.Integration.Mvc;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.Web;

namespace TalentGoManagerWebApp
{
    public static class ControllerExtension
    {
        /// <summary>
        /// Gets current user.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static Person CurrentUser(this Controller controller)
        {
            if (controller.User.Identity.IsAuthenticated)
            {
                //Fetch user from request context.
                var cachedPerson = controller.HttpContext.Items["CurrentPerson"] as Person;

                //if not found.
                if (cachedPerson == null)
                {
                    var resolver = AutofacDependencyResolver.Current.RequestLifetimeScope;
                    var service = resolver.Resolve<ApplicationUserManager>();
                    var getUserTask = Task.Run(async () => await service.FindByNameAsync(controller.User.Identity.Name));
                    cachedPerson = getUserTask.Result;
                    controller.HttpContext.Items["CurrentPerson"] = cachedPerson;
                }

                return cachedPerson;
            }
            return null;
        }

        /// <summary>
        /// Gets current user.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static Person CurrentUser(this HtmlHelper helper)
        {
            return CurrentUser(helper.ViewContext.Controller as Controller);
        }

        /// <summary>
        /// 返回操作已成功的界面。
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static ViewResult OK(this Controller controller)
        {
            var result = new ViewResult()
            {
                ViewName = OKView,
                MasterName = null,
                ViewData = controller.ViewData,
                TempData = controller.TempData,
                ViewEngineCollection = controller.ViewEngineCollection,
            };
            return result;
        }

        public static ViewResult FeatureNotImplemented(this Controller controller)
        {
            var result = new ViewResult()
            {
                ViewName = FeatureNotImplementedView,
                MasterName = null,
                ViewData = controller.ViewData,
                TempData = controller.TempData,
                ViewEngineCollection = controller.ViewEngineCollection,
            };
            return result;
        }

        const string FeatureNotImplementedView = "~/Views/Shared/NotImplemented.cshtml";
        public static readonly string OKView = "~/Views/Shared/OK.cshtml";
    }
}
