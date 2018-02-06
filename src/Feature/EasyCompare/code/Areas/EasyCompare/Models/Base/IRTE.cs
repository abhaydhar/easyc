using Glass.Mapper.Sc.Configuration.Attributes;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface IRTE:IBaseModel
    {
       [SitecoreField("Body")]
       string Body { get; set; }
    }
}