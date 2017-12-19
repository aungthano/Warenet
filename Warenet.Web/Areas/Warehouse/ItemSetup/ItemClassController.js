'use strict';

define(['appconfig'], function (app) {

    function ItemClassController($scope, $state, $stateParams, $location, $timeout, httpService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.itemClass = {};
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.btnSave_Click = function () {
            if ($scope.frmItemClass.$dirty) {
                saveItemClass().then(() => {
                    showParentForm();
                });
            }
            else showParentForm(); 
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                if (vm.isDeleted) {
                    undeleteItemClass();
                }
                else {
                    deleteItemClass().then(() => {
                        showParentForm();
                    });
                }
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
        }

        //#endregion

        //#region Api Callbacks

        function getItemClass(itemClassCode) {
            return httpService.get('api/itemclass/getitemclass',
                {
                    ItemClassCode: itemClassCode
                }).then(itemClass => {
                    vm.itemClass = itemClass;

                    vm.isReadOnly = vm.itemClass["StatusCode"] === "DEL" ? true : false;
                    vm.isDeleted = vm.itemClass["StatusCode"] === "DEL" ? true : false;
                });
        }

        function saveItemClass() {
            return httpService.post('api/itemclass/saveitemclass', vm.itemClass).then(() => {
                vm.isEdit = false;
            });
        }

        function deleteItemClass() {
            return httpService.get('api/itemclass/deleteitemclass',
                {
                    ItemClassCode: vm.itemClass.ItemClassCode,
                    Type: 1
                }).then(() => {
                    vm.itemClass["StatusCode"] = "DEL";
                    vm.isReadOnly = true;
                    vm.isDeleted = true;
                });
        }

        function undeleteItemClass() {
            return httpService.get('api/itemclass/deleteitemclass',
                {
                    ItemClassCode: vm.itemClass.ItemClassCode,
                    Type: 2
                }).then(() => {
                    vm.itemClass["StatusCode"] = "USE";
                    vm.isReadOnly = false;
                    vm.isDeleted = false;
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            if (vm.isEdit) {
                var itemClassCode = $stateParams.key;
                getItemClass(itemClassCode);
            }
            else {
                vm.itemClass.StatusCode = 'USE';
            }
        }

        initCtrl();
    };

    app.register.controller('ItemClassController', ItemClassController);

});