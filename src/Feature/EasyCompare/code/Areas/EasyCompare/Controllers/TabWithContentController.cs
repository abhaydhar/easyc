using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models;


namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Controllers
{
    public class TabWithContentController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        public TabWithContentController()
        {
            _sitecoreContext = new SitecoreContext();

        }
        

        public ActionResult GetTabWithContent()
        {
            TabWithContent support = default(TabWithContent);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                support = _sitecoreContext.GetItem<TabWithContent>(RenderingContext.Current.Rendering.DataSource);
                if (support!=null && support.PageType != null)
                {
                    switch (support.PageType.Id.ToString().ToUpper())
                    {
                        case Constants.SupportPageType:
                            support.SupportContent = _sitecoreContext.GetItem<ImageWithDescription>(support.Content.Id.ToString());
                            break;
                        case Constants.FAQPageType:
                            support.FAQContent = _sitecoreContext.GetItem<FAQ>(support.Content.Id.ToString());
                            break;
                        case Constants.AskQuestionPageType:
                            support.AskQuestionContent = _sitecoreContext.GetItem<BaseTitle>(support.Content.Id.ToString());
                            break;
                    }
                }
                 
                   
            }
            return View("~/Areas/EasyCompare/Views/Components/TabWithContent.cshtml", support);

        }


        public ActionResult GetTabList()
        {
            TabList tablist = default(TabList);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                tablist = _sitecoreContext.GetItem<TabList>(RenderingContext.Current.Rendering.DataSource);

                var currentpage = _sitecoreContext.GetCurrentItem<IBaseModel>();
                tablist.currentPageUrl = currentpage.Url;
            }

            return View("~/Areas/EasyCompare/Views/Components/TabList.cshtml", tablist);
        }

    }
}