window.cloud = window.cloud || {};
cloud.app = cloud.app || angular.module('cloud', ['angularFileUpload', 'ui.bootstrap']);

// constants
cloud.app.constant('constants', cloud.models.constants);

// services
cloud.app.service('userTokenService', [
	'$window',
	'constants',
	cloud.services.userTokenService
]);

// controllers
cloud.app.controller('cloudController', [
	'$scope',
	'$http',
	'$window',
	'constants',
	'userTokenService',
	'FileUploader',
	'$modal',
	cloud.controllers.cloudController
]);

cloud.app.controller('renameFileModalController', [
	'$scope',
	'$http',
	'$modalInstance',
	'constants',
	'userTokenService',
	'file',
	cloud.controllers.renameFileModalController
]);