using Autofac;
using System.Linq;

namespace TalentGo
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //将命名空间TalentGo.EntityFramework下的类注册到IoC容器。
            builder.RegisterAssemblyTypes(typeof(AutofacModule).Assembly)
                .Where(t => t.Namespace == "TalentGo.EntityFramework")
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

        }
    }
}
