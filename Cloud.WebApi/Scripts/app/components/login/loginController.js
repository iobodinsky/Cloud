window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.loginController = function($scope, $state,
    userTokenService, loginService, alertService, constants) {
    var self = this;

    self.initialize = function() {
        if (userTokenService.isTokenExist()) $state.go('cloud');
    };
    self.clearUserLoginData = function() {
        $scope.loginName = '';
        $scope.loginPassword = '';
    };

    $scope.login = function() {
        var loginData = {
            userName: $scope.loginName,
            userPassword: $scope.loginPassword
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failLogin);

            self.clearUserLoginData();
        };

        loginService.login(loginData, null, error);
    };

    self.initialize();
};