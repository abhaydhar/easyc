using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface IArticles:BaseTitle
    {

        List<IArticleModel> ArticlesList { get; set; }

        
        [SitecoreField("ArticleLink Text")]
        string ArticleLinkText { get; set; }

        [SitecoreField("All Article Link")]
       Link AllArticleLink { get; set; }
        

        List<string> ArticleTagList { get; set; }
        List<string> ArticleTagID { get; set; }

        List<IArticleModel> ArticlesWithTagList { get; set; }

        string activeClassTag { get; set; }
        string articlePageUrl { get; set; }
    }
}