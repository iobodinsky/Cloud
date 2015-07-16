window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.manageStoragesController = function($scope,
    $modalInstance, storageService) {
    var self = this;

    self.inirialize = function() {
        storageService.getStorages();
    };

    $scope.storages = storageService.userStorages;

    $scope.authorizeStorage = function(storage) {
        var data = {
            authorize: true,
            storage: storage
        }

        $modalInstance.close({
            data: data
        });
    };

    $scope.disconnect = function(storage) {
        var data = {
            disconnect: true,
            storage: storage
        }

        $modalInstance.close({
            data: data
        });
    }

    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.getStorageLargeImageClass = function(storage) {
        return 'logo-lg-' + storage;
    };

    self.inirialize();
};