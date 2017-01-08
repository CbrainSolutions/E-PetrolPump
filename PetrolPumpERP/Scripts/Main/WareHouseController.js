PetroliumApp.controller("WareHouseController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.WareHouseList = [];
    $scope.SearchWareHouseList = [];
    //$scope.SubLedgers = [];
    //$scope.AddedSubledgers = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsName: false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.WareHouseNo = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.AccountTypeModel = { AccountTypeId: 0, AccountType: "", SubledgerId: 0 };

    $scope.Prefix = "";

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }

    $scope.FilterList = function () {
        $scope.WareHouseList = $filter('filter')(JSON.parse($("#warehouselist").val()), { WareHouseName: $scope.Prefix })
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.WareHouseList = JSON.parse($("#warehouselist").val());
        $scope.SearchWareHouseList = $scope.WareHouseList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#WareHouseName").val("");
        $("#Address").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (WareHouseModel) {
        $("#WareHouseName").val(WareHouseModel.WareHouseName);
        $scope.WareHouseNo = WareHouseModel.WareHouseNo;
        $("#Address").val(WareHouseModel.Address);
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {
        if ($("#WareHouseName").val() == "") {
            $scope.ErrorModel.IsName = true;
            $scope.ErrorMessage = "Ware house name should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsName = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);

        var model = [];
        if (isEdit == false) {
            model = { WareHouseNo: $scope.WareHouseNo, WareHouseName: $("#WareHouseName").val() };
        }
        else {
            model = { WareHouseName: $("#WareHouseName").val(),Address:$("#Address").val() };
        }
        var url = GetVirtualDirectory() + '/WareHouse/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/WareHouse/Update';
        }
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
                if (isEdit == true) {
                    model = { WareHouseName: $("#WareHouseName").val(), WareHouseNo: response.data.Id,Address:$("#Address").val() };
                    $scope.WareHouseList.push(model);
                }
                else {
                    angular.forEach($scope.WareHouseList, function (value, key) {
                        if (value.WareHouseNo == model.WareHouseNo) {
                            $scope.WareHouseList[key].ProductType = model.ProductType;
                            $scope.WareHouseList[key].Address = model.Address;
                        }
                    });
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        $("#warehouselist").val(JSON.stringify($scope.WareHouseList));
                        $scope.SearchWareHouseList = $scope.WareHouseList;
                        $scope.First();
                        $scope.CancelClick();
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
        $scope.SearchWareHouseList = $filter('limitTo')($scope.WareHouseList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchWareHouseList = $filter('limitTo')($scope.WareHouseList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.WareHouseList.length) {
            $scope.SearchWareHouseList = $filter('limitTo')($scope.WareHouseList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.WareHouseList.length;
        var rem = parseInt($scope.WareHouseList.length) % parseInt($scope.Paging);
        var position = $scope.WareHouseList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.WareHouseList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchWareHouseList = $filter('limitTo')($scope.WareHouseList, $scope.Paging, position);
    }

    $scope.init = function () {
        $scope.WareHouseList = JSON.parse($("#warehouselist").val());
        $scope.SearchWareHouseList = $filter('limitTo')($scope.WareHouseList, $scope.Paging, $scope.CurruntIndex);
    }

    $scope.init();

}]);