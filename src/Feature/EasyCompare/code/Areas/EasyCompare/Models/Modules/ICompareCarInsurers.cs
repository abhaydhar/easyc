using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    [SitecoreType]
    public interface ICompareCarInsurers:IBaseModel
    {
        [SitecoreField("Title")]
        string Title { get; set; }

        [SitecoreField("Let us Call you button Link")]
        Link LetUsCallyouLink { get; set; }

        [SitecoreField("Let us Call you button Text")]
        string LetUsCallyouText { get; set; }

        [SitecoreField("Start Comparing Button Link")]
        Link StartComparingLink { get; set; }

        [SitecoreField("Start Comparing Button Text")]
        string StartComparingText { get; set; }

        [SitecoreField("Bottom Text")]
        string BottomText { get; set; }
        
    }
}
