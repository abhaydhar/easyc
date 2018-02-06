using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    public class PolicyDetailViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string LineId { get; set; }

        public string CallBackTime { get; set; }

        public string IsSubscribeNewsPromo { get; set; }

        public string IsSubscribeMail { get; set; }

        public string IsSubscribeEmail { get; set; }

        public string IsSubscribeSMS { get; set; }

        public string SKey { get; set; }
    }
}