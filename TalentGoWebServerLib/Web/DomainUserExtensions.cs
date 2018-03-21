using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TalentGo.Web
{
    /// <summary>
    /// DomainUserExtensions
    /// </summary>
    public static class DomainUserExtensions
    {
        /// <summary>
        /// Get Domain User via controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static DomainUser DomainUser(this Controller controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (controller.HttpContext.Items[DomainUserKey] == null)
            {
#if DEBUG
                var domainUser = new DomainUser
                {
                    DisplayName = "开发人员",
                };
                controller.HttpContext.Items["DomainUser"] = domainUser;
#else
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    using (var principal = Principal.FindByIdentity(context, controller.User.Identity.Name))
                    {
                        var domainUser = new DomainUser();
                        if (principal != null)
                        {
                            domainUser.DisplayName = principal.DisplayName;
                        }
                        else
                            domainUser.DisplayName = "N/A";
                        controller.HttpContext.Items[DomainUserKey] = domainUser;
                    }
                }
#endif
            }
            return controller.HttpContext.Items[DomainUserKey] as DomainUser;
        }

        /// <summary>
        /// Get Domain User via Html helper.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static DomainUser DomainUser(this HtmlHelper helper)
        {
            return DomainUser(helper.ViewContext.Controller as Controller);
        }

        const string DomainUserKey = "DomainUser";
    }
}
