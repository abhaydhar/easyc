using System;
using System.Collections.Generic;

using Glass.Mapper.Sc.Configuration.Attributes;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    public interface SearchModel : IBaseModel
    {
        [SitecoreField("Title")]
        string Title { get; set; }

        [SitecoreField("Not Found Text")]
        string NotFoundText { get; set; }

        [SitecoreField("Found Text")]
        string FoundText { get; set; }

        [SitecoreField("Search Page Url")]
        string SearchPageUrl { get; set; }


        List<IArticleModel> articles { get; set; }

        string searchTerm { get; set; }
    }
}