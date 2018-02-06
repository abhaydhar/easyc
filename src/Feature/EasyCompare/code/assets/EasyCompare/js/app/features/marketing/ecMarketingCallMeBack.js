
(function (angular) {
    'use strict';
    angular.module('easyCompare')
        .controller('ecMarketingCallMeBackController', [
                        '$scope', '$window', '$location', '$timeout', '$rootScope', 'appService',
                        function ($scope, $window, $location, $timeout, $rootScope, appService) {
                            $scope.pageConfigData = $window.pageConfigData;
                            $scope.portalConstants = $window.portalConstants;
                            $scope.masterData = $scope.pageConfigData.MasterData;
                          
                            $scope.callback = {
                                option: 'Phone',
                            };

                            $scope.init = function () {

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