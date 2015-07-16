window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.userStoragesController = function($scope, $state,
    userStoragesService, userTokenService, constants) {
    var self = this;

    self.initialize = function() {
        if (!userTokenService.isTokenExist()) $state.go(constants.routeState.login);

        userStoragesService.getStorages();
    };

    $scope.storages = userStoragesService.storages;

    $scope.connect = function(storage) {
        userStoragesService.connect(storage);
    };

    $scope.disconnect = function(storage) {
        userStoragesService.disconnect(storage);
    };

    $scope.getStorageLargeImageClass = function(storage) {
        return 'logo-lg-' + storage;
    };

    self.initialize();
};