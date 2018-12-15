using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FluentValidation;
using Spinx.Api.Admin;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Services.AdminUsers;
using Spinx.Services.AdminUsers.Validators;
using Spinx.Services.Pages.Actions;
using Spinx.Web.Core.AppSettings;
using Spinx.Web.Infrastructure;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace Spinx.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
            //Configure AutoMapper
            //AutoMapperConfiguration.Configure();

            // stops executing a rule as soon as a validator fails for validation lib.
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterApiControllers(Assembly.GetCallingAssembly());
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            builder.Register(c => new AppSettings<IAppSettings>().Settings)
                .As<IAppSettings>().SingleInstance();

            builder.RegisterControllers(Assembly.GetAssembly(typeof(Areas.Admin.Controllers.HomeController)));
            builder.RegisterApiControllers(Assembly.GetAssembly(typeof(AdminUsersController)));
            //builder.RegisterApiControllers(Assembly.GetAssembly(typeof(PagesController)));

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(AdminUserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(AdminUserService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(PageAdminActionFactory).Assembly)
                .Where(t => t.Name.EndsWith("ActionFactory"))
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(PageAdminActiveAction).Assembly)
                .Where(t => t.Name.EndsWith("Action"))
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(AdminUserValidator).Assembly)
                .Where(t => t.Name.EndsWith("Validator"))
                .InstancePerRequest();

            builder.RegisterFilterProvider();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
 
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}