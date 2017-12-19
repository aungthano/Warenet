'use strict';

define(['appconfig'], function (app) {

    function GoodsAdjustController($scope, $stateParams, $rootScope, $timeout, $q, httpService, uiService) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.itemList = [];

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'BatchLineItemNo', visible: false },
            { name: 'BatchNo', width: 120 },
            { name: 'ItemCode', width: 120 },
            { name: 'Description', width: 200 },
            { name: 'BinNo', width: 120 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 120 },
            { name: 'Qty', width: 60, visible: true },
            { name: 'UomCode', displayName: 'Uom', width: 100 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', Width: 120 },
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
        };

        vm.btnSave_Click = function () {
            if ($scope.frmGoodsAdjust.$valid) {
                alert('Changes have successfully updated to the inventory!');
            }
        }

        vm.btnAddItem_Click = function () {
            var isValid = $scope.frmGoodsAdjust.txtWhCode.$valid &&
                          $scope.frmGoodsAdjust.txtSupplierCode.$valid;
            if (isValid) {
                showGrnItemListDialog().then(function (items) {
                    items.forEach(function (item) {
                        vm.itemList.push(item);
                    });
                });
            }
        };

        vm.btnRemoveItem_Click = function () {
            if (vm.gridApi.selection.getSelectedRows().length > 0) {
                angular.forEach(vm.gridOptions.data, function (data) {
                    angular.forEach(vm.gridApi.selection.getSelectedRows(), function (entity, index) {
                        if (entity.$$hashKey === data.$$hashKey) {
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

        //#endregion

        //#region Js Callbacks

        function showGrnItemListDialog() {
            var deferred = $q.defer();

            uiService.showDialog('Warehouse/Inventory/GrnItemList',
                                {
                                    WarehouseCode: vm.WarehouseCode,
                                    SupplierCode: vm.SupplierCode,
                                    ExcludeItems: vm.itemList
                                }, 'lg')
                .then(function (items) {
                    deferred.resolve(items);
                },
                function () {
                    deferred.reject();
                });
            return deferred.promise;
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            
        }

        initCtrl();
    };

    app.register.controller('GoodsAdjustController', GoodsAdjustController);

});