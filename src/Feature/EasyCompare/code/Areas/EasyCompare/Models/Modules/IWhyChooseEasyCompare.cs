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
    public interface IWhyChooseEasyCompare:IBaseModel
    {
        [SitecoreField("Main Title")]
        string MainTitle { get; set; }

        [SitecoreField("Left Image")]
        Image LeftImage { get; set; }

        [SitecoreField("Left Bottom Title")]
        string LeftBottomTitle { get; set; }

        [SitecoreField("Left bottom SubTitle")]
        string LeftBottomSubTitle { get; set; }

        [SitecoreField("Right Image")]
        Image RightImage { get; set; }

        [SitecoreField("RightBottom Title")]
        string RightBottomTitle { get; set; }

        [SitecoreField("Right bottom SubTitle")]
        string RightBottomSubTitle { get; set; }
    }
}
