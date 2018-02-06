using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    public interface ICountrySelector :BaseTitle
    {
        [SitecoreChildren]
        IEnumerable<Country> Children { get; set; }
    }
}
