window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.registerController = function($scope, httpService,
    loginService, alertService, constants) {
    var self = this;

    self.crearRegisterData = function() {
        $scope.registrationName = '';
        $scope.registrationEmail = '';
        $scope.registrationPassword = '';
        $scope.registrationConfirmPassword = '';
    };

    $scope.register = function() {
        var registrationData = {
            UserName: $scope.registrationName,
            Email: $scope.registrationEmail,
            Password: $scope.registrationPassword,
            ConfirmPassword: $scope.registrationConfirmPassword
        };

        function success() {
            var loginData = {
                userName: registrationData.UserName,
                userPassword: registrationData.Password
            }

            loginService.login(loginData);
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failRegister);

            self.crearRegisterData();
        };

        var requestHeaders = {};
        requestHeaders[constants.httpHeader.name.contentType] =
            constants.httpHeader.value.json;

        httpService.makeRequest(constants.httpMethod.post, constants.urls.cloud.register,
            requestHeaders, JSON.stringify(registrationData), success, error);
    };
};