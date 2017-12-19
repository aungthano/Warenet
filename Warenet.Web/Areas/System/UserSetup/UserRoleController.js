'use strict';

define(['appconfig'], function (app) {

    function UserRoleController($scope, $state, $stateParams, $q, $timeout, httpService, uiService) {
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.userRole = {};
        vm.isEdit = $stateParams.key ? true : false;

        vm.btnSave_Click = function () {
            if ($scope.frmUserRole.$dirty) {
                saveUserRole().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                deleteUserRole().then(() => {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        //#endregion

        //#region Api Callbacks

        function getUserRole(userRoleId) {
            var defer = $q.defer();

            httpService.get('api/userrole/getuserrole', { UserRoleId: userRoleId })
                .then(userRole => {
                    defer.resolve(userRole);
                });

            return defer.promise;
        }

        function saveUserRole() {
            var defer = $q.defer();

            httpService.post("api/userrole/saveuserrole", vm.userRole).then(() => {
                defer.resolve();
            });

            return defer.promise;
        }

        function deleteUserRole(userRoleId, type) {
            var defer = $q.defer();

            httpService.get("api/userrole/deleteuserrole", { UserRoleId: userRoleId, Type: type })
                .then(() => {
                    defer.resolve();
                });

            return defer.promise;
        }

        //#endregion

        //#region Js Callbacks

        function showParentForm() {
            $timeout(function () {
                var parentState = module.ModuleId + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        // initialize controller
        function initData() {
            if (vm.isEdit) {
                var userRoleId = $stateParams.key;
                getUserRole(userRoleId).then(userRole => {
                    vm.userRole = userRole;
                });
            }
            else {
                vm.userRole.StatusCode = 'USE';
            }
        }

        initData();
    }

    app.register.controller("UserRoleController", UserRoleController);

});