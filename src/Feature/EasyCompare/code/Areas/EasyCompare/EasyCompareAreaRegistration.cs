using System.Web.Mvc;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public class EasyCompareAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EasyCompare";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EasyCompare_default",
                "EasyCompare/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}