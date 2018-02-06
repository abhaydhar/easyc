using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    [SitecoreType]
    public class AskAQuestionModel
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string LineId { get; set; }
        public string WhenWecanCall { get; set; }

        public string YourQuestion { get; set; }

        [SitecoreField("From Email Name")]
        public string FromEmailName { get; set; }

        [SitecoreField("From Email Id")]
        public string FromEmailId { get; set; }

        [SitecoreField("Subject")]
        public string Subject { get; set; }

        [SitecoreField("Body")]
        public string Body { get; set; }



    }
}