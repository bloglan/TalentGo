using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using System.Web;
using System.Web.Mvc;
using TalentGo.Identity;

[assembly: OwinStartup(typeof(TalentGoManagerWebApp.Startup))]
namespace TalentGoManagerWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            //Config Ioc Container
            AutofacConfig.Config(builder);


            //注册该程序集中的所有Controller。
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build(); //创建容器
            AutofacContainer = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //覆盖MVC默认的依赖关系解析器。Controller创建器，以便使用提供参数的构造函数来创建。

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
		}

        public static IContainer AutofacContainer { get; private set; }
    }
}
