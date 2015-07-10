window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || { };

window.cloud.controllers.folderController = function($scope, $state, $window, httpService, alertService,
    loaderService, constants, userTokenService, fileUploader, $modal) {
    var self = this;

    self.initilize = function() {
        if (!userTokenService.isTokenExist()) $state.go('login');
        self.clearFolderData();
        self.getRootFolderData();

        // todo: removed local cloud
        //self.initUploader();
    };

    self.clearFolderData = function() {
        $scope.files = [];
        $scope.folders = [];
        $scope.uploader = null;
        $scope.cloudFolders = [];
    };

    self.getRootFolderData = function() {
        httpService.makeRequest(
            constants.httpMethod.get,
            constants.urls.cloud.folders.rootFolderData,
            null, null, success, error);

        function success(data) {
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].folders.length; j++) {
                    $scope.folders.push(data[i].folders[j]);
                }
                for (var k = 0; k < data[i].files.length; k++) {
                    $scope.files.push(data[i].files[k]);
                }
            }

            //self.addRootFolder();
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failGetRootFolderData);
        };
    };

    self.initUploader = function() {
        $scope.uploader = new fileUploader();
        $scope.uploader.removeAfterUpload = true;
        $scope.uploader.url = '';
        $scope.uploader.headers = {};
        $scope.uploader.headers[constants.httpHeader.name.authorization] =
            userTokenService.getAuthorizationToken();

        $scope.uploader.onCompleteItem =
            function(uploadedItem, response, status) {
                $scope.uploader.clearQueue();

                if (status === 200) {
                    $scope.files.push(response);

                    alertService.show(constants.alert.type.success,
                        constants.message.successUploadFile);
                } else {
                    alertService.show(constants.alert.type.danger,
                        constants.message.failUploadFile);
                }
            };
    };

    self.animationsEnabled = true;

    $scope.folders = [];
    $scope.files = [];
    $scope.cloudFolders = [];

    // Files
    $scope.download = function(file) {
        switch (file.storageId) {
        case constants.storages.googleDriveId: // Drive
            if (file.downloadUrl) {
                $window.open(file.downloadUrl, '_blank');
            } else {
                alertService.show(constants.alert.type.info,
                    constants.message.infoDriveFileDownloadingNotAllowed);
            }
            break;
        case constants.storages.dropboxId: // Dropbox
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

    $scope.deleteFile = function(file, $index) {
        var deleteEntity = {
            type: constants.cloudEntities.file,
            data: file
        };

        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: 'scripts/app/components/modals/deleteConfirm/deleteConfirmView.html',
            controller: cloud.controllers.deleteConfirmController,
            resolve: {
                deleteEntity: function() {
                    return deleteEntity;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {
                $scope.files.splice($index, 1);
                alertService.show(constants.alert.type.success,
                    constants.message.successDelete);
            } else {
                alertService.show(constants.alert.type.danger,
                    constants.message.failDelete);
            }
        });
    };

    $scope.renameFile = function(file, $index) {
        var renameEntity = {
            type: constants.cloudEntities.file,
            data: file
        };

        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: 'scripts/app/components/modals/rename/renameView.html',
            controller: cloud.controllers.renameController,
            resolve: {
                renameEntity: function() {
                    return renameEntity;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {
                if (file.storageId == constants.storages.dropboxId) {
                    $scope.files[$index].id = options.newName;
                    $scope.files[$index].name =
                        options.newName.substring(options.newName.lastIndexOf('|') + 1);
                } else {
                    $scope.files[$index].name = options.newName;
                }

                alertService.show(constants.alert.type.success,
                    constants.message.successRename);
            } else {
                alertService.show(constants.alert.type.danger,
                    constants.message.failRename);
            }
        });
    };

    $scope.renameFolder = function(folder, $index) {
        var renameEntity = {
            type: constants.cloudEntities.folder,
            data: folder
        };
        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: 'scripts/app/components/modals/rename/renameView.html',
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
                    if (folder.storageId == constants.storages.dropboxId) {
                        $scope.folders[$index].id = options.newName;
                        $scope.folders[$index].name =
                            options.newName.substring(options.newName.lastIndexOf('|') + 1);
                    } else {
                        $scope.folders[$index].name = options.newName;
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

    // Folders
    $scope.createFolder = function() {
        var currentFolder = $scope.cloudFolders[$scope.cloudFolders.length - 1];
        if (currentFolder && currentFolder.id) {
            var modalInstance = $modal.open({
                animation: self.animationsEnabled,
                templateUrl: 'scripts/app/components/modals/createFolder/createFolderView.html',
                controller: cloud.controllers.createFolderController,
                resolve: {
                    folderId: function() {
                        return currentFolder.id;
                    }
                }
            });

            modalInstance.result.then(function(options) {
                if (options.isSuccess) {
                    $scope.folders.push(options.data);
                    alertService.show(constants.alert.type.success,
                        constants.message.successfolderCreate);
                } else {
                    alertService.show(constants.alert.type.danger,
                        constants.message.failCreatFolder);
                }
            });
        } else {
            alertService.show(constants.alert.type.warning,
                constants.message.warningNotInCloudFolder);
        }
    };

    $scope.deleteFolder = function(folder, $index) {
        var deleteEntity = {
            type: constants.cloudEntities.folder,
            data: folder
        };

        var modalInstance = $modal.open({
            animation: self.animationsEnabled,
            templateUrl: 'scripts/app/components/modals/deleteConfirm/deleteConfirmView.html',
            controller: cloud.controllers.deleteConfirmController,
            resolve: {
                deleteEntity: function() {
                    return deleteEntity;
                }
            }
        });

        modalInstance.result.then(function(options) {
            if (options.isSuccess) {
                $scope.folders.splice($index, 1);
                alertService.show(constants.alert.type.success,
                    constants.message.successDelete);
            } else {
                alertService.show(constants.alert.type.danger,
                    constants.message.failDelete);
            }
        });
    };

    $scope.openFolder = function(folder) {
        function success(data) {
            $scope.folders = [];
            $scope.files = [];
            if (!data.folder.name) {
                data.folder.name = folder.name;
            }
            $scope.cloudFolders.push(data.folder);

            $scope.folders = data.folders;
            $scope.files = data.files;
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failOpenFolder);
        };

        var url = constants.urls.cloud.folders.constructFolderData(
            folder.id, folder.storageId);

        httpService.makeRequest(
            constants.httpMethod.get, url, null, null, success, error);
    };

    $scope.openFolderFromHeader = function(folder, $index) {
        if (folder.name === constants.rootCloudFolderName) {
            self.getRootFolderData();
        } else {
            $scope.cloudFolders.length = $index;
            $scope.openFolder(folder);
        }
    };

    // Helpers
    $scope.getFileNameWithoutExtention = function(fileName) {
        var lastIndexOfDot = fileName.lastIndexOf('.');
        if (lastIndexOfDot >= 0) {
            return fileName.substr(0, lastIndexOfDot);
        } else {
            return fileName;
        }
    };

    $scope.getStorageImageClass = function(storageId) {
        return 'logo-' + storageId;
    };

    self.initilize();
};