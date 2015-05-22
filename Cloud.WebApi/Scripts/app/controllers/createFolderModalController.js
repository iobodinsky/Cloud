window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.createFolderModalController = cloud.controllers.createFolderModalController ||
	function ($scope, $http, $modalInstance, constants, userTokenService, currentFolderId) {

		$scope.create = function() {
			var folder = {
				'Name': $scope.folderName,
				'ParentId': currentFolderId
			};
			var createFolderRequest = {
				method: 'POST',
				url: constants.urls.cloud.folders.create,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				},
				data: folder
			};

			$http(createFolderRequest)
				.success(function(data, status, headers, config) {
				})
				.error(function(data, status, headers, config) {
				});

			$modalInstance.close(0);
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};

	};