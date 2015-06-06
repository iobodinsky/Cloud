window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.createFolderModalController = cloud.controllers.createFolderModalController ||
	function ($scope, $http, $modalInstance, constants, userTokenService, folderId) {

		$scope.create = function() {
			var folder = {
				'Name': $scope.folderName,
				'ParentId': folderId
			};
			var createFolderRequest = {
				method: 'POST',
				url: constants.urls.cloud.folders.constructCreate(constants.cloudId),
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				},
				data: folder
			};

			$http(createFolderRequest)
				.success(function (data, status, headers, config) {
					$modalInstance.close({
						isSuccess: true,
						data: data,
						status: status,
						headers: headers,
						config: config
					});
				})
				.error(function (data, status, headers, config) {
					$modalInstance.close({
						isSuccess: false,
						data: data,
						status: status,
						headers: headers,
						config: config
					});
				});
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};

	};