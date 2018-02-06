using AutoGeneralTH.ApiModel.Constants;
using AutoGeneralTH.ApiModel.Policy;
using AutoGeneralTH.ApiModel.Portal;
using Celes.Framework.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare.Repositories
{
    public class ECCarRepository
    {
        private ECQuoteAndBuyRepository qbRepository;

        public ECCarRepository(ECQuoteAndBuyRepository _qbRep)
        {
            this.qbRepository = _qbRep;
        }

        public object GetPagePlanData(ApiGetCarRateResponse plan)
        {
            return new
            {
                id = plan.Id,
                iName = qbRepository.IsThaiLanguage ? plan.InsurerName_th : plan.InsurerName,
                iLogo = GetInsurerLogo(plan),
                pcName = plan.PlanCoverName,
                premium = plan.TotalPremiumAfterDiscount,

                hasDis = string.IsNullOrEmpty(plan.DiscountPerc) ? false : true,
                disPerc = plan.DiscountPerc,
                disExpDt = plan.EffectiveEndDate,

                odDeduct = plan.OwnDamageDeductible == -1 ? plan.MaxSumInsured : plan.OwnDamageDeductible,
                odSI = plan.OwnDamageSumInsured == -1 ? plan.MaxSumInsured : plan.OwnDamageSumInsured,
                odFT = plan.OwnDamageFireTheft == -1 ? plan.MaxSumInsured : plan.OwnDamageFireTheft,
                odFlood = plan.OwnDamageFlood == -1 ? plan.MaxSumInsured : plan.OwnDamageFlood,

                isAuthorisedGarage = (plan.DealerGrossPremium > 0),
                garage = (plan.DealerGrossPremium > 0 ? "Y" : (plan.NetworkGrossPremium > 0 ? "N" : "")),
                sDriver = plan.SpecificDriver,
                rAssistance = plan.RoadsideAssistance,
                aResponse = plan.AccidentResponse,

                tpPerson = plan.ThirdPartyLiabilityBodilyInjuryPerson,
                tpAccident = plan.ThirdPartyLiabilityBodilyInjuryAccident,
                tpPDamage = plan.ThirdPartyLiabilityPropertyDamage,

                acPA = plan.AdditionalCoveragePersonalAccident == -1 ? plan.MaxSumInsured : plan.AdditionalCoveragePersonalAccident,
                acME = plan.AdditionalCoverageMedicalExpense == -1 ? plan.MaxSumInsured : plan.AdditionalCoverageMedicalExpense,
                acBB = plan.AdditionalCoverageBailBond == -1 ? plan.MaxSumInsured : plan.AdditionalCoverageBailBond,
                acWayside = plan.AdditionalCoverageFallWayside == -1 ? plan.MaxSumInsured : plan.AdditionalCoverageFallWayside,
                acNoOfDriver = plan.NoOfPassenger == -1 ? plan.MaxSumInsured : plan.NoOfPassenger,

                sumInsured = plan.MaxSumInsured,

                hasCompulsory = plan.IsIncludeCompulsory ? (plan.CMIPrice > 0) : false
            };
        }

        private object GetInsurerLogo(ApiGetCarRateResponse plan)
        {
            var key = string.Format("insurer-logo-{0}", plan.InsurerName.ToLower().Trim());
            return Sitecore.Globalization.Translate.TextByDomain(Constants.QuoteAndBuyDictionary_Name, key);
        }

        public void PopulateApiAssetFromPlan(ApiPolicyHeader sData, ApiGetCarRateResponse selectedPlan)
        {
            sData.CarAsset.DateEntered = DateTime.Now;

            if (selectedPlan != null)
            {
                sData.CarAsset.InsurerName = selectedPlan.InsurerName_th;
                sData.CarAsset.PlanName = selectedPlan.PlanName_th;

                var filter = sData.CarPlanOptionFilter;

                //re-compute premium
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

                sData.CarAsset.GrossPremium = qbRepository.GetPremiumString(selectedPlan.GrossPremium??0);
                sData.CarAsset.NetPremium = qbRepository.GetPremiumString(selectedPlan.NetPremium ?? 0);
                sData.CarAsset.CMIPrice = qbRepository.GetPremiumString(selectedPlan.CMIPrice ?? 0);
                sData.CarAsset.TotalPrice = qbRepository.GetPremiumString(selectedPlan.TotalPremium ?? 0);
                sData.CarAsset.DiscountBroker = qbRepository.GetPremiumString(selectedPlan.TotalPremiumSaving ?? 0);
                sData.CarAsset.TotalPriceSavings = sData.CarAsset.TotalPriceSavings;
                sData.CarAsset.TotalPriceAfterDiscount = qbRepository.GetPremiumString(selectedPlan.TotalPremiumAfterDiscount ?? 0);

                sData.CarAsset.SpecificDriver = qbRepository.GetStringFromBool(selectedPlan.SpecificDriver);
                sData.CarAsset.RoadsideAssistance = qbRepository.GetStringFromBool(selectedPlan.RoadsideAssistance);
                sData.CarAsset.AccidentResponseTime = qbRepository.GetStringFromBool(selectedPlan.AccidentResponse);

                sData.CarAsset.OwnDmgDeductible = qbRepository.GetSumInsuredString(selectedPlan.OwnDamageDeductible, selectedPlan.MaxSumInsured);
                sData.CarAsset.OwnDmgSumInsured = qbRepository.GetSumInsuredString(selectedPlan.OwnDamageSumInsured, selectedPlan.MaxSumInsured);
                sData.CarAsset.OwnDmgFireTheft = qbRepository.GetSumInsuredString(selectedPlan.OwnDamageFireTheft, selectedPlan.MaxSumInsured);
                sData.CarAsset.OwnDmgFlood = qbRepository.GetSumInsuredString(selectedPlan.OwnDamageFlood, selectedPlan.MaxSumInsured);

                sData.CarAsset.TplAccidentInjury = qbRepository.GetSumInsuredString(selectedPlan.ThirdPartyLiabilityBodilyInjuryAccident, null);
                sData.CarAsset.TplPersonInjury = qbRepository.GetSumInsuredString(selectedPlan.ThirdPartyLiabilityBodilyInjuryPerson, null);
                sData.CarAsset.TplPropertyDmg = qbRepository.GetSumInsuredString(selectedPlan.ThirdPartyLiabilityPropertyDamage, null);

                sData.CarAsset.AdditionalPersonalAccident = qbRepository.GetSumInsuredString(selectedPlan.AdditionalCoveragePersonalAccident, selectedPlan.MaxSumInsured);
                sData.CarAsset.AdditionalBailbond = qbRepository.GetSumInsuredString(selectedPlan.AdditionalCoverageBailBond, selectedPlan.MaxSumInsured);
                sData.CarAsset.AdditionalWayside = qbRepository.GetSumInsuredString(selectedPlan.AdditionalCoverageFallWayside, selectedPlan.MaxSumInsured);
                sData.CarAsset.AddtionalMedicalExpenses = qbRepository.GetSumInsuredString(selectedPlan.AdditionalCoverageMedicalExpense, selectedPlan.MaxSumInsured);
                sData.CarAsset.PersonCoveredNo = CommonHelper.GetString(selectedPlan.NoOfPassenger);

                sData.CarAsset.SumInsured = qbRepository.GetSumInsuredString(selectedPlan.MaxSumInsured, null);

                sData.CarAsset.IsCompulsory = selectedPlan.IsIncludeCompulsory ? "yes" : "no";
                sData.CarAsset.SpecificDriver = selectedPlan.SpecificDriver ? "yes" : "no";
                sData.CarAsset.RoadsideAssistance = selectedPlan.RoadsideAssistance ? "yes" : "no";
                sData.CarAsset.AccidentResponseTime = selectedPlan.AccidentResponse ? "yes" : "no";
                sData.CarAsset.CarUsage = selectedPlan.Usage.ToLower();

                if (selectedPlan.DealerGrossPremium > 0 || selectedPlan.NetworkGrossPremium > 0)
                {
                    if (selectedPlan.NetworkGrossPremium > 0)
                    {
                        sData.CarAsset.NetworkGarageOrDealer = MotorConstants.NetworkGarageOrDealer.Network;
                    }

                    if (selectedPlan.DealerGrossPremium > 0)
                    {
                        sData.CarAsset.NetworkGarageOrDealer = MotorConstants.NetworkGarageOrDealer.Garage;
                    }
                }

                sData.CarAsset.PromoName = selectedPlan.PromoName;
                sData.CarAsset.PromoName_th = selectedPlan.PromoName_th;
                sData.CarAsset.PromoDetails = selectedPlan.PromoDetails_1;
                sData.CarAsset.PromoDetails_th = selectedPlan.PromoDetails_1_th;

                sData.CarAsset.Benefits_1 = selectedPlan.Benefits_1;
                sData.CarAsset.Benefits_1_th = selectedPlan.Benefits_1_th;
                sData.CarAsset.Benefits_2 = selectedPlan.Benefits_2;
                sData.CarAsset.Benefits_2_th = selectedPlan.Benefits_2_th;

                sData.CarAsset.Conditions_1 = selectedPlan.Conditions_1;
                sData.CarAsset.Conditions_1_th = selectedPlan.Conditions_1_th;
                sData.CarAsset.Conditions_2 = selectedPlan.Conditions_2;
                sData.CarAsset.Conditions_2_th = selectedPlan.Conditions_2_th;
                sData.CarAsset.Conditions_3 = selectedPlan.Conditions_3;
                sData.CarAsset.Conditions_3_th = selectedPlan.Conditions_3_th;

                sData.CarAsset.CarCode = selectedPlan.CarCode;

                if(qbRepository.IsSameStringIgnoreCase(selectedPlan.Country, MotorConstants.CarBangkokUpcontry.Bangkok_SP))
                {
                    sData.CarAsset.CarBangkokUpcontry = MotorConstants.CarBangkokUpcontry.Bangkok;
                }

                if (qbRepository.IsSameStringIgnoreCase(selectedPlan.Country, MotorConstants.CarBangkokUpcontry.Upcountry_SP))
                {
                    sData.CarAsset.CarBangkokUpcontry = MotorConstants.CarBangkokUpcontry.Upcountry;
                }
            }
        }

        public List<ApiGetCarRateResponse> GetFilteredPlan(ApiPolicyHeader sData)
        {
            var planList = sData.CarRates;
            var filteredPlan = planList;

            if (sData.CarPlanOptionFilter != null)
            {
                var filter = sData.CarPlanOptionFilter;

                filteredPlan = new List<ApiGetCarRateResponse>();

                foreach (var plan in planList)
                {
                    if (IsValidPlan(plan, filter))
                    {
                        if(filter.IsIncludeCompulsory ?? false)
                        {
                            plan.IsIncludeCompulsory = true;
                            plan.TotalPremium = plan.GrossPremium + (plan.CMIPrice ?? 0);
                        }
                        else
                        {
                            plan.IsIncludeCompulsory = false;
                            plan.TotalPremium = plan.GrossPremium;
                        }

                        plan.TotalPremiumAfterDiscount = plan.TotalPremium - (plan.DiscountPremium ?? 0);
                        plan.TotalPremiumSaving = plan.DiscountPremium;

                        filteredPlan.Add(plan);
                    }
                }
            }

            switch (sData.BudgetSortDirection)
            {
                case AutoGeneralTH.ApiModel.Common.ApiSortDirection.Descending:
                    filteredPlan = filteredPlan.OrderByDescending(x => x.TotalPremiumAfterDiscount).ToList();
                    //switch (sData.CoverageSortDirection)
                    //{
                    //    case AutoGeneralTH.ApiModel.Common.ApiSortDirection.Descending:
                    //        filteredPlan = filteredPlan.OrderByDescending(x=> x.TotalPremiumAfterDiscount).ThenByDescending(x => x.MaxSumInsured).ToList();
                    //        break;

                    //    case AutoGeneralTH.ApiModel.Common.ApiSortDirection.Ascending:
                    //        filteredPlan = filteredPlan.OrderByDescending(x => x.TotalPremiumAfterDiscount).ThenBy(x => x.MaxSumInsured).ToList();
                    //        break;
                    //}
                    break;

                case AutoGeneralTH.ApiModel.Common.ApiSortDirection.Ascending:
                    filteredPlan = filteredPlan.OrderBy(x => x.TotalPremiumAfterDiscount).ToList();
                    //switch (sData.CoverageSortDirection)
                    //{
                    //    case AutoGeneralTH.ApiModel.Common.ApiSortDirection.Descending:
                    //        filteredPlan = filteredPlan.OrderBy(x => x.TotalPremiumAfterDiscount).ThenByDescending(x => x.MaxSumInsured).ToList();
                    //        break;

                    //    case AutoGeneralTH.ApiModel.Common.ApiSortDirection.Ascending:
                    //        filteredPlan = filteredPlan.OrderBy(x => x.TotalPremiumAfterDiscount).ThenBy(x => x.MaxSumInsured).ToList();
                    //        break;
                    //}
                    break;
            }

            return filteredPlan;
        }

        private bool IsCompulsory(ApiGetCarRateResponse plan)
        {
            return (plan.CMIPrice??0) > 0;
        }

        private bool IsValidPlan(ApiGetCarRateResponse plan, ApiCarPlanOptionFilter filter)
        {
            //if (filter.IsIncludeCompulsory.HasValue)
            //{
            //    if (filter.IsIncludeCompulsory.Value)
            //    {
            //        if (!IsCompulsory(plan))
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        if (IsCompulsory(plan))
            //        {
            //            return false;
            //        }
            //    }
            //}

            //if (filter.IsVoluntary)
            //{
            //    if ((plan.CMIPrice ?? 0) > 0)
            //    {
            //        return false;
            //    }
            //}

            if(!filter.IsPersonal || !filter.IsCommercial)
            {
                if(filter.IsPersonal)
                {
                    if(!qbRepository.IsSameStringIgnoreCase(plan.Usage, MotorConstants.Usage.Personal))
                    {
                        return false;
                    }
                }

                if(filter.IsCommercial)
                {
                    if (!qbRepository.IsSameStringIgnoreCase(plan.Usage, MotorConstants.Usage.Commercial))
                    {
                        return false;
                    }
                }
            }

            if(!filter.IsBangkok || !filter.IsUpcountry)
            {
                if(filter.IsBangkok)
                {
                    if(!qbRepository.IsSameStringIgnoreCase(plan.Country, MotorConstants.CarBangkokUpcontry.Bangkok_SP))
                    {
                        return false;
                    }
                }

                if (filter.IsUpcountry)
                {
                    if (!qbRepository.IsSameStringIgnoreCase(plan.Country, MotorConstants.CarBangkokUpcontry.Upcountry_SP))
                    {
                        return false;
                    }
                }
            }

            if(!filter.IsNetwork || !filter.IsDealer)
            {
                if(filter.IsNetwork)
                {
                    if ((plan.NetworkGrossPremium ?? 0) == 0)
                    {
                        return false;
                    }
                }

                if (filter.IsDealer)
                {
                    if ((plan.DealerGrossPremium ?? 0) == 0)
                    {
                        return false;
                    }
                }
            }

            //if (filter.Garage)
            //{
            //    if (plan.NetworkGrossPremium > 0)
            //    {
            //        return false;
            //    }
            //}

            if (filter.Deductible)
            {
                if (plan.OwnDamageDeductible == 0)
                {
                    return false;
                }
            }

            if (filter.Flood)
            {
                if (plan.OwnDamageFlood == 0)
                {
                    return false;
                }
            }

            if (filter.FireTheft)
            {
                if (plan.OwnDamageFireTheft == 0)
                {
                    return false;
                }
            }

            if (filter.PersonalAccident)
            {
                if (plan.AdditionalCoveragePersonalAccident == 0)
                {
                    return false;
                }
            }

            if (filter.MedicalExpense)
            {
                if (plan.AdditionalCoverageMedicalExpense == 0)
                {
                    return false;
                }
            }

            if (filter.BailBond)
            {
                if (plan.AdditionalCoverageBailBond == 0)
                {
                    return false;
                }
            }

            //if (filter.Wayside)
            //{
            //    if (plan.AdditionalCoverageFallWayside == 0)
            //    {
            //        return false;
            //    }
            //}

            if((filter.SumInsured??0) > 0)  
            {
                if(plan.MinSumInsured > filter.SumInsured)
                {
                    return false;
                }

                if(plan.MaxSumInsured < filter.SumInsured)
                {
                    return false;
                }
            }

            return true;
        }

    }
}