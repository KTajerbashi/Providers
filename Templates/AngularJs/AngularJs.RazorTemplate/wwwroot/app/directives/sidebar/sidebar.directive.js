(function () {
    'use strict';

    angular
        .module('mainApp')
        .directive('appSidebar', appSidebar);

    function appSidebar() {
        return {
            restrict: 'E',
            templateUrl: '/app/directives/sidebar/sidebar.view.html',
            controller: SidebarController,
            controllerAs: 'vm',
            bindToController: true,
            scope: {}
        };
    }

    SidebarController.$inject = ['$location'];

    function SidebarController($location) {
        var vm = this;

        vm.menuItems = [
            { label: 'داشبورد', path: '/', icon: 'bi-speedometer2' },
            { label: 'کاربران', path: '/users', icon: 'bi-people' },
            { label: 'تنظیمات', path: '/about', icon: 'bi-gear' }
        ];

        vm.isActive = function (path) {
            return $location.path() === path;
        };
    }

})();
