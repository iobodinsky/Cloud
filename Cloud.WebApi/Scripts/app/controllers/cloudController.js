var cloud = cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.cloudController = cloud.controllers.cloudController || function ($scope, $http, $window) {
    var self = this;

    $scope.isLogin = false;
    $scope.isCloud = true;

    var token = getToken();

    if (token) {
        var request = {
            method: 'GET',
            url: cloud.models.constants.urls.userInfo,
            headers: {
                'Authorization': 'Bearer ' + token
            },
            data: { test: 'test' }
        };
        $http(request)
            .success(function(data, status, headers, config) {

            })
            .error(function(data, status, headers, config) {

            });
    } else {
        $scope.isLogin = true;
    }

    $scope.login = function () {
        var userLogin = this.userLogin;
        var userPassword = this.userPassword;

        var loginData = {
            grant_type: 'password',
            username: userLogin,
            password: userPassword
        }

        var loginRequest = {
            method: 'POST',
            url: cloud.models.constants.urls.token,
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            data: 'grant_type=password&username=' + loginData.username + '&password=' + loginData.password

    };

        $http(loginRequest)
            .success(function(data, status, headers, config) {
                $window.sessionStorage.setItem(
                    cloud.models.constants.userTokenKey, data.access_token);
            })
            .error(function(data, status, headers, config) {

            });
    }

    // Private
    function getToken() {
        return $window.sessionStorage.getItem(cloud.models.constants.userTokenKey) || '';
    }
};