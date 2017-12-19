'use strict';

define(['appconfig'], function (app) {

    function CountryController($scope, $state, $stateParams, $timeout, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.country = {};
        vm.isEdit = $stateParams.key ? true : false;

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                deleteCountry().then(function () {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            var isChanged = $scope.frmCountry.$dirty;
            if (isChanged) {
                saveCountry().then(function () {
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

        function saveCountry() {
            var defer = $q.defer();

            httpService.post('api/country/savecountry', vm.country).then(function () {
                defer.resolve();
            });

            return defer.promise;
        }

        function deleteCountry() {
            var defer = $q.defer();

            httpService.get('api/country/deletecountry', { CountryCode: vm.country.CountryCode, Type: 1 })
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

        //Initialize Controller
        function initData() {
            if (vm.isEdit) {
                var countryCode = $stateParams.key;
                httpService.get('api/country/getcountry', { CountryCode: countryCode })
                    .then(function (country) {
                        vm.country = country;
                    });
            }
            else {
                vm.country.StatusCode = 'USE';
            }
        }

        initData();
    }

    app.register.controller("CountryController", CountryController);

});