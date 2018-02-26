using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Web;
using System.Web.Mvc;
using TalentGo.Identity;
using TalentGoWebApp.App_Start;

[assembly: OwinStartup(typeof(TalentGoWebApp.Startup))]
namespace TalentGoWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            //Config Ioc Container
            AutofacConfig.Config(builder);

            //向Autofac注册用于AspNet Identity.
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();

            //注册该程序集中的所有Controller。
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build(); //创建容器
            AutofacContainer = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //覆盖MVC默认的依赖关系解析器。Controller创建器，以便使用提供参数的构造函数来创建。

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            

            ConfigureAuth(app);
		}

        public static IContainer AutofacContainer { get; private set; }
    }
}
