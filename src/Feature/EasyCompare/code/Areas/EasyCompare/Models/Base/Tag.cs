using Glass.Mapper.Sc.Configuration.Attributes;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface Tag:IBaseModel
    {
        [SitecoreField("Tag Name")]
       string TagName { get; set; }

    }
}