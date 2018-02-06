using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;


namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface IPageModel:IBaseModel
    {
       
        [SitecoreField("Page Title")]
        string PageTitle { get; set; }

        [SitecoreField("Exclude From Top Navigation")]
        Boolean ExcludeFromTopNavigation { get; set; }

        [SitecoreField("Top Navigation Title")]
        string TopNavigationTitle { get; set; }

        [SitecoreField("Meta Title")]
        string MetaTitle { get; set; }

        [SitecoreField("Meta Description")]
        string MetaDescription { get; set; }

        //[SitecoreInfo(SitecoreInfoType.Url)]
        //string Url { get; set; }
    }
}
