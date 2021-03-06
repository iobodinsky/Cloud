﻿window.cloud = window.cloud || {};

window.cloud.app = angular.module('cloud', [
    'ui.router',
	'angularFileUpload',
	'ui.bootstrap',
    'ngMessages'
]);

window.cloud.app.config([
    '$stateProvider',
    '$urlRouterProvider',
    'constants',
    window.cloud.routeConfig
]);

// constants
window.cloud.app.constant('constants', window.cloud.services.constants);

// services
window.cloud.app.service('httpService', [
	'$http',
	'$window',
    '$state',
	'loaderService',
	'userTokenService',
	'alertService',
	'constants',
	window.cloud.services.httpService
]);

window.cloud.app.service('loginService', [
    '$state',
    'httpService',
    'userTokenService',
	'alertService',
	'constants',
	window.cloud.services.loginService
]);

window.cloud.app.service('userTokenService', [
	'$window',
	'constants',
	window.cloud.services.userTokenService
]);

window.cloud.app.service('folderService', [
    'httpService',
    'folderHistoryService',
    'storageService',
    'alertService',
    'constants',
    window.cloud.services.folderService
]);

window.cloud.app.service('folderHistoryService', [
	window.cloud.services.folderHistoryService
]);

window.cloud.app.service('storageService', [
    '$window',
    '$state',
    'httpService',
    'alertService',
    'constants',
    window.cloud.services.storageService
]);

window.cloud.app.service('uploadService', [
    window.cloud.services.uploadService
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
    'constants',
    window.cloud.directives.alertDirective
]);

window.cloud.app.directive('cloudLoader', [
    'loaderService',
    'constants',
    window.cloud.directives.loaderDirective
]);

window.cloud.app.directive('matchPassword', [
    window.cloud.directives.matchPasswordDirective
]);

window.cloud.app.directive('requiredDigit', [
    window.cloud.directives.requiredDigitDirective
]);

// controllers
window.cloud.app.controller('loginController', [
    '$scope',
    '$state',
    'userTokenService',
    'loginService',
    'alertService',
    'constants',
    window.cloud.controllers.loginController
]);

window.cloud.app.controller('registerController', [
    '$scope',
    'httpService',
    'loginService',
    'alertService',
    'constants',
    window.cloud.controllers.registerController
]);

window.cloud.app.controller('userAccountController', [
	'$scope',
	'$window',
    '$modal',
    '$state',
    'storageService',
	'httpService',
    'userTokenService',
	'alertService',
	'constants',
	window.cloud.controllers.userAccountController
]);

window.cloud.app.controller('folderController', [
	'$scope',
    '$state',
	'$window',
    '$modal',
	'httpService',
    'folderService',
    'userTokenService',
	'alertService',
	'loaderService',
	'constants',
	window.cloud.controllers.folderController
]);

window.cloud.app.controller('folderHistoryController', [
    '$scope',
    'folderHistoryService',
    'folderService',
    window.cloud.controllers.folderHistoryController
]);

window.cloud.app.controller('storageController', [
    '$scope',
    '$state',
    'storageService',
    'userTokenService',
    'constants',
    window.cloud.controllers.storageController
]);

window.cloud.app.controller('uploadController', [
    '$scope',
    '$modal',
    'alertService',
    'storageService',
    'folderService',
    'constants',
    window.cloud.controllers.uploadController
]);

window.cloud.app.controller('renameController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'userTokenService',
    'alertService',
	'constants',
	'renameEntity',
	window.cloud.controllers.renameController
]);

window.cloud.app.controller('createFolderController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'constants',
	'userTokenService',
	'storage',
	window.cloud.controllers.createFolderController
]);

window.cloud.app.controller('deleteConfirmController', [
	'$scope',
	'$modalInstance',
	'httpService',
	'userTokenService',
    'alertService',
	'constants',
	'deleteEntity',
	window.cloud.controllers.deleteConfirmController
]);

window.cloud.app.controller('manageStoragesController', [
	'$scope',
	'$modalInstance',
	'storageService',
	window.cloud.controllers.manageStoragesController
]);