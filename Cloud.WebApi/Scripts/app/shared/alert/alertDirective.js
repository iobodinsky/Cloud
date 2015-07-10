window.cloud = window.cloud || {};

window.cloud.directives = cloud.directives || {};

window.cloud.directives.alertDirective = function(alertService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'scripts/app/shared/alert/alertView.html',
        link: function($scope) {
            $scope.alerts = alertService.alerts;
        }
    };
};