PetroliumApp.controller("SalesController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {
    $scope.MainSalesList = [];
    $scope.SalesList = [];
    $scope.BankList = [];
    $scope.CustomerList = [];
    $scope.ProductList = [];
    $scope.TaxList = [];
    $scope.SwipeMachineList = [];
    $scope.SelectedBankList = [];
    $scope.SelectedTaxList = [];
    $scope.Math = window.Math;


    $scope.selectedMachineId = "0";
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
    $scope.IsRoundOff = false;
    $scope.TotalAmount = 0;
    $scope.NetAmount = 0;
    $scope.FinalAmount = 0;
    $scope.CustomerInfo = {Address:"",MobileNo:"",LedgerId:"0"};
    $scope.ProductInfo = { ProductId: "0", UOM: "", Price: "0", UOMId: "0", ProductName: "" };
    $scope.TaxInfo = { OtherAccountId: "0", AccountName: "", LedgerId: "0", PercentOrFixedAmount: 0, Amount:0 };
    $scope.SwipeMachine = { MachineId: "0", MachineNo: "", BankId: "0", AccountNo: "0", BankName: "" };
    $scope.InvoiceNo = 0;

    function GetSalesInvoices()
    {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/Sales/GetSalesInvoices";
        $http.get(url)
        .then(function (response) {
            $scope.SalesList = response.data.SalesList;
            $scope.MainSalesList = response.data.SalesList;
            $scope.SearchSalesList = $filter('limitTo')($scope.SalesList, $scope.Paging, $scope.CurruntIndex);
            $scope.BankList = response.data.BankList.BankList;
            $scope.CustomerList = response.data.Customers.CustomerList;
            $scope.ProductList = response.data.ProductList.ProductList;
            $scope.TaxList = response.data.TaxList;
            var taxlist = "<option value='0'>--Select Tax--</option>";
            angular.forEach($scope.TaxList, function (value, key) {
                if (value.IsPercent==true) {
                    taxlist += "<option value='" + value.LedgerId + "'>" + value.AccountName + "</option>";
                }
            });
            $("#Tax").html(taxlist);
            $scope.SwipeMachineList = response.data.SwipeMachines.SwipeMachineList;
            var html = "<option value='0'>--Select Bank--</option>";
            $("#Bank").html(html);
            $("#Bank").val("0");
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


    $scope.getDateformat = function (x) {
        var re = /\/Date\(([0-9]*)\)\//;
        var m = x.match(re);
        if( m ) return new Date(parseInt(m[1]));
        else return null;
    }
    

    $scope.CalculateRoundOff = function ()
    {
        if ($scope.IsRoundOff==true) {
            //$scope.FinalAmount = $scope.NetAmount;
            $scope.NetAmount = $scope.Math.round($scope.NetAmount);
        }
    }

    $scope.SwipeMachineSelection = function ()
    {
        if (parseInt($scope.selectedMachineId) > 0) {
            $scope.SwipeMachine = $filter('filter')($scope.SwipeMachineList, function (d) { return d.MachineId === parseInt($scope.selectedMachineId); })[0];
            if ($scope.SwipeMachine.BankId>0) {
                $scope.SelectedBankList = $filter('filter')($scope.BankList, function (d) { return d.BankId === parseInt($scope.SwipeMachine.BankId); });
                var bank = $scope.SelectedBankList[0];
                $("#Bank").html("");
                var html="<option value='0'>--Select Bank--</option>";
                html += "<option value='" + bank.LedgerId + "'>" + bank.BankName + " " + bank.AccountNo + "</option>";
                $("#Bank").html(html);
                $("#Bank").val(bank.LedgerId);
            }
        }
        else {
            $scope.SwipeMachine = { MachineId: "0", MachineNo: "", BankId: "0", AccountNo: "0", BankName: "" };
            $("#Bank").val("0");
        }
    }

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
        if (parseInt($("#Tax").val()) > 0) {
            $scope.TaxInfo = $filter('filter')($scope.TaxList, function (d) { return d.LedgerId === parseInt($("#Tax").val()); })[0];
            var tax = $filter('filter')($scope.SelectedTaxList, function (d) { return d.LedgerId === parseInt($("#Tax").val()); })[0];
            if (tax === undefined) {
                $scope.SelectedTaxList.push($scope.TaxInfo);
                $scope.getTotal();
                $("#Tax").val("0");
            }   
        }
        else {
            $scope.TaxInfo = { OtherAccountId: "0", AccountName: "", LedgerId: "0", PercentOrFixedAmount: 0, Amount: 0 };
            $("#TaxAmount").val("");
        }
    }

    
    $scope.getTotal = function () {
        var total = 0;
        for (var i = 0; i < $scope.InvoiceProductList.length; i++) {
            var product = $scope.InvoiceProductList[i];
            total += product.ProductAmount;
        }
        $scope.TotalAmount = total;
        $scope.NetAmount = 0;
        $scope.FinalAmount = 0;
        for (var i = 0; i < $scope.SelectedTaxList.length; i++) {
            if ($scope.SelectedTaxList[i].RoundOff == true) {

            }
            else {
                if ($scope.SelectedTaxList[i].IsPercent == true) {
                    $scope.SelectedTaxList[i].Amount = parseFloat(parseFloat($scope.SelectedTaxList[i].PercentOrFixedAmount / 100) * ($scope.TotalAmount)).toFixed(2);
                }
                else {
                    $scope.SelectedTaxList[i].Amount = $scope.SelectedTaxList[i].PercentOrFixedAmount;
                }
                $scope.NetAmount =parseFloat($scope.NetAmount) + parseFloat($scope.SelectedTaxList[i].Amount);
            }
        }
        $scope.NetAmount += $scope.TotalAmount;
        $scope.FinalAmount = $scope.NetAmount;
    }

    $scope.ProductSelection = function ()
    {
        if (parseInt($scope.selectedProductId) > 0) {
            $scope.ProductInfo = $filter('filter')($scope.ProductList, function (d) { return d.ProductId === parseInt($scope.selectedProductId); })[0];
            $("#txtRate").val($scope.ProductInfo.Price);
            $scope.CalculateAmount();
        }
        else {
            $scope.ProductInfo = { ProductId: "0", UOM: "", Price: "0", UOMId: "0", ProductName: "" };
            $("#txtRate").val("");
            $("#txtQty").val("");
            $("#txtAmount").val("");
        }
    }

    $scope.ValidateForm = function () {
        var valid = false;
        if ($scope.IsCashInvoice) {
            if ($("#CustomerName").val() == "") {
                $scope.ErrorMessage = "Please select customer name.";
                $scope.ErrorModel.IsCustomerName = true;
                return valid;
            }
        }
        if (!$scope.IsCashInvoice) {
            if ($("#Customers").val() == "0") {
                $scope.ErrorMessage = "Please select customer name.";
                $scope.ErrorModel.IsCustomerName = true;
                return valid;
            }
        }
        if ($("#InvoiceDate").val() == "") {
            $scope.ErrorMessage = "Invoice date should be selected.";
            $scope.ErrorModel.IsInvoiceDate = true;
            return valid;
        }
        if ($scope.InvoiceProductList.length==0) {
            $scope.ErrorMessage = "Please add products in invoice.";
            $scope.ErrorModel.IsProductsSelected = true;
            return valid;
        }
        
        valid = true;
        return valid;
    }

    $scope.Last = function () {

    }

    $scope.Prev = function () {

    }

    $scope.Next = function () {

    }

    $scope.First = function ()
    {
        $scope.CurruntIndex = 0;
        $scope.SearchSalesList = $filter('limitTo')($scope.SalesList, $scope.Paging, 0);
    }


    $scope.Save = function (IsEdit)
    {
        if ($scope.ValidateForm()) {
            var spinner = new Spinner().spin();
            document.getElementById("mainbody").appendChild(spinner.el);
            var model =
                {
                    InvoiceNo: IsEdit == false ? $scope.InvoiceNo : 0,
                    ISCASH: $scope.IsCashInvoice,
                    BankLedgerId: $scope.IsCashInvoice == true ? $("#Bank").val() : 0,
                    CustomerLedgerId: $("#Customers").val(),
                    CustName: $("#CustomerName").val(),
                    CustAddress: $("#Address").val(),
                    CustContactNo: $("#ContactNo").val(),
                    InvoiceDate: $("#InvoiceDate").val(),
                    NetInvoiceAmount: $scope.TotalAmount,
                    NetAmount: $scope.FinalAmount,
                    TaxList: $scope.SelectedTaxList,
                    SalesDetails: $scope.InvoiceProductList,
                    RoundOff: $scope.IsRoundOff,
                };

            var url = GetVirtualDirectory() + '/Sales/Save';
            if (IsEdit == false) {
                url = GetVirtualDirectory() + '/Sales/Update';
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
                    var objShowCustomAlert = new ShowCustomAlert({
                        Title: "Success",
                        Message: "Invoice saved successfully.",
                        Type: "alert",
                        OnOKClick: function ()
                        {
                            window.location.reload();
                        },
                    });
                    objShowCustomAlert.ShowCustomAlertBox();
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
        $scope.getTotal();
    }

    $scope.CalculateAmount = function ()
    {
        if (parseInt($scope.selectedProductId) > 0) {
            var product = $filter('filter')($scope.InvoiceProductList, function (d) { return d.ItemCode === parseInt($scope.selectedProductId); })[0];
            if (product===undefined) {
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
            else {
                var objShowCustomAlert = new ShowCustomAlert({
                    Title: "",
                    Message: "Product already added in Invoice.",
                    Type: "alert",
                });
                objShowCustomAlert.ShowCustomAlertBox();
            }
        }
    }

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }
    

    $scope.init = function () {
        GetSalesInvoices();
    }

    $scope.init();

}]);