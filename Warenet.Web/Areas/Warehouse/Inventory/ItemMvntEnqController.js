'use strict';

define(['appconfig'], function (app) {

    function ItemMvntEnqController($scope,$stateParams, $rootScope, $timeout, httpService) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.itemList = [];
        vm.warehouseCode = "";
        vm.warehouseName = "";
        vm.supplierCode = "";
        vm.supplierName = "";
        vm.receiptFromDate = new Date();
        vm.receiptToDate = new Date();
        vm.itemCode = "";
        vm.itemName = "";

        vm.gridApi = {};

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'BatchLineItemNo', visible: false, width: 50 },
            { name: 'BatchNo', displayName: 'Grn No', width: 120 },
            { name: 'ItemCode', width: 120 },
            { name: 'Description', width: 200 },
            { name: 'BinNo', width: 120 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 120 },
            { name: 'Qty', width: 60 },
            { name: 'UomCode', displayName: 'Uom', width: 100 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', visible: false },
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
            data: vm.itemList,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
            //expandableRowTemplate: "<div ui-grid=\"row.entity.subGridOptions\" style=\"height:150px;\"></div>",
            //expandableRowHeight: 150,
            ////subGridVariable will be available in subGrid scope
            //expandableRowScope: {
            //  subGridVariable: 'subGridScopeVariable'
            //}
        };

        vm.receiptDateFromOptions = {
            isOpen: false,
            startingDay: 1
        };

        vm.receiptDateToOptions = {
            isOpen: false,
            minDate: vm.receiptFromDate,
            startingDay: 1
        };

        vm.btnReceiptDateFrom_Click = function () {
            vm.receiptDateFromOptions.isOpen = true;
        };

        vm.receiptFromChanged = function () {
            if (vm.receiptToDate < vm.receiptFromDate) {
                vm.receiptToDate = vm.receiptFromDate;
            }
            vm.receiptDateToOptions.minDate = vm.receiptFromDate;
        }

        vm.btnReceiptDateTo_Click = function () {
            vm.receiptDateToOptions.isOpen = true;
        };

        vm.btnQuery_Click = function () {
            if ($scope.frmItemDetail.$valid) {
                getInvItems();
            }
        };

        //#endregion

        //#region Api Callbacks

        function getInvItems() {
            return httpService.get("api/itemmvnt/getInvItems",
                {
                    WarehouseCode: vm.warehouseCode,
                    SupplierCode: vm.supplierCode,
                    ReceiptFromDate: vm.receiptFromDate,
                    ReceiptToDate: vm.receiptToDate,
                    ItemCode: vm.itemCode
                }).then(items => {
                    vm.itemList.splice(0, vm.itemList.length);
                    items.forEach(item => {
                        vm.itemList.push(item);
                    });
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            
        }

        initCtrl();
    };

    app.register.controller('ItemMvntEnqController', ItemMvntEnqController);

});