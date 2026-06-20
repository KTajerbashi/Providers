(function () {
    'use strict';

    angular
        .module('mainApp', ['ngRoute'])
        .config(config);

    config.$inject = ['$routeProvider', '$locationProvider'];

    function config($routeProvider, $locationProvider) {

        $routeProvider
            .when('/', {
                templateUrl: '/app/pages/home/home.view.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            })
            .when('/about', {
                templateUrl: '/app/pages/about/about.view.html',
                controller: 'AboutController',
                controllerAs: 'vm'
            })
            .when('/users', {
                templateUrl: '/app/pages/users/users.view.html',
                controller: 'UsersController',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: '/' });

        // از hash-based routing استفاده می‌کنیم (#!/...) چون با
        // فایل استاتیک Razor Pages سازگارتره و نیاز به تنظیمات سرور اضافه نداره
        $locationProvider.hashPrefix('!');
    }

})();
