namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;
    using Autofac;
    using DemoSite.Models;

    public class IntegrationFacts
    {
        [Fact]
        public void Can_Integrate_with_web_autofac()
        {
            var config = AutofacContainer.Container.Resolve<DemoConfig>();
        }
    }
}
