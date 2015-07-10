window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.appController = function($scope, $window, httpService, alertService,
    loaderService, constants, userTokenService, fileUploader, $modal) {
    var self = this;

    self.clearAppData = function() {
        $scope.files = [];
        $scope.folders = [];
        $scope.uploader = null;
        $scope.userName = null;
        self.driveFolder = null;
        self.dropboxFolder = null;
        $scope.cloudFolders = [];
        $scope.userInfo = {
            name: ''
        };
        $scope.storages = {
            connected: [],
            available: []
        };
    };
    self.getUserInfo = function() {
        httpService.makeRequest(
            constants.httpMethod.get,
            constants.urls.cloud.userInfo,
            null,
            null,
            success, error);

        function success(data) {
            $scope.userInfo = {
                name: data.email
            }
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failLoadUserInfo);
        };
    };
    self.getUserStorages = function() {
        httpService.makeRequest(
            constants.httpMethod.get,
            constants.urls.cloud.storages,
            null, null, success, null);

        function success(data) {
            $scope.storages.connected = data.connected;
            $scope.storages.available = data.available;
        };
    };
    self.addRootFolder = function() {
        if ($scope.cloudFolders.length <= 0) {
            $scope.cloudFolders = [];
            $scope.cloudFolders.push({ name: constants.rootCloudFolderName });
        }
    };
    $scope.userInfo = {
        name: ''
    };

    $scope.storages = {
        connected: [],
        available: []
    };
    $scope.cloudFolders = [];
    $scope.alerts = alertService.alerts;
    $scope.isLoader = loaderService.isLoader();

    // todo: should make as private 

    $scope.logout = function() {
        function success() {
            userTokenService.removeToken();
            self.clearAppData();
            $scope.isLoginView = true;
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failLogout);
        };

        httpService.makeRequest(constants.httpMethod.post,
            constants.urls.cloud.logout, null, null, success, error);
    };

    // Storage
    $scope.authorizeStorage = function(storageId) {
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
            break;
        }

        function success() {
            $scope.initialize();
        };
    };

    $scope.disconnect = function(storageId) {
        httpService.makeRequest(
            constants.httpMethod.post,
            constants.urls.cloud.constructDisconnect(storageId), null, null, success);

        function success() {
            $scope.initialize();
        }
    };

    $scope.manageStorages = function() {
        var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'storagesModal.html',
            controller: cloud.controllers.storagesModalController,
            resolve: {
                storages: function() {
                    return $scope.storages;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.data.authorize) {
                $scope.authorizeStorage(options.data.storageId);
            }
            if (options.data.disconnect) {
                $scope.disconnect(options.data.storageId);
            }
        });
    };

    // Helpers
    $scope.getStorageLargeImageClass = function(storageId) {
        return 'logo-lg-' + storageId;
    };

    $scope.initialize();
};