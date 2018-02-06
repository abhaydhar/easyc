using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models
{
    [SitecoreType]
    public class InsuranceQuoteModel
    {
        [SitecoreField("Quote Title")]
        public string QuoteTitle { get; set; }

        [SitecoreField("Quote CTA Label")]
        public string QuoteCTALabel { get; set; }

        [SitecoreField("Brand Label")]
        public string BrandLabel { get; set; }

        [SitecoreField("Brand Pop up Title")]
        public string BrandPopupTitle { get; set; }

        [SitecoreField("Brand Search Text")]
        public string BrandSearchText { get; set; }

        public string IsFullButtonWidth { get; set; }


        [SitecoreField("Model Label")]
        public string ModelLabel { get; set; }

        [SitecoreField("Model Pop up Title")]
        public string ModelPopupTitle { get; set; }

        [SitecoreField("Model Search Text")]
        public string ModelSearchText { get; set; }



        [SitecoreField("Year Label")]
        public string YearLabel { get; set; }

        [SitecoreField("Year Pop up Title")]
        public string YearPopupTitle { get; set; }

        [SitecoreField("Year Search Text")]
        public string YearSearchText { get; set; }




        [SitecoreField("Insurance Type Label")]
        public string InsuranceTypeLabel { get; set; }

        [SitecoreField("Popup Title")]
        public string InsurancePopupTitle { get; set; }

        [SitecoreField("Insurance Search Text")]
        public string InsuranceSearchText { get; set; }

        [SitecoreField("Insurance Description")]
        public string InsuranceDescription { get; set; }




        //Property to be filled
        public List<string> BrandList { get; set; }

        public List<string> ModelList { get; set; }
        public List<string> InsuranceList { get; set; }
        public Dictionary<string, List<string>> YearRangeList { get; set; }
    }
}