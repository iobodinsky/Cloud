var cloud = cloud || {};
cloud.app = cloud.app || angular.module('cloud', []);

cloud.app.controller('cloudController', [
    '$scope',
    '$http',
    '$window',
    cloud.controllers.cloudController
]);
