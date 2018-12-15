var templatePath = 'Content/Admin/app/';

myApp.controller("ModelEditController",
    [
        '$scope', '$http', function ($scope, $http) {
            $scope.parentObj = {};
            $scope.parentObj.tabs = ['dashboard', 'editProfile'];

            $scope.parentObj.tab = 'dashboard';
            $scope.parentObj.template = templatePath + 'members/member-dashboard.html';

            $scope.tabChange = function ($event, tabId, filterType = "",filterValue=0) {

                $scope.parentObj.tab = tabId;
                var template = '';

                switch (tabId) {
                    case 'dashboard': template = 'members/member-dashboard.html'; break;
                    case 'editProfile': template = 'members/member-edit.html'; break;
                }

                // member dashboard filter jump 

                /*
                $scope.parentObj.filterPostAJobStatus = "";
                $scope.parentObj.filterPremiumJobs = "";
                $scope.parentObj.filterResumes = "";
                $scope.parentObj.filterPayment = "";
                $scope.parentObj.filterAdvertisement = -1;

                if (filterType != "") {
                    switch (filterType) {
                        case 'FilterPostedJobs': 
                        if (filterValue != 0)
                            $scope.parentObj.filterPostAJobStatus = filterValue;
                            break;
                        case 'FilterPremiumJobs': 
                            if (filterValue != 0)
                                $scope.parentObj.filterPremiumJobs = filterValue;
                            break;
                        case 'FilterResumes':
                            if (filterValue != 0)
                                $scope.parentObj.filterResumes = filterValue;
                            break;
                        case 'FilterPayment':
                            if (filterValue != 0)
                                $scope.parentObj.filterPayment = filterValue;
                            break;
                        case 'FilterAdvertisement':
                            if (filterValue != -1)
                                $scope.parentObj.filterAdvertisement = filterValue;
                            break;
                    }
                }
                */

                $scope.parentObj.template = templatePath + template;
            };
        }
    ]);

myApp.controller('ctrlMemberDashboard',
    ['$scope', '$http', 'spinxList', function ($scope, $http, spinxList) {

        var apiUrl = 'api/admin/members/GetMemberDashboard/' + Id;

        //var loadResumesStatus = function () {
        //    $http.get(apiUrl).then(
        //        function (resp) {
        //            $scope.memberInfo = resp.data.data;
        //        });
        //};
        //loadResumesStatus();
    }
    ]);

myApp.controller('ctrlMemberEditForm',
    [
        '$scope', '$http', 'spinxForm', function ($scope, $http, spinxForm) {
            var apiUrl = 'api/admin/members/' + Id;
            var url = 'admin/members';

            $scope.form = new spinxForm(url, apiUrl, Id);
            $scope.virtualDir = virtualDir;
            var defaultEntity = $scope.form.entity;

            var loadEntity = function () {

                $http.get(apiUrl).then(function (resp) {
                    $scope.form.entity = resp.data;
                },
                    function (resp) {
                        if (resp.status === 404) {
                            $scope.form.entity = defaultEntity;
                            $scope.form.entity.isActive = true;
                        }
                    });
            };
            loadEntity();
        }
    ]);

/*
myApp.controller('ctrlResumeAccessPurchased',
    ["$scope", "$http", "spinxList", "ngDialog", "$controller", function ($scope, $http, spinxList, ngDialog, $controller) {

        $controller('BaseCommonController', { $scope: $scope });

        var searchApiUrl = 'api/admin/ResumeAccessPlanTransactions/GetResumePurchasedByMember/' + Id;
        $scope.list = new spinxList(searchApiUrl);
        $scope.list.actions = [];
        $scope.parentObj.planActive = false;

        $http.get(searchApiUrl).then(
            function (resp) {
                if (resp.data.data.length > 0) {
                    $scope.parentObj.planActive = resp.data.data[0].anyActivePlan;
                }
            });

        $scope.addResumePlan = function () {
            ngDialog.open({
                template: templatePath + 'members/add-resume-access-plan.html',
                className: 'ngdialog-theme-default add-days-popup',
                scope: $scope
            });
            setTimeout(function () { $("a.close").remove(); }, 500);
        };

        $scope.addDays = function (id, title) {
            $scope.Common(id, title);
        }
    }
    ]);

myApp.controller('ctrlResumeAccessPlanList', ['$scope', '$http', 'spinxForm', "ngDialog", "$cookies",
    function ($scope, $http, spinxForm, ngDialog, $cookies) {

        var apiUrl = 'admin/resumeaccessplantransactions/addResumePlanByMember/' + Id;
        var url = 'admin/resumeaccessplantransactions/AddResumePlanByMember';
        $scope.form = new spinxForm(url, apiUrl);

        $scope.form.afterSubmit = function () {
            $scope.frm.$setPristine();
        }

        $scope.backAdvertisement = function () {
            ngDialog.close();
            $scope.parentObj.template = templatePath + 'members/resume-access-purchased.html';
            $scope.list.load();
            $scope.parentObj.planActive = true;
            var success = 1;
            var cookie = $cookies.get(`Flash.${success}`);
            if (cookie !== undefined) {
                $scope.list.message = cookie;
                $scope.list.messageType = success;
                $cookies.remove(`Flash.${success}`);
            }
        };

        $scope.form.onSaved = function () {
            $scope.backAdvertisement();
        };

        $scope.result = {};
        $scope.filters = {};
        $scope.filters.sortColumn = 'Days';
        $scope.filters.sortType = 'Asc';

        var load = function () {
            $scope.data = [];
            $http.get('api/List/ResumeAccessPlanList/', { params: $scope.filters }).then(function (resp) {
                $scope.result = resp.data;
                $scope.parentObj.list = resp.data.data;
                if ($scope.parentObj.list.length === 0) {
                    $(".resume-access-list").hide();
                    $("#no-records").show();
                }
                else {
                    $(".resume-access-list").show();
                    $("#no-records").hide();
                }
            });
        }
        if (!angular.isDefined($scope.parentObj.list))
            load();
    }
]);

myApp.controller('ctrlPostedResume',
    ["$scope", "$http", "spinxList", function ($scope, $http, spinxList) {

        var searchApiUrl = 'api/admin/PostAResumes/GetPostedResumeByMember/' + Id;

        $scope.list = new spinxList(searchApiUrl);
        $scope.list.filters['status'] = $scope.parentObj.filterResumes;
        $scope.list.actions = [];
        $scope.list.paging = {};

        $scope.filterSite = function () {
            $scope.list.load();
        };

        var loadResumesStatus = function () {
            $http.get("admin/Members/ResumesStatusList").then(
                function (resp) {
                    $scope.resumesStatusList = resp.data;
                });
        };
        loadResumesStatus();
        $scope.addResume = function () {
            $scope.parentObj.template = templatePath + 'members/resume-add.html';
        };

        $scope.editResume = function (resumeId) {
            $scope.parentObj.editResume = {};
            $scope.parentObj.editResume.siblingId = resumeId;
            $scope.parentObj.template = templatePath + 'members/resume-edit.html';
        };

        $scope.editResumeSite = function (resumeId) {
            $scope.editResume(resumeId);
        };
    }
    ]);

myApp.controller('ctrlResumeAdd',
    ["$scope", "$http", "spinxForm", 'FileUploader', function ($scope, $http, spinxForm, FileUploader) {

        var apiUrl = 'admin/PostAResumes/Create';
        var url = 'admin/PostAResumes';
        var uploadurl = virtualDir + "Upload/FileUpload";

        $scope.form = new spinxForm(url, apiUrl);

        $scope.clickUpload = function () {
            angular.element('#upload').trigger('click');
        };

        $scope.uploader = new FileUploader({
            url: uploadurl
        });

        var loadMembers = function () {
            $http.get("api/list/memberslist").then(
                function (resp) {
                    $scope.members = resp.data;

                    $.each(resp.data,
                        function (idx, obj) {
                            if (obj.id === Id) {
                                $scope.form.entity.memberId = Id;
                                $scope.form.entity['name'] = obj.name;
                                $scope.form.entity['email'] = obj.email;
                            }
                        });
                });
        };

        loadMembers();

        $scope.backPostedResumes = function () {
            $scope.parentObj.template = templatePath + 'members/posted-resumes.html';
        };

        $scope.form.onSaved = function () {
            $scope.backPostedResumes();
        }

        $scope.uploader.onAfterAddingFile = function (fileItem) {
            fileItem.uploader.queue[0].upload();
        };

        $scope.uploader.onCompleteItem = function (fileItem, response, status, headers) {
            if (response.Success) {
                $scope.form.entity.uploadResume = response.Data.fileName;
                $scope.result = response.Data;
                $scope.filepath = virtualDir + response.Data.filePath + response.Data.fileName;
                $scope.form.errors.uploadResume = '';
            }
            else {
                $scope.form.errors.uploadResume = response.Message;
                $scope.form.entity.uploadResume = '';
            }
            fileItem.uploader.clearQueue();
        }
    }
    ]);

myApp.controller('ctrlResumeEdit',
    ["$scope", "$http", "spinxForm", "FileUploader", function ($scope, $http, spinxForm, FileUploader) {
        $scope.siblingId = $scope.parentObj.editResume.siblingId;
        var apiListUrl = 'api/admin/PostAResumes/' + $scope.parentObj.editResume.siblingId;
        var apiPostUrl = 'admin/PostAResumes/EditResume/' + $scope.parentObj.editResume.siblingId;
        var url = 'admin/PostAResumes';

        var uploadurl = virtualDir + "Upload/FileUpload";
        $scope.form = new spinxForm(url, apiPostUrl, $scope.siblingId);

        $scope.form.afterSubmit = function () {
            $scope.frm.$setPristine();
        }

        $scope.defaultEntity = $scope.form.entity;

        var loadResumesStatus = function () {
            $http.get("admin/Members/ResumesStatusList").then(
                function (resp) {
                    $scope.resumesStatusList = resp.data;
                });
        };

        var loadEntity = function () {
            $http.get(apiListUrl).then(function (resp) {
                $scope.form.entity = resp.data;
                $scope.form.entity.oldUploadResume = $scope.form.entity.uploadResume;
                $scope.filepath = filePath + $scope.form.entity.uploadResume;
            },
                function (resp) {
                    if (resp.status === 404) {

                        $scope.form.entity = $scope.defaultEntity;
                        $scope.form.entity.isActive = true;
                    }
                });
        };
        loadResumesStatus();
        loadEntity();

        $scope.backPostedResumes = function () {
            $scope.parentObj.template = templatePath + 'members/posted-resumes.html';
        };

        $scope.form.onSaved = function () {
            $scope.backPostedResumes();
        }

        $scope.clickUpload = function () {
            angular.element('#upload').trigger('click');
        };

        $scope.uploader = new FileUploader({
            url: uploadurl
        });

        $scope.uploader.onAfterAddingFile = function (fileItem) {
            fileItem.uploader.queue[0].upload();
        };

        $scope.uploader.onCompleteItem = function (fileItem, response, status, headers) {
            if (response.Success) {

                $scope.form.entity.uploadResume = response.Data.fileName;
                $scope.result = response.Data;
                $scope.filepath = virtualDir + response.Data.filePath + response.Data.fileName;
                $scope.form.errors.uploadResume = '';
            } else {
                $scope.form.errors.uploadResume = response.Message;
                $scope.form.entity.uploadResume = '';
            }
            fileItem.uploader.clearQueue();
        }

        $scope.$watch('parentObj',
            function () {
                $scope.form.apiUrl =
                    'admin/Members/ResumeEdit/' + $scope.parentObj.editResume.siblingId;
                $scope.form.message = null;
                $scope.form.messageType = null;

                $scope.form.errors = [];
                loadEntity();
            });
    }
    ]);

myApp.controller('ctrlPaymentTransaction',
    ["$scope", "$http", "spinxList", function ($scope, $http, spinxList) {

        var searchApiUrl = 'api/admin/PaymentHistory/GetPaymentHistoryByMember/' + Id;


        var loadPaymentType = function () {
            $http.get("api/admin/PaymentHistory/PaymentTypeList").then(
                function (resp) {
                    $scope.paymentTypeData = resp.data.data;
                });
        };
            loadPaymentType();


        $scope.list = new spinxList(searchApiUrl);
        $scope.list.filters['paymentType'] = $scope.parentObj.filterPayment; 
        $scope.list.actions = [];
        $scope.list.paging = {};
    }
    ]);

myApp.controller('ctrlMemberAdvertisement',
    ["$scope", "$http", "spinxList", "ngDialog", "$controller", function ($scope, $http, spinxList, ngDialog, $controller) {

        $controller('BaseCommonController', { $scope: $scope });

        var searchApiUrl = 'api/admin/AdvertisementManagements/GetAdvertisementUserWise/' + Id;

        $scope.list = new spinxList(searchApiUrl);
        $scope.list.filters['advertisementStatus'] = $scope.parentObj.filterAdvertisement;
        $scope.list.actions = [];
        $scope.list.paging = {};

        $scope.addAdvertisement = function () {
            $scope.parentObj.template = templatePath + 'members/banners-ads-purchased-add.html';
        };

        $scope.advertisementEdit = function (advertisementId) {
            $scope.parentObj.advertisementEdit = {};
            $scope.parentObj.advertisementEdit.siblingId = advertisementId;
            $scope.parentObj.template = templatePath + 'members/banners-ads-purchased-edit.html';
        };

        $scope.advertisementEditSite = function (advertisementId) {
            $scope.advertisementEdit(advertisementId);
        };

        var loadAdvertisementTypes = function () {
            $http.get("api/list/AdvertisementTypesList").then(
                function (resp) {
                    $scope.advertisementTypes = resp.data;
                });
        };
        var loadAdvertisementPages = function () {
            $http.get("api/list/AdvertisementPagesList").then(
                function (resp) {
                    $scope.advertisementPages = resp.data;
                });
        };
        var loadAdvertisementCodes = function () {
            $http.get("api/list/AdvertisementPageTypesallList").then(
                function (resp) {
                    $scope.advertisementCodes = resp.data;
                });
        };

        var loadAdvertisementStatus = function () {
                $http.get("admin/Members/AdvertisementStatuslist").then(
                    function (resp) {
                        $scope.advertisementStatuslist = resp.data;
                    });
        };
        loadAdvertisementTypes();
        loadAdvertisementPages();
        loadAdvertisementCodes();
        loadAdvertisementStatus();
        $scope.filterSite = function () {
            $scope.list.load();
            this.loadAdvertisementTypes();
            this.loadAdvertisementPages();
        };

        $scope.shouldShow = function (advertisementStatus) {
            var resultReturn = false;
            if (advertisementStatus.key === 5)
                return resultReturn;
            else
                return true;
        }


        $scope.addDays = function (id, title) {
            $scope.Common(id, title);
        }
    }
    ]);

myApp.controller('ctrlFormAdvertisementEdit', ['$scope', '$http', 'spinxForm', 'filterFilter',
    function ($scope, $http, spinxForm, filterFilter) {

        $scope.siblingId = $scope.parentObj.advertisementEdit.siblingId;

        var apiUrl = 'admin/members/memberEdit/' + $scope.parentObj.advertisementEdit.siblingId;
        var apiListUrl = 'api/admin/advertisementManagements/' + $scope.parentObj.advertisementEdit.siblingId;
        var url = '';

        $scope.form = new spinxForm(url, apiUrl, $scope.parentObj.Id);

        $scope.form.afterSubmit = function () {
            $scope.frm.$setPristine();
        }

        $scope.defaultEntity = $scope.form.entity;

        $scope.backAdvertisement = function () {
            $scope.parentObj.template = templatePath + 'members/banners-ads-purchased.html';
        };

        $scope.form.onSaved = function () {
            $scope.backAdvertisement();
        }

        var loadAdvertisementPageTypes = function () {
            $http.get("api/list/AdvertisementPageTypesallList").then(
                function (resp) {
                    $scope.advertisementPageTypeCodes = resp.data;
                });
        };

        var loadAdvertisementStatus = function () {
            $http.get("admin/Members/AdvertisementStatuslist").then(
                function (resp) {
                    $scope.advertisementStatuslist = resp.data;
                });
        };

        var loadMembers = function () {
            $http.get("api/list/memberslist").then(
                function (resp) {
                    $scope.members = resp.data;
                });
        };

        var loadImageSize = function () {
            $scope.imageHeight = 742;
            $scope.imageWidth = 300;
            setTimeout(function () {
                $scope.$watch('advertisementPageTypeCode',
                    function (newValue, oldValue, scope) {
                        if (newValue !== undefined && newValue !== '') {
                            switch ($scope.advertisementPageTypeCode.advertisementTypeId) {
                                case 2:
                                    $scope.imageWidth = 320;
                                    $scope.imageHeight = 1200;
                                    break;
                                case 3:
                                    $scope.imageWidth = 1456;
                                    $scope.imageHeight = 180;
                                    break;
                                default:
                                    $scope.imageWidth = 742;
                                    $scope.imageHeight = 300;
                                    break;
                            }
                        }
                    });
            }, 3000);
        }

        loadAdvertisementPageTypes();
        loadAdvertisementStatus();
        loadMembers();

        var loadEntity = function () {
            $http.get(apiListUrl).then(function (resp) {
                loadImageSize();
                $scope.form.entity = resp.data;
                $scope.form.entity.advertisementPageTypeId = resp.data.advertisementPageTypeId;
                $scope.form.entity.finalStatus = resp.data.advertisementStatus;
                $scope.parentObj.applicationEdit = {};
                $scope.parentObj.applicationEdit.id = resp.data.id;
                setTimeout(function () {
                    $scope.advertisementPageTypeCode = filterFilter($scope.advertisementPageTypeCodes,
                        { id: $scope.form.entity.advertisementPageTypeId },
                        true)[0];
                },
                    200);
            },
                function (resp) {
                    if (resp.status === 404) {
                        $scope.form.entity = $scope.defaultEntity;
                        $scope.form.entity.isActive = true;
                    }
                });
        };

        loadEntity();

        $scope.shouldShow = function (advertisementStatus) {
            var resultReturn = false;
           
            switch ($scope.form.entity.finalStatus) {
                case 0:
                    {
                        if (advertisementStatus.key === 0 ||
                            advertisementStatus.key === 1 ||
                            advertisementStatus.key === 2)
                            resultReturn = true;
                        break;
                    }
                case 1:
                    {
                        if (advertisementStatus.key === 1 ||
                            advertisementStatus.key === 2 ||
                            advertisementStatus.key === 3)
                            resultReturn = true;
                        break;
                    }
                case 2:
                    {
                        if (advertisementStatus.key === 0 ||
                            advertisementStatus.key === 1 ||
                            advertisementStatus.key === 2)
                            resultReturn = true;
                        break;

                    }
                case 3:
                    {
                        if (advertisementStatus.key === 3 ||
                            advertisementStatus.key === 5)
                            resultReturn = true;
                        break;

                    }
                case 4:
                    {
                        if (advertisementStatus.key === 1 ||
                            advertisementStatus.key === 4)
                            resultReturn = true;
                        break;
                    }
                case 5:
                    {
                        if (advertisementStatus.key === 1 ||
                            advertisementStatus.key === 5)
                            resultReturn = true;
                        break;
                    }

            }
            return resultReturn;
        }

        $scope.$watch('parentObj',
            function () {
                $scope.form.apiUrl =
                    `admin/Members/MemberEdit/${$scope.parentObj.advertisementEdit.siblingId}`;

                $scope.form.message = null;
                $scope.form.messageType = null;

                $scope.form.errors = [];

                loadEntity();
            });
    }
]);

myApp.controller('ctrlFormAdvertisementAdd', ['$scope', '$http', 'spinxForm', 'filterFilter', function ($scope, $http, spinxForm, filterFilter) {

    var apiUrl = 'admin/AdvertisementManagements/Create/';
    var apiListUrl = 'api/admin/AdvertisementManagements/';
    var url = '';

    $scope.form = new spinxForm(url, apiUrl);

    $scope.defaultEntity = $scope.form.entity;

    $scope.backAdvertisement = function () {
        $scope.parentObj.template = templatePath + 'members/banners-ads-purchased.html';
    };

    $scope.form.onSaved = function () {
        $scope.backAdvertisement();
    }

    var loadMembers = function () {
        $http.get("api/list/memberslist").then(
            function (resp) {
                $scope.members = resp.data;
                $scope.form.entity.userId = Id;
            });
    };

    //code for bind advertisement status into create section
    //var loadAdvertisementStatus = function () {
    //    $http.get("admin/Members/AdvertisementStatuslist").then(
    //        function (resp) {
    //            $scope.advertisementStatuslist = resp.data;
    //        });
    //};

    $http.get("api/list/advertisementPageTypesList").then(
        function (resp) {
            $scope.advertisementPageTypeCodes = resp.data;
            loadMembers();
           // loadAdvertisementStatus();
        });

    $scope.imageHeight = 742;
    $scope.imageWidth = 300;
    $scope.$watch('advertisementPageTypeCode',
        function (newValue, oldValue, scope) {
            if (newValue !== undefined && newValue !== '') {
                switch ($scope.advertisementPageTypeCode.advertisementTypeId) {
                    case 2: $scope.imageWidth = 320; $scope.imageHeight = 1200; break;
                    case 3: $scope.imageWidth = 1456; $scope.imageHeight = 180; break;
                    default: $scope.imageWidth = 742; $scope.imageHeight = 300; break;
                }
            }
        });

    // only approve and pending status display when its create into admin side
    //$scope.shouldShow = function (advertisementStatus) {
    //    var resultReturn = false;
    //    if (advertisementStatus.key === 0 ||
    //        advertisementStatus.key === 1)
    //        resultReturn = true;

    //    return resultReturn;
    //}

    $scope.onSelectCallback = function ($item, $model, prop) {
        $scope.form.entity[prop] = $model.id;
    };
}
]);

myApp.controller('ctrlPostedJobs',
    [
        "$scope", "$http", "spinxList", "ngDialog", "$cookies", function ($scope, $http, spinxList, ngDialog, $cookies) {

            var searchApiUrl = 'api/admin/postajobs/getjobsbymember/' + Id;
            $scope.standardJobPostCredit = 0;
            $scope.premiumJobPostCredit = 0;
            $scope.list = new spinxList(searchApiUrl);
            $scope.list.filters['jobType'] = $scope.parentObj.filterPremiumJobs;
            $scope.list.filters['postAJobStatus'] = $scope.parentObj.filterPostAJobStatus;
            $scope.list.actions = [];

            var loadEntity = function () {

                $http.get('api/admin/members/' + Id).then(function (resp) {
                    $scope.standardJobPostCredit = resp.data.standardJobPostCredit;
                    $scope.premiumJobPostCredit = resp.data.premiumJobPostCredit;
                });
            };
            loadEntity();

            var loadPostAJobStatus = function () {
                $http.get("api/admin/PostAJobStatusList").then(
                    function (resp) {
                        $scope.postAJobStatusData = resp.data.data;
                    });
            };
            loadPostAJobStatus();
            $scope.loadJobBoards = function () {
                $http.get(window.virtualDir + "api/list/jobboardslist/")
                    .then(function (resp) {
                        $scope.jobboards = resp.data;
                    });
            };
            $scope.loadJobBoards();

            var loadPostAJobStatus = function () {
                $http.get("api/admin/PostAJobTypeList").then(
                    function (resp) {
                        $scope.postAJobTypeData = resp.data.data;
                    });
            };
            loadPostAJobStatus();

            $scope.filterSite = function () {
                $scope.list.load();
                this.loadMembers();
                this.loadJobBoards();
            };

            $scope.EditApprove = function (jobPostId, jobPostParentID) {
                $.confirm({
                    text: "Are you sure want to approve the edit?",
                    title: "Confirmation required",
                    confirmButtonClass: "btn-danger",
                    confirm: function (button) {

                        $scope.list.action = 'approved';
                        $scope.list.filters.parentId = jobPostParentID;
                        $scope.list.ids[0] = jobPostId;
                        $scope.list.fireAction();
                    }
                });
            };

            $scope.addDays = function (id, JobTitle) {
                $scope.PostAjobId = id;
                $scope.JobTitle = JobTitle;
                ngDialog.open({
                    template: 'Content/Admin/app/members/add-days.html',
                    className: 'ngdialog-theme-default add-days-popup',
                    scope: $scope,
                    controller: 'ctrlForm'
                });
                setTimeout(function () { $("a.close").remove(); }, 500);
            };

            $scope.addCredit = function () {
                ngDialog.open({
                    template: 'Content/Admin/app/members/add-credits.html',
                    className: 'ngdialog-theme-default add-days-popup',
                    scope: $scope,
                    controller: ['$scope', 'spinxForm', function ($scope, spinxForm) {
                        var apiUrl = 'admin/postajobs/AddCredit';
                        var url = 'admin/postajobs/';
                        $scope.form = new spinxForm(url, apiUrl);
                        $scope.form.entity.MemberId = Id;
                        $scope.form.onSaved = function () {
                            ngDialog.close();
                            loadEntity();
                            var success = 1;
                            var cookie = $cookies.get(`Flash.${success}`);
                            if (cookie !== undefined) {
                                $scope.list.message = cookie;
                                $scope.list.messageType = success;
                                $cookies.remove(`Flash.${success}`);
                            }
                        }
                    }]
                });
                setTimeout(function () { $("a.close").remove(); }, 500);
            };

            $scope.addJob = function () {
                $scope.parentObj.template = templatePath + 'members/job-add.html?v=2';
            };

            $scope.editJob = function (postJobId) {
                $scope.parentObj.postJobEditId = postJobId;
                $scope.parentObj.template = templatePath + 'members/job-edit.html';
            };
        }
    ]);

myApp.controller('ctrlAddJob',
    [
        '$scope', '$http', 'spinxForm', function ($scope, $http, spinxForm) {
            var apiUrl = 'admin/postajobs/CreateByMember/';
            var url = 'admin/postajobs';

            $scope.form = new spinxForm(url, apiUrl);

            $scope.form.onSaved = function () {
                $scope.backPostAJobList();
            }
            $scope.backPostAJobList = function () {
                $scope.parentObj.template = templatePath + 'members/posted-jobs.html';
            };

            var loadJobBoards = function () {
                $http.get("api/list/jobboardslist/").then(
                    function (resp) {
                        $scope.jobboards = resp.data;
                    });
            };

            var loadMembers = function () {
                $http.get("api/list/memberslist").then(
                    function (resp) {
                        $scope.members = resp.data;
                        $scope.form.entity.MemberId = Id;
                    });
            };

            loadMembers();
            loadJobBoards();

            $scope.generateSlug = function (title) {
                $scope.form.entity.slug = Slug.slugify(title);
            };

            $scope.loadCreditPremiumList = function (intJobBoardId) {
                var parameters = {
                    jobBoardId: intJobBoardId
                };

                $http.get('api/list/PostAJobsCreditBackendPremiumList', { params: parameters }).then(function (resp) {
                    $scope.creditList = resp.data.data;
                    $scope.form.entity.postAJobsCreditId = $scope.creditList[0].id;
                    $scope.form.entity.jobType = 1;
                    delete $scope.form.entity.postAJobsCreditPremiumId;
                });
            }

            $scope.creditchange = function () {
                delete $scope.form.entity.postAJobsCreditPremiumId;
                //Enum Standard = 1,Premium = 2
                $scope.form.entity.jobType = 1;
            };

            $scope.creditPremiumchange = function (value) {
                //Enum Standard = 1,Premium = 2
                $scope.form.entity.jobType = 2;
                //$scope.form.entity.postAJobsCreditId = value;
            };

            $scope.onSelectCallback = function ($item, $model, prop) {
                $scope.form.entity[prop] = $model.id;
            };
        }
    ]);

myApp.controller('ctrlEditJob',
    [
        '$scope', '$http', 'spinxForm', function ($scope, $http, spinxForm) {
            $scope.Id = Id;
            var apiUrl = 'admin/postajobs/Edit/' + $scope.parentObj.postJobEditId;
            var apiListUrl = 'api/admin/postajobs/' + $scope.parentObj.postJobEditId;
            var url = 'admin/postajobs';
            $scope.form = new spinxForm(url, apiUrl, $scope.parentObj.postJobEditId);
            var defaultEntity = $scope.form.entity;

            $scope.backPostAJobList = function () {
                $scope.parentObj.template = templatePath + 'members/posted-jobs.html';
            };

            $scope.form.onSaved = function () {
                $scope.backPostAJobList();
            }
            var loadPostAJobStatus = function () {
                $http.get("api/admin/PostAJobStatusList").then(
                    function (resp) {
                        $scope.postAJobStatusData = resp.data.data;
                    });
            };
            loadPostAJobStatus();
            var loadMembers = function () {
                $http.get("api/list/memberslist").then(
                    function (resp) {
                        $scope.members = resp.data;
                        $scope.form.entity.userId = Id;
                    });
            };
            var loadJobBoards = function () {
                $http.get("api/list/jobboardslist/").then(
                    function (resp) {
                        $scope.jobboards = resp.data;
                    });
            };
            var loadEntity = function () {
                $http.get(apiListUrl).then(function (resp) {
                    loadMembers();
                    loadJobBoards();
                    $scope.form.entity = resp.data.data;
                },
                    function (resp) {
                        if (resp.status === 404) {
                            $scope.form.entity = defaultEntity;
                            $scope.form.entity.isActive = true;
                        }
                    });
            };
            loadEntity();
        }
    ]);

myApp.controller('ctrlForm', ['$scope', '$http', 'spinxForm', 'ngDialog', '$cookies', function ($scope, $http, spinxForm, ngDialog, $cookies) {
    var apiUrl = 'admin/postajobs/SaveDays';
    var url = 'admin/postajobs/';
    $scope.form = new spinxForm(url, apiUrl);
    $scope.form.entity.PostAjobId = $scope.PostAjobId;
    $scope.form.onSaved = function () {
        ngDialog.close();
        var success = 1;
        var cookie = $cookies.get(`Flash.${success}`);
        if (cookie !== undefined) {
            $scope.list.message = cookie;
            $scope.list.messageType = success;
            $cookies.remove(`Flash.${success}`);
        }
        $scope.list.load();
    }
}]);

myApp.controller('BaseCommonController',
    ["$scope", "$http", "spinxForm", "ngDialog", "$cookies", function ($scope, $http, spinxForm, ngDialog, $cookies) {

        $scope.Common = function (id, title) {
            $scope.Id = id;
            $scope.Title = title;

            ngDialog.open({
                template: templatePath + 'members/add-days.html',
                className: 'ngdialog-theme-default add-days-popup',
                scope: $scope
            });
            setTimeout(function () { $("a.close").remove(); }, 500);

            var apiUrl = '';
            var url = '';

            if (title === 'Resume Access') {
                apiUrl = 'admin/ResumeAccessPlanTransactions/SaveDays/' + id;
                url = 'admin/ResumeAccessPlanTransactions/';
            }
            else if (title === "Job") {
                apiUrl = 'admin/postajobs/SaveDays' + id;
                url = 'admin/postajobs/';
            }
            else if (title === "Advertisement") {
                apiUrl = 'admin/AdvertisementManagements/ExtendedDays/' + id;
                url = 'admin/members/edit/' + id;
            }

            if (apiUrl !== '') {
                $scope.form = new spinxForm(url, apiUrl);
                if (title === "Advertisement") {
                    $scope.backAdvertisement = function () {
                        ngDialog.close();
                        $scope.parentObj.template = templatePath + 'members/banners-ads-purchased.html';
                        $scope.list.load();
                        var success = 1;
                        var cookie = $cookies.get(`Flash.${success}`);
                        if (cookie !== undefined) {
                            $scope.list.message = cookie;
                            $scope.list.messageType = success;
                            $cookies.remove(`Flash.${success}`);
                        }
                    };
                    $scope.form.onSaved = function () {
                        $scope.backAdvertisement();
                    }
                }
                else if (title === "Resume Access") {
                    $scope.backPage = function () {
                        ngDialog.close();
                        $scope.parentObj.template = templatePath + 'members/resume-access-purchased.html';
                        $scope.list.load();
                        $scope.parentObj.planActive = true;
                        var success = 1;
                        var cookie = $cookies.get(`Flash.${success}`);
                        if (cookie !== undefined) {
                            $scope.list.message = cookie;
                            $scope.list.messageType = success;
                            $cookies.remove(`Flash.${success}`);
                        }
                    };
                    $scope.form.onSaved = function () {
                        $scope.backPage();
                    }
                }
            }

        }
    }
    ]);
    */