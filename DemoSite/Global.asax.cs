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
    using System.Threading.Tasks;
    #endregion

    public class MvcApplication : HttpApplication
    {
        private static IContainer _Container;

        #region Vanilla MVC stuff

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
        #endregion
        
        protected void Application_Start()
        {
            var builder = BuildAutofac();
            _Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_Container));

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            LogReq(sender, e);
        }

        private void LogReq(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var req = app.Request;
            string userAgent = req.UserAgent;
            string requestUrl = req.Url.AbsolutePath;
            string referer = req.UrlReferrer != null ? req.UrlReferrer.AbsolutePath : "";
            string userHostAddress = req.UserHostAddress;
            string userName = req.LogonUserIdentity.Name; // change as needed

            Task.Factory.StartNew(() =>
                {
                    using (var scope = _Container.BeginLifetimeScope())
                    {
                        var db = scope.Resolve<LoggingDBDataContext>();
                        WebRequestLog newLogItem = new WebRequestLog();
                        newLogItem.Referer = referer;
                        newLogItem.RequestUrl = requestUrl;
                        newLogItem.UserAgent = userAgent;
                        newLogItem.UserHostAddress = userHostAddress;
                        newLogItem.UserName = userName;
                        db.WebRequestLogs.InsertOnSubmit(newLogItem);
                        db.SubmitChanges();
                    }
                });
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
                var env = ConfigurationManager.AppSettings["Env"];
                var cll = new Services.ConfigLoader(env, serverMapPath);
                return cll.DemoConfig;
            }).SingleInstance();

            builder.Register<LoggingDBDataContext>(ctx => new LoggingDBDataContext(ctx.Resolve<DemoConfig>().LoggingDBConnectionString));
            builder.Register<ProductEF>(ctx => new ProductEF(ctx.Resolve<DemoConfig>().ProductEFConnectionString));

            return builder;
        }
    }
}