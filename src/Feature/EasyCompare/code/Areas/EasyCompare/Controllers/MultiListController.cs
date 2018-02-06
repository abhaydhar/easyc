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
    public class MultiListController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        public MultiListController()
        {
            _sitecoreContext = new SitecoreContext();

        }
        // GET: EasyCompare/Articles
        public ActionResult GetMultiList()
        {
            MultiList multilist = default(MultiList);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                multilist = _sitecoreContext.GetItem<MultiList>(RenderingContext.Current.Rendering.DataSource);
            }

            return View("~/Areas/EasyCompare/Views/Components/MultiList.cshtml", multilist);

        }

       
    }
}