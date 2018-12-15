var myApp = angular.module('myApp', ["ngCookies","ngSanitize"]);

myApp.run(["$rootScope", "$http", "$cookies", function ($rootScope, $http, $cookies) {

    //$http.get('api/admin/myaccess').then(function (resp) {

    //    var data = JSON.stringify(resp.data);
    //    var ciphertext = CryptoJS.AES.encrypt(data, secretKey).toString();
    //    localStorageService.set('access', ciphertext, 15);
    //});   

    //$rootScope.q = getParameterByName('q');
    //$rootScope.year = (new Date()).getFullYear();
}]);

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}