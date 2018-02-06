using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Component
{
    [SitecoreType(AutoMap = true)]
    public interface IBlockQuote
    {
        [SitecoreField("Block Quote Text")]
        string BlockQuoteText { get; set; }
    }
}
