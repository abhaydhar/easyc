using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    
    public class ArticleSearch: SearchResultItem
    {
        [IndexField("Title")]
        public string Title { get; set; }

        [IndexField("Sub Title")]
        public string SubTitle { get; set; }

        [IndexField("Body")]
        public string Body { get; set; }
        [IndexField("Page Title")]
        public string PageTitle { get; set; }
    }
}