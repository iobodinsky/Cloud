window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.renameModalController = cloud.controllers.renameModalController ||
	function($scope, $modalInstance, httpService, userTokenService, constants, renameEntity) {
		// todo: duplicated in appController
		$scope.getFileNameWithoutExtention = function(name) {
			var lastIndexOfDot = name.lastIndexOf('.');
			if (lastIndexOfDot >= 0) {
				return name.substr(0, lastIndexOfDot);
			} else {
				return name;
			}
		}

		var self = this;
		self.oldName = $scope.getFileNameWithoutExtention(renameEntity.data.name);

		$scope.entity = renameEntity.data;
		$scope.entity.name = $scope.getFileNameWithoutExtention(renameEntity.data.name);
		$scope.rename = function(newFileName) {
			var url = '';
			switch (renameEntity.type) {
			case constants.cloudEntities.folder:
				url = constants.urls.cloud.folders.constructRename(
					renameEntity.data.id, renameEntity.data.storageId);
				break;
			case constants.cloudEntities.file:
				url = constants.urls.cloud.files.constructRename(
					renameEntity.data.id, renameEntity.data.storageId);
				break;
			default:
			};

			function success(data, status, headers, config) {
				$modalInstance.close({
					isSuccess: true,
					newName: data,
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

				$scope.entity.name = self.oldName;
			};

			httpService.makeRequest(
				constants.httpMethod.post, url, null, { name: newFileName }, success, error);
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};
	};