PetroliumApp.controller("ProductController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.ProductList = [];
    $scope.SearchProductList = [];
    $scope.ProductTypeList = [];
    $scope.UOMList = [];

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
        if ($("#ddlsubledger").val() == "0") {
            $scope.ErrorMessage = "Subledger should be selected.";
            $scope.ErrorModel.IsSubledger = true;
            return valid;
        }
        if ($("#SupplierName").val() == "") {
            $scope.ErrorMessage = "Supplier Name should be filled.";
            $scope.ErrorModel.IsSupplierName = true;
            return valid;
        }
        if ($("#Address").val() == "") {
            $scope.ErrorMessage = "Address should be filled.";
            $scope.ErrorModel.IsAddressFilled = true;
            return valid;
        }
        if ($("#MobileNo").val() == "") {
            $scope.ErrorMessage = "Mobile no should be filled.";
            $scope.ErrorModel.IsPhoneNo = true;
            return valid;
        }
        if ($("#VATCSTNo").val() == "") {
            $scope.ErrorMessage = "VAT CST No should be filled.";
            $scope.ErrorModel.IsVATCSTNo = true;
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
                    ProductDescription: $("#ExciseNo").val(),
                    Price: $("#Price").val(),
                    ProductTypeId: $("#ProductType").val(),
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
                            }
                        });
                    }
                    else {
                        model.ProductId = response.data.Id;
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
    }

    $scope.init();

}]);