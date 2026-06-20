(function () {
    'use strict';

    angular
        .module('mainApp')
        .controller('AboutController', AboutController);

    AboutController.$inject = [];

    function AboutController() {
        var vm = this;

        vm.title = 'درباره ما';
        vm.description = 'این یک پروژه‌ی نمونه است که نشان می‌دهد چگونه می‌توان ' +
            'AngularJS 1.x را در بستر ASP.NET Core Razor Pages اجرا کرد؛ ' +
            'به‌طوری که پس از بارگذاری اولیه، تمام ناوبری توسط AngularJS مدیریت شود.';
    }

})();
