window.cloud = window.cloud || {};

window.cloud.directives = cloud.directives || {};

window.cloud.directives.alertDirective = function(alertService, constants) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: constants.viewTemplatePath.alert,
        link: function($scope) {
            $scope.alerts = alertService.alerts;
        }
    };
};