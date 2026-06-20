(function () {
    'use strict';

    angular
        .module('mainApp')
        .controller('HomeController', HomeController);

    HomeController.$inject = [];

    function HomeController() {
        var vm = this;

        vm.title = 'صفحه‌ی اصلی';
        vm.counter = 0;

        vm.increment = function () {
            vm.counter++;
        };

        vm.decrement = function () {
            vm.counter--;
        };
    }

})();
