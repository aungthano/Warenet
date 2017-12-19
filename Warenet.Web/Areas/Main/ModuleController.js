'use strict';

define(['appconfig'], function (app) {

    function ModuleController($rootScope, $stateParams, $state,$location, $timeout, httpService) {
        var vm = this;
        $rootScope.$state = $state;     // attach $state to $rootScope for ng-class 'active'

        vm.module = $stateParams.module;
        vm.moduleId = $stateParams.module.ModuleId;
        vm.contentId = 0;
        vm.contents = [];
        vm.content = {};

        vm.listItemClick = function (event, content) {
            event.preventDefault();
            if (!vm.isParentNode(content)) {
                $timeout(function () {
                    var path = content.FormPath.split('/');
                    //var formPath = $location.path() + '?path=' + vm.modulePath + '/' + path[1] + '/' + path[2];
                    //var formPath = '/' + path[1] + '/' + path[2];
                    var formPath = $location.path() + '/' + path[1] + '/' + path[2];
                    //$location.path(formPath);
                    $location.path(formPath).search({ path: vm.modulePath });
                }, 10);
            }

            //Set Current Content
            vm.content = content;

            var contentid = event.target.id;
            vm.contentId = contentid;
            if (vm[contentid] === undefined) {
                vm[contentid] = true;
            }
            else vm[contentid] = !vm[contentid];
        };

        vm.isCurrentContent = function myfunction(contentid) {
            return (vm.contentId == '#' + contentid);
        };

        vm.isExpandContent = function (contentid) {
            return vm['#' + contentid];
        };

        vm.isParentNode = function (data) {
            var childContent = vm.contents.filter(function (content) {
                return content.ParentContentId == data.ContentId;
            });
            return (childContent.length > 0);
        };

        vm.getChildContents = function (parentContent) {
            var childContents = vm.contents.filter(function (content) {
                return content.ParentContentId == parentContent.ContentId;
            });
            return childContents;
        };

        function getModule(modulePath) {
            var modules = $rootScope.modules;
            if (modules) {
                for (var i = 0; i < modules.length; i++) {
                    if (modules[i].ModulePath === modulePath) {
                        return modules[i];
                    }
                }
            }
            else return null;
        }

        function initCtrl() {
            if (vm.moduleId && vm.module) {
                // get contents
                httpService.get('api/module/getcontents', { ModuleId: vm.moduleId }).then(function (data) {
                    vm.contents = data;

                    // add contents states
                    vm.contents.forEach(function (content) {
                        var stateName = vm.moduleId + '.' + content.ContentId;
                        if ($state.includes(stateName) === undefined) {
                            var viewName = content.ViewName;
                            var url = content.FormPath;
                            var baseName = '';
                            var path = '';
                            var modulePath = $stateParams.module.ModulePath;
                            var params = { content: content };

                            if (!viewName || viewName == undefined || viewName == "") {
                                baseName = content.FormName;
                                path = modulePath + content.FormPath;
                            }
                            else {
                                baseName = 'ViewMaster';
                                path = 'shared/viewmaster';
                            }
                            app.register.state(stateName, baseName, path, url, params);
                        }
                    });
                });
            }
        }

        initCtrl();
    }

    app.register.controller('ModuleController', ModuleController);

});