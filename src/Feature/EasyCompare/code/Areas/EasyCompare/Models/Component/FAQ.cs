using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    [SitecoreType]
    public interface FAQ:BaseTitle
    {

        [SitecoreChildren(InferType = true)]
        IEnumerable<FrequentlyAskedQuestion> Children { get; set; }
    }
}