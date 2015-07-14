window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.registerController = function ($scope, httpService,
    loginService, alertService, constants) {
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
        };

        var requestHeaders = {};
        requestHeaders[constants.httpHeader.name.contentType] =
            constants.httpHeader.value.json;

        httpService.makeRequest(constants.httpMethod.post, constants.urls.cloud.register,
            requestHeaders, JSON.stringify(registrationData), success, error);
    };
};