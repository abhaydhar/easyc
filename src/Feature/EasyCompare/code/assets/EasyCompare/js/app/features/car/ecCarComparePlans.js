
(function (angular) {
    'use strict';
    angular.module('easyCompare')
        .controller('ecCarComparePlansController', [
                        '$scope', '$window', '$location', '$timeout', '$rootScope', 'motorCarConstants', 'appService',
                        function ($scope, $window, $location, $timeout, $rootScope, motorCarConstants, appService) {
                            $scope.pageConfigData = $window.pageConfigData;
                            $scope.motorCarConstants = motorCarConstants;
                            $scope.errorMessage = null;
                            $scope.isSubmitting = false;
                            $scope.masterData = $scope.pageConfigData.MasterData;
                            $scope.popupMsg = { title: '', msg: '', redirectUrl: '' };
                            $scope.quote = angular.copy($scope.pageConfigData.FormData);
                            $scope.masterData.Today = new Date($scope.masterData.Today);
                            $scope.isMotorCar = true;
                            $scope.selectedItemChanged = false;
                            $scope.hasPromotion = false;

                            $scope.callback = {
                                option: 'Email',
                            };

                            $scope.init = function()
                            {
                                angular.forEach($scope.quote.plans, function (item, key) {
                                    if(angular.isDefined(item.disPerc) && item.disPerc != '')
                                    {
                                        $scope.hasPromotion = true;
                                    }
                                });
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

                            $scope.removeItem = function(item, event)
                            {
                                var index = $scope.quote.plans.indexOf(item);
                                $scope.quote.plans.splice(index, 1);
                                $scope.selectedItemChanged = true;
                            }

                            $scope.addMoreItem = function(event)
                            {
                                if ($scope.selectedItemChanged) {
                                    var requestData = {
                                        SKey: $scope.pageConfigData.SKey,
                                        ComparePlans: []
                                    };

                                    angular.forEach($scope.quote.plans, function (item, key) {
                                        requestData.ComparePlans.push(item.id);
                                    });

                                    appService.PostData(requestData, $scope.pageConfigData.addMoreCallBackUrl).then(function (results) {
                                        console.log(results);
                                        if (results.data.status == "S") {
                                            var absUrl = results.data.redirectUrl;
                                            angularRedirect($window, absUrl);
                                        }
                                    });
                                }
                                else {
                                    angularRedirect($window, $scope.pageConfigData.addMoreUrl);
                                }
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

                            $scope.init();
                        }
        ]);
})(window.angular);