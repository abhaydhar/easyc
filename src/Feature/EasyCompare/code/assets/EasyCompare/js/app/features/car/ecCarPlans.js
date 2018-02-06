
(function (angular) {
    'use strict';
    angular.module('easyCompare')
        .controller('ecCarPlansController', [
                        '$scope', '$window', '$location', '$timeout', '$rootScope', 'motorCarConstants', 'appService',
                        function ($scope, $window, $location, $timeout, $rootScope, motorCarConstants, appService) {
                            $scope.pageConfigData = $window.pageConfigData;
                            $scope.portalConstants = $window.portalConstants;
                            $scope.motorCarConstants = motorCarConstants;
                            $scope.errorMessage = null;
                            $scope.isSubmitting = false;
                            $scope.masterData = $scope.pageConfigData.MasterData;
                            $scope.popupMsg = { title: '', msg: '', redirectUrl: '' };
                            $scope.quote = angular.copy($scope.pageConfigData.FormData);
                            $scope.masterData.Today = new Date($scope.masterData.Today);
                            $scope.isMotorCar = true;
                            $scope.viewItem = {};
                            $scope.selectedItems = [];

                            $scope.callback = {
                                option: 'Email',
                            };

                            $scope.filterOption = {
                                IsIncludeCompulsory: null,
                                IsPersonal: false,
                                IsCommercial: false,
                                IsBangkok: false,
                                IsUpcountry: false,
                                IsNetwork: false,
                                IsDealer: false,
                                Garage: false,
                                Deductible: false,
                                Flood: false,
                                FireTheft: false,
                                PersonalAccident: false,
                                MedicalExpense: false,
                                BailBond: false,
                                Wayside: false,
                                SumInsured: null,
                            };

                            $scope.sortOption = {
                            };

                            $scope.init = function () {
                                if (angular.isDefined($scope.quote.selectedPlans) && $scope.quote.selectedPlans.length > 0) {
                                    angular.forEach($scope.quote.plans, function (item, key) {
                                        angular.forEach($scope.quote.selectedPlans, function (id, key) {
                                            if (item.id == id) {
                                                item["selected"] = true;
                                                $scope.selectedItems.push(item);
                                            }
                                        });
                                    });
                                }

                                if (angular.isDefined($scope.quote.filterOption) && $scope.quote.filterOption != null) {
                                    $scope.filterOption = angular.copy($scope.quote.filterOption);
                                }

                                if (angular.isDefined($scope.quote.sortOption) && $scope.quote.sortOption != null) {
                                    $scope.sortOption = angular.copy($scope.quote.sortOption);
                                }

                                if($scope.selectedItems != null && $scope.selectedItems.length > 0)
                                {
                                    showCompareBar();
                                }
                            }

                            $scope.selectItem = function (item, event) {

                                if (isMobileBrowser()) {
                                    if ($scope.selectedItems.length >= motorCarConstants.maxNumOfPlansForCompareMobile) {
                                        showAlertMessage($scope.masterData.ErrMsgs.MaxOptionsSelectedMsgTitle, $scope.masterData.ErrMsgs.MaxOptionsSelectedMsg);
                                        return;
                                    }
                                }
                                else {
                                    if ($scope.selectedItems.length >= motorCarConstants.maxNumOfPlansForCompare) {
                                        showAlertMessage($scope.masterData.ErrMsgs.MaxOptionsSelectedMsgTitle, $scope.masterData.ErrMsgs.MaxOptionsSelectedMsg);
                                        return;
                                    }
                                }

                                item["selected"] = true;
                                $scope.selectedItems.push(item);
                                showCompareBar();
                            }

                            var showCompareBar = function()
                            {
                                //Original code from TH-99 Compare bar to show on first card selection
                                /*
                                if (document.getElementById("cs_hide").style.display == "") {
                                    document.getElementById("cs_hide").style.display = "flex";
                                    document.getElementById("cardselection-sticky-wrapper").height = "50px";
                                    document.getElementById("cardselection-sticky-wrapper").height = "222px";
                                    $(window).scrollTop($(window).scrollTop() + 1);
                                }
                                */

                                jQuery("#cs_hide").css("display", "flex");
                                var wrapper = $("#cardselection-sticky-wrapper");
                                wrapper.height(220);
                                jQuery(window).scrollTop(jQuery(window).scrollTop() + 1);
                            }

                            $scope.removeItem = function (item, event) {
                                item["selected"] = false;

                                var index = $scope.selectedItems.indexOf(item);
                                $scope.selectedItems.splice(index, 1);
                            }

                            $scope.buyPolicy = function (item, event) {
                                var requestData = {
                                    SKey: $scope.pageConfigData.SKey,
                                    PlanId: item.id
                                };

                                appService.PostData(requestData, $scope.pageConfigData.buyUrl).then(function (results) {
                                    console.log(results);
                                    if (results.data.status == "S") {
                                        var absUrl = results.data.redirectUrl;
                                        angularRedirect($window, absUrl);
                                    }
                                });
                            }

                            $scope.comparePlan = function (event) {
                                var requestData = {
                                    SKey: $scope.pageConfigData.SKey,
                                    ComparePlans: []
                                };

                                angular.forEach($scope.selectedItems, function (item, key) {
                                    requestData.ComparePlans.push(item.id);
                                });

                                appService.PostData(requestData, $scope.pageConfigData.compareUrl).then(function (results) {
                                    console.log(results);
                                    if (results.data.status == "S") {
                                        var absUrl = results.data.redirectUrl;
                                        angularRedirect($window, absUrl);
                                    }
                                });
                            }

                            $scope.editCarDetail = function (event) {
                                var requestData = {
                                    SKey: $scope.pageConfigData.SKey,
                                };

                                appService.PostData(requestData, $scope.pageConfigData.updateCarDetailUrl).then(function (results) {
                                    console.log(results);
                                    if (results.data.status == "S") {
                                        var absUrl = results.data.redirectUrl;
                                        angularRedirect($window, absUrl);
                                    }
                                });
                            };

                            $scope.callMeBack = function (form, event) {
                                if (form.$valid) {
                                    var requestData = {
                                        SKey: $scope.pageConfigData.SKey,
                                        Name: $scope.callback.name,
                                        Option: $scope.callback.option,
                                        OptionValue: $scope.callback.optionVal,
                                        CallBackTime: $scope.callback.callBackTime,
                                    };

                                    appService.PostData(requestData, $scope.pageConfigData.callMeBackUrl).then(function (results) {
                                        console.log(results);
                                        if (results.data.status == "S") {
                                            var absUrl = results.data.redirectUrl;
                                            angularRedirect($window, absUrl);
                                        }
                                    });
                                }
                                else {
                                    __hightlightFormError(form, null, true);

                                    if (form.FirstName.$error.required) {
                                        showAlertMessage($scope.masterData.ErrMsgs.NameTitle, $scope.masterData.ErrMsgs.NameRequiredMsg);
                                    }
                                    else if (form.Email && form.Email.$error && form.Email.$error.required) {
                                        showAlertMessage($scope.masterData.ErrMsgs.EmailTitle, $scope.masterData.ErrMsgs.EmailRequiredMsg);
                                    }
                                    else if (form.Email && form.Email.$error && form.Email.$error.emailaddress) {
                                        showAlertMessage($scope.masterData.ErrMsgs.EmailTitle, $scope.masterData.ErrMsgs.EmailInvalidMsg);
                                    }
                                    else if (form.Phone && form.Phone.$error && form.Phone.$error.required) {
                                        showAlertMessage($scope.masterData.ErrMsgs.PhoneTitle, $scope.masterData.ErrMsgs.PhoneRequiredMsg);
                                    }
                                    else if (form.Line && form.Line.$error && form.Line.$error.required) {
                                        showAlertMessage($scope.masterData.ErrMsgs.LineTitle, $scope.masterData.ErrMsgs.LineRequiredMsg);
                                    }
                                    else if (form.callBackTime.$error.required) {
                                        showAlertMessage($scope.masterData.ErrMsgs.CallbackTimeTitle, $scope.masterData.ErrMsgs.CallbackTimeRequiredMsg);
                                    }
                                }
                            }

                            $scope.selectAllInsurance = function(event)
                            {
                                $scope.filterOption.IsCompulsory = true;
                                $scope.filterOption.IsVoluntary = true;
                            }

                            /*
                            $scope.selectCompulsory = function (event) {
                                $scope.filterOption.IsCompulsory = !$scope.filterOption.IsCompulsory;
                                //$scope.filterOption.IsVoluntary = false;
                            }

                            $scope.selectVoluntary = function (event) {
                                //$scope.filterOption.IsCompulsory = false;
                                $scope.filterOption.IsVoluntary = !$scope.filterOption.IsVoluntary;
                            }
                            */

                            $scope.selectAllUsage = function(event)
                            {
                                $scope.filterOption.IsPersonal = true;
                                $scope.filterOption.IsCommercial = true;
                            }

                            /*
                            $scope.selectPersonalUsage = function (event) {
                                $scope.filterOption.IsPersonal = !$scope.filterOption.IsPersonal;
                            }

                            $scope.selectCommercialUsage = function (event) {
                                $scope.filterOption.IsCommercial = !$scope.filterOption.IsCommercial;
                            }
                            */

                            $scope.selectAllRegistration = function(event)
                            {
                                $scope.filterOption.IsBangkok = true;
                                $scope.filterOption.IsUpcountry = true;
                            }

                            $scope.selectAllGarageType = function (event) {
                                $scope.filterOption.IsNetwork = true;
                                $scope.filterOption.IsDealer = true;
                            }

                            $scope.toggleSelection = function (str, event) {
                                $scope.filterOption[str] = !$scope.filterOption[str];
                            }

                            $scope.filterPlans = function (event) {
                                var requestData = {
                                    SKey: $scope.pageConfigData.SKey,
                                    Filter: angular.copy($scope.filterOption)
                                };

                                appService.PostData(requestData, $scope.pageConfigData.filterPlanUrl).then(function (results) {
                                    console.log(results);
                                    if (results.data.status == "S") {
                                        $scope.quote.plans = results.data.plans;
                                        $scope.selectedItems = [];
                                        closebox("lb_filter");
                                        _updatePlanLayout();
                                    }
                                });
                            }

                            var _updatePlanLayout = function () {
                                $timeout(function () {
                                    var grid = document.querySelector('.grid');
                                    var msnry = new Masonry(grid);
                                    msnry.layout();
                                }, 10);
                            }

                            $scope.viewPlan = function (item, event) {
                                $scope.viewItem = item;
                                show('lb_details');
                            }

                            $scope.showCompareSection_M = function (event) {

                                jQuery("#mcard_selection_3").hide();
                                jQuery("#mcard_selection_3f").hide();
                                jQuery("#mcard_selection_4").hide();
                                jQuery("#mcard_selection_4f").hide();

                                if (document.getElementById("cardselectionmobile").clientHeight < 80) {
                                    document.getElementById("cardselectionmobile").style.height = "380px";
                                    document.getElementById("marrow").src = "/assets/EasyCompare/img/home/u119.png";
                                } else {

                                    document.getElementById("cardselectionmobile").style.height = "60px";
                                    document.getElementById("marrow").src = "/assets/EasyCompare/img/home/u118.png";
                                }
                            };

                            $scope.sortPlans = function (str, event)
                            {
                                var requestData = angular.copy($scope.sortOption);
                                requestData["SKey"] = $scope.pageConfigData.SKey;

                                if (requestData[str] == portalConstants.Ascending)
                                {
                                    requestData[str] = portalConstants.Descending
                                }
                                else
                                {
                                    requestData[str] = portalConstants.Ascending
                                }

                                appService.PostData(requestData, $scope.pageConfigData.sortPlanUrl).then(function (results) {
                                    console.log(results);
                                    if (results.data.status == "S") {
                                        $scope.quote.plans = results.data.plans;
                                        $scope.sortOption = results.data.sortOption;
                                        _updatePlanLayout();
                                    }
                                });
                            }

                            $scope.init();
                        }
        ]);
})(window.angular);