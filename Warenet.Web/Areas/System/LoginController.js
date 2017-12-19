'use strict';

define(['appconfig'], function (app) {

    function LoginController($scope, $stateParams, $rootScope, $timeout, $location, $window, $cookies, httpService, authFactory) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.Message = '';
        vm.UserId = $cookies.get('UserId');
        vm.Pwd = '';
        vm.ServerName = $cookies.get('ServerName');
        vm.Sites = {};
        vm.Site = '';

        vm.txtServerName_KeyDown = function ($event) {
            var keyCode = $event.which || $event.keyCode;
            if (keyCode === 13) {
                vm.btnSite_Click();
            }
        };

        vm.btnSite_Click = function () {
            if ($scope.frmLogin.txtServerName.$valid) {
                loadSites();
            }
            else {
                $scope.frmLogin.txtServerName.$touched = true;
                $scope.frmLogin.txtServerName.$$element[0].focus();
            }
        };

        vm.login = function () {
            if ($scope.frmLogin.$valid) {
                $cookies.put('UserId', vm.UserId);
                $cookies.put('ServerName', vm.ServerName);

                var loginInfo = CreateLoginCredentials();
                httpService.site = vm.Site;

                authFactory.login(loginInfo).then(function () {
                    $window.location = '/index.html';
                },
                function (err) {
                    vm.Message = err.error_description;
                    $scope.frmLogin.$valid = false;
                });
            }
        };

        //#endregion

        //#region Api Callbacks

        function loadSites()
        {
            httpService.wsBaseUrl = httpService.wsBaseUrl.replace("localhost", vm.ServerName);
            vm.Message = '';

            // get sites
            httpService.get('api/login/getsites').then(
                function (data) {
                    vm.Sites = data;
                    vm.Site = vm.Sites[0];
                });

            //// ping server
            //var url = 'http://' + vm.ServerName;
            //httpService.ping(url).then(function () {
            //    // set server url
            //    httpService.wsBaseUrl = httpService.wsBaseUrl.replace("localhost", vm.ServerName);
            //    vm.Message = '';

            //    // get databases
            //    httpService.get('api/login/getsites').then(
            //        function (data) {
            //            vm.Sites = data;
            //            vm.Site = vm.Sites[0];
            //        });
            //},
            //function () {
            //    vm.Message = 'Unable to connect the server!';
            //});
        }

        //#endregion

        //#region Js Callbacks

        function CreateLoginCredentials() {
            var LoginInfo = {};
            LoginInfo.userName = vm.UserId;
            LoginInfo.password = vm.Pwd;
            return LoginInfo;
        }

        //#endregion

        // initialize controller
        function initCtrl() {

            // logout current user
            if (authFactory.authentication.isAuth) {
                authFactory.logOut();
            }

            // load sites
            if (vm.ServerName) {
                loadSites();
            }
        }

        initCtrl();
    }

    app.register.controller('LoginController', LoginController);

});