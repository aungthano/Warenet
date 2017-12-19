'use strict';

define(['appconfig'], function (app) {

    function VendorController($scope, $state, $stateParams, $timeout, $q, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;

        //#region ViewModel

        vm.businessParty = {};
        vm.isEdit = $stateParams.key ? true : false;

        vm.showWarehouseDialog = function () {
            uiService.showViewDialog('vw_whwh1', '[Warehouse Code]').then(function (row) {
                vm.businessParty.WarehouseCode = row['Warehouse Code'];
                vm.WarehouseName = row['Warehouse Name'];
                $scope.frmBusinessParty.$dirty = true;
                angular.element('#txtWarehouseCode').focus();
            });
        };

        vm.showCountryDialog = function () {
            uiService.showViewDialog('vw_rfcy1', '[Country Code]').then(function (row) {
                vm.businessParty.CountryCode = row['Country Code'];
                vm.CountryName = row['Country Name'];
                $scope.frmBusinessParty.$dirty = true;
                angular.element('#txtCountryCode').focus();
            });
        };

        vm.showCityDialog = function () {
            uiService.showViewDialog('vw_rfct1', '[City Code]').then(function (row) {
                vm.businessParty.CityCode = row['City Code'];
                vm.CityName = row['City Name'];
                $scope.frmBusinessParty.$dirty = true;
                angular.element('#txtCityCode').focus();
            });
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                deleteBusinessParty().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmBusinessParty.$valid) {
                if ($scope.frmBusinessParty.$dirty) {
                    saveBusinessParty().then(function () {
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

        //#region Api Callbacks

        function saveBusinessParty() {
            var defer = $q.defer();

            httpService.post('api/businessParty/savebusinessParty', vm.businessParty).then(function () {
                vm.isEdit = true;
                defer.resolve();
            });

            return defer.promise;
        }

        function deleteBusinessParty() {
            var defer = $q.defer();

            httpService.get('api/businessparty/deletebusinessparty',
                {
                    BusinessPartyCode: vm.businessParty.BusinessPartyCode,
                    Type: 1
                })
                .then(function () {
                    defer.resolve();
                });

            return defer.promise;
        }

        //#endregion

        //#region Js Callbacks

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        // initialize controller
        function initData() {
            if (vm.isEdit) {
                let BusinessPartyCode = $stateParams.key;

                httpService.get('api/businessparty/getbusinessparty', { BusinessPartyCode: BusinessPartyCode })
                .then(function (businessParty) {
                    vm.businessParty = businessParty;
                });
            }
            else {
                vm.businessParty.BusinessPartyCode = "";
                vm.businessParty.PartyType = 'V';
                vm.businessParty.StatusCode = 'USE';
            }
        }

        initData();
    }

    app.register.controller("VendorController", VendorController);

});