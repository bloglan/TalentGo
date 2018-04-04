using Autofac;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TalentGo;
using TalentGo.EntityFramework;
using TalentGo.Services;
using TalentGo.Web;

namespace TalentGoManagerWebApp
{
    public class AutofacConfig
    {
        public static void Config(ContainerBuilder builder)
        {
            //Register TalentGoCore
            builder.RegisterAssemblyTypes(typeof(Person).Assembly);

            //Register TalentGoEntityFramework
            builder.RegisterAssemblyTypes(typeof(TalentGoDbContext).Assembly).Where(tn => tn.Name.EndsWith("Store")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(TalentGoDbContext).Assembly).Where(tn => tn.BaseType.Equals(typeof(DbContext))).As<DbContext>().InstancePerRequest();

            //Register TalentGoWebServerLib
            builder.RegisterAssemblyTypes(typeof(WebUser).Assembly);
            builder.RegisterType<NotificationService>().AsImplementedInterfaces();
            builder.RegisterType<EaseIMTemplatedShortMessageService>().AsImplementedInterfaces();
            builder.RegisterType<NoneOperationEmailService>().AsImplementedInterfaces();
            builder.RegisterType<MemorySMSQueueStore>().AsImplementedInterfaces();
            builder.RegisterType<RecruitmentPlanManager>().PropertiesAutowired();
        }
    }
}