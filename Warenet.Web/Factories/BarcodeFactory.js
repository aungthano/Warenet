'use strict';

define(['appconfig','signalr'], function (app) {

    app.factory('barcodeFactory', ['$rootScope', function ($rootScope) {

        var on = function (eventName, callback) {
            var connection = $.hubConnection('http://localhost/Warenet.WebApi/');
            var hubProxy = connection.createHubProxy('BarcodeHub');

            hubProxy.on(eventName, function () {
                var args = arguments;
                $rootScope.$apply(function () {
                    callback.apply(hubProxy, args);
                });
            });

            connection.start().done(function () { });
        }

        return { on: on };

    }]);

});