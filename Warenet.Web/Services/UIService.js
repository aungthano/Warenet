'use strict';

define(['appconfig'], function () {

    var app = angular.module('uiService', []);

    app.service('uiService', ['$rootScope', '$q', '$timeout', '$http', '$window', '$uibModal', 'blockUI', 'httpService', function ($rootScope, $q, $timeout, $http, $window, $uibModal, blockUI, httpService) {

        this.showViewDialog = function (viewName, viewKey, filter) {
            var deferred = $q.defer();
            this.showDialog('Shared/BrowseView', { ViewName: viewName, ViewKey: viewKey, Filter: filter || '' })
                .then(function (entity) {
                    deferred.resolve(entity);
                },
                function () {
                    deferred.reject();
                });

            return deferred.promise;
        }

        this.showInnerDialog = function (path, appScope) {
            if (path) {
                return $uibModal.open({
                    animation: false,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: "Areas/" + path + ".html",
                    size: 'md',
                    scope: appScope
                });
            }
        }

        this.showDialog = function (path, params, size) {
            if (path) {
                blockUI.start();
                var deferred = $q.defer();

                var templateUrl = 'Areas/' + path + '.html';
                var controllerPath = 'Areas/' + path + 'Controller';
                var parsePath = controllerPath.split('/');
                var controllerName = parsePath[parsePath.length - 1];

                resolveController(controllerPath).then(function () {
                    var frmDialog = $uibModal.open({
                        animation: false,
                        ariaLabelledBy: 'modal-title',
                        ariaDescribedBy: 'modal-body',
                        templateUrl: templateUrl,
                        controller: controllerName,
                        controllerAs: 'vm',
                        size: size || 'md',
                        resolve: {
                            params: function () {
                                return params;
                            }
                        }
                    });

                    frmDialog.opened.then(function () {
                        blockUI.stop();
                    });

                    frmDialog.result.then(function (data) {
                        deferred.resolve(data);
                    }, function () {
                        deferred.reject();
                    });
                });
                
                return deferred.promise;
            }
        }

        this.getDialog = function () {

        }

        this.showReportDialog = function (apiUrl, params) {
            blockUI.start();
            var deferred = $q.defer();

            var url = $rootScope.webApi + apiUrl;
            $http.get(url,
                {
                    params: params,
                    responseType: 'arraybuffer'
                })
            .then(function (response) {
                var rptData = new Uint8Array(response.data);
                var width = 1024;
                var height = 768;
                var left = (screen.width / 2) - (width / 2);
                var top = 30;

                blockUI.stop();
                deferred.resolve();

                //var rptViewerUrl = 'views/shared/ReportViewer.html';
                var rptViewerUrl = 'scripts/pdfjs/web/viewer.html';
                $window.open(rptViewerUrl, '_blank',
                    'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, copyhistory=no,replace=false, resizable=yes, width=' + width + ', height=' + height + ', left=' + left + ', top=' + top);
                $window.data = rptData;

            }, function () {
                blockUI.stop();
                deferred.reject();
            });

            return deferred.promise;
        }

        this.showReportDialog2 = function (apiUrl, param) {
            var url = httpService.wsBaseUrl + apiUrl;

            blockUI.start();
            var deferred = $q.defer();

            $http.get(url,
                {
                    params: param,
                    responseType: 'arraybuffer'
                })
            .then(function (response) {
                var fileArray = new Uint8Array(response.data);
                var file = new Blob([fileArray], { type: 'application/pdf' });

                var width = 1024;
                var height = 768;
                var left = (screen.width / 2) - (width / 2);
                var top = 30;

                blockUI.stop();
                
                if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                    window.navigator.msSaveOrOpenBlob(file, 'report.pdf');
                }
                else {
                    var fileUrl = URL.createObjectURL(file);
                    $window.open(fileUrl, '_blank');

                    //'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, copyhistory=no,replace=false, resizable=yes, width=' + width + ', height=' + height + ', left=' + left + ', top=' + top
                }

                deferred.resolve();

            }, function () {
                blockUI.stop();
                deferred.reject();
            });

            return deferred.promise;
        }

        function resolveController(controllerPath) {
            var deferred = $q.defer();
            require([controllerPath], function () {
                deferred.resolve();
                $rootScope.$apply();
            });
            return deferred.promise;
        }

    }]);

});