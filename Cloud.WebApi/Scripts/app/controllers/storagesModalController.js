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

		$scope.authorizeStorage = function (storageId) {
			var data = {
				authorize: true,
				storageId: storageId
			}

			$modalInstance.close({
				data: data
			});
		};

		$scope.disconnect = function (storageId) {
			var data = {
				disconnect: true,
				storageId: storageId
			}

			$modalInstance.close({
				data: data
			});
		}
	};