using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface BannerCarousel: BaseTitle
    {
        [SitecoreField("Banner Image")]
       Image BannerImage { get; set; }
       
    }

}