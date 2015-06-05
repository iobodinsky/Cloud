window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.renameFileModalController = cloud.controllers.renameFileModalController ||
	function ($scope, $http, $modalInstance, constants, userTokenService, file) {
		var self = this;
		self.oldFileName = file.name;

		$scope.file = file;
		$scope.rename = function (newFileName) {
			var renameRequest = {
				method: 'POST',
				url: constants.urls.cloud.files.constructRename(file.id, file.cloudId),
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
					$scope.file.name = self.oldFileName;
				});
			

			$modalInstance.close();
		};

		$scope.cancel = function () {
			$modalInstance.dismiss('cancel');
		};
	};