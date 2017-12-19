'use strict';

define(['appconfig'], function (app) {

    function CityController($scope, $state, $stateParams, $timeout, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;

        //#region ViewModel

        vm.city = {};
        vm.isEdit = $stateParams.key ? true : false;

        vm.showCountryDialog = function () {
            uiService.showViewDialog('vw_rfcy1', '[Country Code]').then(function (row) {
                vm.city.CountryCode = row['Country Code'];
                vm.CountryName = row['Country Name'];
                $scope.frmCity.$dirty = true;
                angular.element('#txtCountryCode').focus();
            });
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                deleteCity().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            if ($scope.frmCity.$dirty) {
                saveCity().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        //#endregion

        //#region Api Callbacks

        function saveCity() {
            var defer = $q.defer();

            httpService.post('api/city/savecity', vm.city).then(function () {
                defer.resolve();
            });

            return defer.promise;
        }

        function deleteCity() {
            var defer = $q.defer();

            httpService.get('api/city/deletecity', { TrxNo: vm.city.TrxNo, Type: 1 })
                .then(function myfunction() {
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

        //Initialize Controller
        function initData() {
            if (vm.isEdit) {
                vm.city.TrxNo = $stateParams.key;
                httpService.get('api/city/getcity', { TrxNo: vm.city.TrxNo })
                .then(function (data) {
                    vm.city = data.city;
                    vm.CountryName = data.CountryName;
                });
            }
            else {
                vm.city.TrxNo = 0;
                vm.city.StatusCode = 'USE';
            }
        }

        initData();
    }

    app.register.controller("CityController", CityController);

});