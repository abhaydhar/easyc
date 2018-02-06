using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    [SitecoreType]
    public interface MultiList:BaseTitle
    {

        [SitecoreChildren(InferType = true)]
        IEnumerable<TitleWithDescription> Children { get; set; }
    }
}