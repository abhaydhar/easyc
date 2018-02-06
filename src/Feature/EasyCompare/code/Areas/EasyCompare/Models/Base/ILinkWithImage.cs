using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Base
{
    public interface ILinkWithImage:IBaseModel
    {
        [SitecoreField("Link Url")]
        string LinkUrl { get; set; }

        [SitecoreField("Link Image")]
        Image LinkImage { get; set; }
    }
}
