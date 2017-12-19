'use strict';

define([// cores
        'jquery', 'bootstrap','angular',
        // thrid parties
        'angular-block-ui', 'angular-cookies', 'ui-router', 'ui-bootstrap', 'ui-grid', 'ui-bootstrap-contextmenu', 'angular-local-storage', 'ng-file-upload',
        // application utils
        'stateProvider', 'httpService', 'uiService',
        'uiDirective'], function () {

    // initialize application
    var app = angular.module('app', ['blockUI', 'ui.router', 'ui.bootstrap', 'LocalStorageModule', 'ngFileUpload',
                             'ui.bootstrap.contextMenu','ngCookies',
                             'ui.grid', 'ui.grid.moveColumns', 'ui.grid.infiniteScroll', 'ui.grid.resizeColumns', 'ui.grid.selection',
                             'stateProvider', 'httpService', 'uiService', 'uiDirective']);

    // blockUI config
    app.config(function (blockUIConfig) {
        // Change the default overlay message
        blockUIConfig.message = 'Loading Data...';
        //Browser navigation block
        blockUIConfig.blockBrowserNavigation = false;
        // Change the default delay to 100ms before the blocking is visible
        blockUIConfig.delay = 0;
        // Disable automatically blocking of the user interface
        blockUIConfig.autoBlock = false;
    });

    // auth interceptor config
    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorFactory');
    }]);

    //app.config(['$cookiesProvider', function ($cookiesProvider) {
    //    $cookiesProvider.defaults.path = '/cpath';
    //}]);

    // registerar config
    app.config(function ($controllerProvider, $compileProvider, $filterProvider, $provide,$stateProvider, stateResolverProvider) {
        app.stateResolver = stateResolverProvider.state;
        app.state = $stateProvider.state;
        
        app.register =
            {
                controller: $controllerProvider.register,
                directive: $compileProvider.directive,
                filter: $filterProvider.register,
                factory: $provide.factory,
                service: $provide.service,
                state: function (stateName, controllerName, path, url, params) {
                    var state = app.stateResolver.resolve(stateName, controllerName, path, url, params);
                    app.state(state);
                }
            };
    });

    // route config
    app.config(function ($stateProvider, $urlRouterProvider, $locationProvider, stateResolverProvider) {
        var state = stateResolverProvider.state;

        var home = state.resolve('home', 'Home', 'main/home', '/home');
        var product = state.resolve('product', 'Product', 'main/product', '/product');
        var support = state.resolve('support', 'Support', 'main/support', '/support');
        var login = state.resolve('login', 'Login', 'system/login', '/login');

        $stateProvider.state(home);
        $stateProvider.state(product);
        $stateProvider.state(support);
        $stateProvider.state(login);

        //$urlRouterProvider.otherwise('login');
    });

    // load auth cache data
    app.run(['authFactory', function (authFactory) {
        authFactory.fillAuthData();
    }]);
    
    // startup event
    angular.element(document).ready(function () {
        // initialize controller
        require(['app'], function () {
            // bootstrap application
            angular.bootstrap(document, ['app']);
        });
    });

    return app;
});