using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public static class Constants
    {

        public const string WebSiteName = "EasyCompare";

        //Contact Details
        public const string ContactListDomain = "Contact Details";
        public const string CompareInsurancePlansDetails = "Compare Insurance Plans Details"; 
        public const string CompareInsurancePlans = "Compare Insurance Plans"; 
        public const string PolicyListDomain = "Policy Details";
        public const string ThanksDomain = "Thanks";
        public const string SearchDomain = "Search";
        public const string CallMeBackDomain = "Call Me Back";
        public const string ArticlesDomain = "Article";
        public const string AskQuestionDomain = "AskQuestion";
        public const string PageLabelsPageNotFound = "404PageLabels";
        public const string QuoteAndBuyDictionary_Name = "EC Quote And Buy";
        public const string StandardPageTemplate = "{39196835-2C02-48CA-B3BC-6CD4FD3E4AEC}";
        public const string SupportPageType = "{3B4D3E9C-1218-4DA8-AB1F-7D1318BE499B}";
        public const string FAQPageType = "{19EDC89A-9F5F-4D03-9E5E-23DA4ED39F61}";
        public const string AskQuestionPageType = "{D10E1753-2651-4860-B031-4DB88DB4ABED}";


        //Thanks Page

        public class CallMeBackOptions
        {
            public const string Email = "Email";
            public const string Phone = "Phone";
            public const string Line = "Line";
        }

        public class CallMeBackTimeOptions
        {
            public const string Morning = "morning";
            public const string Afternoon = "afternoon";
            public const string Evening = "evening";
        }

        public struct QuoteAndBuySettings
        {
            //public static ID ID = new ID("{F832A4C4-388A-4A66-9A2F-DD6369540D55}");

            public struct Fields
            {
                //public static readonly ID MotorCarLandingPage = new ID("{CC6EFA7E-7654-44DD-9E7B-8274545D3130}");
                //public static readonly ID MotorCarPlanDetailPage = new ID("{CD9076B3-52E3-44A6-B2DE-4071F7E2DB7A}");
                //public static readonly ID MotorCarPlanComparisonPage = new ID("{C106A952-6FB1-4C4E-B935-1AC94C3C1BD5}");
                //public static readonly ID MotorCarPolicyDetailPage = new ID("{24E28C6E-F26B-4BF9-AD66-6AEB2B81DB39}");

                public static readonly ID MotorCarLandingPage = new ID("{A9566B90-DAAB-45E7-99CC-EDDAF7BABE80}");
                public static readonly ID MotorCarPlanDetailPage = new ID("{800CA1B9-D383-4B35-B63B-1FF2AC3B77B4}");
                public static readonly ID MotorCarPolicyDetailPage = new ID("{3E06E128-97A0-48F2-8C39-D93C8656F442}");
                public static readonly ID MotorCarPlanComparisonPage = new ID("{2625DF0B-C0DD-4D30-BF5C-E19FD73943A0}");
                public static readonly ID MotorCarQuoteThankYouPage = new ID("{CCE82142-DE19-45BE-9482-53930E4F60A2}");
                public static readonly ID MotorCarDetailPage = new ID("{927D16DA-4839-4D37-B264-7F67828C24AB}");

                public static readonly ID MarketingCallMeBackThankYouPage = new ID("{7AC5AB48-A20B-45A3-AA51-FD15A2D4F95C}");
            }
        }
    }
    //Website Name

}
