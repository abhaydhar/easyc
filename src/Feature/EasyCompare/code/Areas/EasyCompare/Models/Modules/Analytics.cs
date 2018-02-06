using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    [SitecoreType]
    public class Analytics
    {
        [SitecoreField("Analytics script")]
        public string AnalyticsScripts { get; set; }
    }
}