PetroliumApp.controller("ProductTypeController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.ProductTypeList = [];
    $scope.SearchProductTypeList = [];
    //$scope.SubLedgers = [];
    //$scope.AddedSubledgers = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsProductTypeDesc: false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.ProductTypeId = 0;
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
        $scope.ProductTypeList = $filter('filter')(JSON.parse($("#producttypelist").val()), { UnitDesc: $scope.Prefix })
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.ProductTypeList = JSON.parse($("#producttypelist").val());
        $scope.SearchProductTypeList = $scope.ProductTypeList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#ProductType").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (ProductTypeModel) {
        $("#ProductType").val(ProductTypeModel.ProductType);
        $scope.UnitCode = ProductTypeModel.ProductTypeId;
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {
        if ($("#ProductType").val() == "") {
            $scope.ErrorModel.IsProductTypeDesc = true;
            $scope.ErrorMessage = "Product Type should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsProductTypeDesc = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);

        var model = [];
        if (isEdit == false) {
            model = { ProductTypeId: $scope.ProductTypeId, ProductType: $("#ProductType").val() };
        }
        else {
            model = { ProductType: $("#ProductType").val() };
        }
        var url = GetVirtualDirectory() + '/ProductType/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/ProductType/Update';
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
                    model = { ProductType: $("#ProductType").val(), ProductTypeId: response.data.Id };
                    $scope.ProductTypeList.push(model);
                }
                else {
                    angular.forEach($scope.ProductTypeList, function (value, key) {
                        if (value.ProductTypeId == model.ProductTypeId) {
                            $scope.ProductTypeList[key].ProductType = model.ProductType;
                        }
                    });
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        $("#producttypelist").val(JSON.stringify($scope.ProductTypeList));
                        $scope.SearchProductTypeList = $scope.ProductTypeList;
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
        $scope.SearchProductTypeList = $filter('limitTo')($scope.ProductTypeList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchProductTypeList = $filter('limitTo')($scope.ProductTypeList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.ProductTypeList.length) {
            $scope.SearchProductTypeList = $filter('limitTo')($scope.ProductTypeList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.ProductTypeList.length;
        var rem = parseInt($scope.ProductTypeList.length) % parseInt($scope.Paging);
        var position = $scope.ProductTypeList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.ProductTypeList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchProductTypeList = $filter('limitTo')($scope.ProductTypeList, $scope.Paging, position);
    }

    $scope.init = function () {
        $scope.ProductTypeList = JSON.parse($("#producttypelist").val());
        $scope.SearchProductTypeList = $filter('limitTo')($scope.ProductTypeList, $scope.Paging, $scope.CurruntIndex);
    }

    $scope.init();

}]);