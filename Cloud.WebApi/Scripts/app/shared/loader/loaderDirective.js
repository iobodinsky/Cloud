window.cloud = window.cloud || {};

window.cloud.directives = window.cloud.directives || {};

window.cloud.directives.loaderDirective = function(loaderService, constants) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: constants.viewTemplatePath.loader,
        link: function($scope) {
            $scope.loaders = loaderService.loaders;
        }
    };
};