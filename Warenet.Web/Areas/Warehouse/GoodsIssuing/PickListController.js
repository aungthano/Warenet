'use strict';

define(['appconfig'], function (app) {

    function PickListController($scope, $q, $state, $stateParams, $rootScope, $timeout, httpService) {
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;

        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isConfirm = false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.pickList = {};
        vm.pickListDetails = [];
        vm.gridApi = {};
        vm.btnConfirmText = 'Confirm';

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'LineItemNo', visible: false},
            { name: 'BatchNo', width: 120 },
            { name: 'ItemCode', width: 120 },
            { name: 'Description', width: 200 },
            { name: 'BinNo', width: 100 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 150 },
            { name: 'Qty', width: 60 },
            { name: 'UomCode', displayName: 'Uom', width: 120 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', width: 100 },
            { name: 'ManufactureDate', visible: false },
            { name: 'ExpiryDate', visible: false }
        ];

        vm.gridOptions = {
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            enableSelectAll: false,
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: true,
            enableColumnMenus: false,
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            },
            columnDefs: vm.gridColumns,
            data: vm.pickListDetails,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.pickDateOptions = {
            isOpen: false,
            minDate: new Date(),
            startingDay: 1
        };

        vm.btnPickDate_Click = function () {
            vm.pickDateOptions.isOpen = true;
        };

        vm.btnAddItem_Click = function () {
            
        };

        vm.btnDeleteItem_Click = function () {
            
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        vm.btnPrintPickList_Click = function () {
            
        };

        vm.btnSave_Click = function () {
            if ($scope.frmPickList.$valid) {
                if ($scope.frmPickList.$dirty) {
                    let promises = [savePickList()];
                    if (vm.pickListDetails.length > 0) {
                        promises.push(savePickListDetails());
                    }

                    $q.all(promises).then(function () {
                        showParentForm();
                    });
                }
                else showParentForm();
            }
        };

        //#region Js Callbacks

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        //#region Api Callbacks

        function getPickList(trxNo) {
            let promises = {
                pickList: httpService.get("api/picklist/getpicklist", { TrxNo: trxNo }),
                pickListDetails: httpService.get("api/picklist/getpicklistdetails", { TrxNo: trxNo })
            };

            $q.all(promises).then(data => {
                // set pick list
                vm.pickList = data.pickList;

                // set pick list details
                data.pickListDetails.forEach(pickListDetail => {
                    vm.pickListDetails.push(pickListDetail);
                });

                // format and set data
                vm.pickList.PickDate = new Date(vm.pickList.PickDate);
                vm.isConfirm = (vm.pickList["StatusCode"] === "CNF");
                vm.isReadOnly = vm.pickList["StatusCode"] === "DEL" ? true : false;
                vm.isDeleted = vm.pickList["StatusCode"] === "DEL" ? true : false;
            });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            if (vm.isEdit) {
                let trxNo = $stateParams.key;
                getPickList(trxNo);
            }
        }
        
        initCtrl();
    };

    app.register.controller('PickListController', PickListController);

});