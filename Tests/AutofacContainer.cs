namespace Tests
{
    using System;
    using Autofac;
    using System.Reflection;
    using DemoSite.Services;
    using DemoSite.Models;

    public static class AutofacContainer
    {
        public static IContainer Container { get; private set; }

        static AutofacContainer()
        {
            var app = new global::DemoSite.MvcApplication();
            var builder = app.BuildAutofac(isTestLoad: true);
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            // unit test specific overrides
            var envXml = @"<Env>
	<UnitTest>
		<SqlHost>localhost</SqlHost>
		<MongoHost>localhost</MongoHost>
		<SSASHost>localhost</SSASHost>
		<SupportEmail>sweinstein@lab49.com</SupportEmail>
		<FileShare>c:\temp\FileShare\Dev3</FileShare>
	</UnitTest>
</Env>";
            builder.Register<ConfigLoader>(ctx => new ConfigLoader(Tuple.Create("unittest", envXml)));
            builder.Register<DemoConfig>(ctx => ctx.Resolve<ConfigLoader>().DemoConfig).SingleInstance();

            Container = builder.Build();
        }
    }
}
