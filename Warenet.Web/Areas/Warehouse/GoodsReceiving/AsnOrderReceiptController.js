'use strict';

define(['appconfig'], function (app) {

    function AsnOrderReceiptController($scope, $state, $stateParams, $timeout, $interval, $q, blockUI, httpService, uiService, Upload) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.asn = {};
        vm.asnDetails = [];
        vm.supplierName = "";
        vm.warehouseName = "";
        vm.gridApi = {};

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'LineItemNo', visible: false },
            { name: 'ItemCode', width: 150 },
            { name: 'Description', width: 250 },
            { name: 'DimensionFlag', width: 150 },
            { name: 'UomCode', width: 120 },
            { name: 'Qty', width: 60 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', visible: false },
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
            data: vm.asnDetails,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.dateOptions = {
            isOpen: false,
            minDate: new Date(),
            startingDay: 1
        };

        vm.rowDblClick = function (row) {
            if (row) {
                let trxNo = row["TrxNo"];
                let lineItemNo = row["LineItemNo"];
                showItemDialog(trxNo, lineItemNo).then(item => {
                    var row = vm.gridApi.selection.getSelectedRows()[0];
                    updateGridRow(row, item);
                });
            }
        };

        vm.btnAsnOrderDate_Click = function () {
            vm.dateOptions.isOpen = true;
        };

        vm.showSupplierDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_rfbp1', '[Business Party Code]', "[Party Type] = 'S'")
                    .then(function (row) {
                        vm.asn.SupplierCode = row['Business Party Code'];
                        vm.SupplierName = row['Business Party Name'];
                        $scope.frmAsnOrder.$dirty = true;
                        angular.element('#txtSupplierCode').focus();
                    });
            }
        };

        vm.showWarehouseDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_whwh1', '[Warehouse Code]').then(function (row) {
                    vm.asn.WarehouseCode = row['Warehouse Code'];
                    vm.WarehouseName = row['Warehouse Name'];
                    $scope.frmAsnOrder.$dirty = true;
                    angular.element('#txtWarehouseCode').focus();
                });
            }
        };

        vm.btnImportAsn_FileChanged = function (files) {
            if (files && files.length) {
                // check valid file
                if (files.length > 0) {
                    // start block ui
                    blockUI.start();

                    var file = files[0];
                    // upload files to web api
                    uploadFile(file).then(asnOrder => {
                        if (asnOrder) {
                            // save asn order
                            saveAsnOrder().then(() => {
                                // add item into grid
                                addItems(asnOrder);
                                // save asn order details
                                saveAsnOrderDetails().then(function () {
                                    $timeout(function () {
                                        // stop block ui
                                        blockUI.stop();
                                    }, 500);
                                });
                            });
                        }
                    });
                }
            }
        };

        vm.btnImportAsn_Click = function (event) {
            if (!validateForm()) {
                event.preventDefault();
            }
        };

        vm.btnDeleteItem_Click = function () {
            vm.gridApi.selection.getSelectedRows().forEach(function (entity) {
                let trxNo = entity["TrxNo"];
                let lineItemNo = entity["LineItemNo"];

                // delete item from db
                deleteAsnOrderDetail(trxNo, lineItemNo).then(() => {
                    // delete row from ui grid
                    vm.gridApi.selection.unSelectRow(entity);
                    var rowIndex = vm.gridOptions.data.indexOf(entity);
                    vm.gridOptions.data.splice(rowIndex, 1);

                    // set row selection
                    var rowCount = vm.gridOptions.data.length;
                    if (rowCount > 0) {
                        if (vm.gridOptions.data[rowIndex]) {
                            vm.gridApi.selection.selectRow(vm.gridOptions.data[rowIndex]);
                        }
                        else {
                            vm.gridApi.selection.selectRow(vm.gridOptions.data[rowCount - 1]);
                        }
                    }
                });
            });
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                if (vm.isDeleted) {
                    undeleteAsnOrder();
                }
                else {
                    deleteAsnOrder().then(() => {
                        showParentForm();
                    });
                }
            }
            else showParentForm();
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmAsnOrder.$dirty) {
                saveAsnOrder().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        //#endregion

        //#region Js Callbacks

        function validateForm() {
            if (!$scope.frmAsnOrder.$valid) {
                var error = $scope.frmAsnOrder.$error;
                if (error.required.length > 0) {
                    var field = error.required[0];
                    var fieldName = field.$name;
                    angular.element('#' + fieldName).focus();
                    return false;
                }
            }
            else return true;
        }

        function showItemDialog(trxNo, lineItemNo) {
            return uiService.showDialog('Warehouse/GoodsReceiving/ItemDetail',
                {
                    TrxNo: trxNo,
                    LineItemNo: lineItemNo
                });
        }

        function updateGridRow(row, entity) {
            for (var key in entity) {
                var value = entity[key];
                row[key] = value;
            }
        }

        function uploadFile(file) {
            var defer = $q.defer();

            // upload file
            Upload.upload({
                url: httpService.wsBaseUrl + 'api/asnorder/uploadasnorder',
                file: file
            }).progress(function (evt) {
                var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                $scope.log = 'progress: ' + progressPercentage + '% ' +
                            evt.config.file.name + '\n' + $scope.log;
            }).success(function (asnOrder, status, headers, config) {
                defer.resolve(asnOrder);
            });

            return defer.promise;
        }

        function getNewLineItemNo(arr, prop) {
            let max = 0;
            if (arr.length > 0) {
                for (var i = 0 ; i < arr.length ; i++) {
                    if (parseInt(arr[i][prop]) > parseInt(max))
                        max = arr[i][prop];
                }
            }
            return max + 1;
        }

        function addItems(items) {
            // get max line item no
            var lineItemNo = getNewLineItemNo(vm.asnDetails, "LineItemNo");

            // add asn order details to grid
            items.forEach(function (item) {
                let asnDetail = {
                    TrxNo: vm.asn.TrxNo,
                    LineItemNo: lineItemNo,
                    ItemCode: item['Item Code'],
                    Description: item['Description'],
                    DimensionFlag: item['Dimension Flag'],
                    UomCode: item['Uom Code'],
                    Qty: item['Qty'],
                    Length: item['Length'],
                    Width: item['Width'],
                    Height: item['Height'],
                    Weight: item['Weight'],
                    Volume: item['Volume'],
                    SpaceArea: item['Space Area'],
                    ManufactureDate: item['Manufacture Date'],
                    ExpiryDate: item['Expiry Date']
                };
                vm.asnDetails.push(asnDetail);
                lineItemNo += 1;
            });
        }

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        //#region Api Callbacks

        function getAsnOrder(trxNo) {
            return httpService.get('api/asnorder/getasnorder', { TrxNo: trxNo })
                .then(data => {
                    vm.asn = data.asnOrder;
                    vm.asn.AsnOrderDate = new Date(vm.asn.AsnOrderDate);

                    data.asnOrderDetails.forEach(orderDetail => {
                        vm.asnDetails.push(orderDetail);
                    });

                    vm.isReadOnly = vm.asn["StatusCode"] === "DEL" ? true : false;
                    vm.isDeleted = vm.asn["StatusCode"] === "DEL" ? true : false;
                });
        }

        function saveAsnOrder() {
            var defer = $q.defer();
            if (vm.isEdit) {
                // update asn order
                httpService.post('api/asnorder/saveasnorder', vm.asn).then(TrxNo => {
                    vm.asn.TrxNo = TrxNo;
                    defer.resolve();
                });
            }
            else {
                // get new asn order no
                httpService.get('api/asnorder/getNewAsnOrderNo', { AsnOrderDate: vm.asn.AsnOrderDate })
                .then(function (newAsnOrderNo) {
                    vm.asn.AsnOrderNo = newAsnOrderNo;

                    // insert asn order
                    httpService.post('api/asnorder/saveasnorder', vm.asn).then(function (TrxNo) {
                        vm.isEdit = true;
                        vm.asn.TrxNo = TrxNo;
                        defer.resolve();
                    });
                });
            }
            return defer.promise;
        }

        function saveAsnOrderDetails() {
            return httpService.post('api/asnorder/saveasnorderdetails', vm.asnDetails);
        }

        function deleteAsnOrder() {
            return httpService.get('api/asnorder/deleteasnorder',
                {
                    AsnOrderNo: vm.asn.AsnOrderNo,
                    Type: 1
                })
                .then(() => {
                    vm.asn["StatusCode"] = "DEL";
                    vm.isReadOnly = true;
                    vm.isDeleted = true;
                });
        }

        function undeleteAsnOrder() {
            return httpService.get('api/asnorder/deleteasnorder',
                {
                    AsnOrderNo: vm.asn.AsnOrderNo,
                    Type: 2
                })
                .then(() => {
                    vm.asn["StatusCode"] = "USE";
                    vm.isReadOnly = false;
                    vm.isDeleted = false;
                });
        }

        function deleteAsnOrderDetail(trxNo, lineItemNo) {
            return httpService.get('api/asnorder/deleteasnorderdetail', { TrxNo: trxNo, LineItemNo: lineItemNo });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            if (vm.isEdit) {
                var trxNo = $stateParams.key;
                getAsnOrder(trxNo);
            }
            else {
                vm.asn.AsnOrderDate = new Date();
                vm.asn.StatusCode = 'USE';
            }
        }

        initCtrl();
    };

    app.register.controller('AsnOrderReceiptController', AsnOrderReceiptController);

});