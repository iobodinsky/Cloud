window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || { };

window.cloud.controllers.folderController = function($scope, $state, $window, $modal,
    httpService, folderService, userTokenService, alertService, loaderService, constants) {
    var self = this;

    self.initilize = function() {
        if (!userTokenService.isTokenExist()) $state.go(constants.routeState.login);
        else folderService.getRootFolderData();
    };
    self.animationsEnabled = true;

    $scope.folders = folderService.folders;
    $scope.files = folderService.files;

    $scope.openFolder = function(folder) {
        folderService.openFolder(folder);
    };

    $scope.renameFolder = function(folder, $index) {
        var renameEntity = {
            type: constants.cloudEntities.folder,
            data: folder
        };
        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: constants.viewTemplatePath.rename,
            controller: cloud.controllers.renameController,
            resolve: {
                renameEntity: function() {
                    return renameEntity;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {
                if (options.isSuccess) {
                    if (folder.storage == constants.storages.dropboxAlias) {
                        folderService.renameDropboxFolder($index, options.newName);
                    } else {
                        folderService.renameFolder($index, options.newName);
                    }

                    alertService.show(constants.alert.type.success,
                        constants.message.successRename);
                } else {
                    alertService.show(constants.alert.type.danger,
                        constants.message.failRename);
                }
            }
        });
    };

    $scope.deleteFolder = function(folder, $index) {
        var deleteEntity = {
            type: constants.cloudEntities.folder,
            data: folder
        };

        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: constants.viewTemplatePath.deleteConfirm,
            controller: cloud.controllers.deleteConfirmController,
            resolve: {
                deleteEntity: function() {
                    return deleteEntity;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {
                folderService.deleteFolder($index);
                alertService.show(constants.alert.type.success,
                    constants.message.successDelete);
            } else {
                alertService.show(constants.alert.type.danger,
                    constants.message.failDelete);
            }
        });
    };

    $scope.downloadFile = function(file) {
        switch (file.storage) {
        case constants.storages.googleDriveAlias: // Drive
            if (file.downloadUrl) $window.open(file.downloadUrl, '_blank');
            else {
                alertService.show(constants.alert.type.info,
                    constants.message.infoDriveFileDownloadingNotAllowed);
            }
            break;
        case constants.storages.dropboxAlias: // Dropbox
            httpService.makeRequest(
                constants.httpMethod.get,
                constants.urls.dropbox.constructDownload(file.id),
                null, null, success);
            break;
        default:
            alertService.show(constants.alert.type.danger,
                constants.message.failCloudNotFound);
            break;
        }

        function success(data) {
            $window.open(data, '_self');
        };
    };

    $scope.renameFile = function(file, $index) {
        var renameEntity = {
            type: constants.cloudEntities.file,
            data: file
        };

        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: constants.viewTemplatePath.rename,
            controller: cloud.controllers.renameController,
            resolve: {
                renameEntity: function() {
                    return renameEntity;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {
                if (file.storage == constants.storages.dropboxAlias) {
                    folderService.renameDropboxFile($index, options.newName);
                } else {
                    folderService.renameFile($index, options.newName);
                }

                alertService.show(constants.alert.type.success,
                    constants.message.successRename);
            } else {
                alertService.show(constants.alert.type.danger,
                    constants.message.failRename);
            }
        });
    };

    $scope.deleteFile = function(file, $index) {
        var deleteEntity = {
            type: constants.cloudEntities.file,
            data: file
        };

        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: constants.viewTemplatePath.deleteConfirm,
            controller: cloud.controllers.deleteConfirmController,
            resolve: {
                deleteEntity: function() {
                    return deleteEntity;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {
                folderService.deleteFile($index);
                alertService.show(constants.alert.type.success,
                    constants.message.successDelete);
            } else {
                alertService.show(constants.alert.type.danger,
                    constants.message.failDelete);
            }
        });
    };

    // Helpers
    $scope.getFileNameWithoutExtention = function(fileName) {
        return folderService.getFileNameWithoutExtention(fileName);
    };

    $scope.getStorageImageClass = function(storage) {
        return 'logo-' + storage;
    };

    self.initilize();
};