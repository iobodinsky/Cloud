window.cloud = window.cloud || {};

cloud.routeConfig = cloud.routeConfig || function($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/login');

    $stateProvider
        .state('login', {
            url: '/login',
            templateUrl: 'scripts/app/components/login/loginView.html',
            controller: 'loginController'
        })
        .state('register', {
            url: '/register',
            templateUrl: 'scripts/app/components/register/registerView.html',
            controller: 'registerController'
        })
        .state('cloud', {
            url: '/cloud',
            views: {
                'userAccount': {
                    templateUrl: 'scripts/app/components/userAccount/userAccountView.html',
                    controller: 'userAccountController'
                },
                '': {
                    templateUrl: 'scripts/app/components/folder/folderView.html',
                    controller: 'folderController'
                }
            }
        });
};