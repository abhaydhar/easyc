
(function (angular) {
    'use strict';
    angular.module('easyCompare')
        .controller('ecCarLandingController', [
                        '$scope', '$window', '$location', '$timeout', '$rootScope', 'motorCarConstants', 'appService',
                        function ($scope, $window, $location, $timeout, $rootScope, motorCarConstants, appService) {
                            $scope.pageConfigData = $window.pageConfigData;
                            $scope.motorCarConstants = motorCarConstants;
                            $scope.errorMessage = null;
                            $scope.isSubmitting = false;
                            $scope.masterData = {};
                            $scope.popupMsg = { title: '', msg: '', redirectUrl: '' };
                            $scope.quote = angular.copy($scope.pageConfigData);
                            $scope.masterData.Today = null;
                            $scope.isMotorCar = true;
                            $scope.car = {};
                            $scope.filteredMakeList = [];
                            $scope.filteredModelList = [];
                            $scope.filteredYearList = [];
                            $scope.fixedMakeList = [];
                            $scope.yearRangeMode = true;
                            $scope.yearRangeList = [];

                            $scope.alphabetList = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('');

                            var _initLoadingBarTimeout = null;
                            var isCallBackDone = false;

                            $scope.init = function () {
                                if ($scope.pageConfigData && $scope.pageConfigData.carConfigUrl) {
                                    var requestData = {};
                                    isCallBackDone = false;
                                    _showInitLoadingBar();

                                    appService.PostData(requestData, $scope.pageConfigData.carConfigUrl).then(function (results) {
                                        console.log(results);
                                        isCallBackDone = true;

                                        if (_initLoadingBarTimeout != null) {
                                            $timeout.cancel(_initLoadingBarTimeout);
                                        }

                                        togglerLoadingOverlay(false);

                                        if (results.data.status == "S") {
                                            $scope.masterData = results.data.content;
                                            $scope.masterData.Today = new Date($scope.masterData.Today);
                                            $scope.filteredYearList = angular.copy($scope.masterData.PurchasingYear);
                                            $scope.fixedMakeList = $scope.masterData.FixedMakeList;
                                            $scope.yearRangeList = $scope.masterData.YearRangeList;
                                        }
                                    });
                                }
                            };

                            var _showInitLoadingBar = function()
                            {
                                if (isCallBackDone) {
                                    return;
                                }

                                togglerLoadingOverlay(true);

                                if (_initLoadingBarTimeout != null) {
                                    $timeout.cancel(_initLoadingBarTimeout);
                                }

                                _initLoadingBarTimeout = $timeout(function () {
                                    _showInitLoadingBar();
                                }, 100);
                            }

                            $scope.formFilter = { make: '', model: '', carYear: '' };

                            $scope.searchMakeByAlphabet = function (str) {
                                $scope.formFilter.make = str;
                                $scope.filterMake();
                                $timeout(function () { jQuery("#makeFilterInput").focus(); }, 100);
                            };

                            //make selector
                            var timeoutFilterMake = null;
                            $scope.filterMake = function () {

                                if (timeoutFilterMake != null) {
                                    $timeout.cancel(timeoutFilterMake);
                                    timeoutFilterMake = null;
                                }

                                timeoutFilterMake = $timeout(function () {
                                    $scope.filteredMakeList = [];

                                    var filter = $scope.formFilter.make.toLowerCase();

                                    if (filter == '')
                                    {
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

                                $timeout(function () { show('lb_model') }, 10);
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

                                if (!angular.isDefined($scope.car.carYear) || $scope.car.carYear == '' || $scope.car.carYear == 0) {
                                    $timeout(function () { show('lb_year') }, 10);
                                }
                            }

                            //Year selector
                            var timeoutFilterYear = null;

                            $scope.filterYear = function () {

                                if (timeoutFilterYear != null) {
                                    $timeout.cancel(timeoutFilterYear);
                                    timeoutFilterYear = null;
                                }

                                timeoutFilterYear = $timeout(function () {

                                    $scope.filteredYearList = [];
                                    var filter = ($scope.formFilter.carYear + '').toLowerCase();

                                    if (filter == '')
                                    {
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

                            $scope.selectYearRange = function(item)
                            {
                                $scope.filteredYearList = [];
                                var curYear = item.toYear;
                                while(curYear >= item.fromYear)
                                {
                                    $scope.filteredYearList.push(curYear);
                                    curYear--;
                                }
                                $scope.yearRangeMode = false;
                            }

                            $scope.showYearRangeView = function()
                            {
                                $scope.yearRangeMode = true;
                            }

                            $scope.selectYear = function (item) {
                                $scope.car.carYear = item;
                                var displayYear = (item * 1) + motorCarConstants.thaiYearDiff;
                                $scope.car.carYearDisplay = item + ' (' + displayYear + ')';
                                $scope.yearRangeMode = true;
                                closebox("lb_year");

                                if (!angular.isDefined($scope.car.insuranceType) || $scope.car.insuranceType == '') {
                                    $timeout(function () { show('lb_itype') }, 10);
                                }
                            }

                            $scope.selectInsuranceType = function (name, code) {
                                $scope.car.insuranceType = name;
                                $scope.car.insuranceTypeCode = code;
                                closebox("lb_itype");
                            }

                            $scope.getCarQuote = function (form, event) {

                                if (form.$valid) {
                                    var requestData = angular.copy($scope.car);

                                    appService.PostData(requestData, $scope.pageConfigData.quoteUrl).then(function (results) {
                                        console.log(results);
                                        if (results.data.status == "S") {
                                            var absUrl = results.data.redirectUrl;
                                            angularRedirect($window, absUrl);
                                        }
                                    });
                                }
                                else {
                                    __hightlightFormError(form, null, true);
                                    if (form.is_Brand.$invalid)
                                    {
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