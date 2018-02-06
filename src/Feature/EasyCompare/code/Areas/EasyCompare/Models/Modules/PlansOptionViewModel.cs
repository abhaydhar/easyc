using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    public class PlansOptionViewModel
    {
        public string PlanId { get; set; }

        public string SKey { get; set; }

        public List<string> ComparePlans { get; set; }
    }
}