'use strict';

define(['appconfig'], function (app) {

    function PurchaseOrderController($scope, $state, $stateParams, $timeout, $q, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        var isEdit = $stateParams.key ? true : false;

        //#region ViewModel

        vm.po = {};
        vm.poDetails = [];
        vm.confirmFlag = false;
        vm.btnConfirmText = "Confirm";
        vm.gridApi = {};

        vm.gridColumns = [
            { name: 'TrxNo', displayName: 'Trx No', visible: false },
            { name: 'LineItemNo', displayName: 'Line Item No', visible: false, width: 50 },
            { name: 'ItemCode', displayName: 'Item Code', width: 120 },
            { name: 'Description', width: 200 },
            { name: 'DimensionFlag', displayName: 'Dimension', width: 120 },
            { name: 'Qty', width: 60 },
            { name: 'UomCode', displayName: 'Uom', width: 100 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', displayName: 'Space Area', visible: false },
            { name: 'ManufactureDate', displayName: 'Manufacture Date', visible: false },
            { name: 'ExpiryDate', displayName: 'Expiry Date', visible: false }
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
            data: vm.poDetails,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.poDateOptions = {
            isOpen: false,
            minDate: new Date(),
            startingDay: 1
        };

        vm.btnPoDate_Click = function () {
            vm.poDateOptions.isOpen = true;
        };

        vm.btnAddItem_Click = function () {
            if ($scope.frmPo.$valid) {
                showAddItemDialog().then((item) => {
                    savePo().then(function () {
                        addItem(item);
                        $scope.frmPo.$dirty = true;
                    });
                });
            }
        };

        vm.btnDeleteItem_Click = function () {
            vm.gridApi.selection.getSelectedRows().forEach(function (entity) {
                let TrxNo = entity["TrxNo"];
                let LineItemNo = entity["LineItemNo"];

                // delete item from db
                deletePoDetail(TrxNo, LineItemNo).then(function () {
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

        vm.btnConfirm_Click = function () {
            if ($scope.frmPo.$valid) {
                if (vm.po.StatusCode == 'USE') {
                    vm.confirmFlag = true;
                    vm.po.StatusCode = 'CNF';
                    vm.btnConfirmText = 'Unconfirm';
                }
                else {
                    vm.confirmFlag = false;
                    vm.po.StatusCode = 'USE';
                    vm.btnConfirmText = 'Confirm';
                }
                saveGrn();
            }
        };

        vm.btnPrint_Click = function () {
            alert('har har');
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmPo.$valid) {
                if ($scope.frmPo.$dirty) {
                    let promises = [savePo(), savePoDetails()];
                    $q.all(promises).then(function () {
                        showParentForm();
                    });
                }
                else showParentForm();
            }
        };

        //#endregion

        //#region Api Callbacks

        function getNewPoNo() {
            var defer = $q.defer();

            httpService.get("api/purchaseorder/getnewpono", { PurchaseOrderDate: vm.po.PurchaseOrderDate })
                .then(function (newPoNo) {
                    vm.po.PurchaseOrderNo = newPoNo;
                    defer.resolve();
                });

            return defer.promise;
        }

        function savePo() {
            var defer = $q.defer();

            if (isEdit) {
                // update po
                httpService.post("api/purchaseorder/savepo", vm.po).then(function () {
                    defer.resolve();
                });
            }
            else {
                getNewPoNo().then(function () {
                    // insert po
                    httpService.post("api/purchaseorder/savepo", vm.po).then(function (TrxNo) {
                        vm.po.TrxNo = TrxNo;
                        isEdit = true;
                        defer.resolve();
                    });
                });
            }

            return defer.promise;
        }

        function savePoDetails() {
            var defer = $q.defer();

            if (vm.poDetails.length > 0) {
                httpService.post("api/purchaseorder/savepodetails", vm.poDetails).then(function () {
                    defer.resolve();
                });
            }
            else defer.resolve();

            return defer.promise;
        }

        function deletePoDetail(trxNo, lineItemNo) {
            var defer = $q.defer();

            httpService.get('api/purchaseorder/deletepodetail', { TrxNo: trxNo, LineItemNo: lineItemNo })
                .then(function () {
                    defer.resolve();
                });

            return defer.promise;
        }

        //#endregion

        //#region Js Callbacks

        function showAddItemDialog() {
            return uiService.showDialog("Warehouse/GoodsIssuing/POItemDetail",
                {
                    Item: {},
                    ConfirmFlag: vm.confirmFlag
                });
        };

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

        function addItem(item) {
            let lineItemNo = getNewLineItemNo(vm.poDetails, "LineItemNo");
            let poItem = {
                TrxNo: vm.po.TrxNo,
                LineItemNo: lineItemNo,
                ItemCode: item.ItemCode,
                Description: item.ItemName,
                DimensionFlag: item.DimensionFlag,
                Qty: item.Qty,
                UomCode: item.UomCode,
                Length: item.Length,
                Width: item.Width,
                Height: item.Height,
                Weight: item.Weight,
                Volume: item.Volume,
                SpaceArea: item.SpaceArea,
                ManufactureDate: item.ManufactureDate,
                ExpiryDate: item.ExpiryDate
            };
            vm.poDetails.push(poItem);
        }

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            if (isEdit) {
                let trxNo = $stateParams.key;
                let promises = {
                    po: httpService.get("api/purchaseorder/getpo", { TrxNo: trxNo }),
                    poDetail: httpService.get("api/purchaseorder/getpodetails", { TrxNo: trxNo })
                };

                $q.all(promises).then(function (data) {
                    // set po
                    vm.po = data.po;

                    // set po details
                    data.poDetail.forEach((item) => {
                        vm.poDetails.push(item);
                    });

                    // set confirm flag, text
                    if (vm.po.StatusCode == 'CNF') {
                        vm.confirmFlag = true;
                        vm.btnConfirmText = 'Unconfirm';
                    }

                    // format values
                    vm.po.PurchaseOrderDate = new Date(vm.po.PurchaseOrderDate);
                });
            }
            else {
                vm.po.TrxNo = 0;
                vm.po.PurchaseOrderNo = "NEW";
                vm.po.PurchaseOrderDate = new Date();
                vm.po.StatusCode = "USE";
            }
        }

        initCtrl();
    };

    app.register.controller('PurchaseOrderController', PurchaseOrderController);

});