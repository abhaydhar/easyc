using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface TabWithContent : IBaseModel
    {
        [SitecoreField("Tabs")]
        IEnumerable<BaseLink> Tabs { get; set; }


        [SitecoreField("Content")]
        IBaseModel Content { get; set; }

        [SitecoreField("Page Type")]
        Tag PageType { get; set; }

        ImageWithDescription SupportContent { get; set; }

        FAQ FAQContent { get; set; }

        BaseTitle AskQuestionContent { get; set; }

    }
}