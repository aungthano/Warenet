'use strict';

define(['appconfig'], function (app) {

    function ViewMasterController($state, $stateParams, $rootScope, $interval, uiGridConstants, httpService, uiService) {
        // variables
        var vm = this;
        var content = $stateParams.content;
        var module = $stateParams.module;
        
        //#region ViewModel

        vm.searchCol1 = '';
        vm.searchText1 = '';
        vm.searchOpr = '';
        vm.searchCol2 = '';
        vm.searchText2 = '';
        vm.statusFlag = '';
        vm.statusCode = '';
        vm.viewName = '';
        vm.viewKey = '';
        vm.viewFilter = '';
        vm.columns = [];
        vm.gridApi = {};

        vm.btnSearch1_Click = function () {
            if (vm.searchText1.length > 0) {
                if (vm.searchText1.substring(0, 1) != '%') {
                    vm.searchText1 = '%' + vm.searchText1;
                }
            }
            else vm.searchText1 = '%' + vm.searchText1;
            angular.element('#txtSearch1').focus();
        };

        vm.btnClear1_Click = function () {
            vm.searchText1 = '';
            angular.element('#txtSearch1').focus();
        };

        vm.btnSearch2_Click = function () {
            if (vm.searchText2.length > 0) {
                if (vm.searchText2.substring(0, 1) != '%') {
                    vm.searchText2 = '%' + vm.searchText2;
                }
            }
            else vm.searchText2 = '%' + vm.searchText2;
            angular.element('#txtSearch2').focus();
        };

        vm.btnClear2_Click = function () {
            vm.searchText2 = '';
            angular.element('#txtSearch2').focus();
        };

        vm.btnRefresh_Click = function () {
            initCtrl();
        };

        vm.btnColSetting_Click = function () {
            uiService.showDialog('Shared/ColumnSetting', { ViewName: vm.viewName }).then(function (columns) {
                initCtrl();
            });
        };

        vm.btnExport_Click = function () {

        };

        vm.btnPrint_Click = function () {

        };

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
                vm.gridApi.grid.registerRowsProcessor(vm.singleFilter);
            },
            rowTemplate: "<div ng-right-click=\"grid.appScope.vm.rowRightClick(row)\" ng-dblclick=\"grid.appScope.vm.rowDblClick(row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>",
        };

        vm.otherMenuOptions = [
          ['New', function ($itemScope, $event) {
              // show new form
              showDetailForm(null);
          }],
          ['Edit', function ($itemScope, $event) {
              // show edit form
              var selRows = vm.gridApi.selection.getSelectedRows()
              if (selRows.length > 0) {
                  var selRow = selRows[0];
                  var keyValue = selRow['@ViewKey'];
                  showDetailForm(keyValue);
              }
          }],
          null,
          ['Delete', function ($itemScope, $event) {
              // show delete confirmation form
              alert('Delete');
          }]
        ]

        //Not Finished Yet
        vm.rowHeaderClick = function () {
            //Not Finished Yet
        };

        vm.rowDblClick = function (row) {
            // show detail form
            var keyValue = row.entity['@ViewKey'];
            showDetailForm(keyValue);
        };

        vm.txtSearchText1_Changed = function () {
            vm.gridApi.grid.refresh();
        }

        vm.txtSearchText2_Changed = function () {
            vm.gridApi.grid.refresh();
        }

        vm.cboSearhOpr_Changed = function () {
            vm.gridApi.grid.refresh();
        }

        vm.cboSearch1_Changed = function () {
            vm.gridApi.grid.refresh();
        }

        vm.cboSearch2_Changed = function () {
            vm.gridApi.grid.refresh();
        }

        vm.cboStatusFlag_Changed = function () {
            vm.gridApi.grid.refresh();
        }

        vm.cboStatus_Changed = function () {
            vm.gridApi.grid.refresh();
        }

        vm.singleFilter = function (renderableRows) {
            // need to fix
            if (vm.statusCode != '' && vm.statusCode != 'ALL') {
                var pattern = vm.statusCode;
                var matcher = new RegExp(pattern, 'i');
                renderableRows.forEach(function (row) {
                    var match = false;
                    if (row.entity['Status'].match(matcher)) {
                        match = true;
                    }
                    if (!match) {
                        row.visible = false;
                    }
                });
            }

            var pattern1 = '';
            if (vm.searchCol1 && vm.searchText1.length > 0) {
                pattern1 += vm.searchText1.replace('%', '.');
            }

            var pattern2 = '';
            if (vm.searchCol2 && vm.searchText2.length > 0) {
                pattern2 = vm.searchText2.replace('%', '.');
            }

            if (vm.searchOpr === 'And') {
                if (pattern1.length > 0) {
                    var matcher = new RegExp(pattern1, 'i');
                    renderableRows.forEach(function (row) {
                        var match = false;
                        if (row.entity[vm.searchCol1.name].match(matcher)) {
                            match = true;
                        }
                        if (!match) {
                            row.visible = false;
                        }
                    });
                }

                if (pattern2.length > 0) {
                    var matcher = new RegExp(pattern2, 'i');
                    renderableRows.forEach(function (row) {
                        var match = false;
                        if (row.entity[vm.searchCol2.name].match(matcher)) {
                            match = true;
                        }
                        if (!match) {
                            row.visible = false;
                        }
                    });
                }
            }
            else {
                if (pattern1.length > 0 || pattern2.length > 0) {
                    var matcher1 = null;
                    if (pattern1.length > 0) {
                        matcher1 = new RegExp(pattern1, 'i');
                    }

                    var matcher2 = null;
                    if (pattern2.length > 0) {
                        matcher2 = new RegExp(pattern2, 'i');
                    }

                    if (matcher1 != null && matcher2 != null) {
                        renderableRows.forEach(function (row) {
                            var match = false;
                            if (row.entity[vm.searchCol1.name].match(matcher)) {
                                match = true;
                            }
                            if (!match) {
                                row.visible = false;
                            }
                        });
                    }
                    else {
                        var matcher = matcher1 ? matcher1 : matcher2;
                        var searchColName = matcher1 ? vm.searchCol1.name : vm.searchCol2.name;

                        renderableRows.forEach(function (row) {
                            var match = false;
                            if (row.entity[searchColName].match(matcher)) {
                                match = true;
                            }
                            if (!match) {
                                row.visible = false;
                            }
                        });
                    }
                }
            }

            return renderableRows;
        };

        vm.rowRightClick = function (row) {
            //Select row
            $interval(function () {
                vm.gridApi.selection.selectRow(row.entity);
            }, 0, 1);
        };

        //#endregion 

        //#region Js Callbacks

        function getColumns(viewColumns) {
            var columns = [];
            for (var i = 1; i < viewColumns.length; i++) {
                var columnName = viewColumns[i].ColumnName.replace("[", "").replace("]", "");
                var columnWidth = viewColumns[i].ColumnWidth;

                if (columnName == '@ViewKey') {
                    columns.push({ name: columnName, width: columnWidth, visible: false });
                }
                else columns.push({ name: columnName, width: columnWidth });
            }
            return columns;
        }

        function showDetailForm(keyValue) {
            var stateName = module['ModuleId'] + '.' + content.ContentId + 'detail';
            $state.go(stateName, { key: keyValue });
        }

        //#endregion

        //Initialize Controller
        function initCtrl() {

            if (content != {} && content.ViewName || '' != '' && content.ViewKey || '' != '') {
                // get view
                vm.viewName = content.ViewName;
                vm.viewKey = content.ViewKey;
                vm.viewFilter = content.ViewFilter;

                httpService.get('api/viewmaster/getview', { ViewName: vm.viewName, ViewKey: vm.viewKey, Filter: vm.viewFilter })
                .then(function (data) {
                    // get columns
                    vm.columns = getColumns(data.Columns);
                    vm.gridOptions.columnDefs = vm.columns;

                    // get data
                    vm.gridOptions.data = data.Data;

                    // register detail content state
                    var stateName = module['ModuleId'] + '.' + content.ContentId + 'detail';
                    var url = content.FormPath + '/:key';
                    var baseName = content.FormName;
                    var modulePath = module.ModulePath;
                    var path = modulePath + content.FormPath;
                    var params = { content: content };
                    if ($state.includes(stateName) === undefined) {
                        app.register.state(stateName, baseName, path, url, params);
                    }

                    // set Default Values
                    $interval(function () {
                        vm.searchCol1 = vm.columns[0];
                        vm.searchCol2 = vm.columns[1] || vm.columns[0];
                        vm.searchOpr = 'And';
                        vm.statusFlag = 'Is';
                        vm.statusCode = 'ALL';
                        vm.gridApi.selection.selectRow(vm.gridOptions.data[0]);
                    }, 0, 1);
                });
            }
        }

        initCtrl();
    };

    app.register.controller("ViewMasterController", ViewMasterController);

});