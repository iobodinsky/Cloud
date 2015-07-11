window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.userAccountController = function($scope, $window, $modal, $state,
    httpService, userTokenService, alertService, constants) {
    var self = this;

    self.initialize = function() {
        if (userTokenService.isTokenExist()) {
            self.getUserInfo();
            self.getUserStorages();
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
    self.authorizeStorage = function(storageId) {
        switch (storageId) {
        case constants.storages.googleDriveId: // google drive
            httpService.makeRequest(constants.httpMethod.get,
                constants.urls.drive.authorize, null, null, success);
            break;
        case constants.storages.dropboxId: // dropbox
            httpService.makeRequest(constants.httpMethod.get,
                constants.urls.dropbox.authorize, null, null, success, dropboxError);

            function dropboxError(data) {
                if (data.message === 'Dropbox account unauthorised') {
                    $window.location.href = data.innerServerError.message;
                }
            };

            break;
        default:
            alertService.show(constants.alert.type.danger,
                constants.message.failConnectToUnknownStorage);
            break;
        }

        function success() {
            $state.reload();
        };
    };
    self.disconnectStorage = function(storageId) {
        httpService.makeRequest(
            constants.httpMethod.post,
            constants.urls.cloud.constructDisconnect(storageId), null, null, success);

        function success() {
            $state.reload();
        }
    };
    self.getUserStorages = function() {
        httpService.makeRequest(
            constants.httpMethod.get,
            constants.urls.cloud.storages,
            null, null, success, error);

        function success(data) {
            $scope.storages.connected = data.connected;
            $scope.storages.available = data.available;
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failGetStorages);
        };
    };
    self.animationsEnabled = true;

    $scope.userInfo = {
        name: ''
    };

    $scope.storages = {
        connected: [],
        available: []
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
            controller: cloud.controllers.manageStoragesController,
            resolve: {
                storages: function() {
                    return $scope.storages;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.data.authorize) {
                self.authorizeStorage(options.data.storageId);
            }
            if (options.data.disconnect) {
                self.disconnectStorage(options.data.storageId);
            }
        });
    };

    self.initialize();
};