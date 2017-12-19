'use strict';

define(['appconfig'], function (app) {

    function PutAwayController($scope, $stateParams, httpService, uiService) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.itemList = [];
        vm.warehouseCode = '';
        vm.warehouseName = '';
        vm.supplierCode = '';
        vm.supplierName = '';
        vm.searchColumn;
        vm.searchText = '';
        vm.btnSelectText = 'Select All';
        vm.gridApi = {};

        vm.searchColumns = [
            { key: 'GoodsReceiptNoteNo', value: 'Grn No' },
            { key: 'ItemCode', value: 'Item Code' },
            { key: 'Description', value: 'Description' },
            { key: 'BinNo', value: 'Bin No' },
            { key: 'DimensionFlag', value: 'Dim Flag' },
            { key: 'UomCode', value: 'Uom Code' },
            { key: 'Qty', value: 'Qty' },
            { key: 'Length', value: 'Length' },
            { key: 'Width', value: 'Width' },
            { key: 'Height', value: 'Height' },
            { key: 'Weight', value: 'Weight' },
            { key: 'Volume', value: 'Volume' },
            { key: 'SpaceArea', value: 'Space Area' }
        ];

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'LineItemNo', visible: false },
            {
                name: 'IsSelected',
                displayName: '',
                width: 40,
                type: 'boolean',
                cellTemplate: '<input type="checkbox" ng-model="row.entity.IsSelected">'
            },
            { name: 'GoodsReceiptNoteNo', displayName: 'Grn No', width: 120 },
            { name: 'ItemCode', displayName: 'Item Code', width: 120 },
            { name: 'Description', width: 200 },
            { name: 'BinNo', visible: true, width: 100 },
            { name: 'DimensionFlag', displayName: 'Dim Flag', width: 100 },
            { name: 'UomCode', displayName: 'Uom', width: 120 },
            { name: 'Qty', width: 60 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', displayName: 'Space Area', visible: true, width: 120 },
            { name: 'ManufactureDate', displayName: 'Manufacture Date', visible: false },
            { name: 'ExpiryDate', displayName: 'Expiry Date', visible: false }
        ];

        vm.gridOptions = {
            enableRowSelection: true,
            enableFiltering: false,
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

        vm.txtSearchText_Changed = function () {
            vm.gridApi.grid.refresh();
        };

        vm.showWarehouseDialog = function () {
            uiService.showViewDialog('vw_whwh1', '[Warehouse Code]').then(function (row) {
                vm.warehouseCode = row['Warehouse Code'];
                vm.warehouseName = row['Warehouse Name'];
                $scope.frmPutAway.$dirty = true;
                angular.element('#txtWhCode').focus();
            });
        };

        vm.showSupplierDialog = function () {
            uiService.showViewDialog('vw_rfbp1', '[Business Party Code]', "[Party Type] = 'S'")
                .then(function (row) {
                    vm.supplierCode = row['Business Party Code'];
                    vm.supplierName = row['Business Party Name'];
                    $scope.frmPutAway.$dirty = true;
                    angular.element('#txtSupplierCode').focus();
                });
        };

        vm.singleFilter = function (renderableRows) {
            if (vm.searchColumn && vm.searchText.length > 0) {
                var matcher = new RegExp(vm.searchText);
                renderableRows.forEach(function (row) {
                    var match = false;
                    if (row.entity[vm.searchColumn.key].match(matcher)) {
                        match = true;
                    }
                    if (!match) {
                        row.visible = false;
                    }
                });
            }
            return renderableRows;
        };

        vm.btnQuery_Click = function () {
            if ($scope.frmPutAway.$valid) {
                getItems();

                vm.btnSelectText = 'Select All'
                $scope.frmPutAway.$dirty = false;
            }
        };

        vm.btnSelect_Click = function () {
            if (vm.gridOptions.data.length > 0) {
                if (vm.btnSelectText == 'Select All') {
                    vm.btnSelectText = 'Unselect All';
                    vm.itemList.forEach(function (entity) {
                        entity.IsSelected = true;
                    });
                }
                else {
                    vm.btnSelectText = 'Select All'
                    vm.itemList.forEach(function (entity) {
                        entity.IsSelected = false;
                    });
                }
            }
        };

        vm.btnPutAway_Click = function () {
            if (vm.itemList.length > 0) {
                let items = [];
                vm.itemList.forEach(function (row) {
                    var isSel = row['IsSelected'];
                    var BinNo = row['BinNo'] ? row['BinNo'] : '';
                    if (isSel && BinNo == '') {
                        items.push(row);
                    }
                });

                if (items.length > 0) {
                    assignBinNos(items);
                }
            }
        };

        vm.btnPrintPutAwayList_Click = function () {
            alert("har har")
        };

        //#endregion

        //#region Js Callbacks

        function updateBinNo(entity) {
            vm.itemList.forEach(function(row) {
                if (entity.TrxNo == row.TrxNo && entity.LineItemNo == row.LineItemNo) {
                    row['BinNo'] = entity['BinNo'];
                    return;
                }
            });
        }

        //#endregion

        //#region Api Callbacks

        function getItems() {
            return httpService.get("api/putaway/getitems",
                {
                    WarehouseCode: vm.warehouseCode,
                    SupplierCode: vm.supplierCode
                })
                .then(function (items) {
                    vm.itemList.splice(0,vm.itemList.length);
                    items.forEach(item => {
                        item.IsSelected = (item.IsSelected == 1 ? true : false);
                        vm.itemList.push(item);
                    });
                });
        }

        function assignBinNos(items) {
            return httpService.post('api/putaway/assignbinnos', items)
                    .then(function (asItems) {
                        asItems.forEach(function (asItem) {
                            updateBinNo(asItem);
                        });
                    });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            vm.searchColumn = vm.searchColumns[0];
        }

        initCtrl();
    };

    app.register.controller('PutAwayController', PutAwayController);

});