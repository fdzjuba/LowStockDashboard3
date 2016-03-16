var App = angular.module('App', ['ngRoute', 'ui.bootstrap', 'chart.js']);

App.service('Api', ['$http', ApiService]);

App.controller('MainController', MainController);
App.controller('GridController', GridController);
App.controller('ViewProductController', ViewProductController);

if (getParameterByName("token") != "") {
    sessionStorage.setItem("token", getParameterByName("token"));
    location.search = "";
}

App.run(['$http', function ($http) {
    $http.defaults.headers.common['Authorization'] = "token " + sessionStorage.getItem("token");
}]);



if (sessionStorage.getItem("token") == null || sessionStorage.getItem("token") == "") {
    window.location.replace("/Home/NotAuthorized");
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


var configFunction = function ($routeProvider, $httpProvider) {
    $routeProvider.
        when('/grid', {
            templateUrl: 'SPA/Views/Grid.html',
            controller: GridController
        })
       .otherwise({
           redirectTo: function () {
               return '/grid';
           }
       });
}
configFunction.$inject = ['$routeProvider', '$httpProvider'];

App.config(configFunction);


App.run(function ($rootScope, $timeout) {
    $rootScope.errorMessage = "";
    $rootScope.isError = false;

    $rootScope.setError = function (errorMessage) {
        $rootScope.errorMessage = errorMessage;
        if (errorMessage != null && errorMessage != "") {
            $rootScope.isError = true;
        } else {
            $rootScope.isError = false;
        }
        $timeout(function () {
            $rootScope.setError(null);
        }, 3000);
    }
});


function SetBusy(element, hide) {
    if (hide == true) {
        element.LoadingOverlay("hide");
    } else {
        element.LoadingOverlay("show", {
            image: "",
            fontawesome: "fa fa-spinner fa-spin"
        });
    }
}
