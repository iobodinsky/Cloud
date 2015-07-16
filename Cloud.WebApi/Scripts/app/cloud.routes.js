window.cloud = window.cloud || {};

cloud.routeConfig = cloud.routeConfig || function($stateProvider, $urlRouterProvider, constants) {
    $stateProvider
        .state(constants.routeState.login, {
            url: '/' + constants.routeState.login,
            templateUrl: constants.viewTemplatePath.login,
            controller: 'loginController'
        })
        .state(constants.routeState.register, {
            url: '/' + constants.routeState.register,
            templateUrl: constants.viewTemplatePath.register,
            controller: 'registerController'
        })
        .state(constants.routeState.storages, {
            url: '/' + constants.routeState.storages,
            views: {
                'folderHistory': {
                    templateUrl: constants.viewTemplatePath.folderHistory,
                    controller: 'folderHistoryController'
                },
                'userAccount': {
                    templateUrl: constants.viewTemplatePath.userAccount,
                    controller: 'userAccountController'
                },
                '': {
                    templateUrl: constants.viewTemplatePath.folder,
                    controller: 'folderController'
                }
            }
        })
        .state(constants.routeState.connect, {
            url: '/' + constants.routeState.connect,
            views: {
                'userAccount': {
                    templateUrl: constants.viewTemplatePath.userAccount,
                    controller: 'userAccountController'
                },
                '': {
                    templateUrl: constants.viewTemplatePath.userStorages,
                    controller: 'userStoragesController'
                }
            }
        });

    $urlRouterProvider.otherwise('/' + constants.routeState.storages);
};