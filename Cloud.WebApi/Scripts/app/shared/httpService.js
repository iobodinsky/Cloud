window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || { };

window.cloud.services.httpService = function($http, $window, $state, loaderService,
    userTokenService, alertService, constants) {

    function makeRequest(method, url, requestHeaders, requestData,
        successCallback, errorCallback) {
        var request = {
            method: method ? method : constants.httpMethod.get,
            url: url,
            headers: requestHeaders,
            data: requestData
        };

        if (userTokenService.isTokenExist) {
            if (!request.headers) {
                request.headers = {};
                request.headers[constants.httpHeader.name.authorization] =
                    userTokenService.getAuthorizationToken();
            } else {
                if (!request.headers[constants.httpHeader.name.authorization]) {
                    request.headers[constants.httpHeader.name.authorization] =
                        userTokenService.getAuthorizationToken();
                }
            }
        }

        loaderService.show();

        $http(request)
            .success(function(data, status, headers, config) {
                if (successCallback) {
                    successCallback(data, status, headers, config);
                }
                loaderService.remove();
            })
            .error(function(data, status, headers, config) {
                if (status === 401) {
                    $state.go('login');
                    loaderService.remove();

                    return;
                }

                if (errorCallback) {
                    errorCallback(data, status, headers, config);
                } else {
                    alertService.show(constants.alert.type.danger,
                        data.message + ' ' + data.stackTrace);
                }

                loaderService.remove();
            });
    };

    return {
        makeRequest: makeRequest
    };
};