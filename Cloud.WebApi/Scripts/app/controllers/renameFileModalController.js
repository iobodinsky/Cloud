window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.renameFileModalController = cloud.controllers.renameFileModalController ||
	function ($scope, $http, $modalInstance, constants, userTokenService, file) {
		$scope.file = file;

		$scope.rename = function (newFileName) {
			var renameRequest = {
				method: 'POST',
				url: constants.urls.common.constructUpload,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				},
				data: newFileName
			};

			$http(renameRequest)
				.success(function(data, status, headers, config) {

				})
				.error(function(data, status, headers, config) {

				});
			

			$modalInstance.close();
		};

		$scope.cancel = function () {
			$modalInstance.dismiss('cancel');
		};
	};