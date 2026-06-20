(function () {
    'use strict';

    angular
        .module('mainApp')
        .controller('UsersController', UsersController);

    UsersController.$inject = ['$http'];

    function UsersController($http) {
        var vm = this;

        vm.title = 'لیست کاربران';
        vm.users = [];
        vm.loading = true;
        vm.error = null;

        activate();

        function activate() {
            // نمونه‌ای از فراخوانی یک API سمت سرور (مثلاً یک Controller در ASP.NET Core)
            // فعلاً چون API واقعی نداریم، از داده‌ی ساختگی استفاده می‌کنیم.
            // برای اتصال به API واقعی کافیست خط زیر را با مسیر API خودتان جایگزین کنید:
            // $http.get('/api/users').then(onSuccess, onError);

            vm.loading = false;
            vm.users = [
                { id: 1, name: 'علی رضایی', email: 'ali@example.com' },
                { id: 2, name: 'سارا محمدی', email: 'sara@example.com' },
                { id: 3, name: 'حسین کریمی', email: 'hosein@example.com' }
            ];
        }

        function onSuccess(response) {
            vm.loading = false;
            vm.users = response.data;
        }

        function onError(response) {
            vm.loading = false;
            vm.error = 'خطا در دریافت اطلاعات کاربران';
        }
    }

})();
