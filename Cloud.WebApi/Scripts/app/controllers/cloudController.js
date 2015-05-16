var cloud = cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.cloudController = cloud.controllers.cloudController || function ($scope, $http, $window, fileUploader) {
    var self = this;

    self.getToken = function () {
        return $window.sessionStorage.getItem(cloud.models.constants.userTokenKey) || '';
    }

    $scope.uploader = new fileUploader();
    $scope.uploader.url = cloud.models.constants.urls.upload;
    $scope.uploader.headers = {
        'Authorization': 'Bearer ' + self.getToken()
    };
    $scope.isCloud = true;

    $scope.register = function () {
        var registrationData = {
            Email: this.userRegistrationEmail,
            Password: this.userRegistrationPassword,
            ConfirmPassword: this.userRegistrationConfirmPassword
        };

        var registerRequest = {
            method: 'POST',
            url: cloud.models.constants.urls.register,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(registrationData)
        };

        $http(registerRequest)
            .success(function (data, status, headers, config) {
                
            })
            .error(function (data, status, headers, config) {

            });
    };

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
                $scope.isLogin = false;
                self.init();
            })
            .error(function(data, status, headers, config) {

            });
    }

    $scope.submitFile = function () {
        var formData = new FormData();
        //Take the first selected file
        //formData.append("uploadedFile", files[0]);
        var s = $scope.uploadedFile;

        $http.post(cloud.models.constants.urls.upload, s, {
            withCredentials: true,
            headers: {
                'Authorization': 'Bearer ' + self.getToken()
            },
            transformRequest: angular.identity
        }).success(function(data) {

            }).error(function(data) {

            }
        );
    };

    // Private
    self.init = function() {
        var token = self.getToken();

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
                .success(function (data, status, headers, config) {
                    $scope.userName = data.Email;
                })
                .error(function (data, status, headers, config) {

                });
        } else {
            $scope.isLogin = true;
        }
    };

    self.init();
};