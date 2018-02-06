using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models;
namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Controllers
{
    public class FeaturedImageContentController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        public FeaturedImageContentController()
        {
            _sitecoreContext = new SitecoreContext();

        }
        // GET: EasyCompare/Articles
        public ActionResult GetFeaturedImageContent()
        {
            FeaturedImageContent featuredImageContent = default(FeaturedImageContent);

            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                featuredImageContent = _sitecoreContext.GetItem<FeaturedImageContent>(RenderingContext.Current.Rendering.DataSource);


            }

            return View("~/Areas/EasyCompare/Views/Components/FeaturedImageContent.cshtml", featuredImageContent);

        }
    }
}