window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.uploadController = function($scope, storageService) {
    var self = this;

    self.initialize = function() {
        storageService.subscibeForFolderStorage(function () {
            $scope.folderStorage = storageService.getFolderStorage();
        });

        storageService.subscibeForStorages(function () {
            $scope.storages = storageService.getStorages();
        });
    };

    $scope.folderStorage = storageService.getFolderStorage();

    $scope.storages = storageService.getStorages();

    self.initialize();
};