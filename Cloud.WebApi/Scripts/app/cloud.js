var cloud = cloud || {};
cloud.app = cloud.app || angular.module('cloud', ['angularFileUpload']);

// controllers
cloud.app.controller('cloudController', [
    '$scope',
    '$http',
    '$window',
    'FileUploader',
    cloud.controllers.cloudController
]);