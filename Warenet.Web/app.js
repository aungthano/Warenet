'use strict';

define(['appconfig',
        'authFactory', 'authInterceptorFactory',
        'barcodeFactory',
        'dateInterceptor'], function (app) {

    app.controller("appController", function ($scope, $rootScope, $location, $state, authFactory, httpService) {
        var vm = this;
        vm.appName = 'Warenet';
        vm.loginFlag = false;
        vm.authentication = authFactory.authentication;
        vm.modules = {};
        
        vm.initController = function () {
            initCtrl();
        };

        function initCtrl() {
            if (vm.authentication.isAuth) {
                // init httpService
                httpService.wsBaseUrl = vm.authentication.wsUrl;
                httpService.site = vm.authentication.site;


                // get modules
                httpService.get('api/module/getmodules').then(function (data) {
                    vm.modules = data;

                    // add module states
                    for (var i = 0; i < vm.modules.length; i++) {
                        var stateName = vm.modules[i].ModuleId;
                        var baseName = 'Module';
                        var path = 'main/module';
                        var url = '/' + vm.modules[i].ModulePath;
                        var params = { module: vm.modules[i] };
                        app.register.state(stateName, baseName, path, url, params);
                    }

                    var defaultStateName = vm.modules[0].ModuleId;
                    $state.go(defaultStateName);
                });
            }
            else {
                $state.go('login');
            }
        }

    });

});