
(function (angular) {
    'use strict';
    angular.module('easyCompare')
        .controller('ecCarThankYouController', [
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
                            
                            $scope.ph = $scope.quote.ph;
                        }
        ]);
})(window.angular);