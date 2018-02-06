using AutoGeneralTH.ApiModel.Common;
using AutoGeneralTH.ApiModel.Policy;
using AutoGeneralTH.ApiModel.Portal;
using AutoGeneralTH.ApiProxy;
using Celes.Framework.Common.Exceptions;
using Celes.Framework.Common.Helper;
using Newtonsoft.Json;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Repositories
{
    public class ECQuoteAndBuyRepository
    {
        public bool IsLocalDev
        {
            get
            {
                return Celes.Framework.Common.Helper.CommonHelper.GetBoolFromString(Celes.Framework.Common.Helper.ConfigHelper.GetAppSetting(configKey: "BudgetDirect.IsLocalDev"));
            }
        }

        public bool IsThaiLanguage
        {
            get
            {
                return Sitecore.Context.Language.ToString().Equals("th-TH", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public QuoteAndBuySetting GetQuoteAndBuySettings()
        {
            var setting = new QuoteAndBuySetting();
            var siteRoot = Context.Database.GetItem(Context.Site.ContentStartPath);
            Data.Database context = Context.Database;

            var landingPage = context.GetItem(siteRoot.Fields[Constants.QuoteAndBuySettings.Fields.MotorCarLandingPage].Value);
            var carPlanDetailPage = context.GetItem(siteRoot.Fields[Constants.QuoteAndBuySettings.Fields.MotorCarPlanDetailPage].Value);
            var carPolicyDetailPage = context.GetItem(siteRoot.Fields[Constants.QuoteAndBuySettings.Fields.MotorCarPolicyDetailPage].Value);
            var carPlanComparisonPage = context.GetItem(siteRoot.Fields[Constants.QuoteAndBuySettings.Fields.MotorCarPlanComparisonPage].Value);
            var carQuoteThankYouPage = context.GetItem(siteRoot.Fields[Constants.QuoteAndBuySettings.Fields.MotorCarQuoteThankYouPage].Value);
            var carDetailPage = context.GetItem(siteRoot.Fields[Constants.QuoteAndBuySettings.Fields.MotorCarDetailPage].Value);

            var marketingCallMeBackPage = context.GetItem(siteRoot.Fields[Constants.QuoteAndBuySettings.Fields.MarketingCallMeBackThankYouPage].Value);

            setting.MotorCarLandingPageUrl = (landingPage != null) ? landingPage.Url() : "~/";
            setting.MotorCarPlanDetailUrl = (carPlanDetailPage != null) ? carPlanDetailPage.Url() : "~/options";
            setting.MotorCarPolicyDetailUrl = (carPolicyDetailPage != null) ? carPolicyDetailPage.Url() : "~/insurance-contact-form";
            setting.MotorCarPlanComparisonUrl = (carPlanComparisonPage != null) ? carPlanComparisonPage.Url() : "~/compare";
            setting.MotorCarQuoteThankYouUrl = (carQuoteThankYouPage != null) ? carQuoteThankYouPage.Url() : "~/Thanks";
            setting.MotorCarDetailsUrl = (carDetailPage != null) ? carDetailPage.Url() : "~/";
            //setting.MotorCarPlanDetailUrl = "~/options";
            //setting.MotorCarPolicyDetailUrl = "~/insurance-contact-form";
            //setting.MotorCarPlanComparisonUrl = "~/compare";
            //setting.MotorCarQuoteThankYouUrl = "~/Thanks";

            setting.MarketingCallMeBackThankYouUrl = (marketingCallMeBackPage != null) ? marketingCallMeBackPage.Url() : "~/";

            return setting;
        }

        public string GetJsDateString(DateTime dt)
        {
            return JsonConvert.SerializeObject(dt).Replace(oldValue: "\"", newValue: "");
        }

        public string GetJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public string GetJsonStringTrimBlank(object obj)
        {

            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public string GetPortalMappedMessage(string code, List<string> parameters = null)
        {
            var dicKey = string.Format("portal-message-code-{0}", code);

            if (code == "RECAPT1")
            {
                dicKey = "re-captcha-required-msg";
            }
            else if (code == "RECAPT2")
            {
                dicKey = "re-captcha-invalid-msg";
            }

            var message = Translate.TextByDomain(Constants.QuoteAndBuyDictionary_Name, dicKey);

            if (parameters != null && parameters.Count > 0)
            {
                message = string.Format(message, parameters.Cast<object>().ToArray());
            }

            return message;
        }

        public object HandleException(Exception ex, bool reThrow = false)
        {
            Sitecore.Diagnostics.Log.Error(message: "Portal Exception", exception: ex, owner: this);
            string errorCode = this.LogException(ex);
            var be = ex as BusinessException;
            string genericErrorMessage = Translate.TextByDomain(Constants.QuoteAndBuyDictionary_Name, "generic-call-us-error-message");

            if (reThrow)
            {
                throw ex;
            }

            if (be != null)
            {
                return new
                {
                    status = "F",
                    errorCode = be.ErrorCode,
                    errorMessage = (be.ErrorMessage == ApiConstants.GenericErrorMessage || be.ErrorCode == "GenericErrorMessage" || be.ErrorCode.StartsWith("E")) ? genericErrorMessage : GetPortalMappedMessage(be.ErrorCode, be.Parameters),
                };
            }
            else
            {

                if (string.IsNullOrEmpty(genericErrorMessage))
                {
                    genericErrorMessage = ex.Message;
                }

                return new
                {
                    status = "F",
                    errorCode = errorCode,
                    errorMessage = IsLocalDev ? ex.ToString() : genericErrorMessage
                };
            }
        }

        private string LogException(Exception ex)
        {
            try
            {
                var request = new ApiElmahError()
                {
                    Source = ex.Source,
                    Message = ex.Message,
                    AllXml = ex.ToString(),
                    Application = ConfigHelper.GetAppSetting(configKey: "BudgetDirect.ApplicationCode"),
                    Host = System.Environment.MachineName,
                    Type = ex.GetType().ToString(),
                };

                var errorId = new AutoGeneralTH.ApiProxy.CommonService().AddErrorLog(request);

                return CommonHelper.GetElmahErrorCode(errorId);
            }
            catch
            {
                return string.Empty;
            }
        }


        public bool ValidateUrlParamter(List<AutoGeneralTH.ApiModel.Common.ApiUrlParam> parameters)
        {
            var crDt = parameters.FirstOrDefault(x => x.ParamName == ApiConstants.QueryStringParamNames.CreateDate);

            if (crDt != null)
            {
                if (!string.IsNullOrEmpty(crDt.ParamValue))
                {
                    var dt = System.Convert.ToDateTime(crDt.ParamValue);

                    if (dt.AddHours(2) < DateTime.Now)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool ValidateUrlAgainstPortalSteps(ApiPolicyHeader sData, string currentStep)
        {
            if (sData.PortalStages.Contains(currentStep))
            {
                return true;
            }

            return false;
        }

        public void AddPolicySessionKey(string sKey, HttpSessionStateBase session)
        {
            var objSession = session[ApiConstants.QuoteAndBuyPolicySessionKey] as List<string>;
            if (objSession == null)
            {
                var li = new List<string>();
                li.Add(sKey);
                session.Add(ApiConstants.QuoteAndBuyPolicySessionKey, li);
            }
            else
            {
                objSession.Add(sKey);
            }
        }

        public bool ValidatePolicySessionKey(string sKey, HttpSessionStateBase session)
        {
            var objSession = session[ApiConstants.QuoteAndBuyPolicySessionKey] as List<string>;
            if (objSession == null)
            {
                return false;
            }
            else
            {
                return objSession.Contains(sKey);
            }
        }

        public bool ValidateInputString(string input)
        {
            if (Celes.Framework.Common.Helper.CrossSiteScriptingValidationHelper.IsDangerousString(input))
            {
                throw new BusinessException("", "xss-potentially-dangerous-input-message");
            }

            return true;
        }

        public string GetStringFromBool(bool res)
        {
            if(res)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        public string GetSumInsuredString(decimal? si, decimal? maxSI)
        {
            if(si == null)
            {
                return string.Empty;
            }

            if(si == -1 && maxSI != null)
            {
                return maxSI.Value.ToString("000");
            }

            return si.Value.ToString("000");
        }

        public string GetPremiumString(decimal premium)
        {
            return premium.ToString("0.00");
        }

        public object GetYearRangeList(List<int> yearList)
        {
            List<object> rangeList = new List<object>();

            var maxYear = yearList[0];
            var minYear = yearList[yearList.Count - 1];

            var curYear = maxYear - maxYear % 10;

            rangeList.Add(new {
                fromYear = curYear,
                toYear = maxYear,
            });

            while (curYear > minYear)
            {
                curYear = curYear - 10;
                rangeList.Add(new
                {
                    fromYear = curYear > minYear ? curYear : minYear,
                    toYear = curYear + 10,
                });
            }

            return rangeList;
        }

        public bool IsSameStringIgnoreCase(string str1, string str2)
        {
            str1 = GetLowerCaseValue(str1);
            str2 = GetLowerCaseValue(str2);

            return str1 == str2;
        }

        private string GetLowerCaseValue(string str)
        {
            if(!string.IsNullOrEmpty(str))
            {
                return str.ToLower();
            }

            return string.Empty;
        }
    }
}