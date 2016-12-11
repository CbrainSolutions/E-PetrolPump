PetroliumApp.controller("CustomerController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.CustomerList = [];
    $scope.SearchCustomerList = [];
    $scope.MainLedgers = [];
    $scope.AccountTypes = [];
    $scope.SubledgerList = [];

    $scope.SelectedSubledgerList = [];
    $scope.Details = true;
    $scope.ErrorModel = {
        IsFirstNameFilled: false,
        IsLastNameFilled: false,
        IsAddressFilled: false,
        IsSelectMainledger: false,
        IsBillingCycle: false,
        IsCreditLimit: false,
        IsChargesPercent: false,
        IsSecurityDeposit: false,
        IsNoofCreditDays: false,
        IsAccountType: false,
        IsSubledger: false,
    };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.SubLedgerId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { SubLedgerId: 0, SubLedgerName: "", MainLedgerId: 0 };
    $scope.ddlvalue = 0;

    $scope.Prefix = "";

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
        if ($("#CustomerFirstName").val() == "") {
            $scope.ErrorMessage = "First name should be filled.";
            $scope.ErrorModel.IsFirstNameFilled = true;
            return valid;
        }
        if ($("#CustomerLName").val() == "") {
            $scope.ErrorMessage = "Last name should be filled.";
            $scope.ErrorModel.IsLastNameFilled = true;
            return valid;
        }
        if ($("#MainLedgerId").val() == "0") {
            $scope.ErrorMessage = "Customer type should be selected.";
            $scope.ErrorModel.IsSelectMainledger = true;
            return valid;
        }
        if ($("#BillingCycle").val() == "") {
            $scope.ErrorMessage = "Billing Cycle should be filled.";
            $scope.ErrorModel.IsBillingCycle = true;
            return valid;
        }
        if ($("#ChargesPercent").val() == "") {
            $scope.ErrorMessage = "Charges Percent should be filled.";
            $scope.ErrorModel.IsChargesPercent = true;
            return valid;
        }
        if ($("#SecurityDeposit").val() == "") {
            $scope.ErrorMessage = "Security Deposit should be filled.";
            $scope.ErrorModel.IsSecurityDeposit = true;
            return valid;
        }
        if ($("#NoofCreditDays").val() == "") {
            $scope.ErrorMessage = "No of Credit Days should be filled.";
            $scope.ErrorModel.IsNoofCreditDays = true;
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
        var reg = new RegExp($scope.Prefix);
        $scope.CustomerList = JSON.parse($("#customerlist").val()).filter(function (customer) {
            return (reg.test(customer.CustomerFirstName) || reg.test(customer.Address) || reg.test(customer.City));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.CustomerList = JSON.parse($("#customerlist").val());
        $scope.SearchCustomerList = $scope.CustomerList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#CustomerFirstName").val("");
        $("#ddlsubledger").val("0");
        $("#CustomerMName").val("");
        $("#CustomerLName").val("");
        $("#Address").val("");
        $("#City").val("");
        $("#Pin").val("");
        $("#State").val("");
        $("#Country").val("");
        $("#MobileNo").val("");
        $("#EMailId").val("");
        $("#MainLedgerId").val("0");
        $("#DuplicateBill").val("0");
        $("#SummaryofBills").val("0");
        $("#ContactPerson").val("");
        $("#VehicleWiseBill").val("0");
        $("#BillingCycle").val("");
        $("#CreditLimit").val("");
        $("#ChargesPercent").val("");
        $("#SecurityDeposit").val("");
        $("#NoofCreditDays").val("");
        $("#VehicleWiseBill").val("0");
        $("#IsRoundOff").val("0");
        $("#AccountType").val("0");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (CustomerModel) {
        $scope.CustomerId = CustomerModel.CustomerId;
        $("#AccountType").val(CustomerModel.AccountTypeId);
        $scope.AcTypeChanged();
        $("#CustomerFirstName").val(CustomerModel.CustomerFirstName);
        $("#ddlsubledger").val(CustomerModel.SubledgerId);
        $("#CustomerMName").val(CustomerModel.CustomerMName);
        $("#CustomerLName").val(CustomerModel.CustomerLName);
        $("#Address").val(CustomerModel.Address);
        $("#City").val(CustomerModel.City);
        $("#Pin").val(CustomerModel.Pin);
        $("#State").val(CustomerModel.State);
        $("#Country").val(CustomerModel.Country);
        $("#MobileNo").val(CustomerModel.MobileNo);
        $("#EMailId").val(CustomerModel.EMailId);
        $("#MainLedgerId").val(CustomerModel.CustomerTypeId);
        $("#DuplicateBill").val(String(CustomerModel.DuplicateBill));
        $("#SummaryofBills").val(String(CustomerModel.SummaryofBills));
        $("#ContactPerson").val(CustomerModel.ContactPerson);
        $("#VehicleWiseBill").val(String(CustomerModel.VehicleWiseBill));
        $("#BillingCycle").val(CustomerModel.BillingCycle);
        $("#CreditLimit").val(CustomerModel.CreditLimit);
        $("#ChargesPercent").val(CustomerModel.ChargesPercent);
        $("#SecurityDeposit").val(CustomerModel.SecurityDeposit);
        $("#NoofCreditDays").val(CustomerModel.NoofCreditDays);
        $("#IsSeperateBill").val(String(CustomerModel.VehicleWiseBill));
        $("#IsRoundOff").val(String(CustomerModel.IsRoundOff));

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
                    CustomerId: isEdit == false ? $scope.CustomerId : 0,
                    CustomerFirstName: $("#CustomerFirstName").val(),
                    SubledgerId: $("#ddlsubledger").val(),
                    CustomerMName: $("#CustomerMName").val(),
                    CustomeLName: $("#CustomerLName").val(),
                    Address: $("#Address").val(),
                    City: $("#City").val(),
                    Pin: $("#Pin").val(),
                    State: $("#State").val(),
                    Country: $("#Country").val(),
                    MobileNo: $("#MobileNo").val(),
                    EMailId: $("#EMailId").val(),
                    CustomerTypeId: $("#MainLedgerId").val(),
                    DuplicateBill: $("#DuplicateBill").val(),
                    SummaryofBills: $("#SummaryofBills").val(),
                    ContactPerson: $("#ContactPerson").val(),
                    VehicleWiseBill: $("#VehicleWiseBill").val(),
                    BillingCycle: $("#BillingCycle").val(),
                    CreditLimit: $("#CreditLimit").val(),
                    ChargesPercent: $("#ChargesPercent").val(),
                    SecurityDeposit: $("#SecurityDeposit").val(),
                    NoofCreditDays: $("#NoofCreditDays").val(),
                    IsSeperateBill: $("#IsSeperateBill").val(),
                    IsRoundOff: $("#IsRoundOff").val(),
                    AccountTypeId: $("#AccountType").val(),
                };

            var url = GetVirtualDirectory() + '/Customer/Save';
            if (isEdit == false) {
                url = GetVirtualDirectory() + '/Customer/Update';
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
                if (isEdit == false) {
                    angular.forEach($scope.CustomerList, function (value, key) {
                        if (value.CustomerId == response.data.Id) {
                            $scope.CustomerList[key].CustomerFirstName = model.CustomerFirstName;
                            $scope.CustomerList[key].CustomerFirstName = model.CustomerFirstName;
                            $scope.CustomerList[key].CustomerMName = model.CustomerMName;
                            $scope.CustomerList[key].CustomeLName = model.CustomeLName;
                            $scope.CustomerList[key].Address = model.Address;
                            $scope.CustomerList[key].City = model.City;
                            $scope.CustomerList[key].Pin = model.Pin;
                            $scope.CustomerList[key].State = model.State;
                            $scope.CustomerList[key].Country = model.Country;
                            $scope.CustomerList[key].MobileNo = model.MobileNo;
                            $scope.CustomerList[key].EMailId = model.EMailId;
                            $scope.CustomerList[key].CustomerTypeId = model.CustomerTypeId;
                            $scope.CustomerList[key].DuplicateBill = model.DuplicateBill;
                            $scope.CustomerList[key].SummaryofBills = model.SummaryofBills;
                            $scope.CustomerList[key].ContactPerson = model.ContactPerson;
                            $scope.CustomerList[key].VehicleWiseBill = model.VehicleWiseBill;
                            $scope.CustomerList[key].BillingCycle = model.BillingCycle;
                            $scope.CustomerList[key].CreditLimit = model.CreditLimit;
                            $scope.CustomerList[key].ChargesPercent = model.ChargesPercent;
                            $scope.CustomerList[key].SecurityDeposit = model.SecurityDeposit;
                            $scope.CustomerList[key].NoofCreditDays = model.NoofCreditDays;
                            $scope.CustomerList[key].IsSeperateBill = model.IsSeperateBill;
                            $scope.CustomerList[key].IsRoundOff = model.IsRoundOff;
                        }
                    });
                }
                else {
                    model.CustomerId = response.data.Id;
                    $scope.CustomerList.push(model);
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        $("#customerlist").val(JSON.stringify($scope.CustomerList));
                        $scope.SearchCustomerList = $scope.CustomerList;
                        $scope.First();
                        $scope.CancelClick();
                    });
                }, 1000);
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
        $scope.SearchCustomerList = $filter('limitTo')($scope.CustomerList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchCustomerList = $filter('limitTo')($scope.CustomerList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.CustomerList.length) {
            $scope.SearchCustomerList = $filter('limitTo')($scope.CustomerList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.CustomerList.length;
        var rem = parseInt($scope.CustomerList.length) % parseInt($scope.Paging);
        var position = $scope.CustomerList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.CustomerList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchCustomerList = $filter('limitTo')($scope.CustomerList, $scope.Paging, position);
    }

    $scope.init = function () {
        $scope.CustomerList = JSON.parse($("#customerlist").val());
        $scope.SearchCustomerList = $filter('limitTo')($scope.CustomerList, $scope.Paging, $scope.CurruntIndex);
        $scope.CustomerTypes = JSON.parse($("#CustomerTypeList").val());
        $scope.AccountTypes = JSON.parse($("#AccontTypeList").val()).AccountTypeList;
        //console.log($scope.AccountTypes);
        $scope.SubledgerList = JSON.parse($("#SubledgerList").val());
    }

    $scope.init();

}]);