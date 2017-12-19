'use strict';

define(['appconfig'], function (app) {

    function GrnItemListController($timeout, $uibModalInstance, $filter, httpService, uiService, blockUI, params) {
        // variables
        var vm = this;
        var modalInstance = $uibModalInstance;

        //#region ViewModel
        
        vm.warehouseCode = "";
        vm.supplierCode = "";
        vm.itemList = [];
        vm.searchCol = {};
        vm.searchText = "";
        vm.btnSelectText = 'Select All';

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'BatchLineItemNo', visible: false },
            {
                name: 'IsSelected',
                displayName: '',
                width: 30,
                type: 'boolean',
                cellTemplate: '<input type="checkbox" ng-model="row.entity.IsSelected">',
                visible: true
            },
            { name: 'BatchNo', width: 120 },
            { name: 'ItemCode', width: 120 },
            { name: 'Description', visible: false },
            { name: 'BinNo', width: 120 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 120 },
            { name: 'Qty', width: 60 },
            { name: 'UomCode', displayName:'Uom', width: 100 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', width: 120 },
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
                vm.gridApi.grid.registerRowsProcessor(vm.singleFilter);
            },
            columnDefs: vm.gridColumns,
            data: vm.itemList,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.txtSearch_TextChanged = function () {
            vm.gridApi.grid.refresh();
        };

        vm.singleFilter = function (renderableRows) {
            if (vm.searchCol && vm.searchText.length > 0) {
                var pattern = vm.searchText;
                var matcher = new RegExp(pattern, 'i');
                renderableRows.forEach(function (row) {
                    var match = false;
                    if (row.entity[vm.searchCol.name].match(matcher)) {
                        match = true;
                    }
                    if (!match) {
                        row.visible = false;
                    }
                });
            }
            return renderableRows;
        };

        vm.btnSelectAll_Click = function () {
            if (vm.itemList.length > 0) {
                if (vm.btnSelectText === "Select All") {
                    vm.btnSelectText = "Unselect All";
                    vm.itemList.forEach(item => {
                        item.IsSelected = true;
                    });
                }
                else {
                    vm.btnSelectText = "Select All";
                    vm.itemList.forEach(item => {
                        item.IsSelected = false;
                    });
                }
            }
        };

        vm.btnCancel_Click = function () {
            vm.modalInstance.dismiss();
        };

        vm.btnSelect_Click = function () {
            if (vm.itemList.length > 0) {
                var selItems = [];
                vm.itemList.forEach(function (item) {
                    var isSel = item["IsSelected"];
                    if (isSel) {
                        selItems.push(item);
                    }
                });
                modalInstance.close(selItems);
            }
            else modalInstance.dismiss();
        };

        //#endregion

        //#region Api Callbacks

        function getItems() {
            var reqParams = { WarehouseCode: vm.warehouseCode, SupplierCode: vm.supplierCode };

            // get exclude item trx nos
            let excludeTrxNos = [];
            if (params.ExcludeItems) {
                params.ExcludeItems.forEach(function (exItem) {
                    excludeTrxNos.push(exItem.TrxNo);
                });
            }
            // add exclude trx nos if exist
            if (excludeTrxNos.length > 0) reqParams["ExcludeTrxNos"] = excludeTrxNos;
            
            return httpService.get('api/grnitemlist/getitems', reqParams)
                .then(function (items) {
                    vm.itemList = items;
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            vm.warehouseCode = params.WarehouseCode;
            vm.supplierCode = params.SupplierCode;
            vm.searchCol = vm.gridColumns[3];

            if (vm.warehouseCode && vm.supplierCode) {
                getItems();
            }
        }

        initCtrl();
    };

    app.register.controller("GrnItemListController", GrnItemListController);

});