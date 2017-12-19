'use strict';

define(['appconfig'], function (app) {

    function StoreTypeController($scope, $stateParams, $state, $timeout, httpService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.storeType = {};

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                if (vm.isDeleted) {
                    undeleteStoreType();
                }
                else {
                    deleteStoreType().then(() => {
                        showParentForm();
                    });
                }
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmStoreType.$dirty) {
                saveStoreType().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        //#endregion

        //#region Js Callbacks

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        };

        //#endregion

        //#region Api Callbacks

        function getStoreType(storeTypeCode) {
            return httpService.get('api/storetype/getstoretype', 
                { 
                    StoreTypeCode: storeTypeCode
                }).then(storeType => {
                    vm.storeType = storeType;
                    vm.isReadOnly = vm.storeType["StatusCode"] === "DEL" ? true : false;
                    vm.isDeleted = vm.storeType["StatusCode"] === "DEL" ? true : false;
                });
        }

        function saveStoreType() {
            return httpService.post('api/storetype/savestoretype', vm.storeType)
                .then(() => {
                    vm.isEdit = true;
                });
        }

        function deleteStoreType() {
            return httpService.get('api/storetype/deletestoretype',
                {
                    StoreTypeCode: vm.storeType.StoreTypeCode,
                    Type: 1
                }).then(() => {
                    vm.storeType["StatusCode"] = "DEL";
                    vm.isReadOnly = true;
                    vm.isDeleted = true;
                });
        }

        function undeleteStoreType() {
            return httpService.get('api/storetype/deletestoretype',
                {
                    StoreTypeCode: vm.storeType.StoreTypeCode,
                    Type: 2
                }).then(() => {
                    vm.storeType["StatusCode"] = "USE";
                    vm.isReadOnly = false;
                    vm.isDeleted = false;
                });
        }

        //#endregion

        // initialize controller
        function initData() {
            if (vm.isEdit) {
                var storeTypeCode = $stateParams.key;
                getStoreType(storeTypeCode);
            }
            else {
                vm.storeType.StatusCode = 'USE';
            }
        };

        initData();
    }

    app.register.controller("StoreTypeController", StoreTypeController);

});