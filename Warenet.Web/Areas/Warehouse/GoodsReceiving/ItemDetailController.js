'use strict';

define(['appconfig'], function (app) {

    function ItemDetailController($timeout, $uibModalInstance, $filter, httpService, uiService, params) {
        // variables
        var vm = this;
        var modalInstance = $uibModalInstance;

        //#region ViewModel
        
        vm.itemDetail = {};

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
            if (vm.itemDetail.ManufactureDate) {
                vm.expDateOptions.minDate = vm.itemDetail.ManufactureDate;
            }
        };

        vm.txtManufactureDate_Click = function () {
            vm.txtManufactureDateOpen = !vm.txtManufactureDateOpen;
            if (vm.itemDetail.ExpiryDate) {
                vm.mfgDateOptions.maxDate = vm.itemDetail.ExpiryDate;
            }
        };

        vm.showUomDialog = function () {
            uiService.showViewDialog('vw_rfum1', '[Uom Code]').then(function (row) {
                vm.itemDetail.UomCode = row['Uom Code'];
                vm.UomDesc = row['Uom Description'];
                $scope.frmItemDetail.$dirty = true;
                angular.element('#txtUomCode').focus();
            });
        };

        vm.btnSave_Click = function () {
            saveAsnOrderDetail().then(() => {
                modalInstance.close(vm.itemDetail);
            });
        };

        vm.btnCancel_Click = function () {
            modalInstance.dismiss();
        };

        //#endregion

        //#region Api Callbacks

        function getAsnOrderDetail(trxNo, lineItemNo) {
            return httpService.get('api/asnorder/getasnorderdetail', { TrxNo: trxNo, LineItemNo: lineItemNo })
                .then(function (itemDetail) {
                    vm.itemDetail = itemDetail;

                    //format dates
                    if (vm.itemDetail.ManufactureDate) {
                        vm.itemDetail.ManufactureDate = new Date(vm.itemDetail.ManufactureDate);
                        //vm.dateOptions.minDate = vm.itemDetail.ManufactureDate;
                    }
                    if (vm.itemDetail.ExpiryDate) {
                        vm.itemDetail.ExpiryDate = new Date(vm.itemDetail.ExpiryDate);
                    }
                });
        }

        function saveAsnOrderDetail() {
            return httpService.post('api/asnorder/saveasnorderdetail', vm.itemDetail);
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            var trxNo = params.TrxNo;
            var lineItemNo = params.LineItemNo;

            if (trxNo || lineItemNo) {
                getAsnOrderDetail(trxNo, lineItemNo);
            }
        }

        initCtrl();
    };

    app.register.controller("ItemDetailController", ItemDetailController);

});