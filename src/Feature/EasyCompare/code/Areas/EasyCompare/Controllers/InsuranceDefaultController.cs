using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Repositories;
using AutoGeneralTH.ApiProxy;
using AutoGeneralTH.ApiModel.Constants;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules;
using AutoGeneralTH.ApiModel.Policy;
using AutoGeneralTH.ApiModel.Portal;
using AutoGeneralTH.ApiModel.Contact;
using AutoGeneralTH.ApiModel.Common;
using Sitecore.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Controllers
{
    public class InsuranceDefaultController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        private ECQuoteAndBuyRepository qbRepository;
        private ECCarRepository carRepository;

        public InsuranceDefaultController()
        {
            _sitecoreContext = new SitecoreContext();
            qbRepository = new ECQuoteAndBuyRepository();
            carRepository = new ECCarRepository(qbRepository);

        }
        // GET: EasyCompare/Articles
        public ActionResult CompareInsurancePlans()
        {
            //Use this controller for Compareinsuanceplans components on OptionsPage

            return View("~/Areas/EasyCompare/Views/Components/CompareInsurancePlans.cshtml");
        }
        public ActionResult CallMeBack()
        {
            var parameters = WebUtil.ParseUrlParameters(RenderingContext.Current.Rendering["Parameters"]);

            CallMeBackModel model = new CallMeBackModel();
            if (parameters != null)
            {
                model.isPageBar = (parameters["Is Page Bar"] != null && parameters["Is Page Bar"] == "1") ? true : false;
            }
            return View("~/Areas/EasyCompare/Views/Components/CallMeBack.cshtml",model);

        }
        public ActionResult InsurancePlans()
        {

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
                string buyUrl = u.Action(actionName: "BuyPolicy", controllerName: "InsuranceDefault", routeValues: null);
                string compareUrl = u.Action(actionName: "ComparePlans", controllerName: "InsuranceDefault", routeValues: null);
                string callMeBackUrl = u.Action(actionName: "CallMeBack", controllerName: "InsuranceDefault", routeValues: null);
                string updateCarDetailUrl = u.Action(actionName: "UpdateCarDetail", controllerName: "InsuranceDefault", routeValues: null);
                string filterPlanUrl = u.Action(actionName: "FilterPlans", controllerName: "InsuranceDefault", routeValues: null);
                string sortPlanUrl = u.Action(actionName: "SortPlans", controllerName: "InsuranceDefault", routeValues: null);
                
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
                            compareUrl = compareUrl,
                            callMeBackUrl = callMeBackUrl,
                            updateCarDetailUrl = updateCarDetailUrl,
                            filterPlanUrl = filterPlanUrl,
                            sortPlanUrl = sortPlanUrl,
                            FormData = GetFormDataFromSession_PlanDetails(sData),
                            MasterData = new
                            {
                                Today = DateTime.Today,
                                ErrMsgs = new
                                {
                                    MaxOptionsSelectedMsgTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.QuoteAndBuyDictionary_Name, "max-number-of-options-selected-msg-title"),
                                    MaxOptionsSelectedMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.QuoteAndBuyDictionary_Name, "max-number-of-options-selected-msg"),
                                    NameTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Name"),
                                    NameRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "name-required-message"),
                                    EmailTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "E-mail"),
                                    EmailRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "email-required-message"),
                                    EmailInvalidMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "email-invalid-message"),
                                    PhoneTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Phone Number"),
                                    PhoneRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "phone-number-required-message"),
                                    LineTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Line ID"),
                                    LineRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "line-id-required-message"),
                                    CallbackTimeTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "callback-time-label"),
                                    CallbackTimeRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "callback-time-required-message"),
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

            return View("~/Areas/EasyCompare/Views/InsuranceQuote/InsurancePlans.cshtml");

        }

        private object GetFormDataFromSession_PlanDetails(ApiPolicyHeader sData)
        {
            var asset = new
            {
                carMake = sData.CarAsset.CarMake,
                carModel = sData.CarAsset.CarModel,
                carYear = sData.CarAsset.YearOfRegistration,
                carClass = sData.CarAsset.CarClassName
            };

            var filteredPlan = carRepository.GetFilteredPlan(sData); ;

            var plans = new List<object>();

            foreach (var plan in filteredPlan)
            {
                plans.Add(carRepository.GetPagePlanData(plan));
            }

            var quote = new
            {
                asset = asset,
                plans = plans,
                selectedPlans = sData.ComparePlans,
                filterOption = sData.CarPlanOptionFilter,
                sortOption = new
                {
                    Budget = sData.BudgetSortDirection.ToString(),
                    Coverage = sData.CoverageSortDirection.ToString()
                }
            };

            return quote;
        }

        private object GetFormDataFromSession_ComparePlans(ApiPolicyHeader sData)
        {
            var asset = new
            {
                carMake = sData.CarAsset.CarMake,
                carModel = sData.CarAsset.CarModel,
                carYear = sData.CarAsset.YearOfRegistration,
                carClass = sData.CarAsset.CarClassName
            };

            var filter = sData.CarPlanOptionFilter;

            var plans = new List<object>();

            foreach (var plan in sData.CarRates)
            {
                plan.IsIncludeCompulsory = (filter.IsIncludeCompulsory ?? false);

                if (sData.ComparePlans.Contains(plan.Id))
                {
                    var tempPlan = carRepository.GetPagePlanData(plan);
                    plans.Add(tempPlan);
                }
            }

            var quote = new
            {
                asset = asset,
                plans = plans,
            };

            return quote;
        }

        public ActionResult CompareInsurancePlansDetails()
        {
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
                string buyUrl = u.Action(actionName: "BuyPolicy", controllerName: "InsuranceDefault", routeValues: null);
                string addMoreCallBackUrl = u.Action(actionName: "AddMoreLinks", controllerName: "InsuranceDefault", routeValues: null);
                string callMeBackUrl = u.Action(actionName: "CallMeBack", controllerName: "InsuranceDefault", routeValues: null);
                string updateCarDetailUrl = u.Action(actionName: "UpdateCarDetail", controllerName: "InsuranceDefault", routeValues: null);

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

                        var addMoreUrl = this.Url.Content(string.Format("{0}?{1}={2}", setting.MotorCarPlanDetailUrl, ApiConstants.QueryStringParamNames.PM, pm));

                        var service = new CommonService();

                        var pageData = new
                        {
                            SKey = sKey,
                            buyUrl = buyUrl,
                            addMoreUrl = addMoreUrl,
                            addMoreCallBackUrl = addMoreCallBackUrl,
                            callMeBackUrl = callMeBackUrl,
                            updateCarDetailUrl = updateCarDetailUrl,
                            FormData = GetFormDataFromSession_ComparePlans(sData),
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
                                    PhoneTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Phone Number"),
                                    PhoneRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "phone-number-required-message"),
                                    LineTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Line ID"),
                                    LineRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "line-id-required-message"),
                                    CallbackTimeTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "callback-time-label"),
                                    CallbackTimeRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "callback-time-required-message"),
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

            return View("~/Areas/EasyCompare/Views/Components/CompareInsurancePlansDetails.cshtml");
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult BuyPolicy(PlansOptionViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.PolicyPlanDetail;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyPlanDetail;
                sData.SelectedPlanId = form.PlanId;

                sData.PortalStages.Add(MotorConstants.PortalSteps.PolicyDetail);

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
                    redirectUrl = this.Url.Content(settings.MotorCarPolicyDetailUrl + "?" + response.QueryStringParam),
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
        public ActionResult ComparePlans(PlansOptionViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.PolicyComparison;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyPlanDetail;

                sData.PortalStages.Add(MotorConstants.PortalSteps.PolicyComparison);

                sData.ComparePlans = form.ComparePlans;

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
                    redirectUrl = this.Url.Content(settings.MotorCarPlanComparisonUrl + "?" + response.QueryStringParam),
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
        public ActionResult UpdateCarDetail(PlansOptionViewModel form)
        {
            try
            {
                this.qbRepository.ValidateInputString(form.SKey);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.CarDetails;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyPlanDetail;

                sData.PortalStages.Add(MotorConstants.PortalSteps.CarDetails);

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
        public ActionResult FilterPlans(PlansOptionFilterViewModel form)
        {
            try
            {
                this.qbRepository.ValidateInputString(form.SKey);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);
                sData.CarPlanOptionFilter = form.Filter;

                var filteredPlan = carRepository.GetFilteredPlan(sData);

                var plans = new List<object>();

                foreach (var plan in filteredPlan)
                {
                    plans.Add(carRepository.GetPagePlanData(plan));
                }

                sData.ComparePlans = new List<string>();
                
                policyService.SavePolicyToSession(new AutoGeneralTH.ApiModel.ApiRequest() { ProductName = MotorConstants.MotorProduct.ProductName, PolicyHeader = sData });

                var result = new
                {
                    status = "S",
                    plans = plans
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
        public ActionResult SortPlans(PlansOptionSortViewModel form)
        {
            try
            {
                this.qbRepository.ValidateInputString(form.SKey);
                this.qbRepository.ValidateInputString(form.Budget);
                this.qbRepository.ValidateInputString(form.Coverage);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.BudgetSortDirection = (ApiSortDirection)Enum.Parse(typeof(ApiSortDirection), form.Budget);
                sData.CoverageSortDirection = (ApiSortDirection) Enum.Parse(typeof(ApiSortDirection), form.Coverage);

                var filteredPlan = carRepository.GetFilteredPlan(sData);

                var plans = new List<object>();

                foreach (var plan in filteredPlan)
                {
                    plans.Add(carRepository.GetPagePlanData(plan));
                }
               
                policyService.SavePolicyToSession(new AutoGeneralTH.ApiModel.ApiRequest() { ProductName = MotorConstants.MotorProduct.ProductName, PolicyHeader = sData });

                var result = new
                {
                    status = "S",
                    plans = plans,
                    sortOption = new
                    {
                        Budget = sData.BudgetSortDirection.ToString(),
                        Coverage = sData.CoverageSortDirection.ToString()
                    }
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(qbRepository.HandleException(ex));
            }
        }

        
        private void ValidateFormInput(PlansOptionViewModel form)
        {
            this.qbRepository.ValidateInputString(form.PlanId);

            if (form.ComparePlans != null)
            {
                foreach (var s in form.ComparePlans)
                {
                    this.qbRepository.ValidateInputString(s);
                }
            }
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult AddMoreLinks(PlansOptionViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.PolicyPlanDetail;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyComparison;

                sData.ComparePlans = form.ComparePlans;

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


        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult CallMeBack(CallMeBackViewModel form)
        {
            try
            {
                this.ValidateCallMeBackFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                sData.PortalStage = MotorConstants.PortalSteps.ThankYou;
                sData.CallerStage = MotorConstants.PortalSteps.PolicyDetail;
                sData.PortalStages.Add(MotorConstants.PortalSteps.ThankYou);


                //validate contact...
                var apiContact = new ApiContact()
                {
                    FirstName = form.Name,
                    PreferredCallBackTime = form.CallBackTime,
                };

                switch (form.Option)
                {
                    case Constants.CallMeBackOptions.Email:
                        apiContact.Email = form.OptionValue;
                        break;

                    case Constants.CallMeBackOptions.Line:
                        apiContact.LineId = form.OptionValue;
                        break;

                    case Constants.CallMeBackOptions.Phone:
                        apiContact.ContactNumber = form.OptionValue;
                        break;
                }

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

        private void ValidateCallMeBackFormInput(CallMeBackViewModel form)
        {
            this.qbRepository.ValidateInputString(form.Name);
            this.qbRepository.ValidateInputString(form.Option);
            this.qbRepository.ValidateInputString(form.OptionValue);
            this.qbRepository.ValidateInputString(form.SKey);
            this.qbRepository.ValidateInputString(form.CallBackTime);
        }
    }
}