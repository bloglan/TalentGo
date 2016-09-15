using Autofac;
using Autofac.Integration.Mvc;
using Owin;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;

namespace TalentGoWebApp
{
    public partial class Startup
    {
        public void ConfigureAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var ctlAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterControllers(ctlAssembly);

            //获取所有被引用的程序集。
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();


            //为所有被引用的程序集使用模块进行注册。
            builder.RegisterAssemblyModules(assemblies.ToArray());

#warning 作为测试
            builder.RegisterAssemblyTypes(typeof(TalentGo.Utilities.TalentGoDbContext).Assembly).Where(t => t.Name.EndsWith("Store")).AsImplementedInterfaces();
            builder.RegisterType<TalentGo.Utilities.TalentGoDbContext>().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(TalentGo.Recruitment.RecruitmentPlan).Assembly);
            //builder.RegisterAssemblyTypes(typeof(HttpRecruitmentContext).Assembly).Where(t => t.Name == "HttpRecruitmentContext").As<RecruitmentContextBase>();

            var container = builder.Build();

            //this.Application["AutfacContainer"] = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}