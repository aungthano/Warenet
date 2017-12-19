'use strict';

define(['appconfig'], function (app) {

    function GrnBarcodeController($scope,$timeout, $uibModalInstance, httpService, blockUI, barcodeFactory, params) {
        // variables
        var vm = this;
        var modalInstance = $uibModalInstance;

        //#region ViewModel
        vm.itemList = [];
        vm.itemRefNo = "";

        vm.gridApi = {};

        vm.gridColumns = [
            { name: 'ItemCode', width: 150 },
            { name: 'ItemName', width: 200 },
            { name: 'DimensionFlag', displayName: 'Dim', width: 120 },
            { name: 'Qty', width: 60 },
            { name: 'UomCode', width: 120 },
            { name: 'Length', width: 100 },
            { name: 'Width', width: 100 },
            { name: 'Height', width: 100 },
            { name: 'Weight', width: 100 },
            { name: 'Volume', width: 120 },
            { name: 'SpaceArea', width: 120 }
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

        vm.btnAddItem_Click = function () {
            if ($scope.frmGrnBarcode.$valid) {
                getItemByItemRefNo().then(function (item) {
                    vm.itemList.push(item);
                    vm.itemRefNo = "";
                    angular.element("#txtBarcode").focus();
                });
            }
        };

        vm.btnCancel_Click = function () {
            modalInstance.dismiss();
        };

        vm.btnOk_Click = function () {
            modalInstance.close(vm.itemList);
        };

        //#endregion

        //#region Api Callbacks

        function getItemByItemRefNo() {
            return httpService.get('api/goodsreceiptnote/getitembyitemrefno', { ItemRefNo: vm.itemRefNo })
                .then(item => {
                    var dimFlag = item["DimensionFlag"];
                    var uom, qty, length, width, height, weight, volume, storeSpace;

                    switch (dimFlag) {
                        case "P":
                            uom = item["PackingUomCode"];
                            qty = item["PackingQty"];
                            length = item["PackingLength"];
                            width = item["PackingWidth"];
                            height = item["PackingHeight"];
                            weight = item["PackingWeight"];
                            volume = item["PackingVolume"];
                            storeSpace = item["PackingSpaceArea"];
                            break;
                        case "W":
                            uom = item["WholeUomCode"];
                            qty = item["WholeQty"];
                            length = item["WholeLength"];
                            width = item["WholeWidth"];
                            height = item["WholeHeight"];
                            weight = item["WholeWeight"];
                            volume = item["WholeVolume"];
                            storeSpace = item["WholeSpaceArea"];
                            break;
                        case "L":
                            uom = item["LooseUomCode"];
                            qty = item["LooseQty"];
                            length = item["LooseLength"];
                            width = item["LooseWidth"];
                            height = item["LooseHeight"];
                            weight = item["LooseWeight"];
                            volume = item["LooseVolume"];
                            storeSpace = item["LooseSpaceArea"];
                            break;
                        default:
                            uom = "";
                            qty = 0;
                            length = 0;
                            width = 0;
                            height = 0;
                            weight = 0;
                            volume = 0;
                            storeSpace = 0;
                            break;
                    }

                    var myItem = {
                        ItemCode: item["ItemCode"],
                        ItemName: item["ItemName"],
                        DimensionFlag: dimFlag,
                        Qty: qty,
                        UomCode: uom,
                        Length: length,
                        Width: width,
                        Height: height,
                        Weight: weight,
                        Volume: volume,
                        SpaceArea: storeSpace
                    };
                    return myItem;
                });
        }

        //#endregion

        //#region Js Callbacks

        // add new barcode item (signalR event)
        barcodeFactory.on("addNewBarcodeItemDetail", function (itemDetail) {
            let item = {
                ItemCode: itemDetail.ItemCode,
                ItemName: itemDetail.ItemName,
                DimensionFlag: itemDetail.DimensionFlag,
                Qty: itemDetail.Qty,
                UomCode: itemDetail.UomCode,
                Length: itemDetail.Length,
                Width: itemDetail.Width,
                Height: itemDetail.Height,
                Weight: itemDetail.Weight,
                Volume: itemDetail.Volume,
                SpaceArea: itemDetail.SpaceArea
            };
            vm.itemList.push(item);
        });

        //#endregion

        // initialize controller
        function initCtrl() {
            
        }

        initCtrl();
    };

    app.register.controller("GrnBarcodeController", GrnBarcodeController);

});