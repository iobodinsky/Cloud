window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.manageStoragesController = function($scope, $modalInstance,
    userStoragesService) {
    var self = this;

    self.inirialize = function() {
        userStoragesService.getStorages();
    };

    $scope.storages = userStoragesService.storages;

    $scope.authorizeStorage = function(storageId) {
        var data = {
            authorize: true,
            storageId: storageId
        }

        $modalInstance.close({
            data: data
        });
    };

    $scope.disconnect = function(storageId) {
        var data = {
            disconnect: true,
            storageId: storageId
        }

        $modalInstance.close({
            data: data
        });
    }

    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.getStorageLargeImageClass = function(storageId) {
        return 'logo-lg-' + storageId;
    };

    self.inirialize();
};