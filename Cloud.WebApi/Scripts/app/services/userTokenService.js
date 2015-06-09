window.cloud = window.cloud || {};

cloud.services = cloud.services || {};

cloud.services.userTokenService = cloud.services.userTokenService ||
	function($window, constants) {
		function storeToken(token) {
			$window.sessionStorage.setItem(constants.userTokenKey, token);
		};

		function isTokenExist() {
			return $window.sessionStorage.getItem(constants.userTokenKey) != null;
		};

		function getToken() {
			return $window.sessionStorage.getItem(
				constants.userTokenKey) || '';
		};

		function removeToken() {
			$window.sessionStorage.removeItem(constants.userTokenKey);
		}

		function getAuthorizationToken() {
			return constants.userTokenType + ' ' + getToken();
		};

		return {
			storeToken: storeToken,
			isTokenExist: isTokenExist,
			removeToken: removeToken,
			getAuthorizationToken: getAuthorizationToken
		}
	};