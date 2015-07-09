window.cloud = window.cloud || {};

cloud.app = cloud.app || angular.module('cloud', [
    'ui.router',
	'angularFileUpload',
	'ui.bootstrap'
]);

cloud.app.config([
    '$stateProvider',
    '$urlRouterProvider',
    cloud.routeConfig
]);

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
cloud.app.controller('loginController', [
    '$scope',
    'httpService',
    'userTokenService',
    'constants',
    'alertService',
    cloud.controllers.loginController
]);

cloud.app.controller('registerController', [
    '$scope',
    'httpService',
    'constants',
    'alertService',
    cloud.controllers.registerController
]);

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