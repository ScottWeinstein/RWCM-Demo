
namespace DemoSite
{
    #region NS
    using System.Configuration;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Autofac;
    using Autofac.Integration.Mvc;
    using System.Reflection;
    using DemoSite.Models;
    #endregion

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            var builder = BuildAutofac();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public ContainerBuilder BuildAutofac(bool isTestLoad = false)
        {
            var builder = new ContainerBuilder();
            Assembly appAssembly = typeof(MvcApplication).Assembly;
            builder.RegisterAssemblyTypes(appAssembly);
            builder.RegisterControllers(appAssembly);
            builder.RegisterModelBinders(appAssembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterModule(new AutofacWebTypesModule());

            string serverMapPath = string.Empty;
            if (!isTestLoad)
            {
                serverMapPath = Server.MapPath("~");
            }

            builder.Register<DemoConfig>(ctx =>
            {
                //                var server = ctx.Resolve<HttpServerUtilityBase>();
                var env = ConfigurationManager.AppSettings["Env"];
                var cll = new Services.ConfigLoader(env, serverMapPath);
                return cll.DemoConfig;
            }).SingleInstance();

            return builder;
        }
    }
}