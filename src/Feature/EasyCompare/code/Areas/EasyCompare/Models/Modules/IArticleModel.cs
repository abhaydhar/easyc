using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface IArticleModel:IPageModel,BaseTitle
    {
        [SitecoreField("Image")]
        Image ArticleImage { get; set; }

        [SitecoreField("Date")]
        DateTime ArticleDate { get; set; }

        [SitecoreField("Article Tagging")]
        IEnumerable<Tag> ArticleTags { get; set; }

        [SitecoreField("Body")]
        string Body { get; set; }

        



    }
}