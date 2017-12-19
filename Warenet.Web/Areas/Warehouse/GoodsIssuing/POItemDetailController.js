'use strict';

define(['appconfig'], function (app) {

    function POItemDetailController($scope, $timeout, $uibModalInstance, $filter, httpService, uiService, params) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.modalInstance = $uibModalInstance;
        vm.item = {};
        vm.ConfirmFlag = false;

        vm.mfgDateOptions = {
            showWeeks: true,
            startingDay: 1
        };

        vm.expDateOptions = {
            showWeeks: true,
            startingDay: 1
        };

        vm.txtExpireDate_Click = function () {
            vm.txtExpireDateOpen = !vm.txtExpireDateOpen;
            if (vm.item.ManufactureDate) {
                vm.expDateOptions.minDate = vm.item.ManufactureDate;
            }
        };

        vm.txtManufactureDate_Click = function () {
            vm.txtManufactureDateOpen = !vm.txtManufactureDateOpen;
            if (vm.item.ExpiryDate) {
                vm.mfgDateOptions.maxDate = vm.item.ExpiryDate;
            }
        };

        vm.showItemDialog = function () {
            uiService.showViewDialog('vw_whit1', '[Item Code]').then(function (item) {
                var dimFlag = item["Dimension Flag"];
                var uom,qty,length, width, height, weight, volume, storeSpace;

                switch (dimFlag) {
                    case "P":
                        uom = item["Packing Uom Code"];
                        qty = item["Packing Qty"];
                        length = item["Packing Length"];
                        width = item["Packing Width"];
                        height = item["Packing Height"];
                        weight = item["Packing Weight"];
                        volume = item["Packing Volume"];
                        storeSpace = item["Packing Space Area"];
                        break;
                    case "W":
                        uom = item["Whole Uom Code"];
                        qty = item["Whole Qty"];
                        length = item["Whole Length"];
                        width = item["Whole Width"];
                        height = item["Whole Height"];
                        weight = item["Whole Weight"];
                        volume = item["Whole Volume"];
                        storeSpace = item["Whole Space Area"];
                        break;
                    case "L":
                        uom = item["Loose Uom Code"];
                        qty = item["Loose Qty"];
                        length = item["Loose Length"];
                        width = item["Loose Width"];
                        height = item["Loose Height"];
                        weight = item["Loose Weight"];
                        volume = item["Loose Volume"];
                        storeSpace = item["Loose Space Area"];
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

                vm.item = {
                    ItemCode: item["Item Code"],
                    ItemName: item["Item Name"],
                    DimensionFlag: dimFlag,
                    Qty: qty,
                    UomCode: uom,
                    Length: length,
                    Width: width,
                    Height: height,
                    Weight: weight,
                    Volume: volume,
                    SpaceArea: storeSpace,
                    ManufactureDate: item["Manufacture Date"],
                    ExpiryDate: item["Expiry Date"]
                };

                $scope.frmItemDetail.$dirty = true;
                angular.element('#txtQty').focus();
            });
        };

        vm.btnClose_Click = function () {
            vm.modalInstance.close();
        };

        vm.btnAdd_Click = function () {
            vm.modalInstance.close(vm.item);
        };

        //#endregion

        function initCtrl() {
            vm.ConfirmFlag = params.ConfirmFlag;
            vm.item = params.Item;
        };

        // initialize controller
        initCtrl();
    };

    app.register.controller("POItemDetailController", POItemDetailController);

});