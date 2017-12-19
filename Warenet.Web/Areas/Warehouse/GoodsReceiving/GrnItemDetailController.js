'use strict';

define(['appconfig'], function (app) {

    function GrnItemDetailController($scope, $timeout, $uibModalInstance, $filter, httpService, uiService, params) {
        // variables
        var vm = this;
        var modalInstance = $uibModalInstance;

        //#region ViewModel
        
        vm.itemDetail = {};
        vm.warehouseCode = "";
        vm.isConfirm = false;

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

        vm.showBinDialog = function () {
            uiService.showViewDialog('vw_whwh2', '[Bin No]', "[Warehouse Code] = '" + vm.WarehouseCode + "'")
                .then(function (row) {
                    vm.itemDetail.BinNo = row['Bin No'];
                    $scope.frmItemDetail.$dirty = true;
                    angular.element('#txtBinNo').focus();
                });
        };

        vm.btnAssignBinNo_Click = function () {
            var trxNo = params.TrxNo;
            var lineItemNo = params.LineItemNo;
            assignBinNo(trxNo, lineItemNo).then(() => {
                $scope.frmItemDetail.$dirty = true;
            });
        };

        vm.btnClose_Click = function () {
            if ($scope.frmItemDetail.$valid) {
                if ($scope.frmItemDetail.$dirty) {
                    modalInstance.close(vm.itemDetail);
                }
                else modalInstance.dismiss();
            }
        };

        //#endregion

        //#region Api Callbacks

        function getGrnDetail(trxNo, lineItemNo) {
            return httpService.get('api/goodsreceiptnote/getgrndetail', { TrxNo: trxNo, LineItemNo: lineItemNo })
                .then(itemDetail => {
                    vm.itemDetail = itemDetail;

                    //format dates
                    if (vm.itemDetail.ManufactureDate) {
                        vm.itemDetail.ManufactureDate = new Date(vm.itemDetail.ManufactureDate);
                    }
                    if (vm.itemDetail.ExpiryDate) {
                        vm.itemDetail.ExpiryDate = new Date(vm.itemDetail.ExpiryDate);
                    }
                });
        }

        function assignBinNo(TrxNo, LineItemNo) {
            return httpService.get('api/goodsreceiptnote/assignbinno', { TrxNo: TrxNo, LineItemNo: LineItemNo })
                .then(function (BinNo) {
                    vm.itemDetail.BinNo = BinNo;
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            var trxNo = params.TrxNo;
            var lineItemNo = params.LineItemNo;
            vm.warehouseCode = params.WarehouseCode;
            vm.isConfirm = params.ConfirmFlag;

            if (trxNo || lineItemNo) {
                getGrnDetail(trxNo, lineItemNo);
            }
        };

        initCtrl();
    };

    app.register.controller("GrnItemDetailController", GrnItemDetailController);

});