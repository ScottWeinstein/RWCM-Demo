namespace DemoSite.Models
{
    using System;
    using System.Collections;

    public class DemoConfig
    {
        private readonly Hashtable _ht;

        public DemoConfig(Hashtable ht)
        {
            _ht = ht;
        }

        public bool IsProduction
        {
            get { return (bool)_ht["IsProduction"]; }
        }
        
        public string SupportEmail
        {
            get { return (string)_ht["SupportEmail"]; }
        }

        public string FileShare
        {
            get { return (string)_ht["FileShare"]; }
        }

        public string SqlEFCS
        {
            get { return (string)_ht["SqlEFCS"]; }
        }

        public string SqlCS
        {
            get { return (string)_ht["SqlCS"]; }
        }

        public string ASCS
        {
            get { return (string)_ht["ASCS"]; }
        }

        public string Env
        {
            get { return (string)_ht["Env"]; }
        }
    }
}
