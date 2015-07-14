window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.loginController = function($scope, $state,
    userTokenService, loginService, alertService, constants) {
    var self = this;

    self.initialize = function() {
        if (userTokenService.isTokenExist()) $state.go('cloud');
    };
    self.clearLoginFormInputs = function() {
        $scope.loginName = '';
        $scope.loginPassword = '';
        $scope.loginForm.$setPristine();
    };

    $scope.login = function() {
        var loginData = {
            userName: $scope.loginName,
            userPassword: $scope.loginPassword
        };

        function error(data) {
            if (data.error_description) 
                alertService.show(constants.alert.type.danger, data.error_description);
            else
                alertService.show(constants.alert.type.danger, constants.message.failLogin);

            self.clearLoginFormInputs();
        };

        loginService.login(loginData, null, error);
    };

    self.initialize();
};