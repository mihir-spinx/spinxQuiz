var myApp = angular.module('myApp', ["ngCookies", "ngSanitize", "checklist-model", "ui.gravatar", "LocalStorageModule",
    "slugifier", "ui.select", "ckeditor", "ui.sortable", "angularFileUpload", "blockUI", "ngDialog"]);

var templatesRoot = virtualDir + 'Content/Admin/app/templates/';

var finderImage;
if (window.CKFinder)
    finderImage = new CKFinder();

myApp.component("textbox",
    {
        templateUrl: templatesRoot + 'textbox.html',
        transclude: true,
        bindings: { name: '@', label: '@?', required: '@', type: '@', maxlength: '@', model: '=' }
    });
myApp.component("textboxArea",
    {
        templateUrl: templatesRoot + 'textbox-area.html',
        transclude: true,
        bindings: { name: '@', label: '@?', required: '@', maxlength: '@', model: '=' },
        controller: ["$scope", function ($scope) {

            $('[data-maxlength]').maxlength({
                alwaysShow: true,
                warningClass: "label label-info",
                limitReachedClass: "label label-danger",
                separator: ' of ',
                preText: 'You have ',
                postText: ' chars remaining.',
                validate: true
            });
        }]
    });
myApp.component("textboxCount",
    {
        templateUrl: templatesRoot + 'textbox-count.html',
        transclude: true,
        bindings: { name: '@', label: '@?', required: '@', type: '@', maxlength: '@', model: '=' },
        controller: ["$scope", function ($scope) {
            $('[data-maxlength]').maxlength({
                alwaysShow: true,
                warningClass: "label label-info",
                limitReachedClass: "label label-danger",
                separator: ' of ',
                preText: 'You have ',
                postText: ' chars remaining.',
                validate: true
            });

        }]
    });

myApp.component("textboxImage",
    {
        templateUrl: templatesRoot + 'textbox-image.html',
        transclude: true,
        bindings: { name: '@', required: '@', model: '=', noteWidth: '@?', noteHeight: '@?' },
        controller: ["$scope", function ($scope) {
            var $that = this;

            this.browseFiles = function () {

                finderImage.selectActionFunction = function (fileUrl) {

                    var fileUrlWithoutVirtualDir = fileUrl.replace(virtualDir, "");
                    $that.model.entity[$that.name] = fileUrlWithoutVirtualDir;
                    $scope.$apply();
                };
                finderImage.popup();
            };

            this.removeFile = function () {

                $that.model.entity[$that.name] = null;
            };
        }]
    });

myApp.component("textboxFile",
    {
        templateUrl: templatesRoot + 'textbox-file.html',
        transclude: true,
        bindings: { name: '@', label: '@?', required: '@', model: '=' },
        controller: ["$scope", function ($scope) {
            var $that = this;

            this.browseFiles = function () {

                finderImage.selectActionFunction = function (fileUrl) {

                    var fileUrlWithoutVirtualDir = fileUrl.replace(virtualDir, "");
                    $that.model.entity[$that.name] = fileUrlWithoutVirtualDir;
                    $scope.$apply();
                };
                finderImage.popup();
            };

            this.removeFile = function () {

                $that.model.entity[$that.name] = null;
            };
        }]
    });

myApp.component("multisiteClone",
    {
        templateUrl: templatesRoot + 'multisite-clone.html',
        bindings: { model: '=', sites: '=', currentSiteId: '=' }
    });

myApp.directive("multisiteNav", function () {
    return {
        scope: {
            sites: '=', currentSiteId: '=', click: '&onClick'
        },
        restrict: 'E',
        templateUrl: templatesRoot + "multisite-nav.html"
    };
});

myApp.component("textboxDate",
    {
        templateUrl: templatesRoot + 'textbox-date.html',
        transclude: true,
        bindings: { name: '@', required: '@', type: '@', maxlength: '@', model: '=' }
    });

myApp.component("textboxSlug",
    {
        templateUrl: templatesRoot + 'textbox-slug.html',
        bindings: { name: '@', required: '@', type: '@', maxlength: '@', model: '=', from: '@', click: '&' }
    });

myApp.component("selectbox",
    {
        templateUrl: templatesRoot + 'selectbox.html',
        transclude: true,
        bindings: { name: '@', label: '@', required: '@', model: '=', data: '=' }
    });

myApp.component("selectboxAdvtype",
    {
        templateUrl: templatesRoot + 'selectbox-advtype.html',
        transclude: true,
        bindings: { name: '@', label: '@', required: '@', model: '=', data: '=' }
    });
myApp.component("ckeditor",
    {
        templateUrl: templatesRoot + 'ckeditor.html',
        transclude: true,
        bindings: { name: '@', required: '@', model: '=' }
    });
myApp.component("status",
    {
        templateUrl: templatesRoot + 'status.html',
        bindings: { model: '=' }
    });
myApp.component("isdefault",
    {
        templateUrl: templatesRoot + 'isdefault.html',
        bindings: { model: '=' }
    });
myApp.component("formActions",
    {
        templateUrl: templatesRoot + 'form-actions.html',
        transclude: true
    });
myApp.component("save",
    {
        templateUrl: templatesRoot + 'save.html',
        bindings: { click: '&' }
    });
myApp.component("saveContinue",
    {
        templateUrl: templatesRoot + 'save-continue.html',
        bindings: { click: '&' }
    });
myApp.component("message",
    {
        templateUrl: templatesRoot + 'message.html',
        bindings: { message: '=', type: '=' }
    });
myApp.component("layoutHeader",
    {
        templateUrl: templatesRoot + "layout-header.html",
        transclude: true,
        bindings: { icon: '@', breadcrumb: '=' }
    });
myApp.component("buttonBack",
    {
        templateUrl: templatesRoot + "button-back.html",
        bindings: { url: '@' }
    });
myApp.component("buttonAdd",
    {
        templateUrl: templatesRoot + "button-add.html",
        transclude: true,
        bindings: { url: '@' }
    });
myApp.component("buttonSequence",
    {
        templateUrl: templatesRoot + "button-sequence.html",
        bindings: { url: '@' }
    });
myApp.filter('messageType', [function () { return function (input) { return getMessageType(input); }; }]);
myApp.filter('messageIcon', [function () { return function (input) { return getMessageIcon(input); }; }]);
function getMessageType(messageType) { switch (messageType) { case 0: return "danger"; case 1: return "success"; case 3: return "warning"; default: return 'info'; } }
function getMessageIcon(messageType) { switch (messageType) { case 0: return "times"; case 1: return "check"; case 3: return "warning"; default: return 'info'; } }
/**
 * Search Page Components
 */

myApp.component("grid",
    {
        templateUrl: templatesRoot + "grid.html",
        transclude: true
    });


myApp.directive('enterKey', function () {
        return function (scope, element, attrs) {
                element.bind("keydown keypress", function (event) {
                        if (event.which === 13) {
                                scope.$apply(function () {
                                        scope.$eval(attrs.enterKey);
                                });
                                event.preventDefault();
                        }
                });
        };
});

myApp.directive("toolbar", function () {
    return {
        scope: {
            model: '='
        },
        templateUrl: templatesRoot + "toolbar.html"
    };
});

myApp.directive("toolbarSites", function () {
    return {
        scope: {
            model: '=',
            change: '&',
            sites: '='
        },
        templateUrl: templatesRoot + "toolbar-sites.html"
    };
});

myApp.directive("thSearch", function () {
    return {
        scope: {
            name: "@",
            model: '=',
            label: '@'
        },
        replace: true,
        templateUrl: templatesRoot + "th-search.html"
    };
});

myApp.directive("thSearchDate", function () {
    return {
        scope: {
            name: "@",
            model: '='
        },
        replace: true,
        templateUrl: templatesRoot + "th-search-date.html",
        link: function (scope, elm, attrs) {
            initDatetimePicker();
        }
    };
});

myApp.directive("thSearchStatus", function () {
    return {
        scope: {
            model: '='
        },
        replace: true,
        templateUrl: templatesRoot + "th-search-status.html"
    };
});

myApp.directive("thSearchIsDefault", function () {
    return {
        scope: {
            model: '='
        },
        replace: true,
        templateUrl: templatesRoot + "th-search-isdefault.html"
    };
});

myApp.directive("thSearchSelect", function () {
    return {
        scope: {
            name: "@",
            model: '=',
            data: '=',
            label: '@'
        },
        replace: true,
        templateUrl: templatesRoot + "th-search-select.html"
    };
});

myApp.directive("thSort", function () {
    return {
        scope: {
            model: '=',
            name: '@',
            width: "@",
            align: '@'
        },
        replace: true,
        transclude: true,
        templateUrl: templatesRoot + "th-sort.html"
    };
});

myApp.directive("thSortStatus", function () {
    return {
        scope: {
            model: '='
        },
        replace: true,
        templateUrl: templatesRoot + "th-sort-status.html"
    };
});

myApp.directive("thSortIsDefault", function () {
    return {
        scope: {
            model: '='
        },
        replace: true,
        templateUrl: templatesRoot + "th-sort-isdefault.html"
    };
});

myApp.directive("thSortDate", function () {
    return {
        scope: {
            model: '=',
            name: '@'
        },
        replace: true,
        transclude: true,
        templateUrl: templatesRoot + "th-sort-date.html"
    };
});

myApp.directive("thSortSelectAll", function () {
    return {
        scope: {
            model: '='
        },
        replace: true,
        templateUrl: templatesRoot + "th-sort-select-all.html"
    };
});

myApp.directive("thSortAction", function () {
    return {
        replace: true,
        templateUrl: templatesRoot + "th-sort-action.html"
    };
});

myApp.directive("rowNoRecords", function () {
    return {
        scope: {
            model: '='
        },
        replace: true,
        templateUrl: templatesRoot + "row-no-records.html"
    };
});

myApp.directive("cellCheckboxId", function () {
    return {
        scope: {
            model: '=',
            item: "="
        },
        replace: true,
        templateUrl: templatesRoot + "cell-checkbox-id.html"
    };
});

myApp.directive("cellStatus", function () {
    return {
        scope: {
            item: "=",
            hide: "="
        },
        replace: true,
        templateUrl: templatesRoot + "cell-status.html"
    };
});

myApp.directive("cellIsDefault", function () {
    return {
        scope: {
            item: "=",
            hide: "="
        },
        replace: true,
        templateUrl: templatesRoot + "cell-isdefault.html"
    };
});

myApp.directive("cellDate", function () {
    return {
        scope: {
            item: "="
        },
        replace: true,
        templateUrl: templatesRoot + "cell-date.html"
    };
});

myApp.directive("cell", function () {
    return {
        scope: {
            model: '=',
            item: "=",
            gravatar: '=',
            align: '@'
        },
        replace: true,
        transclude: true,
        templateUrl: templatesRoot + "cell.html"
    };
});

myApp.directive("actionEdit", function () {
    return {
        scope: {
            model: '=',
            item: "=",
            url: "@"
        },
        replace: true,
        templateUrl: templatesRoot + "action-edit.html"
    };
});

myApp.directive("actionView", function () {
    return {
        scope: {
            model: '=',
            item: "=",
            url: "@"
        },
        replace: true,
        templateUrl: templatesRoot + "action-view.html"
    };
});
myApp.directive("actionDelete", function () {
    return {
        scope: {
            model: '=',
            item: "=",
            click: "&"
        },
        replace: true,
        templateUrl: templatesRoot + "action-delete.html"
    };
});

myApp.factory('spinxList', ["$http", "$cookies", function ($http, $cookies) {
    var spinxList = function (url) {

        this.url = url;

        this.result = {};
        this.paging = {};

        this.filters = {};
        this.filters.page = 1;
        this.filters.size = 10;

        this.message = null;
        this.messageType = null;

        this.pageSizes = [10, 25, 50, 100, 200];

        this.selectAll = false;
        this.ids = [];
        this.action = '';
        this.actions = ['Active', 'Inactive', 'Delete'];

        this.data = [];

        this.showMessage();

        this.load();
    };

    spinxList.prototype.load = function (clearPage = false) {
        if (clearPage)
            this.filters.page = 1;

        $http({
            url: this.url,
            method: "GET",
            params: this.filters
        }).then(function (resp) {

            var result = resp.data;

            this.result = result;
            this.paging = result.paging;
            this.data = result.data;
            this.filters.page = this.paging.page;
            this.filters.size = this.paging.size;

            this.filters.action = null;
            this.filters.ids = null;
            this.action = '';
            this.ids = [];
            this.selectAll = false;

            if (result.message !== null) {
                this.message = result.message;
                this.messageType = result.messageType;
            }

        }.bind(this));
    };

    spinxList.prototype.sort = function ($event, name) {

        if (this.loading) return;

        var elmt = angular.element($event.target);
        var sortType = 'asc';

        elmt.removeClass('sorting');

        if (elmt.hasClass('sorting_asc'))
            sortType = 'desc';

        this.filters.sortColumn = name;
        this.filters.sortType = sortType;
        angular.element('.sorting_asc, .sorting_desc').removeClass('sorting_asc sorting_desc')
            .addClass('sorting');
        elmt.addClass(sortType === 'asc' ? 'sorting_asc' : 'sorting_desc')
            .removeClass(sortType === 'asc' ? 'sorting_desc' : 'sorting_asc');

        this.load(true);
    };

    spinxList.prototype.previousPage = function () {
        if (this.result.paging.Page === 1) return;

        this.filters.page = this.result.paging.page - 1;

        this.load();
    };

    spinxList.prototype.nextPage = function () {
        if (this.result.paging.lastPage === this.result.paging.page) return;

        this.filters.page = this.result.paging.page + 1;

        this.load();
    };

    spinxList.prototype.deleteRow = function (id) {

        var $that = this;

        $.confirm({
            text: "Do you want to delete record?",
            title: "Confirmation required",
            confirmButtonClass: "btn-danger",
            confirm: function (button) {

                $that.filters.action = "delete";
                $that.filters.ids = [id];

                $that.load();
            }
        });
    };

    spinxList.prototype.fireAction = function () {

        this.message = null;
        this.messageType = null;

        if (this.action !== '' && this.ids.length === 0) {
            this.message = 'Select at least one item from action list.';
            this.messageType = 2;
            return;
        }

        var $that = this;

        if (this.action === 'delete') {
            $.confirm({
                text: "Do you want to delete record?",
                title: "Confirmation required",
                confirmButtonClass: "btn-danger",
                confirm: function () {

                    $that.filters.action = "delete";
                    $that.filters.ids = $that.ids;

                    $that.load();
                }
            });
        } else {
            this.filters.action = this.action;
            this.filters.ids = this.ids;
            this.load();
        }
    };

    spinxList.prototype.checkAll = function () {

        var $that = this;

        if (this.selectAll) {
            angular.forEach(this.data.map(function (item) { return item.id }),
                function (value) {
                    $that.ids.push(value);
                });
        } else {
            this.ids.splice(0, this.ids.length);
        }
    };

    spinxList.prototype.fireIdChange = function () {
        if (this.ids.length !== this.result.data.length)
            this.selectAll = false;
    };

    spinxList.prototype.showMessage = function () {
        angular.forEach([0, 1, 2, 3],
            function (value) {
                var cookie = $cookies.get(`Flash.${value}`);
                if (cookie !== undefined) {
                    this.message = cookie;
                    this.messageType = value;
                    $cookies.remove(`Flash.${value}`);
                }
            }.bind(this));
    }

    /* Extra for esl project */
    spinxList.prototype.siteAvailable = function (sites, site) {

        var result = false;

        angular.forEach(sites,
            function (value, key) {
                if (value.siteId === site || value.siteId === site.id)
                    result = true;
            });

        return result;
    };

    spinxList.prototype.setSortingClass = function (model, name) {

        if (model === null)
            return '';

        if (model.filters.sortColumn === name) {
            return model.filters.sortType === 'asc' ? 'sorting_asc' : 'sorting_desc';
        }

        return 'sorting';
    }

    return spinxList;
}]);

myApp.factory('spinxMultiSiteEditForm', ["$http", "$cookies", function ($http, $cookies) {
    var spinxMultiSiteEditForm = function (apiUrl, listPageUrl) {

        this.apiUrl = apiUrl;
        this.listPageUrl = listPageUrl;

        this.method = "PUT";

        this.errors = {};
        this.entity = {};

        this.message = null;
        this.messageType = null;

        this.result = {};

        this.showMessage();

        this.httpOptions = {};

        this.redirectOnSave = false;

        this.afterSubmit = null;
    };

    spinxMultiSiteEditForm.prototype.beforeSave = function () {

        this.message = null;
        this.messageType = null;

        this.loading = true;
        this.errors = {};

        this.httpOptions = {
            url: this.apiUrl,
            method: this.method,
            data: angular.toJson(this.entity)
        };
    };

    spinxMultiSiteEditForm.prototype.submit = function () {

        this.redirectOnSave = true;
        this.submitAndContinue();
    }

    spinxMultiSiteEditForm.prototype.submitAndContinue = function () {

        this.beforeSave();

        if (this.entity.cloneSiteIds == null || this.entity.cloneSiteIds.length === 0) {
            $http(this.httpOptions).then(function (resp) {
                this.afterSuccessForEditPage(resp.data);
            }.bind(this));
        } else {

            var $that = this;

            $.confirm({
                text: "With save it will override data to other selected site(s).",
                title: "Confirmation required",
                confirmButtonClass: "btn-danger",
                confirm: function () {

                    $http($that.httpOptions).then(function (resp) {
                        $that.afterSuccessForEditPage(resp.data);
                    }.bind($that));

                }
            });
        }
    }

    spinxMultiSiteEditForm.prototype.afterSuccessForEditPage = function (result) {
        this.errors = result.errors;

        if (result.success) {

            if (this.redirectOnSave) {

                if (result.message !== null && result.message.length > 0)
                    $cookies.put(`Flash.${result.messageType}`, result.message);

                if (typeof this.onSaved === "function") {
                    this.onSaved();
                    return;
                }

                window.location.pathname = virtualDir + this.listPageUrl;
            } else {

                this.message = result.message;
                this.messageType = result.messageType;

                scrollToAlert();

                if (this.afterSubmit !== null)
                    this.afterSubmit();
            }
        }
    };

    spinxMultiSiteEditForm.prototype.showMessage = function () {
        angular.forEach([0, 1, 2, 3],
            function (value) {
                var cookie = $cookies.get(`Flash.${value}`);
                if (cookie !== undefined) {
                    this.message = cookie;
                    this.messageType = value;
                    $cookies.remove(`Flash.${value}`);
                }
            }.bind(this));
    }

    return spinxMultiSiteEditForm;
}]);

myApp.factory('spinxForm', ["$http", "$cookies", function ($http, $cookies) {
    var spinxForm = function (url, apiUrl, id = null) {

        this.url = url;
        this.apiUrl = apiUrl;
        this.id = id;
        this.method = id == null ? "POST" : "PUT";

        this.siteId = null;

        this.errors = {};
        this.entity = {};

        this.message = null;
        this.messageType = null;

        this.result = {};

        this.showMessage();
        this.onSaved = null;
        this.onSavedAndContinue = null;
    };

    spinxForm.prototype.beforeSave = function () {

        this.message = null;
        this.messageType = null;

        this.loading = true;
        this.errors = {};

    };

    spinxForm.prototype.submit = function () {
        this.beforeSave();
        $http({
            url: this.apiUrl,
            method: this.method,
            data: angular.toJson(this.entity)
        }).then(function (resp) {
            if (this.id === null || this.id === 0) this.afterSuccessForCreatePage(resp.data);
            else this.afterSuccessForEditPage(resp.data);
        }.bind(this),
            function (error) { }.bind(this));
    }

    spinxForm.prototype.submitAndContinue = function () {
        this.beforeSave();
        $http({
            url: this.apiUrl,
            method: this.method,
            data: angular.toJson(this.entity)
        }).then(function (resp) {
            if (this.id === null || this.id === 0) this.afterSuccessForCreatePage(resp.data, true);
            else this.afterSuccessForEditPage(resp.data, false);
        }.bind(this),
            function (error) { }.bind(this));
    }

    spinxForm.prototype.afterSuccessForCreatePage = function (result, redirectToEdit = false) {
        this.errors = result.errors;
        this.loading = false;
        if (result.success) {
            this.message = result.message;
            this.messageType = result.messageType;
            if (result.message !== '')
                $cookies.put(`Flash.${result.messageType}`, result.message);

            if (typeof this.onSaved === "function") {
                this.onSaved();
                return;
            }

            if (redirectToEdit) {

                if (this.siteId != null && this.siteId > 0) {
                    window.location.pathname = virtualDir + this.url + '/edit/' + this.siteId + '/' + (this.id || result.id);
                } else {
                    window.location.pathname = virtualDir + this.url + '/edit/' + (this.id || result.id);
                }

                return;
            }
            if (this.url != '')
                window.location.pathname = virtualDir + this.url;
        }
        else {
            if (result.message !== '') {
                this.message = result.message;
                this.messageType = result.messageType;
            }
        }
    };

    spinxForm.prototype.afterSuccessForEditPage = function (result, redirectToList = true) {
        this.errors = result.errors;
        this.loading = false;
        if (result.success) {

            if (result.message !== '' && redirectToList) {
                $cookies.put(`Flash.${result.messageType}`, result.message);


                if (typeof this.onSaved === "function") {
                    this.onSaved();
                    return;
                }

                if (this.url != '')
                    window.location.pathname = virtualDir + this.url;
            }

            this.message = result.message;
            this.messageType = result.messageType;
        }
        else {
            if (result.message !== '') {
                this.message = result.message;
                this.messageType = result.messageType;
            }
        }
    };

    spinxForm.prototype.showMessage = function () {
        angular.forEach([0, 1, 2, 3],
            function (value) {
                var cookie = $cookies.get(`Flash.${value}`);
                if (cookie !== undefined) {
                    this.message = cookie;
                    this.messageType = value;
                    $cookies.remove(`Flash.${value}`);
                }
            }.bind(this));
    }

    return spinxForm;
}]);

myApp.filter('humanize', function () {
    return function (text) {
        if (text) {
            text = text.split(/(?=[A-Z])/);

            // go through each word in the text and capitalize the first letter
            for (var i in text) {
                var word = text[i];
                word = word.toLowerCase();
                word = word.charAt(0).toUpperCase() + word.slice(1);
                text[i] = word;
            }

            return text.join(" ");
        }
    };
});

function initDatetimePicker() {
    $('.datepicker').each(function () {

        var $that = $(this),
            dataDateFormat = $that.attr("data-dateformat") || "dd.mm.yy";

        $that.datepicker({
            dateFormat: dataDateFormat,
            prevText: '<i class="fa fa-chevron-left"></i>',
            nextText: '<i class="fa fa-chevron-right"></i>',
            onSelect: function (d, i) {
                if (d !== i.lastVal) {
                    $(this).change();
                }
            }
        });

    });
}

/**
 * extend the localStorageService (https://github.com/grevory/angular-local-storage)
 * 
 * - now its possible that data stored in localStorage can expire and will be deleted automagically
 * - usage localStorageService.set(key, val, expire)
 * - expire is an integer defininig after how many hours the value expires
 * - when it expires, it is deleted from the localStorage
 */
myApp.config(($provide) => {
    $provide.decorator('localStorageService', ($delegate) => {

        //store original get & set methods
        var originalGet = $delegate.get,
            originalSet = $delegate.set;

        /**
         * extending the localStorageService get method
         *
         * @param key
         * @returns {*}
         */
        $delegate.get = (key) => {
            if (originalGet(key)) {
                var data = originalGet(key);

                if (data.expire) {
                    var now = Date.now();

                    // delete the key if it timed out
                    if (data.expire < now) {
                        $delegate.remove(key);
                        return null;
                    }

                    return data.data;
                } else {
                    return data;
                }
            } else {
                return null;
            }
        };

        /**
         * set
         * @param key               key
         * @param val               value to be stored
         * @param {int} expires     hours until the localStorage expires
         */
        $delegate.set = (key, val, expires) => {
            var expiryDate = null;

            if (angular.isNumber(expires)) {
                expiryDate = Date.now() + (1000 * 60 * expires);
                originalSet(key, {
                    data: val,
                    expire: expiryDate
                });
            } else {
                originalSet(key, val);
            }
        };

        return $delegate;
    });
});

var secretKey = 'sh4x@ulY;ZD"XiUFZJ(T3@elB@eOStu6Zq)vms$J,1a03wX@m=8PY7CZro{+f9Z';

//var bytes  = CryptoJS.AES.decrypt(ciphertext.toString(), secretKey);
//var plaintext = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
//$.inArray("adminusers.create", access.permissions);

//var adminMenu = {
//    dashboard: {
//        url: "admin",
//        icon: 'home'
//    },
//    content: {
//        icon: 'fa-pencil-square-o',
//        childrens: {
//            pages: {
//                url: 'admin/pages'
//            }
//        }
//    },
//    system: {
//        icon: 'cog',
//        childrens: {
//            account: {
//                childrens: {
//                    editProfile: {
//                        url: 'admin/account/editprofile'
//                    },
//                    changePassword: {
//                        url: 'admin/account/changepassword'
//                    }
//                }
//            },
//            permissions: {
//                childrens: {
//                    adminUsers: {
//                        url: 'admin/account/adminusers'
//                    },
//                    adminRoles: {
//                        url: 'admin/account/adminroles'
//                    },
//                    adminPermissions: {
//                        url: 'admin/account/adminpermissions'
//                    }
//                }
//            },
//            emailTemplates: {
//                url: 'admin/emailtemplates'
//            },
//            sites: {
//                url: 'admin/sites'
//            }
//        }
//    }
//};

myApp.run(["$rootScope", "$http", "localStorageService", function ($rootScope, $http, localStorageService) {

    $rootScope.virtualDir = virtualDir;

    if (localStorageService.get('access') == null) {
        $http.get('api/admin/myaccess').then(function (resp) {

            var data = JSON.stringify(resp.data);
            var ciphertext = CryptoJS.AES.encrypt(data, secretKey).toString();
            localStorageService.set('access', ciphertext, 15);
        });
    }

    $rootScope.year = (new Date()).getFullYear();
}]);

function scrollToAlert() {
    $('html, body').animate({
        scrollTop: $("form").offset().top - 200
    }, 500);
}