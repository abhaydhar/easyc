using Glass.Mapper.Sc;
using System;
using Sitecore.Globalization;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare;
using Sitecore.Data.Items;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public static class ItemProvider
    {
        public static string GetItemById(string itemId)
        {
            using (var scContext = GetSitecoreContext())
            {
                return scContext.GetItem<Tag>(itemId) != null ? scContext.GetItem<Tag>(itemId).TagName : string.Empty;
            }
        }

        private static SitecoreContext GetSitecoreContext()
        {
            var easycompareWebsite = Sitecore.Configuration.Factory.GetSite("EasyCompare");
            var scContext = new SitecoreContext(easycompareWebsite.Database);
            Sitecore.Context.Language = Language.Parse(easycompareWebsite.Language);
            return scContext;
        }

        public static T TargetModel<T>(this Item item) where T : class
        {
            Glass.Mapper.Sc.SitecoreContext scContext = new Glass.Mapper.Sc.SitecoreContext();
            return scContext.GetItem<T>(item.ID.Guid);
        }
    }
}