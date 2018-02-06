
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
    public class ECCarDetailsController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        private ECQuoteAndBuyRepository qbRepository;
        private ECCarRepository carRepository;

        public ECCarDetailsController()
        {
            _sitecoreContext = new SitecoreContext();
            qbRepository = new ECQuoteAndBuyRepository();
            carRepository = new ECCarRepository(qbRepository);

        }
        // GET: InsuranceQuote
        public ActionResult GetCarDetails()
        {
            try
            {
                string pm = Request.QueryString[ApiConstants.QueryStringParamNames.PM];
                var setting = this.qbRepository.GetQuoteAndBuySettings();

                //if (!qbRepository.IsLocalDev && !Sitecore.Context.PageMode.IsExperienceEditor && !Sitecore.Context.PageMode.IsPreview)
                //{
                //    if (string.IsNullOrEmpty(pm))
                //    {
                //        return new RedirectResult(setting.MotorCarLandingPageUrl);
                //    }
                //}

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

                        var makeModels = commonService.GetCarMakeModels(null);

                        var topMakes = Translate.TextByDomain(Constants.QuoteAndBuyDictionary_Name, "ec-car-top-popular-makes");
                        var topMakeList = new List<object>();

                        if (!string.IsNullOrEmpty(topMakes))
                        {
                            var makesArray = topMakes.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var s in makesArray)
                            {
                                var temp = s.Trim().ToLower();
                                var objMake = makeModels.MakeList.FirstOrDefault(x => x.MakeName.Trim().ToLower() == temp);
                                if (objMake != null)
                                {
                                    topMakeList.Add(new
                                    {
                                        id = objMake.MakeId,
                                        name = objMake.MakeName
                                    });
                                }
                            }
                        }


                        string quoteUrl = u.Action(actionName: "UpdateQuote", controllerName: "ECCarDetails", routeValues: null);

                        var yearList = commonService.GetCarPurchasingYearList();

                        var pageData = new
                        {
                            SKey = sKey,
                            quoteUrl = quoteUrl,
                            FormData = GetFormDataFromSession(sData),
                            MasterData = new
                            {
                                Makes = makeModels.MakeList,
                                Models = makeModels.ModelList,
                                FixedMakeList = topMakeList,
                                PurchasingYear = yearList,
                                YearRangeList = qbRepository.GetYearRangeList(yearList),
                                Today = qbRepository.GetJsDateString(DateTime.Today)
                            }
                        };

                        this.ViewBag.pageConfigData = qbRepository.GetJsonStringTrimBlank(pageData);
                    }
                }
                else
                {
                    var pageData = new
                    {
                        isNew = true,
                        MasterData = new
                        {
                            Today = qbRepository.GetJsDateString(DateTime.Today)
                        }
                    };

                    //this.ViewBag.NgControllerName = "ecCarDetailController";
                    this.ViewBag.pageConfigData = qbRepository.GetJsonStringTrimBlank(pageData);
                }
            }
            catch (Exception ex)
            {
                this.qbRepository.HandleException(ex, true);
            }

            return View("~/Areas/EasyCompare/Views/Components/CarDetailsEdit.cshtml");
        }

        private object GetFormDataFromSession(ApiPolicyHeader sData)
        {
            var seletedPlan = sData.CarRates.FirstOrDefault(x => x.Id == sData.SelectedPlanId);

            var asset = new
            {
                make = sData.CarAsset.CarMake,
                makeId = sData.CarAsset.CarMakeId,
                model = sData.CarAsset.CarModel,
                modelId = sData.CarAsset.CarModelId,
                carYear = sData.CarAsset.YearOfRegistration,
                insuranceType = sData.CarAsset.CarClassName,
                insuranceTypeCode = sData.CarAsset.CarClass
            };

            var quote = new
            {
                asset = asset,
            };

            return quote;
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult UpdateQuote(CarDetailsViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var sData = policyService.GetPolicyFromSession(MotorConstants.MotorProduct.ProductName, form.SKey);

                this.PopulateAssetInfo(sData, form);

                var request = new AutoGeneralTH.ApiModel.ApiRequest()
                {
                    ProductName = MotorConstants.MotorProduct.ProductName,
                    PolicyHeader = sData,
                };

                sData.PortalStage = MotorConstants.PortalSteps.PolicyPlanDetail;
                sData.CallerStage = MotorConstants.PortalSteps.CarDetails;

                var response = policyService.PremiumCalculate(request);

                var settings = qbRepository.GetQuoteAndBuySettings();

                var result = new
                {
                    status = "S",
                    redirectUrl = this.Url.Content(settings.MotorCarPlanDetailUrl + "?" + response.QueryStringParam)
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(qbRepository.HandleException(ex));
            }
        }

        private void PopulateAssetInfo(ApiPolicyHeader sData, CarDetailsViewModel form)
        {
            sData.CarAsset.CarMakeId = form.MakeId;
            sData.CarAsset.CarModelId = form.ModelId;
            sData.CarAsset.CarMake = form.Make;
            sData.CarAsset.CarModel = form.Model;
            sData.CarAsset.YearOfRegistration = form.CarYear;
            sData.CarAsset.CarClass = form.InsuranceTypeCode;
            sData.CarAsset.CarClassName = form.InsuranceType;
        }

        private void ValidateFormInput(CarDetailsViewModel form)
        {
            this.qbRepository.ValidateInputString(form.Make);
            this.qbRepository.ValidateInputString(form.Model);
            this.qbRepository.ValidateInputString(form.InsuranceType);
            this.qbRepository.ValidateInputString(form.InsuranceTypeCode);
        }
    }
}