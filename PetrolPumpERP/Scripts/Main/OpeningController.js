PetroliumApp.controller("OpeningController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.DisplayBalType = false;
    $scope.MainOpeningBalanceList = [];
    $scope.OpeningBalanceList = [];
    $scope.SearchOpeningBalanceList = [];
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;

    function GetAllOpeningBalLedgers()
    {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/OpenignBalance/GetAllOpeningBalLedgers";
        $http.get(url)
        .then(function (response) {
            $scope.MainOpeningBalanceList = response.data.OpeningBalanceList;
            $scope.OpeningBalanceList = response.data.OpeningBalanceList;
            $scope.SearchOpeningBalanceList = $filter('limitTo')($scope.OpeningBalanceList, $scope.Paging, $scope.CurruntIndex);
            document.getElementById("mainbody").removeChild(spinner.el);
        },
        function (response) {
            var objShowCustomAlert = new ShowCustomAlert({
                Title: "",
                Message: response.data.Message,
                Type: "alert",
            });
            objShowCustomAlert.ShowCustomAlertBox();
            document.getElementById("mainbody").removeChild(spinner.el);
        });

    }

    $scope.EditClick = function (model)
    {
        $("#spanbaltype_" + model.OpeningBalanceId).hide();
        $("#spanopening_" + model.OpeningBalanceId).hide();
        $("#ddlbaltype_" + model.OpeningBalanceId).show();
        $("#txt_" + model.OpeningBalanceId).show();
        if (model.BalType!="-") {
            $("#ddlbaltype_" + model.OpeningBalanceId).val(model.BalType);
        }
        $("#txt_" + model.OpeningBalanceId).val(model.OpeningBal);
    }

    $scope.UpdateClick = function (model) {
        $("#spanbaltype_" + model.OpeningBalanceId).hide();
        $("#spanopening_" + model.OpeningBalanceId).hide();
        $("#ddlbaltype_" + model.OpeningBalanceId).show();
        $("#txt_" + model.OpeningBalanceId).show();
        if ($("#ddlbaltype_" + model.OpeningBalanceId).val() == "0") {
            var objShowCustomAlert = new ShowCustomAlert({
                Title: "",
                Message: "Please select Opening balance type from drop down.",
                Type: "alert",
            });
            objShowCustomAlert.ShowCustomAlertBox();
            return false;
        }
        if ($("#txt_" + model.OpeningBalanceId).val() == "")
        {
            var objShowCustomAlert = new ShowCustomAlert({
                Title: "",
                Message: "Please fill amount for Opening balance",
                Type: "alert",
            });
            objShowCustomAlert.ShowCustomAlertBox();
            return false;
        }

        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var model =
            {
                OpeningBalanceId: model.OpeningBalanceId,
                OpeningBal: $("#txt_" + model.OpeningBalanceId).val(),
                BalType:$("#ddlbaltype_" + model.OpeningBalanceId).val(),
            };

        var url = GetVirtualDirectory() + '/OpenignBalance/Save';
        var req = {
            method: 'POST',
            url: url,
            headers: {
                'Content-Type': "application/json"
            },
            data: model,
        }
        $http(req).then(function (response) {
            if (response.data.Status == true) {
                angular.forEach($scope.OpeningBalanceList, function (value, key) {
                    if (value.OpeningBalanceId == model.OpeningBalanceId) {
                        $scope.OpeningBalanceList[key].OpeningBal = model.OpeningBal;
                        $scope.OpeningBalanceList[key].BalType = model.BalType;
                    }
                });
                setTimeout(function () {
                    $scope.$apply(function () {
                        $scope.MainOpeningBalanceList= $scope.OpeningBalanceList;
                        $scope.SearchOpeningBalanceList = $scope.OpeningBalanceList;
                        $scope.First();
                        $("#spanbaltype_" + model.OpeningBalanceId).show();
                        $("#spanopening_" + model.OpeningBalanceId).show();
                        $("#ddlbaltype_" + model.OpeningBalanceId).hide();
                        $("#txt_" + model.OpeningBalanceId).hide();
                        //$("#spanbaltype_" + model.OpeningBalanceId).html();
                        //$("#spanopening_" + model.OpeningBalanceId).show();
                    });
                }, 1000);
            }
            else {
                var objShowCustomAlert = new ShowCustomAlert({
                    Title: "",
                    Message: response.data.Message,
                    Type: "alert",
                });
                objShowCustomAlert.ShowCustomAlertBox();
            }
            document.getElementById("mainbody").removeChild(spinner.el);
        },
        function (response) {
            var objShowCustomAlert = new ShowCustomAlert({
                Title: "",
                Message: response.data.Message,
                Type: "alert",
            });
            objShowCustomAlert.ShowCustomAlertBox();
            document.getElementById("mainbody").removeChild(spinner.el);
        });
    }

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
        GetAllOpeningBalLedgers();
        //$scope.OpeningBalanceList = JSON.parse($("#openingbalanceList").val());
        //$scope.SearchOpeningBalanceList = $filter('limitTo')($scope.OpeningBalanceList, $scope.Paging, $scope.CurruntIndex);
    }

    $scope.init();

}]);