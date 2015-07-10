window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || { };

window.cloud.controllers.createFolderController = function ($scope, $modalInstance,
    httpService, constants, userTokenService, folderId) {

    $scope.create = function() {
        var folder = {
            'Name': $scope.folderName,
            'ParentId': folderId
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

        // todo: not implemented
        //httpService.makeRequest(constants.httpMethod.post, , null, folder, success, error);
    };

    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
    };
};