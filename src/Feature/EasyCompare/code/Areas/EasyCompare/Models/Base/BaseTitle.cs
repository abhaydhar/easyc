using Glass.Mapper.Sc.Configuration.Attributes;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface BaseTitle:IBaseModel
    {
        [SitecoreField("Title")]
       string Title { get; set; }

        [SitecoreField("Sub Title")]
        string SubTitle { get; set; } 
    }
}