window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || {};

window.cloud.services.storageService = function ($window,
    $state, httpService, alertService, constants) {

    var userStorages = {
        connected: [],
        available: []
    };

    var folderStorage = '';

    function getStorages() {
        function success(data) {
            if (!data.connected.length) $state.go(constants.routeState.connect);

            userStorages.connected = data.connected;
            userStorages.available = data.available;
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failGetStorages);
        };

        httpService.makeRequest(
            constants.httpMethod.get,
            constants.urls.cloud.storages,
            null, null, success, error);
    };

    function connect(storage) {
        switch (storage) {
        case constants.storages.googleDriveAlias: // googledrive
            httpService.makeRequest(constants.httpMethod.get,
                constants.urls.drive.authorize, null, null, success, error);
            break;
        case constants.storages.dropboxAlias: // dropbox
            httpService.makeRequest(constants.httpMethod.get,
                constants.urls.dropbox.authorize, null, null, dropboxSuccess, error);

            function dropboxSuccess(url) {
                $window.location = url;
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

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failConnectToStorage);
        }
    };

    function disconnect(storage) {
        httpService.makeRequest(
            constants.httpMethod.post,
            constants.urls.cloud.constructDisconnect(storage), null, null, success, error);

        function success() {
            $state.reload();
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failDisconnectFromStorage);
        };
    };

    function setFolderStorage(value) {
        folderStorage = value;
    };

    function clearFolderStorage() {
        folderStorage = '';
    };

    return {
        userStorages: userStorages,
        getStorages: getStorages,
        connect: connect,
        disconnect: disconnect,
        folderStorage: folderStorage,
        setFolderStorage: setFolderStorage,
        clearFolderStorage: clearFolderStorage
    };
};