window.cloud = window.cloud || {};

window.cloud.directives = window.cloud.directives || {};

window.cloud.directives.matchPasswordDirective = function() {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            otherModelValue: '=matchPassword'
        },
        link: function(scope, element, attributes, ngModel) {
            ngModel.$validators.matchPassword = function(modelValue) {
                return modelValue === scope.otherModelValue;
            };
        }
    };
};