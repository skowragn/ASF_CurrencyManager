var app = angular.module('CurrencyApp', ['ui.bootstrap']);
app.run(function () { });

app.controller('CurrencyAppController', ['$rootScope', '$scope', '$http', '$timeout', function ($rootScope, $scope, $http, $timeout) {

    $scope.refresh = function () {
        $http.get('api/CurrencyApi?c=' + new Date().getTime())
            .then(function (data, status) {
                $scope.currencies = data;
            }, function (data, status) {
                $scope.currencies = undefined;
            });
    };

    $scope.remove = function (item) {
        $http.delete('api/CurrencyApi/' + item)
            .then(function (data, status) {
                $scope.refresh();
            })
    };

    $scope.add = function (item) {
        var fd = new FormData();
        fd.append('item', item);
        $http.put('api/CurrencyApi/' + item, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function (data, status) {
                $scope.refresh();
                $scope.item = undefined;
            })
    };
}]);