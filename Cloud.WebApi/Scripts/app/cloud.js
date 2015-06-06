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
	'$log',
	'constants',
	'userTokenService',
	'FileUploader',
	'$modal',
	cloud.controllers.cloudController
]);

cloud.app.controller('userAccountController', [
	'$scope',
	'$http',
	'$window',
	'constants',
	'userTokenService',
	cloud.controllers.userAccountController
]);

cloud.app.controller('renameModalController', [
	'$scope',
	'$http',
	'$modalInstance',
	'constants',
	'userTokenService',
	'entity',
	cloud.controllers.renameModalController
]);

cloud.app.controller('createFolderModalController', [
	'$scope',
	'$http',
	'$modalInstance',
	'constants',
	'userTokenService',
	'currentFolderId',
	cloud.controllers.createFolderModalController
]);