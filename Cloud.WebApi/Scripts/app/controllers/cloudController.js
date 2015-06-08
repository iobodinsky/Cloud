window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.cloudController = cloud.controllers.cloudController ||
	function($scope, $http, $window, $log, alertService, constants,
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
				function(uploadedItem, response, status, headers) {
					$scope.uploader.clearQueue();

					if (status === 200) {
						$scope.files.push(response);

						alertService.show(constants.alert.type.success,
							constants.message.successUploadFile);
					} else {
						alertService.show(constants.alert.type.danger,
							constants.message.failUploadFile);
					}
				};
		};
		self.getRootFolderData = function() {
			var rootfolderDataRequest = {
				method: 'GET',
				url: constants.urls.cloud.folders.rootFolderData,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(rootfolderDataRequest)
				.success(function (data, status, headers, config) {
					$scope.folders = [];
					$scope.files = [];
					$scope.cloudFolders = [];

					for (var i = 0; i < data.length; i++) {
						for (var j = 0; j < data[i].folders.length; j++) {
							$scope.folders.push(data[i].folders[j]);
						}
						for (var k = 0; k < data[i].files.length; k++) {
							$scope.files.push(data[i].files[k]);
						}
						if (data[i].folder.cloudId === constants.cloudId) {
							data[i].folder.name = 'Cloud';
							$scope.cloudFolders.push(data[i].folder);
						}
					}
				})
				.error(function(data, status, headers, config) {
					alertService.show(constants.alert.type.danger,
						constants.message.failGetRootFolderData);
				});
		};
		self.clearCloudData = function() {
			$scope.files = [];
			$scope.folders = [];
			$scope.uploader = null;
			$scope.userName = null;
			self.driveFolder = null;
			self.dropboxFolder = null;
			$scope.cloudFolders = [];
			$scope.userInfo = {
				Name: ''
			};
		};
		self.getUserInfo = function() {
			var userInfoRequest = {
				method: 'GET',
				url: constants.urls.cloud.userInfo,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};
			$http(userInfoRequest)
				.success(function(data, status, headers, config) {
					$scope.userInfo = {
						Name: data.Email
					}
				})
				.error(function(data, status, headers, config) {
					alertService.show(constants.alert.type.danger,
						constants.message.failLoadUserInfo);
				});
		};

		self.driveFolder = null;
		self.dropboxFolder = null;

		$scope.userInfo = {
			Name: ''
		};

		$scope.cloudFolders = [];

		$scope.alerts = alertService.alerts;

		// todo: should make as private 
		$scope.initialize = function() {
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

		$scope.logout = function() {
			var logoutRequest = {
				method: 'POST',
				url: constants.urls.cloud.logout,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(logoutRequest)
				.success(function(data, status, headers, config) {
					userTokenService.removeToken();
					self.clearCloudData();
					$scope.isLoginView = true;
				})
				.error(function(data, status, headers, config) {
					alertService.show(constants.alert.type.danger,
						constants.message.failLogout);
				});
		};

		// Files
		$scope.download = function(file) {
			switch (file.cloudId) {
				case 1: // Drive
					if (file.downloadUrl) {
						$window.open(file.downloadUrl, '_blank');
					} else {
						alertService.show(constants.alert.type.info,
							constants.message.infoDriveFileDownloadingNotAllowed);
					}
				break;
			case 2: // Cloud
				var url = constants.urls.cloud.files.constructDownloadLink(file.id);
				var downloadFileRequest = {
					method: 'GET',
					url: url,
					headers: {
						'Authorization': userTokenService.getAuthorizationHeader()
					}
				};

				$http(downloadFileRequest)
					.success(function(data, status, headers, config) {
						$window.open(data, '_self');
					})
					.error(function(data, status, headers, config) {
						alertService.show(constants.alert.type.danger,
							constants.message.failRequestDownloadLink);
					});

				break;
				case 3: // Dropbox
					alertService.show(constants.alert.type.danger,
						constants.message.failDelete);
				break;
			default:
				alertService.show(constants.alert.type.danger,
					'cloudId no specified');
				break;
			}
		};

		$scope.getStorageImageClass = function(cloudId) {
			return 'logo-' + cloudId;
		};

		$scope.deleteFile = function(file) {
			var entity = {
				type: constants.cloudEntities.file,
				data: file
			};

			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'deleteConfirmModal.html',
				controller: cloud.controllers.deleteConfirmModalController,
				resolve: {
					entity: function() {
						return entity;
					}
				}
			});

			modalInstance.result.then(function(options) {
				if (options.isSuccess) {
					for (var i = 0; i < $scope.files.length; i++) {
						if ($scope.files[i].id === file.id) {
							$scope.files.splice(i, 1);
						}
					}
					alertService.show(constants.alert.type.success,
						constants.message.successDelete);
				} else {
					alertService.show(constants.alert.type.danger,
						constants.message.failDelete);
				}
			});
		};

		$scope.animationsEnabled = true;

		$scope.renameFile = function(file, $index) {
			var entity = {
				type: constants.cloudEntities.file,
				data: file
			};

			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'renameModal.html',
				controller: cloud.controllers.renameModalController,
				resolve: {
					entity: function() {
						return entity;
					}
				}
			});

			modalInstance.result.then(function(options) {
				if (options.isSuccess) {
					$scope.files[$index].name = options.newName;
					alertService.show(constants.alert.type.success,
						constants.message.successRename);
				} else {
					alertService.show(constants.alert.type.danger,
						constants.message.failRename);
				}
			});
		};

		$scope.renameFolder = function(folder, $index) {
			var entity = {
				type: constants.cloudEntities.folder,
				data: folder
			};
			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'renameModal.html',
				controller: cloud.controllers.renameModalController,
				resolve: {
					entity: function() {
						return entity;
					}
				}
			});

			modalInstance.result.then(function(options) {
				if (options.isSuccess) {
					$scope.folders[$index].name = options.newName;
					alertService.show(constants.alert.type.success,
						constants.message.successRename);
				} else {
					alertService.show(constants.alert.type.danger,
						constants.message.failRename);
				}
			});
		};

		// Folders
		$scope.createFolder = function() {
			var currentFolder = $scope.cloudFolders[$scope.cloudFolders.length - 1];
			if (currentFolder && currentFolder.id) {
				var modalInstance = $modal.open({
					animation: $scope.animationsEnabled,
					templateUrl: 'createFolderModal.html',
					controller: cloud.controllers.createFolderModalController,
					resolve: {
						folderId: function() {
							return currentFolder.id;
						}
					}
				});

				modalInstance.result.then(function(options) {
					if (options.isSuccess) {
						$scope.folders.push(options.data);
						alertService.show(constants.alert.type.success,
							constants.message.successfolderCreate);
					} else {
						alertService.show(constants.alert.type.danger,
							constants.message.failCreatFolder);
					}
				});
			} else {
				alertService.show(constants.alert.type.warning,
					constants.message.warningNotInCloudFolder);
			}
		};

		$scope.deleteFolder = function(folder) {
			var entity = {
				type: constants.cloudEntities.folder,
				data: folder
			};

			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'deleteConfirmModal.html',
				controller: cloud.controllers.deleteConfirmModalController,
				resolve: {
					entity: function() {
						return entity;
					}
				}
			});

			modalInstance.result.then(function(options) {
				if (options.isSuccess) {
					for (var i = 0; i < $scope.folders.length; i++) {
						if ($scope.folders[i].id === options.data) {
							$scope.folders.splice(i, 1);
						}
					}
					alertService.show(constants.alert.type.success,
						constants.message.successDelete);
				} else {
					alertService.show(constants.alert.type.danger,
						constants.message.failDelete);
				}
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
					if (!data.folder.name) {
						data.folder.name = folder.name;
					}
					$scope.cloudFolders.push(data.folder);
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
					alertService.show(constants.alert.type.danger,
						constants.message.failOpenFolder);
				});
		};

		$scope.openFolderFromHeader = function(folder, $index) {
			if (folder.name === 'Cloud') {
				self.getRootFolderData();
			} else {
				$scope.cloudFolders.length = $index;
				$scope.openFolder(folder);
			}
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