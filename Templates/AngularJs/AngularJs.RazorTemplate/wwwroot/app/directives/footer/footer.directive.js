(function () {
    'use strict';

    angular
        .module('mainApp')
        .directive('appFooter', appFooter);

    function appFooter() {
        return {
            restrict: 'E',
            templateUrl: '/app/directives/footer/footer.view.html',
            controller: FooterController,
            controllerAs: 'vm',
            bindToController: true,
            scope: {}
        };
    }

    function FooterController() {
        var vm = this;
        vm.year = new Date().getFullYear();
    }

})();
