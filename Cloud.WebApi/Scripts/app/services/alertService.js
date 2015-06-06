window.cloud = window.cloud || {};

cloud.services = cloud.services || {};

cloud.services.alertService = cloud.services.alertService ||
	function($timeout, constants) {
		var alerts = [];

		function add(type, message) {
			if (alerts.length >= constants.alert.maxCount) {
				alerts.shift();
			}
			var alert = { type: type, msg: message };
			alerts.push(alert);
			$timeout(function() {
					remove(alert);
				},
				constants.alert.timeout);
		};

		function remove(alert) {
			alerts.pop(alert);
		};

		return {
			alerts: alerts,
			add: add
		}
	};