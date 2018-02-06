using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules
{
    public class CarDetailsViewModel
    {
        public int MakeId { get; set; }

        public int ModelId { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int CarYear { get; set; }

        public string InsuranceTypeCode { get; set; }

        public string InsuranceType { get; set; }

        public string SKey { get; set; }
    }
}