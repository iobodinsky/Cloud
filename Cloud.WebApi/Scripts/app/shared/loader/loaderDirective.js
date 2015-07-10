window.cloud = window.cloud || {};

window.cloud.directives = window.cloud.directives || {};

window.cloud.directives.loaderDirective = function(loaderService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'scripts/app/shared/loader/loaderView.html',
        link: function($scope) {
            $scope.loaders = loaderService.loaders;
        }
    };
};