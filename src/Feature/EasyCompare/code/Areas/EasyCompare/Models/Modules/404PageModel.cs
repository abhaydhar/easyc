using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    [SitecoreType]
    public interface Page404Model: BaseTitle
    {
        [SitecoreField("404 Page Article Title")]
        string PageArticleTitle { get; set; }

        [SitecoreField("404 Page Article Summary")]
        string PageArticleSummary { get; set; }

        [SitecoreField("404 Page Article Image")]
        Image PageArticleImage { get; set; }

    }
}