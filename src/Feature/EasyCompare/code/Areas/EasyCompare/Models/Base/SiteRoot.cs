using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    [SitecoreType]
    public interface SiteRoot:IBaseModel
    {
        [SitecoreField("Articles Root")]
        string ArticlesRoot { get; set; }
    }
}
