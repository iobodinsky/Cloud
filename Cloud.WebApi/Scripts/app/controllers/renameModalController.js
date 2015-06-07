window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.renameModalController = cloud.controllers.renameModalController ||
	function($scope, $http, $modalInstance, constants, userTokenService, entity) {
		var self = this;
		self.oldName = entity.data.name;

		$scope.entity = entity.data;
		$scope.rename = function(newFileName) {
			var url = '';
			switch (entity.type) {
			case constants.cloudEntities.folder:
				url = constants.urls.cloud.folders.constructRename(
					entity.data.id, entity.data.cloudId);
				break;
			case constants.cloudEntities.file:
				url = constants.urls.cloud.files.constructRename(
					entity.data.id, entity.data.cloudId);
				break;
			default:
			};

			var renameRequest = {
				method: 'POST',
				url: url,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader(),
				},
				data: {
					name: newFileName
				}
			};

			$http(renameRequest)
				.success(function(data, status, headers, config) {
					$modalInstance.close({
						isSuccess: true,
						newName: data,
						status: status,
						headers: headers,
						config: config
					});
				})
				.error(function(data, status, headers, config) {
					$modalInstance.close({
						isSuccess: false,
						data: data,
						status: status,
						headers: headers,
						config: config
					});

					$scope.entity.name = self.oldName;
				});
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};
	};