
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models;
using System.Collections.Generic;
using AutoGeneralTH.ApiProxy;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Repositories;
using System.Linq;
using AutoGeneralTH.ApiModel.Constants;
using System;
using AutoGeneralTH.ApiModel.Policy;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules;
using AutoGeneralTH.ApiModel.Contact;
using Celes.Framework.Common.Helper;
using Sitecore.Globalization;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public class ContactDetailsController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        private ECQuoteAndBuyRepository qbRepository;
        private ECCarRepository carRepository;

        public ContactDetailsController()
        {
            _sitecoreContext = new SitecoreContext();
            qbRepository = new ECQuoteAndBuyRepository();
            carRepository = new ECCarRepository(qbRepository);

        }
        // GET: InsuranceQuote
        public ActionResult ContactDetailsForm()
        {
            
            return View("~/Areas/EasyCompare/Views/Components/ContactDetails.cshtml");
        }

        public ActionResult SubmitForm()
        {
            //{Name} palceholder has to be replaced by Signed Up User Name
            string ThankYouSignature = Translate.TextByDomain(Constants.ContactListDomain, "First Name");

            try
            {
                string pm = Request.QueryString[ApiConstants.QueryStringParamNames.PM];
                var setting = this.qbRepository.GetQuoteAndBuySettings();

                if (!qbRepository.IsLocalDev && !Sitecore.Context.PageMode.IsExperienceEditor && !Sitecore.Context.PageMode.IsPreview)
                {
                    if (string.IsNullOrEmpty(pm))
                    {
                        return new RedirectResult(setting.MotorCarLandingPageUrl);
                    }
                }

                var u = new UrlHelper(this.Request.RequestContext);

                if (!string.IsNullOrEmpty(pm))
                {
                    var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                    var commonService = new AutoGeneralTH.ApiProxy.CommonService();
                    var qParams = commonService.DecryptUrlParams(pm);
                    string sKey = qParams.Where(x => x.ParamName == ApiConstants.QueryStringParamNames.SessionKey).FirstOrDefault().ParamValue;

                    if (!this.qbRepository.ValidateUrlParamter(qParams))
                    {
                        return new RedirectResult(setting.MotorCarLandingPageUrl);
                    }

                    if (!this.qbRepository.ValidatePolicySessionKey(sKey, Session))
                    {
                        return new RedirectResult(setting.MotorCarLandingPageUrl);
                    }

                    var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, sKey);

                    if (sData != null)
                    {
                        var service = new CommonService();

                        var pageData = new
                        {
                            SKey = sKey,
                            FormData = GetFormDataFromSession(sData),
                            MasterData = new
                            {
                                Today = DateTime.Today
                            }
                        };

                        this.ViewBag.pageConfigData = qbRepository.GetJsonStringTrimBlank(pageData);
                    }
                }
            }
            catch (Exception ex)
            {
                this.qbRepository.HandleException(ex, true);
            }

            //Need to be implemented (First Name has to be passed for Thank you message)
            return View("~/Areas/EasyCompare/Views/Components/Thanks.cshtml");
        }

        private object GetFormDataFromSession(ApiPolicyHeader sData)
        {
            var ph = new
            {
                FullName = sData.PolicyHolder.FirstName
            };

            var quote = new
            {
                ph = ph
            };

            return quote;
        }
    }
}