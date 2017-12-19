'use strict';

define(['appconfig'], function (app) {

    function UomController($scope, $state, $stateParams, $timeout, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.uom = {};

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                if (vm.isDeleted) {
                    undeleteUom();
                }
                else {
                    deleteUom().then(() => {
                        showParentForm();
                    });
                }
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmUom.$dirty) {
                saveUom().then(() => {
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
        }

        //#endregion

        //#region Api Callbacks

        function getUom(uomCode) {
            return httpService.get('api/uom/getuom',
                {
                    UomCode: uomCode
                })
            .then(uom => {
                vm.uom = uom;
                vm.isReadOnly = vm.uom["StatusCode"] === "DEL" ? true : false;
                vm.isDeleted = vm.uom["StatusCode"] === "DEL" ? true : false;
            });
        }

        function saveUom() {
            return httpService.post('api/uom/saveuom', vm.uom).then(() => {
                vm.isEdit = true;
            });
        }

        function deleteUom() {
            return httpService.get('api/uom/deleteuom',
                {
                    UomCode: vm.uom.UomCode,
                    Type: 1
                }).then(() => {
                    vm.uom["StatusCode"] = "DEL";
                    vm.isReadOnly = true;
                    vm.isDeleted = true;
                });
        }

        function undeleteUom() {
            return httpService.get('api/uom/deleteuom',
                {
                    UomCode: vm.uom.UomCode,
                    Type: 2
                }).then(() => {
                    vm.uom["StatusCode"] = "USE";
                    vm.isReadOnly = false;
                    vm.isDeleted = false;
                });
        }

        //#endregion

        // initialize controller
        function initData() {
            if (vm.isEdit) {
                var uomCode = $stateParams.key;
                getUom(uomCode);
            }
            else {
                vm.uom.StatusCode = "USE";
            }
        }

        initData();
    }

    app.register.controller("UomController", UomController);

});