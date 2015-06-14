window.cloud = window.cloud || {};
cloud.app = cloud.app || angular.module('cloud', ['angularFileUpload', 'ui.bootstrap']);

// constants
cloud.app.constant('constants', cloud.models.constants);

// services
cloud.app.service('httpService', [
	'$http',
	'$window',
	'loaderService',
	'userTokenService',
	'alertService',
	'constants',
	cloud.services.httpService
]);

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

cloud.app.service('loaderService', [
	cloud.services.loaderService
]);

// controllers
cloud.app.controller('appController', [
	'$scope',
	'$window',
	'httpService',
	'alertService',
	'loaderService',
	'constants',
	'userTokenService',
	'FileUploader',
	'$modal',
	cloud.controllers.appController
]);

cloud.app.controller('userAccountController', [
	'$scope',
	'$window',
	'httpService',
	'alertService',
	'userTokenService',
	'loaderService',
	'constants',
	cloud.controllers.userAccountController
]);

cloud.app.controller('renameModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'userTokenService',
	'constants',
	'renameEntity',
	cloud.controllers.renameModalController
]);

cloud.app.controller('createFolderModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'constants',
	'userTokenService',
	'folderId',
	cloud.controllers.createFolderModalController
]);

cloud.app.controller('deleteConfirmModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'userTokenService',
	'constants',
	'deleteEntity',
	cloud.controllers.deleteConfirmModalController
]);

cloud.app.controller('storagesModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'alertService',
	'storages',
	cloud.controllers.storagesModalController
]);