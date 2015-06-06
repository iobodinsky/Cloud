window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.renameModalController = cloud.controllers.renameModalController ||
	function ($scope, $http, $modalInstance, constants, userTokenService, entity) {
		var self = this;
		self.oldName = entity.data.name;

		$scope.entity = entity.data;
		$scope.rename = function (newFileName) {
			var url = '';
			switch (entity.type) {
			case constants.renameEntities.folder:
				url = constants.urls.cloud.folders.constructRename(
					entity.data.id, entity.data.cloudId);
				break;
			case constants.renameEntities.file:
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

				})
				.error(function(data, status, headers, config) {
					$scope.entity.name = self.oldName;
				});


			$modalInstance.close();
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};
	};