using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    [SitecoreType]
    public interface Footer : IBaseModel
    {
        [SitecoreField("Footer Logo Image")]
        Image LogoImage { get; set; }

        [SitecoreField("Left Copyright Text")]
        string LeftCopyRightText { get; set; }

        [SitecoreField("Advisory Center Title")]
        string AdvisoryTitle { get; set; }

        [SitecoreField("Advisory Working Time")]
        string AdvisoryWorkTime { get; set; }

        [SitecoreField("Advisory Center Phone No")]
        string AdvisoryCenterPhoneNo { get; set; }

        [SitecoreField("Advisory Mail Id Contact")]
        string AdvisoryMailIdContact { get; set; }

        [SitecoreField("Social Links Title")]
        string SocialLinksTitle { get; set; }

        [SitecoreField("Facebook")]
        Link FacebookUrl { get; set; }

        [SitecoreField("Facebook Image")]
        Image FacebookImage { get; set; }

        [SitecoreField("Twitter")]
        Link TwitterUrl { get; set; }

        [SitecoreField("Twitter Image")]
        Image TwitterImage { get; set; }

        [SitecoreField("Youtube")]
        Link YoutubeUrl { get; set; }

        [SitecoreField("Youtube Image")]
        Image YoutubeImage { get; set; }

        [SitecoreField("Line")]
        Link LineUrl { get; set; }

        [SitecoreField("Line Image")]
        Image LineImage { get; set; }

        [SitecoreField("Site Index Label")]
        string SiteIndexLabel { get; set; }

        [SitecoreField("Site Index Link")]
        Link SiteIndexLink { get; set; }

        [SitecoreField("Back to top Label")]
        string BacktoTopLabelText { get; set; }

        [SitecoreField("Bottom section links set")]
        IEnumerable<ILinkComponent> FooterBottomLinks { get; set; }

        [SitecoreChildren]
        IEnumerable<IImage> PartnerImageList { get; set; }
    }
}