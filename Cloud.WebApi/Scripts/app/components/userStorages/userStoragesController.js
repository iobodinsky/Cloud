window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.userStoragesController = function($scope, userStoragesService) {
    var self = this;

    self.initialize = function() {
        userStoragesService.getStorages();
    };

    $scope.storages = userStoragesService.storages;

    $scope.connect = function(storageId) {
        userStoragesService.connect(storageId);
    };

    $scope.disconnect = function(storageId) {
        userStoragesService.disconnect(storageId);
    };

    $scope.getStorageLargeImageClass = function(storageId) {
        return 'logo-lg-' + storageId;
    };

    self.initialize();
};