
(function (angular) {
    'use strict';
    angular.module('easyCompare')
        .controller('ecCarPolicyDetailController', [
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
                            $scope.contact = {};

                            if (angular.isDefined($scope.quote.ph) && $scope.quote.ph != null) {
                                $scope.contact = $scope.quote.ph;
                            }

                            var _isSubscribeModeSelected = function()
                            {
                                if($scope.contact.IsSubscribeEmail == '1')
                                {
                                    return true;
                                }

                                if ($scope.contact.IsSubscribeMail == '1') {
                                    return true;
                                }

                                if ($scope.contact.IsSubscribeSMS == '1') {
                                    return true;
                                }

                                return false;
                            }

                            $scope.confirmQuote = function (form, event) {
                                if (form.$valid) {

                                    if ($scope.contact.IsSubscribeNewsPromo != 'yes' && $scope.contact.IsSubscribeNewsPromo != 'no') {
                                        showAlertMessage($scope.masterData.ErrMsgs.SubscribeTitle, $scope.masterData.ErrMsgs.SubscribeRequiredMsg);
                                        return;
                                    }

                                    if ($scope.contact.IsSubscribeNewsPromo == 'yes') {
                                        $scope.contact.IsSubscribeEmail = '1';
                                        $scope.contact.IsSubscribeMail = '1';
                                        $scope.contact.IsSubscribeSMS = '1';
                                        //if (!_isSubscribeModeSelected()) {
                                        //    showAlertMessage($scope.masterData.ErrMsgs.SubscribeModeTitle, $scope.masterData.ErrMsgs.SubscribeModeRequiredMsg);
                                        //    return;
                                        //}
                                    }
                                    else {
                                        $scope.contact.IsSubscribeEmail = '0';
                                        $scope.contact.IsSubscribeMail = '0';
                                        $scope.contact.IsSubscribeSMS = '0';
                                    }

                                    var requestData = angular.copy($scope.contact);
                                    requestData["SKey"] = $scope.pageConfigData.SKey;

                                    appService.PostData(requestData, $scope.pageConfigData.buyUrl).then(function (results) {
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
                                    else if (form.Email.$error.required) {
                                        showAlertMessage($scope.masterData.ErrMsgs.EmailTitle, $scope.masterData.ErrMsgs.EmailRequiredMsg);
                                    }
                                    else if (form.Email.$error.emailaddress) {
                                        showAlertMessage($scope.masterData.ErrMsgs.EmailTitle, $scope.masterData.ErrMsgs.EmailInvalidMsg);
                                    }
                                }
                            }

                            $scope.changePlan = function (form, event) {
                                var requestData = angular.copy($scope.contact);
                                requestData["SKey"] = $scope.pageConfigData.SKey;

                                appService.PostData(requestData, $scope.pageConfigData.changePlanUrl).then(function (results) {
                                    console.log(results);
                                    if (results.data.status == "S") {
                                        var absUrl = results.data.redirectUrl;
                                        angularRedirect($window, absUrl);
                                    }
                                });
                            }

                            $scope.editCarDetail = function (event) {
                                var requestData = angular.copy($scope.contact);
                                requestData["SKey"] = $scope.pageConfigData.SKey;

                                appService.PostData(requestData, $scope.pageConfigData.updateCarDetailUrl).then(function (results) {
                                    console.log(results);
                                    if (results.data.status == "S") {
                                        var absUrl = results.data.redirectUrl;
                                        angularRedirect($window, absUrl);
                                    }
                                });
                            };

                            $scope.toggleSubSelection = function (str, event) {
                                if ($scope.contact[str] == "1") {
                                    $scope.contact[str] = "0";
                                }
                                else {
                                    $scope.contact[str] = "1";
                                }
                            }
                        }
        ]);
})(window.angular);