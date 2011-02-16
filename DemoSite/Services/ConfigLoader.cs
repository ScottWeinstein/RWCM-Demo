using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace DemoSite.Services
{
    public class ConfigLoader
    {
        public ConfigLoader(string env, string location)
        {
            var scriptFile = Path.Combine(location, @"..\Scripts\Get-Config.ps1");
            GetConfigRaw(env, scriptFile);
        }
        public ConfigLoader(string env)
        {
            var sfd = GetScriptFile();
            var scriptFile = sfd.Item1;
            using (sfd.Item2)
            {
                GetConfigRaw(env, scriptFile);
            }
        }
        private static void GetConfigRaw(string env, string scriptFile)
        {
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                using (var pipeline = runspace.CreatePipeline())
                {
                    Command getConfigCmd = new Command(scriptFile);
                    CommandParameter envParam = new CommandParameter("Env", env);
                    getConfigCmd.Parameters.Add(envParam);
                    pipeline.Commands.Add(getConfigCmd);
                    runspace.Open();
                    
                    Collection<PSObject> results = pipeline.Invoke();
                    var res = results[0].BaseObject as Hashtable;

                }
            }
        }
        //string getConfigScript = Path.Combine(Assembly.GetExecutingAssembly().Location, @"Get-Config.ps1");
                    
            
            

        
        private static Tuple<string,IDisposable> GetScriptFile()
        {
            var scriptFile = Path.GetTempFileName();
            scriptFile = Path.ChangeExtension(scriptFile, "ps1");
            File.WriteAllBytes(scriptFile, Properties.Resources.Get_Config);
            return new Tuple<string,IDisposable>(scriptFile, System.Disposables.Disposable.Create(()=> File.Delete(scriptFile)));
        }
    }
}