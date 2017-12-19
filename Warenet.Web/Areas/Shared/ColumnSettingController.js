'use strict';

define(['appconfig'], function (app) {

    function ColumnSettingController($timeout, $uibModalInstance, httpService, params) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.viewName = params.ViewName;
        vm.modalInstance = $uibModalInstance;

        vm.colMoveLeft = function () {
            if (vm.selVwCols.length > 0) {
                var firstIndex = vm.vwColumns.indexOf(vm.selVwCols[0]);

                vm.selVwCols.forEach(function (col) {
                    //Move selected view columns to avaliable columns
                    vm.avColumns.push(col);

                    //Remove selected view columns from view columns
                    vm.vwColumns.splice(vm.vwColumns.indexOf(col), 1);
                });

                //Remove selected view columns
                vm.selVwCols = [];

                //Set selected view columns
                if (vm.vwColumns.length > 0) {
                    if (vm.vwColumns[firstIndex] != undefined) {
                        vm.selVwCols.push(vm.vwColumns[firstIndex]);
                    }
                    else {
                        vm.selVwCols.push(vm.vwColumns[0]);
                    }
                }
            }
        };

        vm.colMoveRight = function () {
            if (vm.selAvCols.length > 0) {
                var firstIndex = vm.avColumns.indexOf(vm.selAvCols[0]);

                vm.selAvCols.forEach(function (col) {
                    //Move Selected Avaliable Columns to View Columns
                    vm.vwColumns.push(col);

                    //Remove Selected Avaliable Columns from Avaliable Columns
                    vm.avColumns.splice(vm.avColumns.indexOf(col), 1);
                });

                //Remove selected avaliable columns
                vm.selAvCols = [];

                //Set selected view columns
                if (vm.avColumns.length > 0) {
                    if (vm.avColumns[firstIndex] != undefined) {
                        vm.selAvCols.push(vm.avColumns[firstIndex]);
                    }
                    else {
                        vm.selAvCols.push(vm.avColumns[0]);
                    }
                }
            }
        };

        vm.saveChanges = function () {
            var viewName = vm.viewName;
            var viewContent = "";
            var viewWidth = "";
            vm.vwColumns.forEach(function (col) {
                viewContent += (viewContent.length > 0 ? ',' : '') + '[' + col.name + ']';
                viewWidth += (viewWidth.length > 0 ? ',' : '') + col.width.toString();
            });

            httpService.post('api/viewmaster/updatecolumninfo',
                {
                    ViewName: viewName,
                    ViewContent: viewContent,
                    ViewWidth: viewWidth
                })
            .then(function () {
                vm.modalInstance.close(vm.vwColumns);
            });
        };

        vm.close = function () {
            vm.modalInstance.dismiss('No Button Clicked')
        };

        //#endregion

        //#region Js Callbacks

        function getColumns(viewColumns) {
            var columns = [];
            for (var i = 0; i < viewColumns.length; i++) {
                var columnName = viewColumns[i].ColumnName.replace("[", "").replace("]", "");
                var columnWidth = viewColumns[i].ColumnWidth;

                columns.push({ name: columnName, width: columnWidth });
            }
            return columns;
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            httpService.get('api/viewmaster/getcolumndef', { ViewName: vm.viewName })
            .then(function (data) {
                //Get View Columns
                vm.vwColumns = getColumns(data.vwColDef);

                //Get Avaliable Columns
                if (data.avColDef != null) {
                    vm.avColumns = getColumns(data.avColDef);
                }
                else vm.avColumns = [];
            });
        };

        initCtrl();
    };

    app.register.controller("ColumnSettingController", ColumnSettingController);

});