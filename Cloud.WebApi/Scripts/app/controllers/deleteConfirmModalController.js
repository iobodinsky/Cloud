window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.deleteConfirmModalController = function($scope, $modalInstance,
    httpService, userTokenService, constants, deleteEntity) {
    $scope.entity = deleteEntity.data;

    $scope.delete = function() {
        var url = '';
        switch (deleteEntity.type) {
        case constants.cloudEntities.folder:
            url = constants.urls.cloud.folders.constructDelete(
                deleteEntity.data.id, deleteEntity.data.storageId);
            break;
        case constants.cloudEntities.file:
            url = constants.urls.cloud.files.constructDelete(
                deleteEntity.data.id, deleteEntity.data.storageId);
            break;
        default:
        };

        function success(data, status, headers, config) {
            $modalInstance.close({
                isSuccess: true,
                data: data,
                status: status,
                headers: headers,
                config: config
            });
        };

        function error(data, status, headers, config) {
            $modalInstance.close({
                isSuccess: false,
                data: data,
                status: status,
                headers: headers,
                config: config
            });
        };

        httpService.makeRequest(
            constants.httpMethod.deleteMethod, url, null, null, success, error);
    };

    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
    };

    // todo: duplicated in appController
    $scope.getFileNameWithoutExtention = function(name) {
        if (name) {
            var lastIndexOfDot = name.lastIndexOf('.');
            if (lastIndexOfDot >= 0) {
                return name.substr(0, lastIndexOfDot);
            } else {
                return name;
            }
        }

        return '';
    };
};