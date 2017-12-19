'use strict';

define(['appconfig'], function (app) {

    function UserController($scope, $state, $stateParams, $timeout, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.user = {};
        vm.isEdit = $stateParams.key ? true : false;

        vm.showUserRoleDialog = function () {
            uiService.showViewDialog('vw_saur1', '[User Role Id]').then(function (row) {
                vm.user.UserRoleId = row['User Role Id'];
                vm.UserRoleName = row['User Role Name'];
                $scope.frmUser.$dirty = true;
                angular.element('#txtUserRoleId').focus();
            });
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                deleteUser().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmUser.$dirty) {
                saveUser().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.cancel = function () {
            showParentForm();
        };

        //#endregion

        //#region Api Callbacks

        function saveUser() {
            var defer = $q.defer();

            httpService.post('api/user/saveuser', vm.user).then(function () {
                defer.resolve();
            });


            return defer.promise;
        }

        function deleteUser() {
            var defer = $q.defer();

            httpService.get('api/user/deleteuser', { UserId: vm.user.UserId, Type: 1 })
                .then(function () {
                    defer.resolve();
                });

            return defer.promise;
        }

        //#endregion

        //#region Js Callbacks

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        function initData() {
            if (vm.isEdit) {
                let userId = $stateParams.key;
                httpService.get('api/user/getuser', { UserId: userId })
                    .then(function (user) {
                        vm.user = user;
                        vm.ConfirmPassword = vm.user.Password;
                    });
            }
            else {
                vm.user.StatusCode = 'USE';
            }
        }

        //#endregion

        //Initialize Controller
        initData();
    };
    
    app.register.controller("UserController", UserController);

});