window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.registerController = cloud.controllers.registerController ||
    function($scope, httpService, constants, alertService) {
        $scope.register = function () {
            var registrationData = {
                UserName: $scope.registrationName,
                Email: $scope.registrationEmail,
                Password: $scope.registrationPassword,
                ConfirmPassword: $scope.registrationConfirmPassword
            };

            function success() {
                $scope.userLoginName = registrationData.UserName;
                $scope.userLoginPassword = registrationData.Password;
                $scope.login();
            };

            function error() {
                alertService.show(constants.alert.type.danger,
					constants.message.failRegister);
            };

            var requestHeaders = {};
            requestHeaders[constants.httpHeader.name.contentType] =
				constants.httpHeader.value.json;

            httpService.makeRequest(
				constants.httpMethod.post,
				constants.urls.cloud.register,
				requestHeaders,
				JSON.stringify(registrationData),
				success, error);
        };
    };