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
                        $scope.parentObj.template = templatePath + 'questions.html?id=1';
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
        "$scope", "$http", "spinxList", "spinxForm", function ($scope, $http, spinxList, spinxForm) {
            var searchApiUrl = 'api/admin/QuizAnswers?sortColumn=sortorder&sortType=asc&QuizQuestionId=' + $scope.parentObj.questionEdit.Qid;


            $scope.parentObj.list = function () {
                $scope.list = new spinxList(searchApiUrl);
                $scope.list.filters.size = 9999;
            }

            $scope.parentObj.list();
            $scope.newField = {};
            $scope.editing = false;

            $scope.editAns = function (answer) {
                var index = -1;
                $scope.list.data.some(function (obj, i) {
                    return obj.id === answer.id ? index = i : false;
                });
                $scope.editing = index;
                answer.editMode = true;
                $scope.newField = angular.copy(answer);
            }

            $scope.addNewAnswer = function () {
                $("#NewAnswer").css("border", "1px solid grey");
                
                $("#BlankAnswerRow").show();
            }
            $scope.cancelAdd = function () {
                $("#BlankAnswerRow").hide();
                $("#NewAnswerError").hide();
            }
            $scope.saveAnswer = function (answerEntity) {
                var apiUrl = 'api/admin/QuizAnswers/' + answerEntity.id;
                if (jQuery.trim(answerEntity.answer) == "") {
                    $("#ans" + answerEntity.id).css("border", "1px solid #b94a48");
                    $("#ansError" + answerEntity.id).show();
                } else {
                    $("#ans" + answerEntity.id).css("border", "1px solid grey");
                    $("#ansError" + answerEntity.id).hide();
                    $http({
                        url: apiUrl,
                        method: "PUT",
                        data: angular.toJson(answerEntity)
                    });
                    //$scope.parentObj.list();
                    answerEntity.editMode = false;
                }
            };

            $scope.saveNewAnswer = function () {
                var apiUrl = 'api/admin/QuizAnswers/';
                var url = 'admin/QuizAnswers';

                $scope.entity = {};

                $scope.entity.answer = $("#NewAnswer").val();
                if (jQuery.trim($("#NewAnswer").val()) == "") {
                    $("#NewAnswer").css("border", "1px solid #b94a48");
                    $("#NewAnswerError").show();
                } else {
                    $("#NewAnswer").css("border", "1px solid grey");
                    $("#NewAnswerError").hide();
                    $scope.entity.quizQuestionId = $scope.parentObj.questionEdit.Qid;
                    $scope.entity.isCorrectAnswer = false;
                    $http({
                        url: apiUrl,
                        method: "POST",
                        data: angular.toJson($scope.entity)
                    });
                    $scope.parentObj.list();
                    $scope.parentObj.list();
                    $("#NewAnswer").val('');
                }
            };

            $scope.cancel = function (answer) {
                if ($scope.editing !== false) {
                    $scope.list.data[$scope.editing] = $scope.newField;
                    $scope.editing = false;
                }
                $scope.parentObj.list();
                answer.editMode = false;
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