"use strict";

define(['appconfig'], function () {

    var app = angular.module('uiDirective', []);

    app.directive('ngcMatch', function () {
        return {
            //scope: true, // use a child scope that inherits from parent
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function(scope, elem, attrs, ngModel) {
                if (!ngModel) return; // do nothing if no ng-model

                // watch own value and re-validate on change
                scope.$watch(attrs.ngModel, function() {
                    validate();
                });

                // observe the other value and re-validate on change
                attrs.$observe('ngcMatch', function(val) {
                    validate();
                });

                var validate = function() {
                    // values
                    var val1 = ngModel.$viewValue;
                    var val2 = attrs.ngcMatch;

                    // set validity
                    ngModel.$setValidity('unmatch', val1 === val2);
                };
            }
        };
    });

    app.directive('inputDialog', function (uiService) {
        return {
            scope: {
                viewname: '@view',
                filter: '@filter',
                valuemember: '@valuemember',
                displaymember: '@displaymember',
                displayinfomember: '@displayinfomember',
                ngModel: '=?',
                ngInfoModel: '=?',
                ngEntity: '=?'
            },
            restrict: 'E',
            require: '?ngModel',
            replace: true,
            template: '<input type="text" class="form-control text-uppercase" ' +
                      'maxlength="6" ' +
                      'ng-maxlength="6" ' +
                      'ng-pattern="/^[a-zA-Z0-9]*$/" ' +
                      'ng-dblclick="showDialog();">',
            link: function (scope, element, attrs, ngModel) {

                scope.showDialog = function () {
                    if (!ngModel) return; // do nothing if no ng-model
                    var viewName = '[' + attrs.viewname + ']';
                    var viewkey = '[' + attrs.valuemember + ']';
                    var viewfilter = attrs.filter;

                    uiService.showViewDialog(viewName, viewkey, viewfilter).then(function (row) {
                        var valueMember = attrs.valuemember;
                        var displayMember = attrs.displaymember;
                        var displayInfoMember = attrs.displayinfomember;

                        scope.ngModel = row[displayMember];
                        scope.ngInfoModel = row[displayInfoMember];

                        ngModel.$render();
                        ngModel.$setDirty();

                        element[0].focus();
                    });
                };

                scope.changedValue = function () {

                };
            }
        };
    });

    app.directive('ngRightClick', ['$parse', function ($parse) {
        return function (scope, element, attrs) {
            var fn = $parse(attrs.ngRightClick);
            element.bind('contextmenu', function (event) {
                scope.$apply(function () {
                    event.preventDefault();
                    fn(scope, { $event: event });
                });
            });
        };
    }]);

    app.directive('ngValidateForm', function () {
        return {
            restrict: 'A',
            link: function (scope, elem) {

                // set up event handler on the form element
                elem.on('submit', function () {

                    // find the first invalid element
                    var firstInvalid = elem[0].querySelector('.ng-invalid');

                    // if we find one, set focus
                    if (firstInvalid) {
                        firstInvalid.focus();
                    }
                });
            }
        };
    });

    app.directive('ngFocus', function ($timeout) {
        return {
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngFocus, function (val) {
                    if (angular.isDefined(val) && val) {
                        $timeout(function () { element[0].focus(); });
                    }
                }, true);

                element.bind('blur', function () {
                    if (angular.isDefined(attrs.ngFocusLost)) {
                        scope.$apply(attrs.ngFocusLost);

                    }
                });
            }
        };
    });

    app.directive('ngConfirmClick', function () {
        return {
            priority: -1,
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.bind('click', function (e) {
                    var message = attrs.ngConfirmClick;
                    // confirm() requires jQuery
                    if (message && !confirm(message)) {
                        e.stopImmediatePropagation();
                        e.preventDefault();
                    }
                });
            }
        };
    });

});