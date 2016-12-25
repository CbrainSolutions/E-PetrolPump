PetroliumApp.controller("OpeningController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.DisplayBalType = false;
    $scope.OpeningBalanceList = [];
    $scope.SearchOpeningBalanceList = [];
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;

    $scope.First = function () {
        $scope.CurruntIndex = 0;
        $scope.SearchOpeningBalanceList = $filter('limitTo')($scope.OpeningBalanceList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.OpeningBalanceList = $filter('limitTo')($scope.OpeningBalanceList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.OpeningBalanceList.length) {
            $scope.SearchOpeningBalanceList = $filter('limitTo')($scope.OpeningBalanceList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.OpeningBalanceList.length;
        var rem = parseInt($scope.OpeningBalanceList.length) % parseInt($scope.Paging);
        var position = $scope.OpeningBalanceList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.OpeningBalanceList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchOpeningBalanceList = $filter('limitTo')($scope.OpeningBalanceList, $scope.Paging, position);
    }

    $scope.init = function () {
        $scope.OpeningBalanceList = JSON.parse($("#openingbalanceList").val());
        $scope.SearchOpeningBalanceList = $filter('limitTo')($scope.OpeningBalanceList, $scope.Paging, $scope.CurruntIndex);
    }

    $scope.init();

}]);