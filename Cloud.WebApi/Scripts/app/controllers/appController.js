window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.appController = function($scope, $window, httpService, alertService,
    loaderService, constants, userTokenService, fileUploader, $modal) {
    var self = this;

    self.addRootFolder = function() {
        if ($scope.cloudFolders.length <= 0) {
            $scope.cloudFolders = [];
            $scope.cloudFolders.push({ name: constants.rootCloudFolderName });
        }
    };

    $scope.cloudFolders = [];

    // Helpers
    $scope.getStorageLargeImageClass = function(storageId) {
        return 'logo-lg-' + storageId;
    };

    $scope.initialize();
};