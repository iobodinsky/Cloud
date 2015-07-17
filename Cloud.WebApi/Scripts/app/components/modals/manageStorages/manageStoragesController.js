window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.manageStoragesController = function($scope,
    $modalInstance, storageService) {
    var self = this;

    self.inirialize = function () {
        storageService.subscibeForStorages(function() {
            $scope.storages = storageService.getStorages();
        });
        storageService.updateStorages();
    };

    $scope.storages = storageService.getStorages();

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