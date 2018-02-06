using System.Linq;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models;
using System.Collections.Generic;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Repositories;
using System;
using AutoGeneralTH.ApiProxy;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules;
using AutoGeneralTH.ApiModel.Constants;
using AutoGeneralTH.ApiModel.Policy;
using Sitecore.Globalization;
using Sitecore.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public class InsuranceQuoteController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        private ECQuoteAndBuyRepository qbRepository;
        private ECCarRepository carRepository;

        public InsuranceQuoteController()
        {
            _sitecoreContext = new SitecoreContext();
            qbRepository = new ECQuoteAndBuyRepository();
            carRepository = new ECCarRepository(qbRepository);

        }
        // GET: InsuranceQuote
        public ActionResult Quote()
        {
            //Sitecore.Context.Items.Add("NgControllerName", "ecCarDetailController");

            InsuranceQuoteModel quote = default(InsuranceQuoteModel);
            try
            {

                if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
                {
                    var parameters = WebUtil.ParseUrlParameters(RenderingContext.Current.Rendering["Parameters"]);
                    
                    quote = _sitecoreContext.GetItem<InsuranceQuoteModel>(RenderingContext.Current.Rendering.DataSource);
                    quote.IsFullButtonWidth = parameters["ButtonFull Width"];
                    //Implementation is incomplete and hardcoding to be removed 
                    //quote.BrandList= new List<string>()
                    //{ "Toyota", "Mazda", "Opel" };
                    //quote.ModelList = new List<string>()
                    //{ "Camry", "Corolla", "Cruiser", "Prius", "Yaris", "Fiva"};

                    //quote.InsuranceList = new List<string>()
                    //{ "Type 1", "Type 2", "Type 3", "Type 4", "Type 5"};

                }

                var u = new UrlHelper(this.Request.RequestContext);
                string quoteUrl = u.Action(actionName: "GetQuote", controllerName: "InsuranceQuote", routeValues: null);
                string carConfigUrl = u.Action(actionName: "GetCarConfigData", controllerName: "InsuranceQuote", routeValues: null);

                var pageData = new
                {
                    quoteUrl = quoteUrl,
                    carConfigUrl = carConfigUrl,
                };

                //this.ViewBag.NgControllerName = "ecCarDetailController";
                this.ViewBag.pageConfigData = qbRepository.GetJsonStringTrimBlank(pageData);

            }
            catch (Exception ex)
            {
                this.qbRepository.HandleException(ex, true);
            }

            return View("~/Areas/EasyCompare/Views/InsuranceQuote/InsuranceQuote.cshtml", quote);
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult GetCarConfigData()
        {
            try
            {
                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();
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

                var makeList = new List<object>();

                foreach(var item in makeModels.MakeList)
                {
                    makeList.Add(new
                    {
                        id = item.MakeId,
                        name = item.MakeName
                    });
                }

                var modelList = new List<object>();

                foreach (var item in makeModels.ModelList)
                {
                    modelList.Add(new
                    {
                        id = item.ModelId,
                        name = item.ModelName,
                        mId = item.MakeId,
                    });
                }

                var yearList = commonService.GetCarPurchasingYearList();

                var MasterData = new
                {
                    Makes = makeList,
                    Models = modelList,
                    FixedMakeList = topMakeList,
                    PurchasingYear = yearList,
                    YearRangeList =  qbRepository.GetYearRangeList(yearList),
                    Today = qbRepository.GetJsDateString(DateTime.Today)
                };

                var result = new
                {
                    status = "S",
                    content = MasterData,
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
        public ActionResult GetQuote(CarDetailsViewModel form)
        {
            try
            {
                this.ValidateFormInput(form);

                var policyService = new AutoGeneralTH.ApiProxy.PolicyService();
                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                AutoGeneralTH.ApiModel.Policy.ApiPolicyHeader sData = new AutoGeneralTH.ApiModel.Policy.ApiPolicyHeader()
                {
                    PortalStage = MotorConstants.PortalSteps.LandingPage,
                    CallerStage = MotorConstants.PortalSteps.LandingPage,
                    CarAsset = new AutoGeneralTH.ApiModel.Policy.ApiCarAsset(),
                    BudgetSortDirection = AutoGeneralTH.ApiModel.Common.ApiSortDirection.Ascending,
                    CoverageSortDirection = AutoGeneralTH.ApiModel.Common.ApiSortDirection.Descending
                };

                sData.CarPlanOptionFilter = new ApiCarPlanOptionFilter()
                {
                    IsIncludeCompulsory = false,
                    IsBangkok = true,
                    IsNetwork = true,
                    IsPersonal = true,
                };

                sData.PortalStages = new List<string>();
                sData.PortalStages.Add(MotorConstants.PortalSteps.LandingPage);
                sData.PortalStages.Add(MotorConstants.PortalSteps.PolicyPlanDetail);

                this.PopulateAssetInfo(sData, form);

                var request = new AutoGeneralTH.ApiModel.ApiRequest()
                {
                    ProductName = MotorConstants.MotorProduct.ProductName,
                    PolicyHeader = sData,
                };

                var response = policyService.PremiumCalculate(request);

                this.qbRepository.AddPolicySessionKey(response.SessionKey, Session);
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