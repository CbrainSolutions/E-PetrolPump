PetroliumApp.controller("SalesController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.SalesList = [];
    $scope.BankList = [];
    $scope.CustomerList = [];
    $scope.ProductList = [];
    $scope.TaxList = [];
    $scope.selectedTaxId = "0";
    $scope.Details = true;
    $scope.ErrorModel = {
        IsInvoiceType: false,
        IsCustomerName: false,
        IsInvoiceDate: false,
    };
    $scope.InvoiceProductList = [];
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.IsCashInvoice = false;
    $scope.selectedCustomerId = "0";
    $scope.selectedProductId = "0";
    $scope.CustomerInfo = {Address:"",MobileNo:"",LedgerId:"0"};
    $scope.ProductInfo = { ProductId: "0", UOM: "", Price: "0", UOMId: "0", ProductName: "" };
    $scope.TaxInfo = { ProductId: "0", UOM: "", Price: "0", UOMId: "0", ProductName: "" };
    $scope.SetCashInvoice = function ()
    {
        if ($scope.IsCashInvoice) {
            $scope.IsCashInvoice = false;
        }
        else {
            $scope.IsCashInvoice = true;
        }
    }

    $scope.TaxSelection = function ()
    {
        if (parseInt($scope.selectedTaxId) > 0) {
            $scope.TaxInfo = $filter('filter')($scope.TaxList, function (d) { return d.LedgerId === parseInt($scope.selectedTaxId); })[0];
        }
        else {
            $scope.TaxInfo = { ProductId: "0", UOM: "", Price: "0", UOMId: "0", ProductName: "" };
            $("#TaxAmount").val("");
        }
    }

    $scope.ProductSelection = function ()
    {
        if (parseInt($scope.selectedProductId) > 0) {
            $scope.ProductInfo = $filter('filter')($scope.ProductList, function (d) { return d.ProductId === parseInt($scope.selectedProductId); })[0];
        }
        else {
            $scope.ProductInfo = { ProductId: "0", UOM: "", Price: "0", UOMId: "0", ProductName: "" };
            $("#txtRate").val("");
            $("#txtQty").val("");
            $("#txtAmount").val("");
        }
    }

    $scope.Save = function (IsEdit)
    {

    }

    $scope.CustomerSelection = function () {
        if (parseInt($scope.selectedCustomerId)>0) {
            $scope.CustomerInfo = $filter('filter')($scope.CustomerList, function (d) { return d.LedgerId === parseInt($scope.selectedCustomerId); })[0];
        }
        else {
            $scope.CustomerInfo = { Address: "", MobileNo: "", LedgerId: "0" };
        }
    }

    $scope.CancelClick = function () {
        $scope.selectedCustomerId = 0;
        $scope.CustomerSelection();
        $scope.selectedProductId = 0;
        $scope.ProductSelection();
        
        $("#txtRate").val("");
        $("#txtQty").val("");
        $("#txtAmount").val("");
        $("#InvoiceDate").val("");
        $("#ContactNo").val("");
        $("#Address").val("");
        $("#CustomerName").val("");

        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.AddNewRow = function ()
    {
        var product = {
            ItemCode: $scope.selectedProductId,
            Quantity: $("#txtQty").val(),
            Rate: parseFloat($("#txtRate").val()),
            UOMId: $scope.ProductInfo.UOMId,
            ProductAmount: parseFloat($("#txtAmount").val()),
            ProductName: $scope.ProductInfo.ProductName,
            UOM: $scope.ProductInfo.UOM,
        };
        $scope.InvoiceProductList.push(product);
        $scope.selectedProductId = "0";
        $scope.ProductSelection();
    }

    $scope.CalculateAmount = function ()
    {
        if ($("#txtQty").val() == "" && $("#txtAmount").val() == "") {

        }
        else {
            if ($("#txtQty").val() != "" && $("#txtAmount").val() != "") {
                $("#txtAmount").val(parseFloat($("#txtQty").val()) * parseFloat($("#txtRate").val()));
            }
            else if ($("#txtQty").val() != "" && $("#txtAmount").val() == "") {
                $("#txtAmount").val(parseFloat(parseFloat($("#txtQty").val()) * parseFloat($("#txtRate").val())).toFixed(2));
            }
            else if ($("#txtQty").val() == "" && $("#txtAmount").val() != "") {
                $("#txtQty").val(parseFloat(parseFloat($("#txtAmount").val()) / parseFloat($("#txtRate").val())).toFixed(2));
            }
            $scope.AddNewRow();
        }
    }

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }
    

    $scope.init = function () {
        $scope.SalesList = JSON.parse($("#saleslist").val());
        $scope.SearchSalesList = $filter('limitTo')($scope.SalesList, $scope.Paging, $scope.CurruntIndex);
        $scope.BankList = JSON.parse($("#BankList").val()).BankList;
        $scope.CustomerList = JSON.parse($("#Customers").val()).CustomerList;
        $scope.ProductList = JSON.parse($("#ProductList").val()).ProductList;
        $scope.TaxList = JSON.parse($("#TaxList").val());
    }

    $scope.init();

}]);