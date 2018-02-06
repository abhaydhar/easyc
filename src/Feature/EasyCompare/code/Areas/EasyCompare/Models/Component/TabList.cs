using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    public interface TabList:IBaseModel
    {
        [SitecoreField("Tabs")]
        IEnumerable<BaseLink> Tabs { get; set; }

        string currentPageUrl { get; set; }
    }
}
