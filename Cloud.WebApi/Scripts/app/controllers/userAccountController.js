window.cloud = window.cloud || {};

cloud.controllers = cloud.controllers || {};

cloud.controllers.userAccountController =
	cloud.controllers.userAccountController ||
	function($scope, $window, httpService, alertService,
		userTokenService, loaderService, constants) {
		var self = this;

        // todo: what is this?
		self.initialize = function() {};

		$scope.userRegistrationName = '';
		$scope.userRegistrationEmail = '';
		$scope.userRegistrationPassword = '';
		$scope.userRegistrationConfirmPassword = '';
		$scope.userLoginName = '';
		$scope.userLoginPassword = '';

		$scope.register = function() {
			var registrationData = {
				UserName: $scope.userRegistrationName,
				Email: $scope.userRegistrationEmail,
				Password: $scope.userRegistrationPassword,
				ConfirmPassword: $scope.userRegistrationConfirmPassword
			};

			function success() {
				$scope.userLoginName = registrationData.UserName;
				$scope.userLoginPassword = registrationData.Password;
				$scope.login();
			};

			function error() {
				alertService.show(constants.alert.type.danger,
					constants.message.failRegister);
			};

			var requestHeaders = {};
			requestHeaders[constants.httpHeader.name.contentType] =
				constants.httpHeader.value.json;

			httpService.makeRequest(
				constants.httpMethod.post,
				constants.urls.cloud.register,
				requestHeaders,
				JSON.stringify(registrationData),
				success, error);
		};

		$scope.login = function() {
			var userLogin = $scope.userLoginName;
			var userPassword = $scope.userLoginPassword;

			var loginData = {
				grant_type: 'password',
				username: userLogin,
				password: userPassword
			};

			function success(data) {
				userTokenService.storeToken(data.access_token);
				self.initialize();
				$scope.initialize();
			};

			function error() {
				alertService.show(constants.alert.type.danger,
					constants.message.failLogin);
			};

			var requestHeaders = {};
			requestHeaders[constants.httpHeader.name.contentType] =
				constants.httpHeader.value.formUrlencoded;

			var requestData = 'grant_type=password&username=' + loginData.username +
				'&password=' + loginData.password;

			httpService.makeRequest(
				constants.httpMethod.post,
				constants.urls.cloud.token,
				requestHeaders,
				requestData,
				success, error);
		};

		self.initialize();
	};