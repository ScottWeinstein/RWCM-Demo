namespace DemoSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DiagStatusItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Exception { get; set; }
        public bool IsOK
        {
            get { return Exception == null; }
        }
        
    }
}
