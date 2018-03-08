using System.Web.Mvc;

namespace TalentGoWebApp
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
#endif

        }
    }
}
