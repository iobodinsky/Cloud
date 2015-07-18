window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || { };

window.cloud.controllers.createFolderController = function ($scope, $modalInstance,
    httpService, constants, userTokenService, storage) {

    $scope.create = function() {
        var folder = {
            'Name': $scope.folderName,
            'Storage': storage
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

        httpService.makeRequest(constants.httpMethod.post,
            constants.urls.cloud.folders.constructCreate(storage), null, folder, success, error);
    };

    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
    };
};