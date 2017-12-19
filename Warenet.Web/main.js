/// <reference path="require.js" />

require.config({

    baseUrl: "",

    // alias libraries paths
    paths: {
        'jquery': 'Scripts/jquery-3.2.1.min',
        'bootstrap': 'Scripts/bootstrap.min',
        'angular': 'Scripts/angular.min',
        'angular-block-ui': 'Scripts/angular-block-ui.min',
        'angular-local-storage': 'Scripts/angular-local-storage.min',
        'angular-cookies': 'Scripts/angular-cookies.min',
        'ui-router': 'Scripts/angular-ui-router.min',
        'ui-grid': 'Scripts/ui-grid.min',
        'ui-bootstrap': 'Scripts/angular-ui/ui-bootstrap-tpls.min',
        'ui-bootstrap-contextmenu': 'scripts/angular-ui/ui-bootstrap-contextmenu',
        'ui-router': 'Scripts/angular-ui-router.min',
        'ng-file-upload': 'Scripts/ng-file-upload.min',
        'signalr': 'Scripts/jquery.signalR-2.2.2.min',

        //Local Dependencies
        'appconfig': 'appconfig',
        'app': 'app',
        'stateProvider': 'Providers/StateProvider',
        'httpService': 'Services/HttpService',
        'uiService': 'Services/UIService',
        'uiDirective': 'Directives/UIDirective',
        'authFactory': 'Factories/AuthFactory',
        'authInterceptorFactory': 'Factories/AuthInterceptorFactory',
        'barcodeFactory': 'Factories/BarcodeFactory',
        'dateInterceptor': 'Factories/DateInterceptor'
    },

    // Add angular modules that does not support AMD out of the box, put it in a shim
    shim: {
        'bootstrap': ['jquery'],
        'signalr': ['jquery'],
        // define angular deps to bootstrap to prevent conflict with ui-bootstrap
        'angular': ['bootstrap'],
        'angular-block-ui': ['angular'],
        'angular-local-storage': ['angular'],
        'angular-cookies': ['angular'],
        'ui-router': ['angular'],
        'ui-grid': ['angular'],
        'ui-bootstrap': ['angular'],
        'ui-bootstrap-contextmenu': ['angular'],
        'ng-file-upload': ['angular'],
    },

    // kick start application
    deps: ['appconfig']
});