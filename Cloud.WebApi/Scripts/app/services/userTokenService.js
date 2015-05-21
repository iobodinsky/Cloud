window.cloud = window.cloud || {};

cloud.services = cloud.services || {};

cloud.services.userTokenService = cloud.services.userTokenService ||
	function($window, constants) {
		function getToken() {
			return $window.sessionStorage.getItem(
				constants.userTokenKey) || '';
		};

		function getAuthorizationHeader() {
			return constants.userTokenType + ' ' + getToken();
		};

		return {
			getToken: getToken,
			getAuthorizationHeader: getAuthorizationHeader
		}
	};