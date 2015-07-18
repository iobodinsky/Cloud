window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.uploadController = function($scope, $modal,
    alertService, storageService, folderService, constants) {
    var self = this;

    self.initialize = function() {
        storageService.subscibeForFolderStorage(function() {
            $scope.folderStorage = storageService.getFolderStorage();
        });

        storageService.subscibeForStorages(function() {
            $scope.storages = storageService.getStorages();
        });
    };
    self.createFolderForStorage = function(storage) {
        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: constants.viewTemplatePath.createFolder,
            controller: cloud.controllers.createFolderController,
            resolve: {
                storage: function() {
                    return storage;
                },
                currentStorageFolder: function() {
                    return folderService.getCurrenStorageFolder();
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {

                alertService.show(constants.alert.type.success,
                    constants.message.successfolderCreate);
            } else {
                alertService.show(constants.alert.type.danger,
                    constants.message.failCreatFolder);
            }
        });
    };
    self.animationsEnabled = true;

    $scope.folderStorage = storageService.getFolderStorage();

    $scope.storages = storageService.getStorages();

    $scope.createFolder = function (storage) {
        if (storage) self.createFolderForStorage(storage);
        else if ($scope.folderStorage) self.createFolderForStorage($scope.folderStorage);
        else alertService.show(constants.alert.type.warning, constants.message.warningChooseStorage);
    };

    self.initialize();
};