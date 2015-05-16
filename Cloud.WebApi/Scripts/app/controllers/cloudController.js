var cloud = cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.cloudController = cloud.controllers.cloudController ||
	function($scope, $http, $window, fileUploader) {
		var self = this;

		self.init = function() {
			$scope.folders = [];
			$scope.files = [];

			var token = self.getToken();
			if (token) {
				self.getUserInfo();
				self.getFiles();
			} else {
				$scope.isLogin = true;
			}
		};
		self.getToken = function() {
			return $window.sessionStorage.getItem(cloud.models.constants.userTokenKey) || '';
		};
		self.getUserInfo = function(token) {
			var userInfoRequest = {
				method: 'GET',
				url: cloud.models.constants.urls.userInfo,
				headers: {
					'Authorization': self.getAuthorizationToken()
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
				url: cloud.models.constants.urls.files,
				headers: {
					'Authorization': self.getAuthorizationToken()
				}
			};

			$http(filesFoldersRequest)
				.success(function(data, status, headers, config) {
					for (var i = 0; i < data.Folders.length; i++) {
						$scope.folders.push(data.Folders[i]);
					}

					for (var j = 0; j < data.Files.length; j++) {
						$scope.files.push(data.Files[j]);
					}
				})
				.error(function(data, status, headers, config) {

				});
		};
		self.getAuthorizationToken = function() {
			return 'Bearer ' + self.getToken();
		};

		$scope.uploader = new fileUploader();
		$scope.uploader.url = cloud.models.constants.urls.upload;
		$scope.uploader.headers = {
			'Authorization': self.getAuthorizationToken()
		};

		$scope.uploader.onCompleteItem =
			function(uploadedItem, response, status, headers) {
				$scope.uploader.clearQueue();

				$scope.files.push(uploadedItem.file);
			}

		$scope.isCloud = true;

		$scope.register = function() {
			var registrationData = {
				Email: this.userRegistrationEmail,
				Password: this.userRegistrationPassword,
				ConfirmPassword: this.userRegistrationConfirmPassword
			};

			var registerRequest = {
				method: 'POST',
				url: cloud.models.constants.urls.register,
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
			}

			var loginRequest = {
				method: 'POST',
				url: cloud.models.constants.urls.token,
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded'
				},
				data: 'grant_type=password&username=' + loginData.username +
					'&password=' + loginData.password
			};

			$http(loginRequest)
				.success(function(data, status, headers, config) {
					$window.sessionStorage.setItem(
						cloud.models.constants.userTokenKey, data.access_token);
					$scope.isLogin = false;
					self.init();
				})
				.error(function(data, status, headers, config) {

				});
		}

		$scope.delete = function(file) {
			var cloudId = 2;
			var url = 'api/files/' + file.id + '/cloud/' + cloudId + '/delete';
			var deleteRequest = {
				method: 'DELETE',
				url: url,
				headers: {
					'Authorization': self.getAuthorizationToken()
				}
			};

			$http(deleteRequest)
				.success(function(data, status, headers, config) {
					for (var i = 0; i < $scope.files.length; i++) {
						if ($scope.files[i].id == file.id) {
							$scope.files.splice(i, 1);
						}
					}
				})
				.error(function(data, status, headers, config) {
				});
		};

		// Private

		self.init();
	};