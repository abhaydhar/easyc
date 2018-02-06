
(function (angular) {
    'use strict';
    angular.module('easyCompare')
        .controller('ecCarDetailsController', [
                        '$scope', '$window', '$location', '$timeout', '$rootScope', 'motorCarConstants', 'appService',
                        function ($scope, $window, $location, $timeout, $rootScope, motorCarConstants, appService) {
                            $scope.pageConfigData = $window.pageConfigDataCarDetail;
                            $scope.pageConfigDataQuote = $window.pageConfigData;
                            $scope.motorCarConstants = motorCarConstants;
                            $scope.errorMessage = null;
                            $scope.isSubmitting = false;
                            $scope.masterData = $window.pageConfigDataCarDetail.MasterData;
                            $scope.popupMsg = { title: '', msg: '', redirectUrl: '' };
                            $scope.quote = angular.copy($window.pageConfigDataCarDetail);
                            $scope.masterData.Today = new Date($scope.masterData.Today);
                            $scope.isMotorCar = true;

                            $scope.car = {};
                            $scope.filteredMakeList = [];
                            $scope.filteredModelList = [];
                            $scope.filteredYearList = angular.copy($scope.masterData.PurchasingYear);

                            $scope.yearRangeMode = true;
                            $scope.yearRangeList = angular.copy($scope.masterData.YearRangeList);
                            $scope.alphabetList = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('');

                            $scope.fixedMakeList = $scope.masterData.FixedMakeList;

                            $scope.formFilter = { make: '', model: '', carYear: '' };

                            $scope.searchMakeByAlphabet = function (str) {
                                $scope.formFilter.make = str;
                                $scope.filterMake();
                                $timeout(function () { jQuery("#makeFilterInput").focus(); }, 100);
                            };

                            $scope.init = function () {
                                console.log($scope.pageConfigData);
                                if ($scope.pageConfigData && $scope.pageConfigData.isNew) {
                                    if ($scope.pageConfigDataQuote && $scope.pageConfigDataQuote.carConfigUrl) {
                                        var isCallBackDone = false;
                                        var requestData = {};
                                        appService.PostData(requestData, $scope.pageConfigDataQuote.carConfigUrl).then(function (results) {
                                            console.log(results);
                                            isCallBackDone = true;
                                            if (results.data.status == "S") {
                                                $scope.masterData = results.data.content;
                                                $scope.masterData.Today = new Date($scope.masterData.Today);
                                                $scope.filteredYearList = angular.copy($scope.masterData.PurchasingYear);
                                                $scope.fixedMakeList = $scope.masterData.FixedMakeList;
                                                $scope.yearRangeList = $scope.masterData.YearRangeList;
                                            }
                                        });

                                        $timeout(function () {
                                            if (!isCallBackDone) {
                                                togglerLoadingOverlay(true);
                                            }
                                        }, 1000);
                                    }
                                }
                                else {
                                    $scope.car = angular.copy($scope.quote.FormData.asset);
                                    var displayYear = ($scope.car.carYear * 1) + motorCarConstants.thaiYearDiff;
                                    $scope.car.carYearDisplay = $scope.car.carYear + ' (' + displayYear + ')';
                                    $scope.filterModel();
                                }
                            };

                            //make selector
                            var timeoutFilterMake = null;
                            $scope.filterMake = function () {

                                if (timeoutFilterMake != null) {
                                    $timeout.cancel(timeoutFilterMake);
                                    timeoutFilterMake = null;
                                }

                                timeoutFilterMake = $timeout(function () {

                                    var filter = $scope.formFilter.make.toLowerCase();

                                    $scope.filteredMakeList = [];

                                    if (filter == '') {
                                        return;
                                    }

                                    angular.forEach($scope.masterData.Makes, function (item, key) {
                                        if (item.name.toLowerCase().indexOf(filter) == 0) {
                                            $scope.filteredMakeList.push(item);
                                        }
                                    });
                                }, 10);
                            }

                            $scope.selectMake = function (make) {
                                $scope.car.make = make.name;
                                $scope.car["makeId"] = make.id;

                                $scope.car.model = null;
                                $scope.car.modelId = null;

                                closebox("lb_brand");
                                $scope.formFilter.model = '';
                                $scope.filterModel();
                            }


                            //model selector
                            var timeoutFilterModel = null;

                            $scope.filterModel = function () {

                                if (timeoutFilterModel != null) {
                                    $timeout.cancel(timeoutFilterModel);
                                    timeoutFilterModel = null;
                                }

                                timeoutFilterModel = $timeout(function () {

                                    var makeId = $scope.car.makeId;
                                    var filter = $scope.formFilter.model.toLowerCase();

                                    $scope.filteredModelList = [];
                                    angular.forEach($scope.masterData.Models, function (item, key) {
                                        if (item.mId == makeId && item.name.toLowerCase().indexOf(filter) >= 0) {
                                            $scope.filteredModelList.push(item);
                                        }
                                    });
                                }, 10);
                            }

                            $scope.selectModel = function (model) {
                                $scope.car.model = model.name;
                                $scope.car.modelId = model.id;
                                closebox("lb_model");
                            }

                            //Year selector
                            var timeoutFilterYear = null;

                            $scope.filterYear = function () {

                                if (timeoutFilterYear != null) {
                                    $timeout.cancel(timeoutFilterYear);
                                    timeoutFilterYear = null;
                                }

                                timeoutFilterYear = $timeout(function () {

                                    var filter = ($scope.formFilter.carYear + '').toLowerCase();

                                    $scope.filteredYearList = [];

                                    if (filter == '') {
                                        $scope.showYearRangeView();
                                        return;
                                    }

                                    angular.forEach($scope.masterData.PurchasingYear, function (item, key) {
                                        if ((item + '').toLowerCase().indexOf(filter) == 0 ||
                                            ((item + motorCarConstants.thaiYearDiff) + '').toLowerCase().indexOf(filter) == 0) {
                                            $scope.filteredYearList.push(item);
                                        }
                                    });

                                    $scope.yearRangeMode = false;
                                }, 10);
                            }

                            $scope.selectYearRange = function (item) {
                                $scope.filteredYearList = [];
                                var curYear = item.toYear;
                                while (curYear >= item.fromYear) {
                                    $scope.filteredYearList.push(curYear);
                                    curYear--;
                                }
                                $scope.yearRangeMode = false;
                            }

                            $scope.showYearRangeView = function () {
                                $scope.yearRangeMode = true;
                            }

                            $scope.selectYear = function (item) {
                                $scope.car.carYear = item;
                                var displayYear = (item * 1) + motorCarConstants.thaiYearDiff;
                                $scope.car.carYearDisplay = item + ' (' + displayYear + ')';
                                $scope.yearRangeMode = true;
                                closebox("lb_year");
                            }

                            $scope.selectInsuranceType = function (name, code) {
                                $scope.car.insuranceType = name;
                                $scope.car.insuranceTypeCode = code;
                                closebox("lb_itype");
                            }

                            $scope.getCarQuote = function (form, event) {
                                if (form.$valid) {
                                    if ($scope.pageConfigData.isNew) {
                                        var requestData = angular.copy($scope.car);

                                        appService.PostData(requestData, $scope.pageConfigDataQuote.quoteUrl).then(function (results) {
                                            console.log(results);
                                            if (results.data.status == "S") {
                                                var absUrl = results.data.redirectUrl;
                                                angularRedirect($window, absUrl);
                                            }
                                        });
                                    }
                                    else {
                                        var requestData = angular.copy($scope.car);
                                        requestData["SKey"] = $scope.quote.SKey;
                                        appService.PostData(requestData, $scope.quote.quoteUrl).then(function (results) {
                                            console.log(results);
                                            if (results.data.status == "S") {
                                                var absUrl = results.data.redirectUrl;
                                                angularRedirect($window, absUrl);
                                            }
                                        });
                                    }
                                }
                                else {
                                    __hightlightFormError(form, null, true);

                                    if (form.is_Brand.$invalid) {
                                        show('lb_brand');
                                    }
                                    else if (form.is_Model.$invalid) {
                                        show('lb_model');
                                    }
                                    else if (form.is_Year.$invalid) {
                                        show('lb_year');
                                    }
                                    else if (form.is_InsuranceT.$invalid) {
                                        show('lb_itype');
                                    }
                                }
                            }

                            $scope.init();
                        }
        ]);
})(window.angular);