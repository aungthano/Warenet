'use strict';

define(['appconfig'], function (app) {

    function ItemController($scope, $state, $stateParams, $timeout, $location, httpService, uiService) {
        // variables
        var vm = this;
        var module = $stateParams.module;
        var content = $stateParams.content;
        
        //#region ViewModel

        vm.isEdit = $stateParams.key ? true : false;
        vm.isReadOnly = false;
        vm.isDeleted = false;

        vm.item = {};
        vm.supplierName = "";
        vm.itemClassDesc = "";
        vm.countryName = "";

        vm.showSupplierDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_rfbp1', '[Business Party Code]', "CHARINDEX('S',[Party Type]) > 0")
                .then(function (row) {
                    vm.item.SupplierCode = row['Business Party Code'];
                    vm.SupplierName = row['Business Party Name'];
                    $scope.frmItemSetup.$dirty = true;
                    angular.element('#txtSupplierCode').focus();
                });
            }
        };

        vm.showCountryDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_rfcy1', '[Country Code]').then(function (row) {
                    vm.item.CountryOfOrigin = row['Country Code'];
                    vm.CountryName = row['Country Name'];
                    $scope.frmItemSetup.$dirty = true;
                    angular.element('#txtCountryCode').focus();
                });
            }
        };

        vm.showItemClassDialog = function () {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_whic1', '[Item Class Code]').then(function (row) {
                    vm.item.ItemClassCode = row['Item Class Code'];
                    vm.ItemClassDesc = row['Description'];
                    $scope.frmItemSetup.$dirty = true;
                    angular.element('#txtItemClassCode').focus();
                });
            }
        };

        vm.showUomDialog = function ($event) {
            if (!vm.isReadOnly) {
                uiService.showViewDialog('vw_rfum1', '[Uom Code]').then(function (row) {
                    switch ($event.target.name) {
                        case 'txtPackingUomCode':
                            vm.item.PackingUomCode = row['Uom Code'];
                            break;
                        case 'txtWholeUomCode':
                            vm.item.WholeUomCode = row['Uom Code'];
                            break;
                        case 'txtLooseUomCode':
                            vm.item.LooseUomCode = row['Uom Code'];
                            break;
                    }
                    $event.target.focus();
                    $scope.frmItemSetup.$dirty = true;
                });
            }
        };

        vm.calcVolume = function ($event) {
            var length = 0;
            var width = 0;
            var height = 0;
            var volume = 0;

            switch ($event.target.name) {
                case 'txtPackingLength': case 'txtPackingWidth': case 'txtPackingHeight':
                    if ((vm.item.PackingVolume ? vm.item.PackingVolume : 0) == 0) {
                        length = vm.item.PackingLength;
                        width = vm.item.PackingWidth;
                        height = vm.item.PackingHeight;
                        if (length && width && height) {
                            volume = length * width * height;
                            vm.item.PackingVolume = volume;
                        }
                    }
                    break;
                case 'txtWholeLength': case 'txtWholeWidth': case 'txtWholeHeight':
                    if ((vm.item.WholeVolume ? vm.item.WholeVolume : 0) == 0) {
                        length = vm.item.WholeLength;
                        width = vm.item.WholeWidth;
                        height = vm.item.WholeHeight;
                        if (length && width && height) {
                            volume = length * width * height;
                            vm.item.WholeVolume = volume;
                        }
                    }
                    
                    break;
                case 'txtLooseLength': case 'txtLooseWidth': case 'txtLooseHeight':
                    if ((vm.item.LooseVolume ? vm.item.LooseVolume : 0) == 0) {
                        length = vm.item.LooseLength;
                        width = vm.item.LooseWidth;
                        height = vm.item.LooseHeight;
                        if (length && width && height) {
                            volume = length * width * height;
                            vm.item.LooseVolume = volume;
                        }
                    }
                    break;
            }
        };

        vm.calcSpaceArea = function ($event) {
            var length = 0;
            var width = 0;
            var area = 0;

            switch ($event.target.name) {
                case 'txtPackingLength': case 'txtPackingWidth':
                    if ((vm.item.PackingSpaceArea ? vm.item.PackingSpaceArea : 0) == 0) {
                        length = vm.item.PackingLength;
                        width = vm.item.PackingWidth;
                        if (length && width) {
                            area = length * width;
                            vm.item.PackingSpaceArea = area;
                        }
                    }
                    break;
                case 'txtWholeLength': case 'txtWholeWidth':
                    if ((vm.item.WholeSpaceArea ? vm.item.WholeSpaceArea : 0) == 0) {
                        length = vm.item.WholeLength;
                        width = vm.item.WholeWidth;
                        if (length && width) {
                            area = length * width;
                            vm.item.WholeSpaceArea = area;
                        }
                    }
                    break;
                case 'txtLooseLength': case 'txtLooseWidth':
                    if ((vm.item.LooseSpaceArea ? vm.item.LooseSpaceArea : 0) == 0) {
                        length = vm.item.LooseLength;
                        width = vm.item.LooseWidth;
                        if (length && width) {
                            area = length * width;
                            vm.item.LooseSpaceArea = area;
                        }
                    }
                    break;
            }
        };

        vm.btnDelete_Click = function () {
            if (vm.isEdit) {
                if (vm.isDeleted) {
                    undeleteItem();
                }
                else {
                    deleteItem().then(() => {
                        showParentForm();
                    });
                }
            }
            else showParentForm();
        };

        vm.btnSave_Click = function () {
            var isChanged = $scope.frmItemSetup.$dirty;
            if (isChanged) {
                saveItem().then(() => {
                    showParentForm();
                });
            }
            else showParentForm();
        };

        vm.btnCancel_Click = function () {
            showParentForm();
        };

        //#endregion
        
        //#region Js Callbacks

        function showParentForm() {
            $timeout(function () {
                var parentState = module['ModuleId'] + '.' + content.ContentId;
                $state.go(parentState);
            }, 10);
        }

        //#endregion

        //#region Api Callbacks

        function getItem(itemCode) {
            return httpService.get('api/item/getitem',
                {
                    ItemCode: itemCode
                })
            .then(item => {
                vm.item = item;
                vm.isReadOnly = vm.item["StatusCode"] === "DEL" ? true : false;
                vm.isDeleted = vm.item["StatusCode"] === "DEL" ? true : false;
            });
        }

        function saveItem() {
            return httpService.post("api/item/saveitem", vm.item);
        }

        function deleteItem() {
            return httpService.get("api/item/deleteitem",
                {
                    ItemCode: vm.item.ItemCode,
                    Type: 1
                }).then(() => {
                    vm.item.StatusCode = "DEL";
                    vm.isReadOnly = true;
                    vm.isDeleted = true;
                });
        }

        function undeleteItem() {
            return httpService.get("api/item/deleteitem",
                {
                    ItemCode: vm.item.ItemCode,
                    Type: 2
                }).then(() => {
                    vm.item.StatusCode = "USE";
                    vm.isReadOnly = false;
                    vm.isDeleted = false;
                });
        }

        //#endregion

        // initialize controller
        function initCtrl() {
            if (vm.isEdit) {
                var itemCode = $stateParams.key;
                getItem(itemCode);
            }
            else {
                vm.item.StatusCode = 'USE';
            }
        }

        initCtrl();
    };

    app.register.controller('ItemController', ItemController);

});