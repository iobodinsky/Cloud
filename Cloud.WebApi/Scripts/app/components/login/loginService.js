window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || { };

window.cloud.services.loginService = function ($state, httpService,
    userTokenService, alertService, constants) {
    function login(loginData) {
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

        var requestData = 'grant_type=password&username=' + loginData.userName +
            '&password=' + loginData.userPassword;

        httpService.makeRequest( constants.httpMethod.post, constants.urls.cloud.token,
            requestHeaders, requestData, success, error);
    };

    return {
        login: login
    }
};