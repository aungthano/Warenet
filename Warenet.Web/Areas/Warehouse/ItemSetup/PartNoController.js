'use strict';

define(['appconfig'], function (app) {

    function PartNoController($scope, $rootScope, $stateParams, $timeout, $location, httpService, uiService) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.partNo = {};

        vm.showItemDialog = function () {
            uiService.showViewDialog('vw_whit1', '[Item Code]').then(function (row) {
                vm.partNo.ItemCode = row['Item Code'];
                vm.ItemName = row['Item Name'];
                $scope.frmPartNo.$dirty = true;
                angular.element('#txtItemCode').focus();
            });
        };

        vm.btnSave_Click = function () {
            if ($scope.frmPartNo.$dirty) {
                savePartNo().then(() => {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        vm.btnDelete_Click = function () {
            
        }

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

        function getPartNo(trxNo) {
            return httpService.get('partno/getpartno',
                {
                    TrxNo: trxNo
                })
            .then(partNo => {
                vm.partNo = partNo;
            });
        }

        function savePartNo() {
            return httpService.post('partno/savepartno', vm.partNo);
        }

        function deletePartNo() {

        }

        //#endregion

        function initCtrl() {
            if (vm.isEdit) {
                let trxNo = $stateParams.key;
                getPartNo(trxNo);
            }
        }

        // initialize controller
        initCtrl();
    };

    app.register.controller('PartNoController', PartNoController);

});