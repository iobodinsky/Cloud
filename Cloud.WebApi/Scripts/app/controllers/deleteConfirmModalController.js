window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.deleteConfirmModalController =
	cloud.controllers.deleteConfirmModalController ||
	function($scope, $http, $modalInstance, constants, userTokenService, entity) {
		$scope.entity = entity.data;

		$scope.delete = function() {
			var url = '';
			switch (entity.type) {
			case constants.cloudEntities.folder:
				url = constants.urls.cloud.folders.constructDelete(
					entity.data.id, entity.data.cloudId);
				break;
			case constants.cloudEntities.file:
				url = constants.urls.cloud.files.constructDelete(
					entity.data.id, entity.data.cloudId);
				break;
			default:
			};

			var deleteRequest = {
				method: 'DELETE',
				url: url,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(deleteRequest)
				.success(function(data, status, headers, config) {
					$modalInstance.close({
						isSuccess: true,
						data: data,
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
				});
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};

		// todo: duplicated in cloudController
		$scope.getFileNameWithoutExtention = function(fileName) {
			var lastIndexOfDot = fileName.lastIndexOf('.');
			if (lastIndexOfDot >= 0) {
				return fileName.substr(0, lastIndexOfDot);
			} else {
				return fileName;
			}
		}
	};