using Glass.Mapper.Sc.Configuration.Attributes;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    public interface TitleWithDescription:BaseTitle,IRTE
    {
        [SitecoreField("Backgroud Color")]
        Tag BgColor { get; set; }
        
    }
}
