using System.IO;
namespace Tests
{
    using System;
    using Autofac;
    using DemoSite.Models;
    using Xunit;
    using DemoSite.Services;
    using System.Linq;

    public class IntegrationFacts
    {
        [Fact]
        public void Can_Integrate_with_web_autofac()
        {
            var config = AutofacContainer.Container.Resolve<DemoConfig>();
            Assert.NotNull(config);
        }

        [Fact]
        public void Bad_filepath_is_detected()
        {
            var ht = new System.Collections.Hashtable();
            ht["FileShare"] = @"q:\IDontExist";
            DemoConfig cfg = new DemoConfig(ht);
            var status = EnvEvaluator.EvaluateFilesystemPath(() => cfg.FileShare);
            Assert.False(status.IsOK);
        }

        [Fact]
        public void Good_filepath_is_validated()
        {
            var ht = new System.Collections.Hashtable();
            ht["FileShare"] = Path.GetTempPath();
            DemoConfig cfg = new DemoConfig(ht);
            var status = EnvEvaluator.EvaluateFilesystemPath(() => cfg.FileShare);
            Assert.True(status.IsOK);
        }

    }
}
