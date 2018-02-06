using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Base;
using System.Collections.Generic;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    public interface IOurPartners: IBaseModel
    {
        [SitecoreField("Title")]
        string Title { get; set; }

        [SitecoreField("SubTitle")]
        string SubTitle { get; set; }

        [SitecoreChildren]        
        IEnumerable<ILinkWithImage> PartnerLinks { get; set; }
    }
}
