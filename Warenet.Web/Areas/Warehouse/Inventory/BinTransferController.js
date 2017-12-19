'use strict';

define(['appconfig'], function (app) {

    function BinTransferController($scope, $stateParams, $q, $timeout, httpService) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.itemList = [];
        vm.warehouseCode = "";
        vm.transferBinNo = "";
        vm.binNo = "";
        vm.binDesc = "";
        vm.transferBinNo = "";
        vm.transferBinDesc = "";

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'BatchLineItemNo', visible: false },
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
            },
            columnDefs: vm.gridColumns,
            data: vm.itemList,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.btnQuery_Click = function () {
            var isValid = $scope.frmBinTransfer.txtWhCode.$valid &&
                          $scope.frmBinTransfer.txtBinNo.$valid;

            if (isValid) {
                getItemByBinNo();
            }
        };

        vm.btnUpdateBin_Click = function () {
            var isValid = $scope.frmBinTransfer.$valid &&
                            (vm.itemList.length > 0) &&
                            (vm.binNo != vm.transferBinNo);
            if (isValid) {
                getBalanceStoreSpace().then(balanceStoreSpace => {
                        var currentStoreSpace = getCurrentStoreSpace();
                        if (currentStoreSpace <= balanceStoreSpace || balanceStoreSpace === undefined) {
                            updateBinNo().then(() => {
                                    getItemByBinNo().then(() => {
                                        $timeout(function () {
                                            alert('Bin transfered success!');
                                        });
                                    });
                                });
                        }
                        else {
                            $timeout(function () {
                                alert('Bin Transfer Failed. Not enough store space to transfer!');
                            });
                        }
                    });
            }
        };

        //#endregion

        //#region Js Callbacks

        function getCurrentStoreSpace() {
            var currentStoreSpace = 0;
            vm.itemList.forEach(function (item) {
                currentStoreSpace += item['SpaceArea'];
            });
            return currentStoreSpace;
        }

        //#endregion

        //#region Api Callbacks

        function getItemByBinNo() {
            return httpService.get('api/bintransfer/getitemsbybinno',
                {
                    WarehouseCode: vm.warehouseCode,
                    BinNo: vm.binNo
                })
                .then(function (items) {
                    vm.itemList.splice(0,vm.itemList.length);
                    items.forEach(item => {
                        vm.itemList.push(item);
                    });
                });
        }

        function getBalanceStoreSpace() {
            return httpService.get('api/bintransfer/getbalancestorespacebybinno',
                {
                    WarehouseCode: vm.warehouseCode,
                    BinNo: vm.transferBinNo
                });
        }

        function updateBinNo() {
            return httpService.post('api/bintransfer/transferbinnos',
                {
                    Items: vm.itemList,
                    TransferBinNo: vm.transferBinNo
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            
        }

        initCtrl();
    };

    app.register.controller('BinTransferController', BinTransferController);

});