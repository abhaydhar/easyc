using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType(AutoMap = true)]
    public interface BaseLink:IBaseModel
    {
        [SitecoreField("Link Url")]
         Link LinkUrl { get; set; }
    }
}