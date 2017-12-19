'use strict';
define(['appconfig'], function (app) {

    app.factory('dateInterceptor', function () {
        var regexIsoUtc = /^(\d{4}|\+\d{6})(?:-(\d{2}))(?:-(\d{2}))(?:T(\d{2})):(\d{2}):(\d{2})Z$/;

        function matchDate(dateString) {
            if (dateString.length === 20) {
                return dateString.match(regexIsoUtc);
            }
            return false;
        };

        function convertDateStringsToDates(object) {
            // ensure that we're processing an object
            if (typeof object !== "object") {
                return object;
            }

            for (var key in object) {
                if (!object.hasOwnProperty(key)) {
                    continue;
                }
                var value = object[key];

                // check for string properties with a date format
                if (typeof value === "string" && matchDate(value)) {
                    var date = new Date(value); // create the date from the date string
                    object[key] = date; // we're mutating the response directly
                } else if (typeof value === "object") {
                    convertDateStringsToDates(value); // recurse into object
                }
            }
            return null;
        }

        var interceptor = {
            'response': function (response) {
                if (response.data) {
                    convertDateStringsToDates(response.data);
                }
                return response;
            }
        };
        return interceptor;
    })

    .config(["$httpProvider", function ($httpProvider) {
        $httpProvider.interceptors.push('dateInterceptor'); // intercept responses and convert date strings into real dates
    }]);
});