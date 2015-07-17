window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || {};

window.cloud.services.storageService = function($window,
    $state, httpService, alertService, constants) {

    var userStorages = {
        connected: [],
        available: []
    };

    var folderStorage = '';

    function getFolderStorage() {
        return folderStorage;
    };

    function getStorages() {
        return userStorages;
    };

    function updateStorages() {
        function success(data) {
            if (!data.connected.length) $state.go(constants.routeState.connect);

            userStorages.connected = data.connected;
            userStorages.available = data.available;
            notifyObserversStorages();
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
        notifyObserversFolderStorage();
    };

    function clearFolderStorage() {
        folderStorage = '';
        notifyObserversFolderStorage();
    };

    var observerFolderStorageCallbacks = [];

    function subscibeForFolderStorage(callback) {
        observerFolderStorageCallbacks.push(callback);
    };

    function notifyObserversFolderStorage() {
        angular.forEach(observerFolderStorageCallbacks, function(callback) {
            if (angular.isFunction(callback)) {
                callback();
            }
        });
    };

    var observerStoragesCallbacks = [];

    function subscibeForStorages(callback) {
        observerStoragesCallbacks.push(callback);
    };

    function notifyObserversStorages() {
        angular.forEach(observerStoragesCallbacks, function(callback) {
            if (angular.isFunction(callback)) {
                callback();
            }
        });
    };

    return {
        getStorages: getStorages,
        updateStorages: updateStorages,
        connect: connect,
        disconnect: disconnect,
        getFolderStorage: getFolderStorage,
        setFolderStorage: setFolderStorage,
        clearFolderStorage: clearFolderStorage,
        subscibeForFolderStorage: subscibeForFolderStorage,
        subscibeForStorages: subscibeForStorages
    };
};