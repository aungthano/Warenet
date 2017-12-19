'use strict';

define(['appconfig'], function (app) {

    function GoodsReceiptNoteController($scope, $state, $stateParams, $timeout, $interval, $window, $q, blockUI, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isConfirm = false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.grn = {};
        vm.grnDetails = [];
        vm.supplierName = "";
        vm.warehouseName = "";
        vm.userName = "";
        vm.btnConfirmText = "Confirm";
        
        vm.gridApi = {};

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'LineItemNo', visible: false },
            { name: 'ItemCode', width: 120 },
            { name: 'Description', width: 200 },
            { name: 'BinNo', width: 100 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 150 },
            { name: 'UomCode', displayName: 'Uom', width: 120 },
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
            data: vm.grnDetails,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.receiptDateOptions = {
            isOpen: false,
            minDate: new Date(),
            startingDay: 1
        };

        vm.showSupplierDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_rfbp1', '[Business Party Code]', "[Party Type] = 'S'")
                .then(function (row) {
                    vm.grn.SupplierCode = row['Business Party Code'];
                    vm.SupplierName = row['Business Party Name'];
                    $scope.frmGrn.$dirty = true;
                    angular.element('#txtSupplierCode').focus();
                });
            }
        };

        vm.showAsnOrderDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_whar1', '[Asn Order No]').then(function (row) {
                    vm.grn.AsnOrderNo = row['Asn Order No'];
                    $scope.frmGrn.$dirty = true;
                    angular.element('#txtAsnOrderNo').focus();
                });
            }
        };

        vm.showWarehouseDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_whwh1', '[Warehouse Code]').then(function (row) {
                    vm.grn.WarehouseCode = row['Warehouse Code'];
                    vm.WarehouseName = row['Warehouse Name'];
                    $scope.frmGrn.$dirty = true;
                    angular.element('#txtWarehouseCode').focus();
                });
            }
        };

        vm.showUserDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_saus1', '[User Id]').then(function (row) {
                    vm.grn.ReceiveBy = row['User Id'];
                    vm.UserName = row['User Name'];
                    $scope.frmGrn.$dirty = true;
                    angular.element('#txtReceiveBy').focus();
                });
            }
        };

        vm.rowDblClick = function (row) {
            let trxNo = row['TrxNo'];
            let lineItemNo = row['LineItemNo'];
            let warehouseCode = vm.grn.WarehouseCode;
            showItemDialog(trxNo, lineItemNo, warehouseCode).then(item => {
                if (item) {
                    var row = vm.gridApi.selection.getSelectedRows()[0];
                    updateGridRow(row, item);
                }
            });
        };

        vm.btnReceiptDate_Click = function () {
            vm.receiptDateOptions.isOpen = true;
        };

        vm.btnBarcode_Click = function () {
            if ($scope.frmGrn.$valid) {
                // queue grn in barcode item list
                queueBarcodeItem().then(function () {
                    showBarcodeDialog().then(function (items) {
                        // save grn
                        saveGrn().then(function () {
                            // unqueue grn from barcode item list
                            unqueueBarcodeItem();

                            if (items.length > 0) {
                                // add items to grid
                                blockUI.start();
                                addItems(items);
                                $scope.frmGrn.$dirty = true;
                                saveGrnDetails().then(() => {
                                    blockUI.stop();
                                });
                            }
                        });
                    });
                }, function () {
                    // unqueue grn from barcode item list
                    unqueueBarcodeItem();
                });
            }
        };

        vm.btnDeleteItem_Click = function () {
            vm.gridApi.selection.getSelectedRows().forEach(function (entity) {
                let trxNo = entity["TrxNo"];
                let lineItemNo = entity["LineItemNo"];

                // delete item from db
                deleteGrnDetail(trxNo, lineItemNo).then(() => {
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

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmGrn.$valid) {
                if ($scope.frmGrn.$dirty) {
                    let promises = [saveGrn()];
                    if (vm.grnDetails.length > 0) {
                        promises.push(saveGrnDetails());
                    }

                    $q.all(promises).then(function () {
                        showParentForm();
                    });
                }
                else showParentForm();
            }
        };

        vm.btnPutaway_Click = function () {
            if ($scope.frmGrn.$valid || vm.grnDetails.length > 0) {
                saveGrn().then(function () {
                    assignBinNo();
                    $scope.frmGrn.$dirty = false;
                });
            }
        };

        vm.btnConfirm_Click = function () {
            if ($scope.frmGrn.$valid) {
                if (vm.grn.StatusCode == 'USE') {
                    vm.isConfirm = true;
                    vm.grn.StatusCode = 'CNF';
                }
                else {
                    vm.isConfirm = false;
                    vm.grn.StatusCode = 'USE';
                }
                saveGrn();
            }
        };

        vm.btnPrint_Click = function () {
            if ($scope.frmGrn.$valid) {
                //Brower default mode
                //var url = "http://localhost/Warenet.WebApi/api/goodsreceiptnote/exportgrnreport?TrxNo=" + vm.grn.TrxNo;
                //$window.open(url);

                //gview
                var url = 'api/goodsreceiptnote/exportgrnreport?TrxNo=' + vm.grn.TrxNo;
                uiService.showReportDialog2('api/goodsreceiptnote/exportgrnreport', { TrxNo: vm.grn.TrxNo });

                //Integrate with Pdf.js
                //var TrxNo = vm.grn.TrxNo;
                //uiService.showReportDialog('goodsreceiptnote/exportgrnreport', { TrxNo: TrxNo });
            }
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                if (vm.isDeleted) {
                    undeleteGrn();
                }
                else {
                    deleteGrn().then(() => {
                        showParentForm();
                    });
                }
            }
            else showParentForm();
        };

        //#endregion

        //#region Js Callbacks

        function showItemDialog(trxNo, lineItemNo, warehouseCode) {
            return uiService.showDialog('Warehouse/GoodsReceiving/GrnItemDetail',
                {
                    TrxNo: trxNo,
                    LineItemNo: lineItemNo,
                    WarehouseCode: warehouseCode,
                    ConfirmFlag: vm.isConfirm
                });
        }

        function updateGridRow(row, entity) {
            for (var key in entity) {
                var value = entity[key];
                row[key] = value;
            }
        }

        function updateBinNos(items) {
            items.forEach(item => {
                let trxNo = item["TrxNo"];
                let lineItemNo = item["LineItemNo"];
                let binNo = item["BinNo"];
                vm.grnDetails.forEach(grnDetail => {
                    if (grnDetail.TrxNo === trxNo && grnDetail.LineItemNo === lineItemNo) {
                        grnDetail.BinNo = binNo;
                    }
                });

                var grnItem = vm.grnDetails.find(grnDetail => { return TrxNo === trxNo && LineItemNo === lineItemNo; });
                grnItem["BinNo"] = binNo;
            });
        }

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        function loadRptController() {
            var deferred = $q.defer();
            require(['Views/Shared/ReportViewerController'], function () {
                $rootScope.$apply(function () {
                    deferred.resolve();
                });
            });
            return deferred.promise;
        }

        function showBarcodeDialog() {
            return uiService.showDialog('Warehouse/GoodsReceiving/GrnBarcode',
                                        {
                                            AsnOrderNo: vm.grn.AsnOrderNo
                                        },'lg');
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
            // get new line item no
            var lineItemNo = getNewLineItemNo(vm.grnDetails,"LineItemNo");

            // add items to grid
            items.forEach(function (item) {
                let grnDetail = {
                    TrxNo: vm.grn.TrxNo,
                    LineItemNo: lineItemNo,
                    ItemCode: item['ItemCode'],
                    Description: item['ItemName'],
                    BinNo: null,
                    DimensionFlag: item['DimensionFlag'],
                    UomCode: item['UomCode'],
                    Qty: item['Qty'],
                    Length: item['Length'],
                    Width: item['Width'],
                    Height: item['Height'],
                    Weight: item['Weight'],
                    Volume: item['Volume'],
                    SpaceArea: item['SpaceArea'],
                    ManufactureDate: null,
                    ExpiryDate: null
                };
                vm.grnDetails.push(grnDetail);
                lineItemNo += 1;
            });
        }

        //#endregion

        //#region Api Callbacks

        function getGrn(trxNo) {
            return httpService.get('api/goodsreceiptnote/getGrn', { TrxNo: trxNo })
                .then(function (data) {
                    vm.grn = data.whgr1;
                    data.whgr2.forEach((item) => {
                        vm.grnDetails.push(item);
                    });

                    // format and set data
                    vm.grn.ReceiptDate = new Date(vm.grn.ReceiptDate);
                    vm.isConfirm = (vm.grn.StatusCode === "CNF");
                    vm.isReadOnly = vm.grn["StatusCode"] === "DEL" ? true : false;
                    vm.isDeleted = vm.grn["StatusCode"] === "DEL" ? true : false;

                    // check allow unconfirm
                    getInvRefCnt().then(recCnt => {
                        if (recCnt > 0) {
                            vm.isReadOnly = true;
                        }
                    });
                });
        }

        function getInvRefCnt() {
            return httpService.get("api/goodsreceiptnote/getinvrefcount",
                {
                    GrnNo: vm.grn.GoodsReceiptNoteNo
                });
        }

        function getNewGrnNo() {
            return httpService.get("api/goodsreceiptnote/getnewgrnno",
                {
                    ReceiptDate: vm.grn.ReceiptDate
                })
                .then(newGrnNo => {
                    vm.grn.GoodsReceiptNoteNo = newGrnNo;
                });
        }

        function saveGrn() {
            var defer = $q.defer();

            if (vm.isEdit) {
                // update grn
                httpService.post('api/goodsreceiptnote/savegrn', vm.grn).then(function () {
                    defer.resolve();
                });
            }
            else {
                // get new grn no
                getNewGrnNo().then(function () {
                    // insert grn
                    httpService.post('api/goodsreceiptnote/savegrn', vm.grn).then(function (TrxNo) {
                        vm.grn.TrxNo = TrxNo;
                        vm.isEdit = true;
                        defer.resolve();
                    });
                });
            }

            return defer.promise;
        }

        function saveGrnDetails() {
            return httpService.post('api/goodsreceiptnote/savegrndetails', vm.grnDetails);
        }

        function queueBarcodeItem() {
            let barcodeItem = {
                TrxNo: vm.grn.TrxNo,
                TablePrefix: 'GRN',
                CreateBy: ''
            };
            return httpService.post('api/goodsreceiptnote/savebarcodeitem', barcodeItem);
        }

        function unqueueBarcodeItem() {
            return httpService.get('api/goodsreceiptnote/deletebarcodeitem',
                {
                    TrxNo: vm.grn.TrxNo,
                    TablePrefix: 'GRN'
                });
        }

        function assignBinNo() {
            return httpService.get('api/goodsreceiptnote/assignbinnos',
                {
                    TrxNo: vm.grn.TrxNo
                }).then(items => {
                    updateBinNos(items);
                });
        }

        function deleteGrn() {
            return httpService.get("api/goodsreceiptnote/deletegrn",
                {
                    TrxNo: vm.grn.TrxNo,
                    Type: 1
                }).then(() => {
                    vm.grn.StatusCode = "DEL";
                    vm.isReadOnly = true;
                    vm.isDeleted = true;
                });
        }

        function undeleteGrn() {
            return httpService.get("api/goodsreceiptnote/deletegrn",
                {
                    TrxNo: vm.grn.TrxNo,
                    Type: 2
                }).then(() => {
                    vm.grn.StatusCode = "USE";
                    vm.isReadOnly = false;
                    vm.isDeleted = false;
                });
        }

        function deleteGrnDetail(trxNo, lineItemNo) {
            return httpService.get("api/goodsreceiptnote/deletegrndetail",
                {
                    TrxNo: trxNo,
                    LineItemNo: lineItemNo
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            if (vm.isEdit) {
                let trxNo = $stateParams.key;
                getGrn(trxNo);
            }
            else {
                vm.grn.ReceiptDate = new Date();
                vm.grn.StatusCode = "USE";
                vm.grn.GoodsReceiptNoteNo = "NEW";
            }
        }

        initCtrl();
    }

    app.register.controller('GoodsReceiptNoteController', GoodsReceiptNoteController);

});