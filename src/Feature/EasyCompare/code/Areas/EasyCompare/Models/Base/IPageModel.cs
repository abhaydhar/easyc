using Glass.Mapper.Sc.Configuration.Attributes;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    [SitecoreType]
    public interface IPageModel:IBaseModel
    {

        [SitecoreField("Page Title")]
        string PageTitle { get; set; }


        [SitecoreField("Page Body")]
        string PageBody { get; set; }
    }
}
