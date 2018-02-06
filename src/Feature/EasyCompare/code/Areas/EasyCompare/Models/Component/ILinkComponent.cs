using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType(AutoMap = true)]
    public interface ILinkComponent:IBaseModel
    {
        [SitecoreField("Links Title")]
        string LinksComponentTitle { get; set; }

        [SitecoreChildren]
        IEnumerable<BaseLink> LinksList { get; set; }

    }
}