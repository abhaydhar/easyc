using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;


namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
   
    public interface FeaturedImageContent:IRTE,BaseTitle
    {
        [SitecoreField("Image")]
        Image ImageCopy { get; set; }

        [SitecoreField("Image Alignment")]
        Tag ImageAlignment { get; set; }
    }
}
