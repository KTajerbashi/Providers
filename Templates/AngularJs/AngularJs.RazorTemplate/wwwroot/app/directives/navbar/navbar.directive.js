(function () {
    'use strict';

    angular
        .module('mainApp')
        .directive('appNavbar', appNavbar);

    function appNavbar() {
        return {
            restrict: 'E', // به صورت <app-navbar></app-navbar> استفاده می‌شه
            templateUrl: '/app/directives/navbar/navbar.view.html',
            controller: NavbarController,
            controllerAs: 'vm',
            bindToController: true,
            scope: {} // isolated scope تا navbar از scope بیرونی اثر نگیره
        };
    }

    NavbarController.$inject = ['$location'];

    function NavbarController($location) {
        var vm = this;

        vm.brand = 'AngularJs.RazorTemplate';

        vm.links = [
            { label: 'Home', path: '/' },
            { label: 'About', path: '/about' },
            { label: 'Users', path: '/users' }
        ];

        // برای هایلایت کردن لینک فعال در navbar
        vm.isActive = function (path) {
            return $location.path() === path;
        };
    }

})();
