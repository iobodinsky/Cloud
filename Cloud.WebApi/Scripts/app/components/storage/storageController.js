window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.storageController = function($scope, $state,
    storageService, userTokenService, constants) {
    var self = this;

    self.initialize = function() {
        if (!userTokenService.isTokenExist()) $state.go(constants.routeState.login);

        storageService.getStorages();
    };

    $scope.storages = storageService.userStorages;

    $scope.connect = function(storage) {
        storageService.connect(storage);
    };

    $scope.disconnect = function(storage) {
        storageService.disconnect(storage);
    };

    $scope.getStorageLargeImageClass = function(storage) {
        return 'logo-lg-' + storage;
    };

    self.initialize();
};