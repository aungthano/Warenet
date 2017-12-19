'use strict';

define(['appconfig'], function (app) {

    function GinItemDetailController($scope, $timeout, $uibModalInstance, $filter, httpService, uiService, params) {
        // variables
        var vm = this;

        //#region ViewModel

        vm.modalInstance = $uibModalInstance;
        vm.item = {
            TrxNo: 0,
            LIneItemNo: 0,
            BinNo: '',
            BatchNo: '',
            BatchLineItemNo: 0,
            Description: '',
            DimensionFlag: '',
            ExpiryDate: undefined,
            Height: undefined,
            Length: undefined,
            Qty: 0,
            ManufactureDate: undefined,
            ItemCode: '',
            PurchaseOrderNo: '',
            SpaceArea: undefined,
            UomCode: '',
            Volume: undefined,
            Weight: undefined,
            Width: undefined
        };

        vm.confirmFlag = false;

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

        vm.showPoDialog = function () {
            uiService.showViewDialog("vw_whpo1", "[Purchase Order No]").then(function (po) {
                vm.item.PurchaseOrderNo = po["Purchase Order No"];
            });
        }

        vm.showItemDialog = function () {
            uiService.showViewDialog('vw_whiv1', '[Trx No]').then(function (item) {
                vm.item.BatchNo = item["Batch No"];
                vm.item.ItemCode = item["Item Code"];
                vm.item.Description = item["Description"];
                vm.item.BinNo = item["Bin No"];
                vm.item.DimensionFlag = item["Dimension Flag"];
                vm.item.Qty = item["Qty"];
                vm.item.UomCode = item["Uom Code"];
                vm.item.Length = item["Length"];
                vm.item.Width = item["Width"];
                vm.item.Height = item["Height"];
                vm.item.Weight = item["Weight"];
                vm.item.Volume = item["Volume"];
                vm.item.SpaceArea = item["Space Area"];
                vm.item.ManufactureDate = item["Manufacture Date"];
                vm.item.ExpiryDate = item["Expiry Date"];

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

        // initialize controller
        function initCtrl() {
            vm.confirmFlag = params.ConfirmFlag;
            if (params.Item)
            {
                vm.item = params.Item;
            }
        };

        initCtrl();
    };

    app.register.controller("GinItemDetailController", GinItemDetailController);

});