window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.cloudController = cloud.controllers.cloudController ||
	function ($scope, $http, $window, $log, constants,
		userTokenService, fileUploader, $modal) {
		var self = this;

		self.initUploader = function() {
			$scope.uploader = new fileUploader();
			var folderId = 'baba2553-f024-4afb-aa8d-358b9e1ebf4a';
			$scope.uploader.url = constants.urls.cloud.files.constructUpload(
				folderId, constants.cloudId);
			$scope.uploader.headers = {
				'Authorization': userTokenService.getAuthorizationHeader()
			};
			$scope.uploader.onCompleteItem =
				function (uploadedItem, response, status, headers) {
					if (status === 200) {
						$scope.uploader.clearQueue();
						$scope.files.push(response);
					} else {
						// todo:
						$log.error("error");
					}
				};
		};
		self.getRootFolderData = function () {
			var rootfolderDataRequest = {
				method: 'GET',
				url: constants.urls.cloud.folders.rootFolderData,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(rootfolderDataRequest)
				.success(function(data, status, headers, config) {
					$scope.cloudFolderPath = 'cloud';

					for (var i = 0; i < data.length; i++) {
						for (var j = 0; j < data[i].folders.length; j++) {
							$scope.folders.push(data[i].folders[j]);
						}
						for (var k = 0; k < data[i].files.length; k++) {
							$scope.files.push(data[i].files[k]);
						}
						//if (data[i].folder.cloudId === constants.cloudId) {
						//	$scope.cloudCurrentFolder = data[i].folder;
						//}
					}
				})
				.error(function(data, status, headers, config) {

				});
		};
		self.clearCloudData = function () {
			$scope.files = [];
			$scope.folders = [];
			$scope.cloudCurrentFolder = null;
			$scope.cloudFolderPath = null;
			$scope.uploader = null;
			$scope.userName = null;
		};
		self.getUserInfo = function () {
			var userInfoRequest = {
				method: 'GET',
				url: constants.urls.cloud.userInfo,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};
			$http(userInfoRequest)
				.success(function (data, status, headers, config) {
					$scope.userInfo = {
						Name: data.Email
					}
				})
				.error(function (data, status, headers, config) {
				});
		};
		$scope.userInfo = {
			Name: ''
		};

		// todo: should make as private 
		$scope.initialize = function () {
			$scope.folders = [];
			$scope.files = [];

			if (userTokenService.isTokenExist()) {
				self.getUserInfo();
				self.getRootFolderData();
				self.initUploader();
				$scope.isLoginView = false;
			} else {
				$scope.isLoginView = true;
			}
		};

		$scope.setLoginView = function() {
			$scope.isLoginView = true;
		};

		$scope.logout = function () {
			var logoutRequest = {
				method: 'POST',
				url: constants.urls.cloud.logout,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(logoutRequest)
				.success(function (data, status, headers, config) {
					userTokenService.removeToken();
					self.clearCloudData();
					$scope.isLoginView = true;
				})
				.error(function (data, status, headers, config) {
				});
		};

		// Files
		$scope.getStorageImageClass = function(cloudId) {
			return 'logo-' + cloudId;
		};

		$scope.deleteFile = function(file) {
			if (file.id) {
				var url = constants.urls.cloud.files.constructDelete(
					file.id, file.cloudId);
				var deleteRequest = {
					method: 'DELETE',
					url: url,
					headers: {
						'Authorization': userTokenService.getAuthorizationHeader()
					}
				};

				$http(deleteRequest)
					.success(function(data, status, headers, config) {
						for (var i = 0; i < $scope.files.length; i++) {
							if ($scope.files[i].id === file.id) {
								$scope.files.splice(i, 1);
							}
						}
					})
					.error(function(data, status, headers, config) {
					});
			} else {
				$log.error("userFile.id");
			}
		};

		$scope.animationsEnabled = true;

		$scope.renameFile = function (file) {
			var entity = {
				type: constants.renameEntities.file,
				data: file
			};
			file.name = $scope.getFileNameWithoutExtention(file.name);
			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'renameModal.html',
				controller: cloud.controllers.renameModalController,
				resolve: {
					entity: function () {
						return entity;
					}
				}
			});

			modalInstance.result.then(function(options) {
				if (options.isSuccess) {
					$scope.files.push(options.data.name);
				} else {
					// todo:
				}
			});
		};

		$scope.renameFolder = function (folder) {
			var entity = {
				type: constants.renameEntities.folder,
				data: folder
			};
			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'renameModal.html',
				controller: cloud.controllers.renameModalController,
				resolve: {
					entity: function () {
						return entity;
					}
				}
			});

			modalInstance.result.then(function (options) {
				if (options.isSuccess) {
					$scope.folders.push(options.data.name);
				} else {
					// todo:
				}
			});
		};
		
		// Folders
		$scope.createFolder = function() {
			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'createFolderModal.html',
				controller: cloud.controllers.createFolderModalController,
				resolve: {
					currentFolderId: function() {
						return $scope.cloudCurrentFolder.id;
					}
				}
			});

			modalInstance.result.then(function(options) {
				if (options.isSuccess) {
					$scope.folders.push(options.data);
				} else {
					// todo:
				}
			});
		};

		$scope.deleteFolder = function(folder) {
			var url = constants.urls.cloud.folders.constructDelete(
				folder.id, folder.cloudId);
			var deleteRequest = {
				method: 'DELETE',
				url: url,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(deleteRequest)
				.success(function(data, status, headers, config) {
					for (var i = 0; i < $scope.folders.length; i++) {
						if ($scope.folders[i].id === data) {
							$scope.folders.splice(i, 1);
						}
					}
				})
				.error(function(data, status, headers, config) {
				});
		};

		$scope.openFolder = function(folder) {
			var openFolderRequest = {
				method: 'GET',
				url: constants.urls.cloud.folders.constructFolderData(
					folder.id, folder.cloudId),
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(openFolderRequest)
				.success(function(data, status, headers, config) {
					$scope.folders = [];
					$scope.files = [];
					$scope.cloudCurrentFolder = data.folder;
					$scope.cloudFolderPath += ' -> ' + folder.name;
					$scope.uploader.url =
						constants.urls.cloud.files.constructUpload(data.folder.id, 2);
					for (var i = 0; i < data.folders.length; i++) {
						$scope.folders.push(data.folders[i]);
					}

					for (var j = 0; j < data.files.length; j++) {
						$scope.files.push(data.files[j]);
					}
				})
				.error(function(data, status, headers, config) {
				});
		};

		// Helpers
		$scope.getFileNameWithoutExtention = function(fileName) {
			var lastIndexOfDot = fileName.lastIndexOf('.');
			if (lastIndexOfDot >= 0) {
				return fileName.substr(0, lastIndexOfDot);
			} else {
				return fileName;
			}
		};

		$scope.initialize();
	};