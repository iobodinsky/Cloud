window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.userAccountController = function($scope, $window, $modal, $state,
    storageService, httpService, userTokenService, alertService, constants) {
    var self = this;

    self.initialize = function () {
        storageService.subscibeForStorages(function () {
            $scope.storages = storageService.getStorages();
        });

        if (userTokenService.isTokenExist()) {
            self.getUserInfo();
            storageService.updateStorages();
        } else {
            $state.go(constants.routeState.login);
        }
    };
    self.getUserInfo = function() {
        httpService.makeRequest(
            constants.httpMethod.get, constants.urls.cloud.userInfo,
            null, null, success, error);

        function success(data) {
            $scope.userInfo = {
                name: data.userName
            }
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failLoadUserInfo);
        };
    };
    self.animationsEnabled = true;

    $scope.storages = storageService.userStorages;

    $scope.userInfo = {
        name: ''
    };

    $scope.logout = function() {
        function success() {
            userTokenService.removeToken();
            $state.go(constants.routeState.login);
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failLogout);
        };

        httpService.makeRequest(constants.httpMethod.post,
            constants.urls.cloud.logout, null, null, success, error);
    };

    $scope.manageStorages = function() {
        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: constants.viewTemplatePath.manageStorages,
            controller: cloud.controllers.manageStoragesController
        });

        modalInstance.result.then(function(options) {
            if (options.data.authorize) {
                storageService.connect(options.data.storage);
            }
            if (options.data.disconnect) {
                storageService.disconnect(options.data.storage);
            }
        });
    };

    self.initialize();
};