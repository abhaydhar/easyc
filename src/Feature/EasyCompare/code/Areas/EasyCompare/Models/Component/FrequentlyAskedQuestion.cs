using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;
using Glass.Mapper.Sc.Fields;



namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    [SitecoreType]
    public interface FrequentlyAskedQuestion : IBaseModel
    {
        [SitecoreField("Question")]
        string Question { get; set; }

        [SitecoreField("Answer")]
        string Answer { get; set; }
    }
}