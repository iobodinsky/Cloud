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

cloud.app.service('alertService', [
	'$timeout',
	'constants',
	cloud.services.alertService
]);

// controllers
cloud.app.controller('cloudController', [
	'$scope',
	'$http',
	'$window',
	'$log',
	'alertService',
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
	'alertService',
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
	'folderId',
	cloud.controllers.createFolderModalController
]);

cloud.app.controller('deleteConfirmModalController', [
	'$scope',
	'$http',
	'$modalInstance',
	'constants',
	'userTokenService',
	'entity',
	cloud.controllers.deleteConfirmModalController
]);