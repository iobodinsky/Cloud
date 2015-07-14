window.cloud = window.cloud || {};

window.cloud.directives = window.cloud.directives || {};

window.cloud.directives.requiredDigitDirective = function() {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function(scope, element, attributes, ngModel) {
            ngModel.$validators.requiredDigit = function() {
                if (ngModel.$viewValue) {
                    for (var i = 0; i < ngModel.$viewValue.length; i++) {
                        var modelChar = ngModel.$viewValue.charAt(i);
                        if (modelChar >= '0' && modelChar <= '9') return true;
                    }

                    return false;
                }

                return false;
            };
        }
    };
};