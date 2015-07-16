window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || {};

window.cloud.services.userStoragesService = function($window, $state, httpService, alertService, constants) {
    var storages = {
        connected: [],
        available: []
    };

    function getStorages() {
        function success(data) {
            if (!data.connected.length) $state.go(constants.routeState.connect);

            storages.connected = data.connected;
            storages.available = data.available;
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

    function connect(storageId) {
        switch (storageId) {
        case constants.storages.googleDriveId: // google drive
            httpService.makeRequest(constants.httpMethod.get,
                constants.urls.drive.authorize, null, null, success, error);
            break;
        case constants.storages.dropboxId: // dropbox
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

    function disconnect(storageId) {
        httpService.makeRequest(
            constants.httpMethod.post,
            constants.urls.cloud.constructDisconnect(storageId), null, null, success, error);

        function success() {
            $state.reload();
        };

        function error() {
            alertService.show(constants.alert.type.danger,
                constants.message.failDisconnectFromStorage);
        };
    };

    return {
        storages: storages,
        getStorages: getStorages,
        connect: connect,
        disconnect: disconnect
    };
};