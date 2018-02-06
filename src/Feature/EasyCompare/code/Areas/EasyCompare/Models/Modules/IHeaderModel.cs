using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;
using Glass.Mapper.Sc.Fields;
namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface IHeaderModel:IBaseModel
    {
        [SitecoreField("Header Link")]
        Link HeaderLink { get; set; }

        [SitecoreField("Quick Link")]
        Link QuickLink { get; set; }

        [SitecoreField("Header Logo")]
        Image HeaderLogo { get; set; }

        bool IsFixedHeader { get; set; }
    }
}