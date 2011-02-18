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

        public string ProductEFConnectionString
        {
            get { return (string)_ht["ProductEFConnectionString"]; }
        }

        public string LoggingDBConnectionString
        {
            get { return (string)_ht["LoggingDBConnectionString"]; }
        }

        public string MarketingASConnectionString
        {
            get { return (string)_ht["MarketingASConnectionString"]; }
        }

        public string Env
        {
            get { return (string)_ht["Env"]; }
        }
    }
}
