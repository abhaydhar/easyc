using AutoGeneralTH.ApiModel.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    public class PlansOptionFilterViewModel
    {
        public ApiCarPlanOptionFilter Filter { get; set; }

        public string SKey { get; set; }
    }
}