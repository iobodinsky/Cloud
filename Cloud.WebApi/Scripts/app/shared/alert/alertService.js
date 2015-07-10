window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || {};

window.cloud.services.alertService = function($timeout, constants) {
    var alerts = [];

    function show(type, message) {
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
        show: show
    }
};