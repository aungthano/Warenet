'use strict';

define(['appconfig'], function (app) {

    function WavePickingController($scope, $stateParams, $rootScope, $timeout, httpService) {
        // variables
        var vm = this;

        //#region ViewModel
        
        vm.warehouseCode = "";
        vm.warehouseName = "";
        vm.vendorCode = "";
        vm.vendorName = "";
        vm.searchColumn = {};
        vm.searchText = "";
        vm.waveBy = "A";
        vm.btnSelectText = "Select All";
        vm.itemList = [];
        vm.gridApi = {};

        vm.poDateOptions = {
            isOpen: false,
            minDate: new Date(),
            startingDay: 1
        };

        vm.searchColumns = [
            { key: 'GoodsIssueNoteNo', value: 'Gin No' },
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
            { name: 'BatchLineItemNo', visible: false },
            {
                name: 'IsSelected',
                displayName: '',
                width: 40,
                type: 'boolean',
                cellTemplate: '<input type="checkbox" ng-model="row.entity.IsSelected">'
            },
            { name: 'BatchNo', displayName: 'Gin No', width: 120 },
            { name: 'ItemCode', width: 120 },
            { name: 'Description', width: 200 },
            { name: 'BinNo', width: 100 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 60 },
            { name: 'UomCode', displayName: 'Uom', width: 100 },
            { name: 'Qty', width: 60 },
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
            getItemList();
        };

        vm.btnSelectAll_Click = function () {
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

        vm.btnGeneratePickList_Click = function () {
            if ($scope.frmWavePicking.$valid) {
                generatePickList().then(pickNos => {
                    $timeout(() => {
                        alert("Wave Picking created below pick list:\n" + pickNos.toString());
                    }, 30)
                });
            }
        };

        //#endregion

        //#region Api Callbacks

        function getItemList() {
            return httpService.get("api/wavepicking/getinvitemlist",
                {
                    WarehouseCode: vm.warehouseCode,
                    VendorCode: vm.vendorCode
                }).then(invItemList => {
                    vm.itemList.splice(0, vm.itemList.length);
                    invItemList.forEach(item => {
                        vm.itemList.push(item);
                    });
                });
        }

        function generatePickList() {
            return httpService.post("api/wavepicking/generatepicklist",
                {
                    InvItemList: vm.itemList,
                    WarehouseCode: vm.warehouseCode,
                    WaveBy: vm.waveBy
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            vm.searchColumn = vm.searchColumns[0];
        }

        initCtrl();
    };

    app.register.controller('WavePickingController', WavePickingController);

});