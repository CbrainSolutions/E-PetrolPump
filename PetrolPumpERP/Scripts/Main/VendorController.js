PetroliumApp.controller("VendorController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.MainVendorList = [];
    $scope.VendorList = [];
    $scope.SearchVendorList = [];
    $scope.AccountTypes = [];
    $scope.SubledgerList = [];

    $scope.SelectedSubledgerList = [];
    $scope.Details = true;
    $scope.ErrorModel = {
        IsSupplierName: false,
        IsPhoneNo: false,
        IsAddressFilled: false,
        IsAccountType: false,
        IsSubledger: false,
        IsVATCSTNo:false,
    };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.SubLedgerId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { SubLedgerId: 0, SubLedgerName: "", MainLedgerId: 0 };
    $scope.ddlvalue = 0;
    $scope.VendorId = 0;

    $scope.Prefix = "";

    function GetVendors()
    {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/Vendor/GetVendors";
        $http.get(url)
        .then(function (response) {
            $scope.VendorList = response.data.VendorList;
            $scope.MainVendorList = response.data.VendorList;
            $scope.SearchVendorList = $filter('limitTo')($scope.VendorList, $scope.Paging, $scope.CurruntIndex);
            $scope.AccountTypes = response.data.AccontTypeList.AccountTypeList;
            $scope.SubledgerList = response.data.SubledgerList;
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
        

    $scope.AcTypeChanged = function () {
        var id = $("#AccountType").val();
        $scope.SelectedSubledgerList = $filter('filter')($scope.SubledgerList, function (d) { return d.AccountTypeId === parseInt(id); });
        var html = "<option value='0'>---Select Subledger---</option>";
        angular.forEach($scope.SelectedSubledgerList, function (value, key) {
            html += "<option value='" + value.SubledgerId + "'>" + value.SubLedgerName + "</option>";
        });
        $("#ddlsubledger").html(html);
    }

    $scope.ValidateForm = function () {
        var valid = false;
        if ($("#AccountType").val() == "0") {
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
        $scope.VendorList = $scope.MainVendorList.filter(function (customer) {
            return (reg.test(customer.SupplierName.toLowerCase()) || reg.test(customer.Address.toLowerCase()) || reg.test(customer.City.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.VendorList = $scope.MainVendorList;
        $scope.SearchVendorList = $scope.VendorList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#SupplierName").val("");
        $("#ddlsubledger").val("0");
        $("#Address").val("");
        $("#PhoneNo").val("");
        $("#MobileNo").val("");
        $("#VATCSTNo").val("");
        $("#ExciseNo").val("");
        $("#Email").val("");
        $("#City").val("");
        $("#State").val("");
        $("#Country").val("");
        $("#Pin").val("");
        $("#AccountType").val("0");
        //$("#BalType").val("0");
        //$("#OpeningBalance").val(""),
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (VendorModel) {
        $scope.VendorId = VendorModel.SupplierCode;
        $("#AccountType").val(VendorModel.AccountTypeId);
        $scope.AcTypeChanged();
        $("#SupplierName").val(VendorModel.SupplierName);
        $("#ddlsubledger").val(VendorModel.SubledgerId);
        $("#VATCSTNo").val(VendorModel.VATCSTNo);
        $("#ExciseNo").val(VendorModel.ExciseNo);
        $("#Address").val(VendorModel.Address);
        $("#City").val(VendorModel.City);
        $("#Pin").val(VendorModel.Pin);
        $("#State").val(VendorModel.State);
        $("#Country").val(VendorModel.Country);
        $("#MobileNo").val(VendorModel.MobileNo);
        $("#Email").val(VendorModel.Email);
        $("#PhoneNo").val(VendorModel.PhoneNo);
        //$("#BalType").val(VendorModel.BalType);
        //$("#OpeningBalance").val(VendorModel.OpeningBalance);

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
                    SupplierCode: isEdit == false ? $scope.VendorId : 0,
                    SupplierName: $("#SupplierName").val(),
                    SubledgerId: $("#ddlsubledger").val(),
                    VATCSTNo: $("#VATCSTNo").val(),
                    ExciseNo: $("#ExciseNo").val(),
                    Address: $("#Address").val(),
                    City: $("#City").val(),
                    Pin: $("#Pin").val(),
                    State: $("#State").val(),
                    Country: $("#Country").val(),
                    MobileNo: $("#MobileNo").val(),
                    Email: $("#Email").val(),
                    AccountTypeId: $("#AccountType").val(),
                    PhoneNo: $("#PhoneNo").val(),
                    //BalType: $("#BalType").val()!="0"?$("#BalType").val():"",
                    //OpeningBalance: $("#OpeningBalance").val() == "" ? 0 : $("#OpeningBalance").val(),
                };

            var url = GetVirtualDirectory() + '/Vendor/Save';
            if (isEdit == false) {
                url = GetVirtualDirectory() + '/Vendor/Update';
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
                        angular.forEach($scope.VendorList, function (value, key) {
                            if (value.SupplierCode == model.SupplierCode) {
                                $scope.VendorList[key].SupplierName = model.SupplierName;
                                $scope.VendorList[key].VATCSTNo = model.VATCSTNo;
                                $scope.VendorList[key].ExciseNo = model.ExciseNo;
                                $scope.VendorList[key].Address = model.Address;
                                $scope.VendorList[key].City = model.City;
                                $scope.VendorList[key].Pin = model.Pin;
                                $scope.VendorList[key].State = model.State;
                                $scope.VendorList[key].Country = model.Country;
                                $scope.VendorList[key].MobileNo = model.MobileNo;
                                $scope.VendorList[key].Email = model.Email;
                                $scope.VendorList[key].PhoneNo = model.PhoneNo;
                                //$scope.VendorList[key].BalType = model.BalType;
                                //$scope.VendorList[key].OpeningBalance = model.OpeningBalance;
                            }
                        });
                    }
                    else {
                        model.SupplierCode = response.data.Id;
                        $scope.VendorList.push(model);
                    }
                    setTimeout(function () {
                        $scope.$apply(function () {
                            $scope.MainVendorList = $scope.VendorList;
                            $scope.SearchVendorList = $scope.VendorList;
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
    }

    $scope.First = function () {
        $scope.CurruntIndex = 0;
        $scope.SearchVendorList = $filter('limitTo')($scope.VendorList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchVendorList = $filter('limitTo')($scope.VendorList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.VendorList.length) {
            $scope.SearchVendorList = $filter('limitTo')($scope.VendorList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.VendorList.length;
        var rem = parseInt($scope.VendorList.length) % parseInt($scope.Paging);
        var position = $scope.VendorList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.VendorList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchVendorList = $filter('limitTo')($scope.VendorList, $scope.Paging, position);
    }

    $scope.init = function () {
        GetVendors();
    }

    $scope.init();

}]);