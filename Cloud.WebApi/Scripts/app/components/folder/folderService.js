window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || {};

window.cloud.services.folderService = function(httpService,
    folderHistoryService, storageService, alertService, constants) {
    var folders = [];
    var files = [];

    function clearFilesFolders() {
        folders.length = 0;
        files.length = 0;
    };

    function clearFolderHistory() {
        folderHistoryService.clearFolderHistory();
    };

    function openFolder(folder) {
        function success(data) {
            clearFilesFolders();

            if (!data.folder.name) data.folder.name = folder.name;

            for (var i = 0; i < data.folders.length; i++) {
                folders.push(data.folders[i]);
            }
            for (var j = 0; j < data.files.length; j++) {
                files.push(data.files[j]);
            }
            
            folderHistoryService.addFolder(data.folder);

            storageService.setFolderStorage(data.storage);
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failOpenFolder);
        };

        var url = constants.urls.cloud.folders.constructFolderData(
            folder.id, folder.storage);

        httpService.makeRequest(
            constants.httpMethod.get, url, null, null, success, error);
    };

    function openFolderFromHeader(folder) {
        if (folder.id === constants.rootCloudFolderId) {
            getRootFolderData();
        } else {
            folderHistoryService.moveTo(folder);
            openFolder(folder);
        }
    };

    function getRootFolderData() {
        function success(data) {
            clearFilesFolders();
            clearFolderHistory();
            storageService.clearFolderStorage();

            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].folders.length; j++) {
                    folders.push(data[i].folders[j]);
                }
                for (var k = 0; k < data[i].files.length; k++) {
                    files.push(data[i].files[k]);
                }
            }

            var rootFolder = {
                id: constants.rootCloudFolderId,
                name: constants.rootCloudFolderName
            };
            folderHistoryService.addFolder(rootFolder);
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failGetRootFolderData);
        };

        httpService.makeRequest(
            constants.httpMethod.get,
            constants.urls.cloud.folders.rootFolderData,
            null, null, success, error);
    };

    function renameFolder(index, newName) {
        folders[index].name = newName;
    };

    function renameDropboxFolder(index, newName) {
        folders[index].id = newName;
        folders[index].name = newName.substring(newName.lastIndexOf('|') + 1);
    };

    function renameFile(index, newName) {
        files[index].name = newName;
    };

    function renameDropboxFile(index, newName) {
        files[index].id = newName;
        files[index].name = newName.substring(newName.lastIndexOf('|') + 1);
    };

    function deleteFolder(index) {
        folders.splice(index, 1);
    };

    function deleteFile(index) {
        files.splice(index, 1);
    };

    function getFileNameWithoutExtention(fileName) {
        var lastIndexOfDot = fileName.lastIndexOf('.');
        if (lastIndexOfDot >= 0) return fileName.substr(0, lastIndexOfDot);
        else return fileName;
    };

    return {
        folders: folders,
        files: files,
        openFolder: openFolder,
        openFolderFromHeader: openFolderFromHeader,
        getRootFolderData: getRootFolderData,
        renameFolder: renameFolder,
        renameDropboxFolder: renameDropboxFolder,
        renameFile: renameFile,
        renameDropboxFile: renameDropboxFile,
        deleteFolder: deleteFolder,
        deleteFile: deleteFile,
        getFileNameWithoutExtention: getFileNameWithoutExtention
    };
};