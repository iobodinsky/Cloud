window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.userAccountController =
	cloud.controllers.userAccountController ||
	function ($scope, $http, $window, constants,
		alertService, userTokenService, loaderService) {
		var self = this;

		self.initialize = function() {};

		$scope.userRegistrationName = '';
		$scope.userRegistrationEmail = '';
		$scope.userRegistrationPassword = '';
		$scope.userRegistrationConfirmPassword = '';
		$scope.userLoginName = '';
		$scope.userLoginPassword = '';

		$scope.register = function () {
			loaderService.show();
			var registrationData = {
				UserName: $scope.userRegistrationName,
				Email: $scope.userRegistrationEmail,
				Password: $scope.userRegistrationPassword,
				ConfirmPassword: $scope.userRegistrationConfirmPassword
			};

			var registerRequest = {
				method: constants.httpMethod.post,
				url: constants.urls.cloud.register,
				contentType: 'application/json; charset=utf-8',
				data: JSON.stringify(registrationData)
			};

			$http(registerRequest)
				.success(function(data, status, headers, config) {
					$scope.userLoginName = registrationData.UserName;
					$scope.userLoginPassword = registrationData.Password;
					$scope.login();
				})
				.error(function (data, status, headers, config) {
					loaderService.remove();
					alertService.show(constants.alert.type.success,
						constants.message.failRegister);
				});
		};

		$scope.login = function () {
			loaderService.show();
			var userLogin = $scope.userLoginName;
			var userPassword = $scope.userLoginPassword;

			var loginData = {
				grant_type: 'password',
				username: userLogin,
				password: userPassword
			};

			var loginRequest = {
				method: constants.httpMethod.post,
				url: constants.urls.cloud.token,
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded'
				},
				data: 'grant_type=password&username=' + loginData.username +
					'&password=' + loginData.password
			};

			$http(loginRequest)
				.success(function(data, status, headers, config) {
					userTokenService.storeToken(data.access_token);
					self.initialize();
					$scope.initialize();
					loaderService.remove();
				})
				.error(function (data, status, headers, config) {
					loaderService.remove();
					alertService.show(constants.alert.type.success,
						constants.message.failLogin);
				});
		};

		self.initialize();
	};