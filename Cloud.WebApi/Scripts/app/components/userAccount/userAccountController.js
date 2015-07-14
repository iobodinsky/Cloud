window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.userAccountController = function($scope, $window, $modal, $state,
    userStoragesService, httpService, userTokenService, alertService, constants) {
    var self = this;

    self.initialize = function() {
        if (userTokenService.isTokenExist()) {
            self.getUserInfo();
            userStoragesService.getStorages();
        } else {
            $state.go('login');
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

    $scope.storages = userStoragesService.storages;

    $scope.userInfo = {
        name: ''
    };

    $scope.logout = function() {
        function success() {
            userTokenService.removeToken();
            $state.go('login');
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
            templateUrl: 'scripts/app/components/modals/manageStorages/manageStoragesView.html',
            controller: cloud.controllers.manageStoragesController
        });

        modalInstance.result.then(function(options) {
            if (options.data.authorize) {
                userStoragesService.connect(options.data.storageId);
            }
            if (options.data.disconnect) {
                userStoragesService.disconnect(options.data.storageId);
            }
        });
    };

    self.initialize();
};