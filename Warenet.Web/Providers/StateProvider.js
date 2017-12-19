'use strict';

define(['appconfig'], function () {

    var stateProvider = angular.module('stateProvider', []);

    stateProvider.provider('stateResolver', function () {

        this.$get = function () {
            return this;
        };

        this.stateConfig = function () {
            var viewsDirectory = 'areas/',
                controllersDirectory = 'areas/',

            setBaseDirectories = function (viewsDir, controllersDir) {
                viewsDirectory = viewsDir;
                controllersDirectory = controllersDir;
            },

            getViewsDirectory = function () {
                return viewsDirectory;
            },

            getControllersDirectory = function () {
                return controllersDirectory;
            };

            return {
                setBaseDirectories: setBaseDirectories,
                getControllersDirectory: getControllersDirectory,
                getViewsDirectory: getViewsDirectory
            };
        }();

        this.state = function (stateConfig) {

            var resolve = function (stateName, baseName, path, url, params) {
                if (!path) path = '';

                var stateDef = {
                    name: stateName,
                    url: url,
                    templateUrl: stateConfig.getViewsDirectory() + path + '.html',
                    controller: baseName + 'Controller',
                    controllerAs: 'vm',
                    params: params,
                    resolve: {
                        lazyload: ['$q', '$rootScope', function ($q, $rootScope) {
                            var loadController = stateConfig.getControllersDirectory() + path + 'Controller';
                            var deferred = $q.defer();
                            require([loadController], function () {
                                $rootScope.$apply(function () {
                                    deferred.resolve();
                                });
                            });
                            return deferred.promise;
                        }]
                    }
                };

                return stateDef;
            };

            return {
                resolve: resolve
            }
        }(this.stateConfig);

    });

});