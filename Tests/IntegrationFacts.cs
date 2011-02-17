namespace Tests
{
    using System;
    using Autofac;
    using DemoSite.Models;
    using Xunit;
    using DemoSite.Services;

    public class IntegrationFacts
    {
        [Fact]
        public void Can_Integrate_with_web_autofac()
        {
            var config = AutofacContainer.Container.Resolve<DemoConfig>();
            Assert.NotNull(config);
        }

        [Fact]
        public void Test()
        {
            DemoConfig cfg = AutofacContainer.Container.Resolve<DemoConfig>();
            var status = EnvEvaluator.Evaluate(cfg);

        }

    }
}
