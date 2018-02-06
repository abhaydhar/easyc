using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models;


namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        public ArticlesController()
        {
            _sitecoreContext = new SitecoreContext();

        }
        // GET: EasyCompare/Articles
        public ActionResult Article()
        {
            IArticleModel article = default(IArticleModel);

            article = _sitecoreContext.GetCurrentItem<IArticleModel>();

            return View("~/Areas/EasyCompare/Views/Components/Articles.cshtml", article);

        }

        public ActionResult GetArticleList()
        {
            IArticles articleList = default(IArticles);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                articleList = _sitecoreContext.GetItem<IArticles>(RenderingContext.Current.Rendering.DataSource);


            }
            articleList.ArticlesList = GetArticles(articleList);
            if (articleList.ArticlesList.Count() >= 3)
            {
                articleList.ArticlesList = articleList.ArticlesList.OrderByDescending(s => s.ArticleDate).Take(3).ToList();
            }


            return View("~/Areas/EasyCompare/Views/Components/ArticleList.cshtml", articleList);

        }

        public ActionResult GetArticleListWithTag()
        {
            IArticles articleList = default(IArticles);
            var tagname = Request.QueryString["tagname"];
            if (String.IsNullOrEmpty(tagname))
            {
                tagname = "all";
            }

                List<IArticleModel> articles = new List<IArticleModel>();
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                articleList = _sitecoreContext.GetItem<IArticles>(RenderingContext.Current.Rendering.DataSource);

            }
            articleList.articlePageUrl = _sitecoreContext.GetCurrentItem<IPageModel>().Url;
            if (!String.IsNullOrEmpty(tagname))
            {
                articleList.activeClassTag = tagname;
            }
            articles = GetArticles(articleList);
            articles = articles.OrderByDescending(s => s.ArticleDate).ToList();
            articleList.ArticleTagList = new List<string>();
            articleList.ArticleTagID = new List<string>();
            articleList.ArticlesWithTagList = new List<IArticleModel>();
            if(tagname=="all")
            {
                articleList.ArticlesWithTagList = articles;
               
            }
            foreach (var article in articles)
            {
                if (article != null)
                {
                    if (article.ArticleTags != null && article.ArticleTags.Any())
                    {
                        foreach (var tag in article.ArticleTags)
                        {
                            if (!articleList.ArticleTagList.Contains(tag.TagName))
                            {
                                articleList.ArticleTagList.Add(tag.TagName);
                                articleList.ArticleTagID.Add(tag.Id.ToString());
                            }
                            if (!String.IsNullOrEmpty(tagname))
                            {
                                if (tag.Id.ToString() == tagname)
                                {
                                    articleList.ArticlesWithTagList.Add(article);
                                }
                                
                            }
                        }
                    }
                }
            }

            return View("~/Areas/EasyCompare/Views/Components/ArticleListWithTags.cshtml", articleList);

        }
        private List<IArticleModel> GetArticles(IArticles articleList)
        {
            var siteRoot = Context.Database.GetItem(Context.Site.ContentStartPath);
            SiteRoot siteRootItem = _sitecoreContext.GetItem<SiteRoot>(siteRoot.ID.ToString());
            var ArticlesRoot = siteRoot.Axes.SelectSingleItem(siteRootItem.ArticlesRoot.ToString());
            if (ArticlesRoot != null && ArticlesRoot.HasChildren)
            {
                articleList.ArticlesList = new List<IArticleModel>();
                foreach (Item item in ArticlesRoot.Children)
                {
                    var articleitem = _sitecoreContext.GetItem<IArticleModel>(item.ID.ToString(),Context.Language);
                    if (articleitem != null)
                    {
                        articleList.ArticlesList.Add(articleitem);
                    }
                }
            }
            return articleList.ArticlesList;
        }

       
    }
}