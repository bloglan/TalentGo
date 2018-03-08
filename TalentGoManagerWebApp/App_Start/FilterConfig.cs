using System.Web;
using System.Web.Mvc;

namespace TalentGoManagerWebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
#if (!DEBUG)
            //添加一个过滤器要求强制Https
            //除非已购买了位于受信任颁发机构的数字证书，否则不要添加此项。
            filters.Add(new RequireHttpsAttribute());

            //在生产环境中要求必须是招聘管理员。
            filters.Add(new AuthorizeAttribute() { Roles = "QJYC\\招聘管理员" });
#endif

        }
    }
}
