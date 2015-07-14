window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.loginController = function ($scope, $state,
    userTokenService, loginService) {
    var self = this;

    self.initialize = function() {
        if (userTokenService.isTokenExist()) $state.go('cloud');
    };

    $scope.login = function () {
        var loginData = {
            userName: $scope.loginName,
            userPassword: $scope.loginPassword
        };

        loginService.login(loginData);
    };

    self.initialize();
};