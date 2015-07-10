﻿window.cloud = window.cloud || {};

window.cloud.app = angular.module('cloud', [
    'ui.router',
	'angularFileUpload',
	'ui.bootstrap'
]);

window.cloud.app.config([
    '$stateProvider',
    '$urlRouterProvider',
    window.cloud.routeConfig
]);

// constants
window.cloud.app.constant('constants', window.cloud.services.constants);

// services
window.cloud.app.service('httpService', [
	'$http',
	'$window',
	'loaderService',
	'userTokenService',
	'alertService',
	'constants',
	window.cloud.services.httpService
]);

window.cloud.app.service('userTokenService', [
	'$window',
	'constants',
	window.cloud.services.userTokenService
]);

window.cloud.app.service('alertService', [
	'$timeout',
	'constants',
	window.cloud.services.alertService
]);

window.cloud.app.service('loaderService', [
	window.cloud.services.loaderService
]);

// directives
window.cloud.app.directive('cloudAlert', [
    'alertService',
    window.cloud.directives.alertDirective
]);

window.cloud.app.directive('cloudLoader', [
    'loaderService',
    window.cloud.directives.loaderDirective
]);

// controllers
window.cloud.app.controller('loginController', [
    '$scope',
    '$state',
    'httpService',
    'userTokenService',
    'constants',
    'alertService',
    window.cloud.controllers.loginController
]);

window.cloud.app.controller('registerController', [
    '$scope',
    'httpService',
    'constants',
    'alertService',
    window.cloud.controllers.registerController
]);

window.cloud.app.controller('folderController', [
	'$scope',
    '$state',
	'$window',
	'httpService',
	'alertService',
	'loaderService',
	'constants',
	'userTokenService',
	'FileUploader',
	'$modal',
	window.cloud.controllers.folderController
]);

window.cloud.app.controller('renameModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'userTokenService',
	'constants',
	'renameEntity',
	window.cloud.controllers.renameModalController
]);

window.cloud.app.controller('createFolderModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'constants',
	'userTokenService',
	'folderId',
	window.cloud.controllers.createFolderModalController
]);

window.cloud.app.controller('deleteConfirmModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'userTokenService',
	'constants',
	'deleteEntity',
	window.cloud.controllers.deleteConfirmModalController
]);

window.cloud.app.controller('storagesModalController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'alertService',
	'storages',
	window.cloud.controllers.storagesModalController
]);