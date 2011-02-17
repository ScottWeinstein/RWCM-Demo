namespace DemoSite.Models
{
    using DemoSite.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DiagInfo
    {
        public DiagInfo(DemoConfig config, List<DiagStatusItem> dstats)
        {
            Config = config;
            StatusItems = dstats;
        }
        
        public IEnumerable<DiagStatusItem> StatusItems { get; private set; }
        
        public bool IsOK
        {
            get { return StatusItems.All(dsi => dsi.IsOK); }
        }

        public DemoConfig Config { get; private set; }
        
    }
}
