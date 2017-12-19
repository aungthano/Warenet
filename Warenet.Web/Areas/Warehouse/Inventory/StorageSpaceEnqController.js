'use strict';

define(['appconfig'], function (app) {

    function StorageSpaceEnqController($scope, $stateParams, $timeout, httpService) {
        // variables
        var vm = this;

        vm.warehouseCode = "";
        vm.warehouseName = "";
        vm.fromDate = new Date();
        vm.toDate = new Date();
        vm.binList = [];

        vm.gridColumns = [
            { name: 'BinNo', width: 100 },
            { name: 'Length', width: 120 },
            { name: 'Width', width: 120 },
            { name: 'Height', width: 120 },
            { name: 'PalletSpace', width: 170 },
            { name: 'StoreSpace', width: 170 },
            { name: 'UsedStoreSpace', width: 170 },
            { name: 'BalanceStoreSpace', width: 170 }
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
            data: vm.binList,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
            //expandableRowTemplate: "<div ui-grid=\"row.entity.subGridOptions\" style=\"height:150px;\"></div>",
            //expandableRowHeight: 150,
            ////subGridVariable will be available in subGrid scope
            //expandableRowScope: {
            //    subGridVariable: 'subGridScopeVariable'
            //}
        };

        vm.btnQuery_Click = function () {
            if ($scope.frmStoreSpaceEnq.$valid) {
                getBinList();
            }
        };

        vm.fromDateOptions = {
            isOpen: false,
            startingDay: 1
        };

        vm.btnFromDate_Click = function () {
            vm.fromDateOptions.isOpen = true;
        };

        vm.fromDate_Changed = function () {
            if (vm.toDate < vm.fromDate) {
                vm.toDate = vm.fromDate;
            }
            vm.toDateOptions.minDate = vm.fromDate;
        };

        vm.toDateOptions = {
            isOpen: false,
            minDate: vm.fromDate,
            startingDay: 1
        };

        vm.btnToDate_Click = function () {
            vm.toDateOptions.isOpen = true;
        };

        function getBinList() {
            return httpService.get("api/storagespaceenq/getbinlist",
                {
                    WarehouseCode: vm.warehouseCode,
                    FromDate: vm.fromDate,
                    ToDate: vm.toDate
                }).then(bins => {
                    vm.binList.splice(0, vm.binList.length);
                    bins.forEach(bin => {
                        vm.binList.push(bin);
                    });
                });
        }
        
        // initialize controller
        function initCtrl() {
            
        }

        initCtrl();
    };

    app.register.controller('StorageSpaceEnqController', StorageSpaceEnqController);

});