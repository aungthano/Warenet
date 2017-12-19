'use strict';

define(['appconfig'], function () {

    var app = angular.module('httpService', []);

    app.service('httpService', ['$http', '$rootScope', '$q', 'blockUI', function ($http, $rootScope, $q, blockUI) {

        // default web api base url
        this.wsBaseUrl = 'http://localhost/Warenet.WebApi/';
        this.site = '';

        this.get = function (route, params) {
            blockUI.start();
            var deferred = $q.defer();
            $http({
                method: 'GET',
                url: this.wsBaseUrl + route,
                params: params
            }).then(function (response, status, headers, config) {
                blockUI.stop();
                deferred.resolve(response.data);
            },
            function (response) {
                blockUI.stop();
                deferred.reject(response);
            });
            return deferred.promise;
        };

        this.post = function (route, data, contentType) {
            contentType = contentType || 'application/json; charset=utf-8';

            blockUI.start();
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: this.wsBaseUrl + route,
                data: data,
                contentType: contentType,
            }).then(function (response, status, headers, config) {
                blockUI.stop();
                deferred.resolve(response.data);
            },
            function (response) {
                blockUI.stop();
                deferred.reject(response.data);
            });
            return deferred.promise;
        };

        this.postWithParam = function (route, params) {
            blockUI.start();
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: this.wsBaseUrl + route,
                params: params
            }).then(function (response, status, headers, config) {
                blockUI.stop();
                deferred.resolve(response.data);
            },
            function (response) {
                blockUI.stop();
                deferred.reject(response);
            });
            return deferred.promise;
        };

        this.ping = function (url) {
            blockUI.start();
            var deferred = $q.defer();
            var responseTime = 0;
            var start = (new Date()).getTime();

            $http.get(url + '?rnd=' + (new Date().getTime()))
                .then(function () {
                    blockUI.stop();
                    responseTime = (new Date().getTime()) - start;
                    deferred.resolve(Math.round(responseTime / 10) / 100);
                },
                function () {
                    blockUI.stop();
                    deferred.reject(responseTime);
                });
            return deferred.promise;
        };

    }]);
});