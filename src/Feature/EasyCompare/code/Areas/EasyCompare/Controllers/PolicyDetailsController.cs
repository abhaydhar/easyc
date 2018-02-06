
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

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public class PolicyDetailsController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        private ECQuoteAndBuyRepository qbRepository;
        private ECCarRepository carRepository;

        public PolicyDetailsController()
        {
            _sitecoreContext = new SitecoreContext();
            qbRepository = new ECQuoteAndBuyRepository();
            carRepository = new ECCarRepository(qbRepository);

        }
        // GET: InsuranceQuote
        public ActionResult GetPolicy()
        {
            PolicyDetails policyDetails = default(PolicyDetails);

            policyDetails = new PolicyDetails();

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
                string buyUrl = u.Action(actionName: "BuyPolicy", controllerName: "PolicyDetails", routeValues: null);
                string changePlanUrl = u.Action(actionName: "ChangePlan", controllerName: "PolicyDetails", routeValues: null);
                string updateCarDetailUrl = u.Action(actionName: "UpdateCarDetail", controllerName: "PolicyDetails", routeValues: null);

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
                        if (sData.PortalStages.Contains(MotorConstants.PortalSteps.ThankYou))
                        {
                            return new RedirectResult(string.Format("{0}?{1}={2}", setting.MotorCarQuoteThankYouUrl, ApiConstants.QueryStringParamNames.PM, pm));
                        }

                        var service = new CommonService();

                        var pageData = new
                        {
                            SKey = sKey,
                            buyUrl = buyUrl,
                            changePlanUrl = changePlanUrl,
                            updateCarDetailUrl = updateCarDetailUrl,
                            FormData = GetFormDataFromSession(sData),
                            MasterData = new
                            {
                                Today = DateTime.Today,
                                ErrMsgs = new
                                {
                                    NameTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Name"),
                                    NameRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "name-required-message"),
                                    EmailTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "E-mail"),
                                    EmailRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "email-required-message"),
                                    EmailInvalidMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "email-invalid-message"),
                                    SubscribeTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "form-subscribe-receive-newsletter"),
                                    SubscribeRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "form-subscribe-receive-newsletter-required-message"),
                                    SubscribeModeTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "form-subscribe-preferred-contact"),
                                    SubscribeModeRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "form-subscribe-preferred-contact-required-message"),
                                }
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


            return View("~/Areas/EasyCompare/Views/Components/PolicyDetails.cshtml", policyDetails);
        }

        private object GetFormDataFromSession(ApiPolicyHeader sData)
        {
            var selectedPlan = sData.CarRates.FirstOrDefault(x => x.Id == sData.SelectedPlanId);

            var filter = sData.CarPlanOptionFilter;

            if (filter.IsIncludeCompulsory ?? false)
            {
                selectedPlan.IsIncludeCompulsory = true;
                selectedPlan.TotalPremium = selectedPlan.GrossPremium + (selectedPlan.CMIPrice ?? 0);
            }
            else
            {
                selectedPlan.IsIncludeCompulsory = false;
                selectedPlan.TotalPremium = selectedPlan.GrossPremium;
            }

            selectedPlan.TotalPremiumAfterDiscount = selectedPlan.TotalPremium - (selectedPlan.DiscountPremium ?? 0);
            selectedPlan.TotalPremiumSaving = selectedPlan.DiscountPremium;

            var asset = new
            {
                carMake = sData.CarAsset.CarMake,
                carModel = sData.CarAsset.CarModel,
                carYear = sData.CarAsset.YearOfRegistration,
                carClass = sData.CarAsset.CarClassName
            };

            object ph = null;

            if(sData.PolicyHolder != null)
            {
                ph = new
                {
                    FirstName = sData.PolicyHolder.FirstName,
                    Email = sData.PolicyHolder.Email,
                    Phone = sData.PolicyHolder.ContactNumber,
                    LineId = sData.PolicyHolder.LineId,
                    CallBackTime = sData.PolicyHolder.PreferredCallBackTime,
                };
            }

            var quote = new
            {
                asset = asset,
                ph = ph,
                plan = carRepository.GetPagePlanData(selectedPlan),
            };

            return quote;
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult BuyPolicy(PolicyDetailViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.ThankYou;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyDetail;
                sData.PortalStages.Add(MotorConstants.PortalSteps.ThankYou);


                //validate contact...
                var apiContact = new ApiContact()
                {
                    Email = form.Email,
                    ContactNumber = form.Phone,
                    FirstName = string.Format("{0} {1}", form.LastName, form.FirstName).Trim(),
                    LineId = form.LineId,
                    PreferredCallBackTime = form.CallBackTime,
                    IsSubscribeNewsPromo = form.IsSubscribeNewsPromo,
                    IsSubscribeEmail = form.IsSubscribeEmail,
                    IsSubscribeMail = form.IsSubscribeMail,
                    IsSubscribeSMS = form.IsSubscribeSMS
                };

                var validateContactResult = policyService.ValidateContact(apiContact);

                if (validateContactResult.ValidateStatusID == 0)
                {
                    apiContact = policyService.CreateContact(apiContact);
                }
                else
                {
                    apiContact.Id = validateContactResult.ContactId;
                    policyService.UpdateContact(apiContact);
                }

                sData.PolicyHolder = apiContact;
                sData.SugarContactId = apiContact.Id;
                var selectedPlan = sData.CarRates.FirstOrDefault(x => x.Id == sData.SelectedPlanId);
                carRepository.PopulateApiAssetFromPlan(sData, selectedPlan);

                var request = new AutoGeneralTH.ApiModel.ApiRequest()
                {
                    ProductName = MotorConstants.MotorProduct.ProductName,
                    PolicyHeader = sData,
                };

                var response = policyService.CreateProposal(request);
                var settings = qbRepository.GetQuoteAndBuySettings();

                var result = new
                {
                    status = "S",
                    redirectUrl = this.Url.Content(settings.MotorCarQuoteThankYouUrl + "?" + response.QueryStringParam),
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(qbRepository.HandleException(ex));
            }
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult UpdateCarDetail(PolicyDetailViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.CarDetails;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyDetail;

                sData.PortalStages.Add(MotorConstants.PortalSteps.CarDetails);

                var apiContact = new ApiContact()
                {
                    Email = form.Email,
                    ContactNumber = form.Phone,
                    FirstName = string.Format("{0} {1}", form.LastName, form.FirstName).Trim(),
                    LineId = form.LineId,
                    PreferredCallBackTime = form.CallBackTime,
                };

                sData.PolicyHolder = apiContact;

                var request = new AutoGeneralTH.ApiModel.ApiRequest()
                {
                    ProductName = MotorConstants.MotorProduct.ProductName,
                    PolicyHeader = sData,
                };

                var response = policyService.SavePolicyToSession(request);
                var settings = qbRepository.GetQuoteAndBuySettings();

                var result = new
                {
                    status = "S",
                    redirectUrl = this.Url.Content(settings.MotorCarDetailsUrl + "?" + response.QueryStringParam),
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(qbRepository.HandleException(ex));
            }
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult ChangePlan(PolicyDetailViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.PolicyPlanDetail;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyDetail;

                var apiContact = new ApiContact()
                {
                    Email = form.Email,
                    ContactNumber = form.Phone,
                    FirstName = string.Format("{0} {1}", form.LastName, form.FirstName).Trim(),
                    LineId = form.LineId,
                    PreferredCallBackTime = form.CallBackTime,
                };

                sData.PolicyHolder = apiContact;

                var request = new AutoGeneralTH.ApiModel.ApiRequest()
                {
                    ProductName = MotorConstants.MotorProduct.ProductName,
                    PolicyHeader = sData,
                };

                var response = policyService.SavePolicyToSession(request);
                var settings = qbRepository.GetQuoteAndBuySettings();

                var result = new
                {
                    status = "S",
                    redirectUrl = this.Url.Content(settings.MotorCarPlanDetailUrl + "?" + response.QueryStringParam),
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(qbRepository.HandleException(ex));
            }
        }

        private void ValidateFormInput(PolicyDetailViewModel form)
        {
            this.qbRepository.ValidateInputString(form.FirstName);
            this.qbRepository.ValidateInputString(form.LastName);
            this.qbRepository.ValidateInputString(form.Email);
            this.qbRepository.ValidateInputString(form.Phone);
            this.qbRepository.ValidateInputString(form.LineId);
            this.qbRepository.ValidateInputString(form.CallBackTime);
        }
    }
}