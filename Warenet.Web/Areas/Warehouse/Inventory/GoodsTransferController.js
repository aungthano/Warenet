'use strict';

define(['appconfig'], function (app) {

    function GoodsTransferController($scope, $stateParams, $q, $timeout, httpService, uiService) {
        // variables
        var vm = this;

        vm.columns = [];
        vm.items = [];

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
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.btnAddItem_Click = function () {
            var isValid = $scope.frmGoodsTransfer.txtWhCode.$valid &&
                            $scope.frmGoodsTransfer.txtSupplierCode.$valid;
            if (isValid) {
                showGrnItemListDialog().then(function (items) {
                    items.forEach(function (item) {
                        vm.items.push(item);
                    });
                });
            }
        };

        vm.btnUpdateBinNo_Click = function () {
            var isValid = $scope.frmGoodsTransfer.$valid &&
                            (vm.items.length > 0);
            if (isValid) {
                httpService.get('api/goodstransfer/getbalancestorespacebybinno', { WarehouseCode: vm.WarehouseCode, BinNo: vm.TransferBinNo })
                    .then(function (balanceStoreSpace) {
                        var currentStoreSpace = getCurrentStoreSpace();
                        if (currentStoreSpace <= balanceStoreSpace || balanceStoreSpace === undefined) {
                            httpService.post('api/goodstransfer/transferbinnos', { Items: vm.items, TransferBinNo: vm.TransferBinNo })
                                .then(function () {
                                    updateBinNo(vm.TransferBinNo);
                                    $timeout(function () {
                                        alert('Bin transfered success!');
                                    });
                                });
                        }
                        else {
                            alert('Bin Transfer Failed. Not enough store space to transfer!');
                        }
                    });
            }
        };

        vm.btnRemoveItem_Click = function () {
            if (vm.gridApi.selection.getSelectedRows().length > 0) {
                angular.forEach(vm.gridOptions.data, function (data) {
                    angular.forEach(vm.gridApi.selection.getSelectedRows(), function (entity, index) {
                        if (entity.$$hashKey == data.$$hashKey) {
                            vm.gridApi.selection.unSelectRow(entity);

                            //delete row from ui grid
                            $timeout(function () {
                                var rowIndex = vm.gridOptions.data.indexOf(entity)
                                vm.gridOptions.data.splice(rowIndex, 1);
                                var rowCount = vm.gridOptions.data.length;
                                if (rowCount > 0) {
                                    if (vm.gridOptions.data[rowIndex]) {
                                        vm.gridApi.selection.selectRow(vm.gridOptions.data[rowIndex]);
                                    }
                                    else {
                                        vm.gridApi.selection.selectRow(vm.gridOptions.data[rowCount - 1]);
                                    }
                                }
                            }, 0, 1);
                        }
                    });
                });
            }
        };

        function updateBinNo(binNo) {
            vm.items.forEach(function (item) {
                item['BinNo'] = binNo;
            });
            vm.gridApi.grid.refresh();
        }

        function addItems(items) {
            items.forEach(function (item) {
                var row = [];
                vm.columns.forEach(function (column) {
                    row[column.name] = item[column.name];
                });
                vm.items.push(row);
            });
        }

        function getCurrentStoreSpace() {
            var currentStoreSpace = 0;
            vm.items.forEach(function (item) {
                currentStoreSpace += item['SpaceArea'];
            });
            return currentStoreSpace;
        }

        function showGrnItemListDialog() {
            var deferred = $q.defer();
            uiService.showDialog('Warehouse/Inventory/GrnItemList',
                                {
                                    WarehouseCode: vm.WarehouseCode,
                                    SupplierCode: vm.SupplierCode,
                                    ExcludeItems: vm.items
                                }, 'lg')
                .then(function (items) {
                    deferred.resolve(items);
                },
                function () {
                    deferred.reject();
                });
            return deferred.promise;
        }

        function getColumns() {
            var columns = [];
            columns.push({ name: 'TrxNo', visible: false });
            columns.push({ name: 'BatchLineItemNo', visible: false});
            columns.push({ name: 'BatchNo', displayName: 'Batch No', width: 120, visible: true });
            columns.push({ name: 'ItemCode', displayName: 'Item Code', width: 120, visible: true });
            columns.push({ name: 'Description', displayName: 'Description', width: 200, visible: true });
            columns.push({ name: 'BinNo', displayName: 'Bin No', width: 120, visible: true });
            columns.push({ name: 'DimensionFlag', displayName: 'Dim Flag', width: 120, visible: true });
            columns.push({ name: 'Qty', displayName: 'Qty', width: 60, visible:true });
            columns.push({ name: 'UomCode', displayName: 'Uom', width: 100, visible:true });
            columns.push({ name: 'Length', displayName: 'Length' , width: 100, visible:true });
            columns.push({ name: 'Width', displayName: 'Width', width: 100, visible: true });
            columns.push({ name: 'Height', displayName: 'Height', width: 100, visible: true });
            columns.push({ name: 'Weight', visible: false });
            columns.push({ name: 'Volume', displayName:'Volume', width: 120, visible: true });
            columns.push({ name: 'SpaceArea', displayName:'Space Area' ,Width: 120, visible:true });
            columns.push({ name: 'ManufactureDate', visible: false });
            columns.push({ name: 'ExpiryDate', visible: false });
            return columns;
        }

        function initCtrl() {
            vm.columns = getColumns();
            vm.gridOptions.columnDefs = vm.columns;
            vm.gridOptions.data = vm.items;
        }

        //Initialize Controller
        initCtrl();
    }

    app.register.controller('GoodsTransferController', GoodsTransferController);

});