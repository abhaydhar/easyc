using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Security;
using Sitecore.Data.Items;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Data;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models;
using Sitecore.Foundation.SitecoreExtensions.Extensions;


namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Controllers
{
    public class EcSearchController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        public EcSearchController()
        {
            _sitecoreContext = new SitecoreContext();

        }
       
        public ActionResult SearchResult(string searchterm)
        {
            SearchModel searchModel = default(SearchModel);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                searchModel = _sitecoreContext.GetItem<SearchModel>(RenderingContext.Current.Rendering.DataSource);
                searchModel.articles = GetSearchedArticles(searchterm);
                searchModel.searchTerm = searchterm;
            }
           
            return View("~/Areas/EasyCompare/Views/Components/SearchResults.cshtml", searchModel);

        }

        private List<IArticleModel> GetSearchedArticles(string searchTerm)
        {
            var siteRoot = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.ContentStartPath);
            Sitecore.Data.Database datacontext = Sitecore.Context.Database;
            var ArticleRootPath = datacontext.GetItem(siteRoot.Fields["Articles Root"].Value).ID.Guid.ToString();
            Item ArticleRoot = Sitecore.Context.Database.GetItem(ArticleRootPath);
            List<IArticleModel> result = new List<IArticleModel>();
            try
            {
                ISearchIndex index = ContentSearchManager.GetIndex((SitecoreIndexableItem)ArticleRoot);

                using (IProviderSearchContext context = index.CreateSearchContext(SearchSecurityOptions.DisableSecurityCheck))
                {
                    var predicate = PredicateBuilder.True<ArticleSearch>();

                    //// Filter on roots
                    var rootsPredicate = PredicateBuilder.False<ArticleSearch>();
                    string shortId = ArticleRoot.ID.ToShortID().ToString().ToLower();
                    rootsPredicate = rootsPredicate.Or(p => p["_path"].Equals(shortId));
                    predicate = predicate.And(rootsPredicate);

                    // Filter on specific templates
                    var templatePredicate = PredicateBuilder.False<ArticleSearch>();
                    ID templateId = new ID("{A5CECB06-DB60-44C6-B345-F24BA2FA3D69}");
                    string templateshortId = templateId.ToShortID().ToString().ToLower();
                    templatePredicate = templatePredicate.Or(p => p["_template"].Equals(templateshortId));
                    predicate = predicate.And(templatePredicate);

                    // filter on current language
                    predicate = predicate.And(p => p.Language.Equals(Sitecore.Context.Language.Name));
                    var query = context.GetQueryable<ArticleSearch>();

                    //filter Based on Articles
                    predicate = predicate.And(p => p.Content.Contains(searchTerm));
                  
                    query = query.Filter(predicate);
                    var searchResult = query.GetResults();
                   
                    if (searchResult != null)
                    {
                        foreach (var hit in searchResult)
                        {
                            Item item = hit.Document.GetItem();
                            if (item != null)
                            {
                                IArticleModel baseItem = _sitecoreContext.GetItem<IArticleModel>(item.ID.Guid);
                                result.Add(baseItem);
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error executing content search in Solr", ex, result);
            }
            return result;
        }

        
    }
}