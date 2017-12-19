'use strict';

define(['appconfig'], function (app) {

    function BrowseViewController($timeout, $interval, $q, $uibModalInstance, httpService, uiGridConstants, params) {
        // variables
        var vm = this;

        //#region ViewModel
        vm.viewName = params.ViewName;
        vm.viewKey = params.ViewKey;
        vm.viewFlter = params.Filter;
        vm.modalInstance = $uibModalInstance;
        vm.columns = [];
        vm.gridApi = {};

        vm.gridOptions = {
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            enableSelectAll: false,
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: true,
            infiniteScrollRowsFromEnd: 50,
            infiniteScrollDown: true,
            enableColumnMenus: false,
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            },
            rowTemplate: "<div ng-right-click=\"grid.appScope.vm.rowRightClick(row.entity)\" ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.rowDblClick = function (row) {
            vm.keyValue = row['@ViewKey'];

            httpService.get('api/viewmaster/getrow', { ViewName: vm.viewName, ViewKey: vm.viewKey, KeyValue: vm.keyValue })
            .then(function (row) {
                vm.modalInstance.close(row);
            });
        };

        vm.rowRightClick = function (row) {
            //Select row
            $interval(function () {
                vm.gridApi.selection.selectRow(row);
            }, 0, 1);
        };

        vm.close = function () {
            vm.modalInstance.dismiss();
        };

        vm.select = function () {
            var rows = vm.gridApi.selection.getSelectedRows();
            if (rows.length > 0) {
                vm.rowDblClick(rows[0]);
            }
        };

        //#endregion

        //#region Js Callbacks

        function getColumns(viewColumns) {
            var columns = [];
            for (var i = 0; i < viewColumns.length; i++) {
                var columnName = viewColumns[i].ColumnName.replace("[", "").replace("]", "");
                var columnWidth = viewColumns[i].ColumnWidth;

                if (columnName === '@ViewKey') {
                    columns.push({ name: columnName, width: columnWidth, visible: false });
                }
                else columns.push({ name: columnName, width: columnWidth });
            }
            return columns;
        }

        //#endregion

        //Initialize Controller
        function initCtrl() {
            if (vm.viewName || '' !== '' && vm.viewKey || '' !== '') {
                var api = 'api/viewmaster/getview';
                var params = { ViewName: vm.viewName, ViewKey: vm.viewKey };
                if (vm.viewFlter || '' !== '') {
                    params['Filter'] = vm.viewFlter;
                }

                httpService.get(api,params)
                .then(function (data) {
                    vm.columns = getColumns(data.Columns);
                        
                    vm.gridOptions.columnDefs = vm.columns;
                    vm.gridOptions.data = data.Data;

                    // set default values
                    $interval(function () {
                        vm.searchCol1 = vm.columns[0];
                        vm.gridApi.selection.selectRow(vm.gridOptions.data[0]);
                    }, 0, 1);
                },
                function (response) {
                    vm.message = "Something went wrong!";
                    deferred.reject();
                });
            }
        }
        
        initCtrl();
    };

    app.register.controller("BrowseViewController", BrowseViewController);

});