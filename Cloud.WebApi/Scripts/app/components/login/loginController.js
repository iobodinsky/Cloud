window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.loginController = function($scope, $state, httpService, userTokenService, constants, alertService) {
    var self = this;

    self.initialize = function() {
        if (userTokenService.isTokenExist()) $state.go('cloud');
    };

    $scope.login = function() {
        function success(data) {
            userTokenService.storeToken(data.access_token);
            $state.go('cloud');
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failLogin);
        };

        var requestHeaders = {};
        requestHeaders[constants.httpHeader.name.contentType] =
            constants.httpHeader.value.formUrlencoded;

        var requestData = 'grant_type=password&username=' + $scope.loginName +
            '&password=' + $scope.loginPassword;

        httpService.makeRequest(
            constants.httpMethod.post,
            constants.urls.cloud.token,
            requestHeaders,
            requestData,
            success, error);
    };

    self.initialize();
};