'use strict';

define(['appconfig'], function (app) {

    function GoodsIssueNoteController($scope, $state, $stateParams, $timeout, $q, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isConfirm = false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.gin = {};
        vm.ginDetails = [];
        vm.gridApi = {};

        vm.gridColumns = [
            { name: 'TrxNo', visible: false },
            { name: 'LineItemNo', visible: false },
            { name: 'BatchNo', width: 120 },
            { name: 'PurchaseOrderNo', width: 120 },
            { name: 'ItemCode', width: 120 },
            { name: 'Description', width: 120 },
            { name: 'BinNo', width: 100 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 150 },
            { name: 'Qty', width: 60 },
            { name: 'UomCode', displayName: 'Uom', width: 120 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', visible: false },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', width: 100 },
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
            data: vm.ginDetails,
            rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.rowDblClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell ></div>"
        };

        vm.issueDateOptions = {
            isOpen: false,
            minDate: new Date(),
            startingDay: 1
        };

        vm.btnIssueDate_Click = function () {
            if (vm.isConfirm || vm.isDeleted || vm.isEdit) return;
            vm.issueDateOptions.isOpen = true;
        };

        vm.btnAddItem_Click = function () {
            if ($scope.frmGin.$valid) {
                showAddItemDialog().then((item) => {
                    saveGin().then(function () {
                        addItem(item);
                        $scope.frmGin.$dirty = true;
                    });
                });
            }
        };

        vm.btnDeleteItem_Click = function () {
            vm.gridApi.selection.getSelectedRows().forEach(function (entity) {
                let trxNo = entity["TrxNo"];
                let lineItemNo = entity["LineItemNo"];

                // delete item from db
                deleteGinDetail(trxNo, lineItemNo).then(function () {
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
            if ($scope.frmGin.$valid) {
                if (vm.gin.StatusCode === "USE") {
                    vm.isConfirm = true;
                    vm.gin.StatusCode = "CNF";

                    saveGin().then(() => {
                        saveToInv();
                    });
                }
                else {
                    vm.isConfirm = false;
                    vm.gin.StatusCode = "USE";
                    saveGin().then(() => {
                        deleteFromInv();
                    });
                }
                
            }
        };

        vm.btnPrint_Click = function () {
            alert('har har');
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmGin.$valid) {
                if ($scope.frmGin.$dirty) {
                    let promises = [saveGin(), saveGinDetails()];
                    $q.all(promises).then(function () {
                        showParentForm();
                    });
                }
                else showParentForm();
            }
        };

        //#endregion

        //#region Js Callbacks

        function showAddItemDialog() {
            return uiService.showDialog("Warehouse/GoodsIssuing/GinItemDetail",
                {
                    ConfirmFlag: vm.isConfirm
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
            let lineItemNo = getNewLineItemNo(vm.ginDetails, "LineItemNo");
            let myGinItem = {
                TrxNo: vm.gin.TrxNo,
                LineItemNo: lineItemNo,
                BatchNo: item["BatchNo"],
                PurchaseOrderNo: item["PurchaseOrderNo"],
                ItemCode: item["ItemCode"],
                Description: item["Description"],
                BinNo: item["BinNo"],
                DimensionFlag: item["DimensionFlag"],
                Qty: item["Qty"],
                UomCode: item["UomCode"],
                Length: item["Length"],
                Width: item["Width"],
                Height: item["Height"],
                Weight: item["Weight"],
                Volume: item["Volume"],
                SpaceArea: item["SpaceArea"],
                ManufactureDate: item["ManufactureDate"],
                ExpiryDate: item["ExpiryDate"]
            };
            vm.ginDetails.push(myGinItem);
        };

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        //#region Api Callbacks

        function getNewGinNo() {
            return httpService.get("api/goodsissuenote/getnewginno",
                {
                    IssueDate: vm.gin.IssueDate
                })
                .then(function (newGinNo) {
                    vm.gin.GoodsIssueNoteNo = newGinNo;
                });
        }

        function saveGin() {
            var defer = $q.defer();

            if (vm.isEdit) {
                // update gin
                httpService.post("api/goodsissuenote/savegin", vm.gin).then(function () {
                    defer.resolve();
                });
            }
            else {
                getNewGinNo().then(function () {
                    // insert gin
                    httpService.post("api/goodsissuenote/savegin", vm.gin).then(function (TrxNo) {
                        vm.gin.TrxNo = TrxNo;
                        vm.isEdit = true;
                        defer.resolve();
                    });
                });
            }

            return defer.promise;
        }

        function saveGinDetails() {
            var defer = $q.defer();

            if (vm.ginDetails.length > 0) {
                httpService.post("api/goodsissuenote/savegindetails", vm.ginDetails).then(function () {
                    defer.resolve();
                });
            }
            else defer.resolve();

            return defer.promise;
        }

        function saveToInv() {
            return httpService.get("api/goodsissuenote/savetoinv",
                {
                    TrxNo: vm.gin.TrxNo
                });
        }

        function deleteFromInv() {
            return httpService.get("api/goodsissuenote/deletefrominv",
                {
                    TrxNo: vm.gin.TrxNo
                });
        }

        function deleteGinDetail(trxNo, lineItemNo) {
            return httpService.get('api/goodsissuenote/deletegindetail',
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
                let promises = {
                    gin: httpService.get("api/goodsissuenote/getgin", { TrxNo: trxNo }),
                    ginDetails: httpService.get("api/goodsissuenote/getgindetails", { TrxNo: trxNo })
                };

                $q.all(promises).then(function (data) {
                    // gin header
                    vm.gin = data.gin;

                    // gin details
                    if (data.ginDetails) {
                        data.ginDetails.forEach((item) => {
                            vm.ginDetails.push(item);
                        });
                    }

                    // set confirm flag
                    vm.isConfirm = (vm.gin.StatusCode === "CNF");
                    vm.isDeleted = (vm.gin.StatusCode === "DEL");
                    vm.isReadOnly = vm.isConfirm || vm.isDeleted;

                    // format values
                    vm.gin.IssueDate = new Date(vm.gin.IssueDate);
                });
            }
            else {
                vm.gin.TrxNo = 0;
                vm.gin.GoodsIssueNoteNo = 'NEW';
                vm.gin.IssueDate = new Date();
                vm.gin.StatusCode = "USE";
            }
        }

        initCtrl();
    };

    app.register.controller('GoodsIssueNoteController', GoodsIssueNoteController);

});