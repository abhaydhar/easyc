using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public interface ITopNavigatioModel :IBaseModel
    {
        [SitecoreChildren]
        IEnumerable<IPageModel> Children { get; set; }
    }
}
