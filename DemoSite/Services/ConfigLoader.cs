namespace DemoSite.Services
{
    #region namespaces
    using System;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Collections;
    using DemoSite.Models;
    #endregion

    public class ConfigLoader
    {
        public DemoConfig DemoConfig { get; private set; }

        public ConfigLoader(string env, string location)
        {
            var scriptFilePath = Path.Combine(location, @"..\Scripts\Get-Config.ps1");
            DemoConfig = CreateDemoConfig(GetConfigRaw(env, scriptFilePath));
        }

        public ConfigLoader(Tuple<string, string> args /* to work-around powershell bug in method resolve*/)
        {
            var env = args.Item1;
            var xmlDefn = args.Item2;

            var sfd = GetScriptFileFromEmbeddedResource();
            var scriptFilePath = sfd.Item1;
            using (sfd.Item2)
            {
                DemoConfig = CreateDemoConfig(GetConfigRaw(env, scriptFilePath, xmlDefn));
            }
        }

        private DemoConfig CreateDemoConfig(Hashtable ht)
        {
            return new DemoConfig(ht);
        }

        private Hashtable GetConfigRaw(string env, string scriptFilePath, string xmlDefn = null)
        {
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                using (var pipeline = runspace.CreatePipeline())
                {
                    var getConfigCmd = new Command(scriptFilePath);
                    var envParam = new CommandParameter("Env", env);
                    getConfigCmd.Parameters.Add(envParam);

                    if (xmlDefn != null)
                    {
                        var envFileContentParam = new CommandParameter("EnvFileContent", xmlDefn);
                        getConfigCmd.Parameters.Add(envFileContentParam);
                    }

                    pipeline.Commands.Add(getConfigCmd);
                    runspace.Open();

                    Collection<PSObject> results = pipeline.Invoke();
                    var res = results[0].BaseObject as Hashtable;
                    if (res == null)
                    {
                        throw new Exception("Missing Config");
                    }

                    return res;
                }
            }
        }
        
        private Tuple<string, IDisposable> GetScriptFileFromEmbeddedResource()
        {
            var path = Path.GetTempPath();
            var scriptFile = Path.Combine(path, "Get-Config.ps1");
            var modFile = Path.Combine(path, "DPAPI.psm1");
            File.WriteAllBytes(scriptFile, Properties.Resources.Get_Config);
            File.WriteAllBytes(modFile, Properties.Resources.DPAPI);

            return new Tuple<string, IDisposable>(scriptFile, System.Disposables.Disposable.Create(() =>
            {
                File.Delete(scriptFile);
                File.Delete(modFile);
            }));
        }
    }
}