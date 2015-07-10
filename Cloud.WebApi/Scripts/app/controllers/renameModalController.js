window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.renameModalController = function($scope, $modalInstance, httpService, userTokenService, constants, renameEntity) {
		// todo: duplicated in appController
		$scope.getFileNameWithoutExtention = function(name) {
			var lastIndexOfDot = name.lastIndexOf('.');
			if (lastIndexOfDot >= 0) {
				return name.substr(0, lastIndexOfDot);
			} else {
				return name;
			}
		}

		$scope.entryNewName = $scope.getFileNameWithoutExtention(renameEntity.data.name);

		$scope.entity = renameEntity.data;
		$scope.rename = function() {
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
			};

			httpService.makeRequest(
				constants.httpMethod.post, url, null, { name: $scope.entryNewName }, success, error);
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};
	};