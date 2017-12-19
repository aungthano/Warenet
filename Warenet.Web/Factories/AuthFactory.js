'use strict';

define(['appconfig'], function (app) {

    app.factory('authFactory', ['$http', '$q', 'localStorageService', 'httpService', function ($http, $q, localStorageService, httpService) {

        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: "",
            wsUrl: "",
            site: ""
        };

        var _saveRegistration = function (registration) {

            _logOut();

            //return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            //    return response;
            //});

        };

        var _login = function (loginData) {
            var deferred = $q.defer();

            var data = "grant_type=password&&username=" + loginData.userName + "&password=" + loginData.password + "&site=" + httpService.site;
            httpService.post('token', data, 'application/x-www-form-urlencoded').then(function (data) {
                localStorageService.set('authorizationData', {
                    token: data.access_token,
                    userName: loginData.userName,
                    wsUrl: httpService.wsBaseUrl,
                    site: httpService.site
                });
                deferred.resolve();
            },
            function (errData) {
                _logOut();
                deferred.reject(errData);
            });

            return deferred.promise;
        };

        var _logOut = function () {
            localStorageService.remove('authorizationData');
            _authentication.isAuth = false;
            _authentication.userName = "";
            _authentication.wsUrl = "";
            _authentication.site = "";
        };

        var _fillAuthData = function () {
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
                _authentication.wsUrl = authData.wsUrl;
                _authentication.site = authData.site;
            }
        }

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;

        return authServiceFactory;
    }]);
});