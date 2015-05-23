window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.cloudController = cloud.controllers.cloudController ||
	function ($scope, $http, $window, $log, constants, userTokenService, fileUploader, $modal) {
		var self = this;

		self.init = function() {
			$scope.folders = [];
			$scope.files = [];

			var token = userTokenService.getToken();
			if (token) {
				self.getUserInfo();
				self.getFiles();
			} else {
				$scope.isLogin = true;
			}
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
					$scope.userName = data.Email;
				})
				.error(function(data, status, headers, config) {

				});
		};
		self.getFiles = function() {
			var filesFoldersRequest = {
				method: 'GET',
				url: constants.urls.cloud.files.getAll,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(filesFoldersRequest)
				.success(function(data, status, headers, config) {
					$scope.currentFolderId = data.currentFolderId;

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
		
		$scope.uploader = new fileUploader();
		// TODO:
		var cloudId = 2;
		var folderId = 'baba2553-f024-4afb-aa8d-358b9e1ebf4a';
		$scope.uploader.url = constants.urls.cloud.files.constructUpload(cloudId, folderId);
		$scope.uploader.headers = {
			'Authorization': userTokenService.getAuthorizationHeader()
		};
		$scope.uploader.onCompleteItem =
			function (uploadedItem, response, status, headers) {
				if (status === 200) {
					$scope.uploader.clearQueue();
					$scope.uploader.cancelAll();
					$scope.files.push(response);
				} else {
					// todo:
					$log.error("error");
				}
			};

		$scope.isCloud = true;

		$scope.register = function() {
			var registrationData = {
				Email: this.userRegistrationEmail,
				Password: this.userRegistrationPassword,
				ConfirmPassword: this.userRegistrationConfirmPassword
			};

			var registerRequest = {
				method: 'POST',
				url: constants.urls.cloud.register,
				contentType: 'application/json; charset=utf-8',
				data: JSON.stringify(registrationData)
			};

			$http(registerRequest)
				.success(function(data, status, headers, config) {

				})
				.error(function(data, status, headers, config) {

				});
		};

		$scope.login = function() {
			var userLogin = this.userLogin;
			var userPassword = this.userPassword;

			var loginData = {
				grant_type: 'password',
				username: userLogin,
				password: userPassword
			};

			var loginRequest = {
				method: 'POST',
				url: constants.urls.cloud.token,
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded'
				},
				data: 'grant_type=password&username=' + loginData.username +
					'&password=' + loginData.password
			};

			$http(loginRequest)
				.success(function(data, status, headers, config) {
					$window.sessionStorage.setItem(
						constants.userTokenKey, data.access_token);
					$scope.isLogin = false;
					self.init();
				})
				.error(function(data, status, headers, config) {

				});
		};

		$scope.deleteFile = function (file) {
			if (file.id) {
				var cloudId = 2;
				var url = constants.urls.cloud.files.constructDelete(file.id, cloudId);
				var deleteRequest = {
					method: 'DELETE',
					url: url,
					headers: {
						'Authorization': userTokenService.getAuthorizationHeader()
					}
				};

				$http(deleteRequest)
					.success(function (data, status, headers, config) {
						for (var i = 0; i < $scope.files.length; i++) {
							if ($scope.files[i].id === file.id) {
								$scope.files.splice(i, 1);
							}
						}
					})
					.error(function (data, status, headers, config) {
					});
			} else {
				$log.error("userFile.id");
			}
		};
		
		$scope.animationsEnabled = true;

		$scope.renameFile = function (file) {
			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'renameFileModal.html',
				controller: cloud.controllers.renameFileModalController,
				resolve: {
					file: function () {
						return file;
					}
				}
			});

			modalInstance.result.then(function(options) {
				if (options.isSuccess) {
					$scope.folders.push(options.data.name);
				} else {
					// todo:
				}
			});
		};

		$scope.createFolder = function() {
			var modalInstance = $modal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'createFolderModal.html',
				controller: cloud.controllers.createFolderModalController,
				resolve: {
					currentFolderId: function() {
						return $scope.currentFolderId;
					}
				}
			});

			modalInstance.result.then(function (options) {
				if (options.isSuccess) {
					$scope.folders.push(options.data);
				} else {
					// todo:
				}
			});
		};

		$scope.deleteFolder = function (folder) {
			var cloudId = 2;
			var url = constants.urls.cloud.folders.constructDelete(folder.id, cloudId);
			var deleteRequest = {
				method: 'DELETE',
				url: url,
				headers: {
					'Authorization': userTokenService.getAuthorizationHeader()
				}
			};

			$http(deleteRequest)
				.success(function (data, status, headers, config) {
					for (var i = 0; i < $scope.folders.length; i++) {
						if ($scope.folders[i].id === data) {
							$scope.folders.splice(i, 1);
						}
					}
				})
				.error(function (data, status, headers, config) {
				});
		};

		self.init();
	};