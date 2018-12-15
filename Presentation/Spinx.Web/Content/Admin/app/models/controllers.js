var templatePath = 'Content/Admin/app/QuizQuestions/';
myApp.controller("quizQuestionsEditController",
    [
        '$scope', '$http', function ($scope, $http) {
            $scope.parentObj = {};
            $scope.parentObj.tabs = ['information', 'questions'];
            $scope.parentObj.tab = 'information';
            $scope.parentObj.template = templatePath + 'quiz-edit.html';

            $scope.tabChange = function ($event, tabId) {

                $scope.parentObj.tab = tabId;

                switch (tabId) {
                    case 'information':
                        $scope.parentObj.template = templatePath + 'quiz-edit.html';
                        break;
                    case 'questions':
                        $scope.parentObj.template = templatePath + 'questions.html';
                        break;
                }
            };

        }
    ]);
myApp.controller('ctrlQuizForm',
    [
        '$scope', '$http', 'spinxForm', 'Slug', function ($scope, $http, spinxForm, Slug) {
            var apiUrl = 'api/admin/quizs/' + Id;
            var url = 'admin/quizs';

            $scope.form = new spinxForm(url, apiUrl, Id);
            $scope.virtualDir = virtualDir;
            var defaultEntity = $scope.form.entity;

            var loadQuizCategories = function () {
                $http.get($scope.virtualDir + "api/list/quizcategorieslist/").then(
                    function (resp) {
                        $scope.quizcategories = resp.data;
                    });
            };

            $scope.generateSlug = function (title) {
                $scope.form.entity.slug = Slug.slugify(title);
            };

            var loadEntity = function () {

                $http.get(apiUrl).then(function (resp) {
                    loadQuizCategories();
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
myApp.controller("ctrlquestionsList",
    [
        "$scope", "$http", "spinxList", function ($scope, $http, spinxList) {

            var searchApiUrl = 'api/admin/QuizQuestions?QuizId=' + Id;
            $scope.list = new spinxList(searchApiUrl);

            $scope.addquestion = function () {
                $scope.parentObj.template = templatePath + 'questions-add.html';
            };

            $scope.questionEdit = function (Qid) {
                $scope.parentObj.questionEdit = {};
                $scope.parentObj.questionEdit.Qid = Qid;
                $scope.parentObj.template = templatePath + 'questions-edit.html';
            };


            $scope.sortableOptions = {
                handle: '> .drag',
                stop: function (e, ui) {

                    var data = $scope.list.data.map(function (i) {
                        return i.id;
                    });
                    var apiUrl = 'api/admin/QuizQuestionsSequence';
                    $http.post(apiUrl, data);
                }
            };
        }
    ]);


myApp.controller('ctrlFormQuestionAdd',
    [
        '$scope', '$http', 'spinxForm', 'Slug', function ($scope, $http, spinxForm, Slug) {

            var apiUrl = 'api/admin/QuizQuestions';
            $scope.form = new spinxForm("", apiUrl);
            $scope.form.entity.quizId = Id;
            $scope.generateSlug = function (title) {
                $scope.form.entity.slug = Slug.slugify(title);
            };


            spinxForm.prototype.submit = function () {
                this.loading = true;
                $http({
                    url: this.apiUrl,
                    method: this.method,
                    data: angular.toJson(this.entity)
                }).then(function (resp) {
                    this.loading = false;
                    this.message = resp.data.message;
                    this.messageType = resp.data.messageType;
                    this.errors = resp.data.errors;
                    $scope.parentObj.questionEdit = {};
                    if (resp.data.id > 0) {
                        $scope.parentObj.questionEdit.Qid = resp.data.id;
                        $scope.parentObj.template = templatePath + 'questions-edit.html';
                    }

                }.bind(this),
                    function (error) { this.loading = false; }.bind(this));
            }

            $scope.backQuestions = function () {
                $scope.parentObj.template = templatePath + 'questions.html';
            };

            $scope.form.onSaved = function () {
                $scope.backQuestions();
            }
        }
    ]);

myApp.controller('ctrlQuestionEdit',
    [
        '$scope', '$http', 'spinxForm', function ($scope, $http, spinxForm) {

            var apiUrl = `api/admin/QuizQuestions/${$scope.parentObj.questionEdit
                .Qid}`;
            $scope.form = new spinxForm("", apiUrl, $scope.parentObj.questionEdit.Qid);

            $scope.defaultEntity = $scope.form.entity;

            var loadEntity = function () {
                $http.get($scope.form.apiUrl).then(function (resp) {

                    $scope.form.entity = resp.data;
                },
                    function (resp) {
                        if (resp.status === 404) {

                            $scope.form.entity = $scope.defaultEntity;

                            $scope.form.entity.isActive = true;
                        }
                    });
            };
            loadEntity();

            $scope.backQuestions = function () {
                $scope.parentObj.template = templatePath + 'questions.html';
            };

            $scope.form.onSaved = function () {
                $scope.backQuestions();
            }
        }
    ]);

myApp.controller("ctrlanswersList",
    [
        "$scope", "$http", "spinxList", function ($scope, $http, spinxList) {
            var searchApiUrl = 'api/admin/QuizAnswers?QuizQuestionId=' + $scope.parentObj.questionEdit.Qid;
            $scope.list = new spinxList(searchApiUrl);

            $scope.addanswer = function () {
                $("#secEditAnswer").hide();
                $("#secAddAnswer").show();
                $("#secAddAnswer").find("input:first").focus();
            };
            $scope.editanswer = function (answerid) {
                $scope.parentObj.loadAnswerEntity(answerid);
                $("#secEditAnswer").show();
                $("#secAddAnswer").hide();
            };

            $scope.questionEdit = function (Qid) {
                $scope.parentObj.questionEdit = {};
                $scope.parentObj.questionEdit.Qid = Qid;
                $scope.parentObj.template = templatePath + 'questions-edit.html';
            };


            $scope.sortableOptions = {
                handle: '> .drag',
                stop: function (e, ui) {

                    var data = $scope.list.data.map(function (i) {
                        return i.id;
                    });
                    var apiUrl = 'api/admin/QuizAnswerSequence';
                    $http.post(apiUrl, data);
                }
            };
        }
    ]);
myApp.controller('ctrlAddAnswer',
    [
        '$scope', '$http', 'spinxForm', 'Slug', function ($scope, $http, spinxForm, Slug) {

            var apiUrl = 'api/admin/QuizAnswers';
            $scope.form = new spinxForm("", apiUrl);
            $scope.form.entity.quizQuestionId = $scope.parentObj.questionEdit.Qid;

            spinxForm.prototype.submit = function () {
                this.loading = true;
                $http({
                    url: this.apiUrl,
                    method: this.method,
                    data: angular.toJson(this.entity)
                }).then(function (resp) {
                    this.loading = false;
                    this.message = resp.data.message;
                    this.messageType = resp.data.messageType;
                    this.errors = resp.data.errors;
                    if (resp.data.success) {
                        $scope.parentObj.questionEdit.Qid = $scope.parentObj.questionEdit.Qid;
                        $scope.parentObj.template = templatePath + 'questions-edit.html';
                        $("#secAddAnswer").find("input:first").focus();
                    }

                }.bind(this),
                    function (error) {
                        this.loading = false;
                    }.bind(this));
            }
        }
    ]);
myApp.controller('ctrlEditAnswer',
    [
        '$scope', '$http', 'spinxForm', function ($scope, $http, spinxForm) {
            $scope.Id = Id;

            $scope.parentObj.loadAnswerEntity = function (answerid) {
                $scope.ansId = answerid;
                var apiUrl = 'api/admin/QuizAnswers/' + answerid;
                var url = 'admin/QuizAnswers';
                $scope.form = new spinxForm(url, apiUrl, answerid);
                var defaultEntity = $scope.form.entity;
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
            spinxForm.prototype.submit = function () {
                var apiUrl = 'api/admin/QuizAnswers/' + $scope.ansId;
                var url = 'admin/QuizAnswers';
                this.loading = true;
                $http({
                    url: this.apiUrl,
                    method: this.method,
                    data: angular.toJson(this.entity)
                }).then(function (resp) {
                    this.loading = false;
                    this.message = resp.data.message;
                    this.messageType = resp.data.messageType;

                    this.errors = resp.data.errors;
                    if (resp.data.success) {
                        $scope.parentObj.questionEdit.Qid = $scope.parentObj.questionEdit.Qid;
                        $scope.parentObj.template = null;
                        $scope.parentObj.template = templatePath + 'questions-edit.html?v=' + Math.random();
                    }

                }.bind(this),
                    function (error) {
                        this.loading = false;
                    }.bind(this));
            }

        }
    ]);