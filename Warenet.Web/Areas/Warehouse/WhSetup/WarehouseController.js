'use strict';

define(['appconfig'], function (app) {

    function WarehouseController($scope, $state, $stateParams, $timeout, $interval, uiGridConstants, blockUI, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        var modalInstance;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.wh = {};
        vm.whDetails = [];
        vm.storeTypeDesc = "";
        vm.cityName = "";
        vm.countryName = "";
        vm.totalPalletSpace = 0;
        vm.totalStoreSpace = 0;
        vm.gridApi = {};

        vm.gridColumns = [
            { name: 'LineItemNo', width: 100 },
            { name: 'BinNo', width: 100 },
            { name: 'Description', width: 250 },
            { name: 'UseFlag', width: 100 },
            { name: 'Length', width: 120 },
            { name: 'Width', width: 120 },
            { name: 'Height', width: 120 },
            { name: 'PalletSpace', width: 170 },
            { name: 'StoreSpace', width: 170 }
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
            data: vm.whDetails,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };
        
        vm.showStoreTypeDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_whst1', '[Store Type Code]').then(function (row) {
                    vm.wh.StoreTypeCode = row['Store Type Code'];
                    vm.StoreTypeDesc = row['Store Type Description'];
                    $scope.frmWhSetup.$dirty = true;
                    angular.element('#txtStoreType').focus();
                });
            }
        };

        vm.showCountryDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_rfcy1', '[Country Code]').then(function (row) {
                    vm.wh.CountryCode = row['Country Code'];
                    vm.CountryName = row['Country Name'];
                    $scope.frmWhSetup.$dirty = true;
                    angular.element('#txtCountryCode').focus();
                });
            }
        };

        vm.showCityDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_rfct1', '[City Code]', "[Country Code] = '" + vm.wh.CountryCode + "'")
                    .then(function (row) {
                        vm.wh.CityCode = row['City Code'];
                        vm.CityName = row['City Name'];
                        $scope.frmWhSetup.$dirty = true;
                        angular.element('#txtCityCode').focus();
                    });
            }
        };

        vm.showStorageLayoutDialog = function () {
            modalInstance = uiService.showInnerDialog('warehouse/whsetup/storagelayoutsetup', $scope);
        };

        $scope.btnOk_Click = function () {
            modalInstance.close();
            blockUI.start();
            httpService.post('api/warehouse/generatebinnos', vm.storageLayout).then(function (binNoList) {
                vm.whDetails = [];

                var lineItemNo = 1;
                var totalPalletSpace = vm.TotalPalletSpace;
                var totalStorageSpace = vm.TotalStoreSpace;

                binNoList.forEach(function (binNo) {
                    var palletSpace = vm.storageLayout.Length * vm.storageLayout.Width * vm.storageLayout.Height;
                    var storageSpace = palletSpace;

                    vm.whDetails.push({
                        WarehouseCode: vm.wh.WarehouseCode,
                        LineItemNo: lineItemNo,
                        BinNo: binNo,
                        Description: '',
                        Height: vm.storageLayout.Height,
                        Length: vm.storageLayout.Length,
                        PalletSpace: palletSpace,
                        StoreSpace: storageSpace,
                        UseFlag: 'Y',
                        Width: vm.storageLayout.Width
                    });

                    lineItemNo += 1;
                    totalPalletSpace += palletSpace;
                    totalStorageSpace += storageSpace;
                });

                vm.TotalPalletSpace = totalPalletSpace;
                vm.TotalStoreSpace = totalStorageSpace;

                vm.gridOptions.data = vm.whDetails;
                $scope.frmWhSetup.$dirty = true;

                blockUI.stop();
            },
            function () {
                blockUI.stop();
            });
        };

        $scope.btnCancel_Click = function () {
            modalInstance.dismiss();
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                if (vm.isDeleted) {
                    undeleteWarehouse();
                }
                else {
                    deleteWarehouse().then(() => {
                        showParentForm();
                    });
                }
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmWhSetup.$valid) {
                if ($scope.frmWhSetup.$dirty) {
                    saveWarehouse().then(() => {
                        showParentForm();
                    });
                }
                else showParentForm();
            }
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        //#endregion

        //#region Js Callbacks

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        //#region Api Callbacks

        function getWarehouse(whCode) {
            return httpService.get("api/warehouse/getwarehouse",
                {
                    WarehouseCode: whCode
                }).then(data => {
                    vm.wh = data.whwh1;

                    var totalPalletSpace = 0;
                    var totalStoreSpace = 0;
                    data.whwh2.forEach(function (row) {
                        vm.whDetails.push(row);
                        totalPalletSpace += row['PalletSpace'];
                        totalStoreSpace += row['StoreSpace'];
                    });
                    vm.totalPalletSpace = totalPalletSpace;
                    vm.totalStoreSpace = totalStoreSpace;

                    vm.isReadOnly = vm.wh["StatusCode"] === "DEL" ? true : false;
                    vm.isDeleted = vm.wh["StatusCode"] === "DEL" ? true : false;
                });
        }

        function saveWarehouse() {
            return httpService.post('api/warehouse/savewarehouse',
                {
                    whwh1: vm.wh,
                    whwh2: vm.whDetails
                }).then(() => {
                    vm.isEdit = true;
                });
        }

        function deleteWarehouse() {
            return httpService.get('api/warehouse/deletewarehouse',
                {
                    WarehouseCode: vm.wh["WarehouseCode"],
                    Type: 1
                }).then(() => {
                    vm.wh["StatusCode"] = "DEL";
                    vm.isReadOnly = true;
                    vm.isDeleted = true;
                });
        }

        function undeleteWarehouse() {
            return httpService.get('api/warehouse/deletewarehouse',
                {
                    WarehouseCode: vm.wh["WarehouseCode"],
                    Type: 2
                }).then(() => {
                    vm.wh["StatusCode"] = "USE";
                    vm.isReadOnly = false;
                    vm.isDeleted = false;
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            if (vm.isEdit) {
                // get warehouse
                var whCode = $stateParams.key;
                getWarehouse(whCode).then(() => {
                    // set grid top 1 row selection
                    $interval(function () {
                        vm.gridApi.selection.selectRow(vm.gridOptions.data[0]);
                    }, 0, 1);
                });
            }
            else {
                vm.wh.StatusCode = 'USE';
            }
        }

        initCtrl();
    };

    app.register.controller('WarehouseController', WarehouseController);

});