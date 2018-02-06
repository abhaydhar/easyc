using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Component
{
    [SitecoreType(AutoMap = true)]
    public interface IOurExpertTeam : BaseTitle
    {
        [SitecoreChildren]
        IEnumerable<IProfileCard> ProfileCardLst { get; set; }

    }
}
