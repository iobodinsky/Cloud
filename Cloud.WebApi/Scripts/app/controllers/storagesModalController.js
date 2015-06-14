window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.storagesModalController =
	cloud.controllers.storagesModalController ||
	function($scope, $modalInstance, httpService, alertService, storages) {
		$scope.storages = storages;

		$scope.getStorageLargeImageClass = function(storageId) {
			return 'logo-lg-' + storageId;
		};

		$scope.disconnect = function(storageId) {
			httpService.makeRequest();
		};

		$scope.cancel = function() {
			$modalInstance.dismiss('cancel');
		};
	};