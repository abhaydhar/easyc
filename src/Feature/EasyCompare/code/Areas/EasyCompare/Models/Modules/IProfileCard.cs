using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    [SitecoreType]
    public interface IProfileCard:IBaseModel
    {
        [SitecoreField("Profile Image")]
        Image ProfileImage { get; set; }

        [SitecoreField("Name")]
        string Name { get; set; }

        [SitecoreField("Role")]
        string Role { get; set; }

        [SitecoreField("Role Description")]
        string RoleDescription { get; set; }

        [SitecoreField("Profile Email Link")]
        Link ProfEmailLink { get; set; }

    }
}
