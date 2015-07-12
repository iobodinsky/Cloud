window.cloud = window.cloud || {};

window.cloud.controllers = window.cloud.controllers || { };

window.cloud.controllers.folderHistoryController = function($scope, folderHistoryService, folderService) {

    $scope.cloudFolders = folderHistoryService.folders;

    $scope.openFolderFromHeader = folderService.openFolderFromHeader;
};