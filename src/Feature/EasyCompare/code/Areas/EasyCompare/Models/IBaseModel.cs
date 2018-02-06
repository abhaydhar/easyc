using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface IBaseModel
    {
        /// <summary>
        /// Gets or Sets Item ID.
        /// </summary>
        [SitecoreId]
        ID Id { get; set; }

        /// <summary>
        /// Gets or Sets DisplayName.
        /// </summary>
        [SitecoreField("__Display name")]
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets ItemName.
        /// </summary>
        [SitecoreInfo(SitecoreInfoType.Name)]
        string ItemName { get; set; }

        //Returns the template ID of the item as type System.Guid.
        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        ID TemplateId { get; set; }

        /// <summary>
        /// Gets or Sets ItemName.
        /// </summary>
        [SitecoreInfo(SitecoreInfoType.Url)]
        string Url { get; set; }

    }

}
