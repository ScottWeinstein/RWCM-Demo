using System.Threading;

namespace DemoSite.Services
{
    using System;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Collections;
    using DemoSite.Models;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class EnvEvaluator
    {
        public static DiagInfo Evaluate(DemoConfig config)
        {
            var dstats = new List<DiagStatusItem>();

            dstats.Add(EvaluateFilesystemPath(() => config.FileShare));

            return new DiagInfo(config,dstats);
        }

        public static DiagStatusItem EvaluateFilesystemPath(Expression<Func<string>> propGetter)
        {
            var parts = GetExprParts(propGetter);
            var dinfo = new DiagStatusItem() { Name = parts.Item1, Value = parts.Item2 };

            var testfile = Path.Combine(dinfo.Value,Guid.NewGuid().ToString());

            try
            {
                File.Create(testfile).Close();
                File.Delete(testfile);
            }
            catch (Exception ex)
            {
                dinfo.Exception = ex.ToString();
            }
            return dinfo;
        }

        private static Tuple<string, T> GetExprParts<T>(Expression<Func<T>> propGetter)
        {
            var mexp = propGetter.Body as MemberExpression;
            if (mexp == null)
            {
                throw new Exception("bad property");
            }
            var name = mexp.Member.Name;
            var value = propGetter.Compile()();
            return Tuple.Create(name,value);
        }
    }
}