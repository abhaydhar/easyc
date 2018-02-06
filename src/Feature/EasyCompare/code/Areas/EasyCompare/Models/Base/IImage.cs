using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface IImage :IBaseModel
    {
        [SitecoreField("Image")]
       Image ImageCopy { get; set; }
 
    }
}