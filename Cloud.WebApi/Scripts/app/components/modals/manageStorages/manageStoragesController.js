window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || {};

window.cloud.controllers.manageStoragesController = function ($scope, $modalInstance,
    httpService, alertService, storages) {
    $scope.storages = storages;

    $scope.getStorageLargeImageClass = function(storageId) {
        return 'logo-lg-' + storageId;
    };

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
};