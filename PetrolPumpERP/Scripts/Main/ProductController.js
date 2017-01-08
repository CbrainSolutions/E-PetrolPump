PetroliumApp.controller("ProductController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.ProductList = [];
    $scope.SearchProductList = [];
    $scope.ProductTypeList = [];
    $scope.UOMList = [];
    $scope.WareHouseList = [];
    $scope.Details = true;
    $scope.ErrorModel = {
        IsProductType: false,
        IsUOM: false,
        IsProductName: false,
    };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    
    
    $scope.ProductId = 0;

    $scope.Prefix = "";

    

    $scope.ValidateForm = function () {
        var valid = false;
        if ($("#ProductType").val() == "0") {
            $scope.ErrorMessage = "Account type should be selected.";
            $scope.ErrorModel.IsAccountType = true;
            return valid;
        }
        if ($("#ProductName").val() == "") {
            $scope.ErrorMessage = "Product Name should be selected.";
            $scope.ErrorModel.IsProductName = true;
            return valid;
        }
        if ($("#UOM").val() == "") {
            $scope.ErrorMessage = "Unit of Measurement should be filled.";
            $scope.ErrorModel.IsUOM = true;
            return valid;
        }
        
        valid = true;
        return valid;
    }

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }

    $scope.FilterList = function () {
        var reg = new RegExp($scope.Prefix.toLowerCase());
        $scope.ProductList = JSON.parse($("#productlist").val()).filter(function (customer) {
            return (reg.test(customer.ProductName.toLowerCase()) || reg.test(customer.ProductType.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.ProductList = JSON.parse($("#productlist").val());
        $scope.SearchProductList = $scope.ProductList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#ProductName").val("");
        $("#ProductType").val("0");
        $("#Price").val("");
        $("#ProductDescription").val("");
        $("#UOM").val("0");
        $("#SUBUOM").val("0");
        $("#OpeningQty").val("");
        
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (ProductModel) {
        $scope.ProductId = ProductModel.ProductId;
        $("#ProductType").val(ProductModel.ProductTypeId);
        $("#UOM").val(ProductModel.UOMId);
        $("#SUBUOM").val(ProductModel.SubUOMId);
        $("#ProductDescription").val(ProductModel.ProductDescription);
        $("#Price").val(ProductModel.Price);
        $("#ProductName").val(ProductModel.ProductName);
        $("#OpeningQty").val(ProductModel.OpeningQty);
        $("#WareHouse").val(ProductModel.WareHouseNo);
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }



    $scope.Save = function (isEdit) {
        if ($scope.ValidateForm()) {
            var spinner = new Spinner().spin();
            document.getElementById("mainbody").appendChild(spinner.el);
            var model =
                {
                    ProductId: isEdit == false ? $scope.ProductId : 0,
                    ProductName: $("#ProductName").val(),
                    UOMId: $("#UOM").val(),
                    SubUOMId: $("#SUBUOM").val(),
                    ProductDescription: $("#ProductDescription").val(),
                    Price: $("#Price").val(),
                    ProductTypeId: $("#ProductType").val(),
                    OpeningQty: $("#OpeningQty").val(),
                    WareHouseNo: $("#WareHouse").val() == "0" ? 1 : $("#WareHouse").val(),
                    
                };

            var url = GetVirtualDirectory() + '/Product/Save';
            if (isEdit == false) {
                url = GetVirtualDirectory() + '/Product/Update';
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
                if (response.data.Status==true) {
                    if (isEdit == false) {
                        angular.forEach($scope.ProductList, function (value, key) {
                            if (value.ProductId == model.ProductId) {
                                $scope.ProductList[key].ProductName = model.ProductName;
                                $scope.ProductList[key].UOMId = model.UOMId;
                                $scope.ProductList[key].SubUOMId = model.SubUOMId;
                                $scope.ProductList[key].ProductDescription = model.ProductDescription;
                                $scope.ProductList[key].Price = model.Price;
                                $scope.ProductList[key].OpeningQty = model.OpeningQty;
                                $scope.ProductList[key].UOM = $("#UOM option:selected").text();
                                $scope.ProductList[key].SubUOM = $("#SUBUOM option:selected").text();
                                $scope.ProductList[key].ProductType = $("#ProductType option:selected").text();
                                $scope.ProductList[key].WareHouseNo = $("#WareHouse").val();

                            }
                        });
                    }
                    else {
                        model.ProductId = response.data.Id;
                        model.UOM = $("#UOM option:selected").text();
                        model.SubUOM = $("#SubUOM option:selected").text();
                        model.ProductType = $("#ProductType option:selected").text();
                        //model.ProductType = $("#ProductType").val();
                        $scope.ProductList.push(model);
                    }
                    setTimeout(function () {
                        $scope.$apply(function () {
                            $("#productlist").val(JSON.stringify($scope.ProductList));
                            $scope.SearchProductList = $scope.ProductList;
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
        //document.getElementById("mainbody").removeChild(spinner.el);
    }

    $scope.First = function () {
        $scope.CurruntIndex = 0;
        $scope.SearchProductList = $filter('limitTo')($scope.ProductList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchProductList = $filter('limitTo')($scope.ProductList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.ProductList.length) {
            $scope.SearchProductList = $filter('limitTo')($scope.ProductList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.ProductList.length;
        var rem = parseInt($scope.ProductList.length) % parseInt($scope.Paging);
        var position = $scope.ProductList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.ProductList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchProductList = $filter('limitTo')($scope.ProductList, $scope.Paging, position);
    }

    $scope.init = function () {
        $scope.ProductList = JSON.parse($("#productlist").val());
        $scope.SearchProductList = $filter('limitTo')($scope.ProductList, $scope.Paging, $scope.CurruntIndex);
        $scope.ProductTypeList = JSON.parse($("#ProductTypeList").val()).ProductTypeList;
        //console.log($scope.AccountTypes);
        $scope.UOMList = JSON.parse($("#UOMList").val()).UOMList;
        $scope.WareHouseList = JSON.parse($("#WareHouseList").val()).WareHouseList;
    }

    $scope.init();

}]);