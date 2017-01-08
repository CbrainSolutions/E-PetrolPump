PetroliumApp.controller("BankController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.BankList = [];
    $scope.SearchBankList = [];
    $scope.AccountTypes = [];
    $scope.SubledgerList = [];

    $scope.SelectedSubledgerList = [];
    $scope.Details = true;
    $scope.ErrorModel = {
        IsBankName: false,
        IsAccountNo: false,
        IsAccountType: false,
        IsSubledger: false,
        IsMobileNo: false,
    };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.SubLedgerId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { SubLedgerId: 0, SubLedgerName: "", MainLedgerId: 0 };
    $scope.ddlvalue = 0;
    $scope.BankId = 0;

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
        if ($("#BankName").val() == "") {
            $scope.ErrorMessage = "Bank Name should be filled.";
            $scope.ErrorModel.IsBankName = true;
            return valid;
        }
        if ($("#AccountNo").val() == "") {
            $scope.ErrorMessage = "Account No should be filled.";
            $scope.ErrorModel.IsAccountNo = true;
            return valid;
        }
        if ($("#MobileNo").val() == "") {
            $scope.ErrorMessage = "Mobile no should be filled.";
            $scope.ErrorModel.IsMobileNo = true;
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
        $scope.BankList = JSON.parse($("#banklist").val()).filter(function (bank) {
            return (reg.test(bank.BankName.toLowerCase()) || reg.test(bank.Address.toLowerCase()) || reg.test(bank.IFSC.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.BankList = JSON.parse($("#banklist").val());
        $scope.SearchBankList = $scope.BankList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#BankName").val("");
        $("#AccountType").val(0);
        $scope.AcTypeChanged();
        $("#ddlsubledger").val("0");
        $("#Address").val("");
        $("#PhoneNo").val("");
        $("#MobileNo").val("");
        $("#AccountNo").val("");
        $("#BranchName").val("");
        $("#IFSC").val("");
        $("#MICR").val("");
        $("#ContactPerson").val("");
        $("#BranchName").val("");
        //$("#BalType").val("0");
        //$("#OpeningBalance").val(""),
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (BankModel) {
        $scope.BankId = BankModel.BankId;
        $("#AccountType").val(BankModel.AccountTypeId);
        $scope.AcTypeChanged();
        $("#BankName").val(BankModel.BankName);
        $("#ddlsubledger").val(BankModel.SubledgerId);
        $("#IFSC").val(BankModel.IFSC);
        $("#MICR").val(BankModel.MICR);
        $("#Address").val(BankModel.Address);
        $("#PhoneNo").val(BankModel.PhoneNo);
        $("#MobileNo").val(BankModel.MobileNo);
        $("#ContactPerson").val(BankModel.ContactPerson);
        $("#BranchName").val(BankModel.BranchName);
        $("#AccountNo").val(BankModel.AccountNo);
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
                    BankId: isEdit == false ? $scope.BankId : 0,
                    BankName: $("#BankName").val(),
                    SubledgerId: $("#ddlsubledger").val(),
                    IFSC: $("#IFSC").val(),
                    MICR: $("#MICR").val(),
                    Address: $("#Address").val(),
                    ContactPerson: $("#ContactPerson").val(),
                    AccountNo:$("#AccountNo").val(),
                    MobileNo: $("#MobileNo").val(),
                    BranchName:$("#BranchName").val(),
                    AccountTypeId: $("#AccountType").val(),
                    PhoneNo: $("#PhoneNo").val(),
                    
                };

            var url = GetVirtualDirectory() + '/Bank/Save';
            if (isEdit == false) {
                url = GetVirtualDirectory() + '/Bank/Update';
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
                    if (isEdit == false) {
                        angular.forEach($scope.BankList, function (value, key) {
                            if (value.BankId == model.BankId) {
                                $scope.BankList[key].BankName = model.BankName;
                                $scope.BankList[key].IFSC = model.IFSC;
                                $scope.BankList[key].MICR = model.MICR;
                                $scope.BankList[key].Address = model.Address;
                                $scope.BankList[key].BranchName = model.BranchName;
                                $scope.BankList[key].ContactPerson = model.ContactPerson;
                                $scope.BankList[key].MobileNo = model.MobileNo;
                                $scope.BankList[key].PhoneNo = model.PhoneNo;
                            }
                        });
                    }
                    else {
                        model.BankId = response.data.Id;
                        $scope.BankList.push(model);
                    }
                    setTimeout(function () {
                        $scope.$apply(function () {
                            $("#banklist").val(JSON.stringify($scope.BankList));
                            $scope.SearchBankList = $scope.BankList;
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
        $scope.SearchBankList = $filter('limitTo')($scope.BankList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchBankList = $filter('limitTo')($scope.BankList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.BankList.length) {
            $scope.SearchBankList = $filter('limitTo')($scope.BankList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.BankList.length;
        var rem = parseInt($scope.BankList.length) % parseInt($scope.Paging);
        var position = $scope.BankList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.BankList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchBankList = $filter('limitTo')($scope.BankList, $scope.Paging, position);
    }

    $scope.init = function () {
        $scope.BankList = JSON.parse($("#banklist").val());
        $scope.SearchBankList = $filter('limitTo')($scope.BankList, $scope.Paging, $scope.CurruntIndex);
        $scope.AccountTypes = JSON.parse($("#AccontTypeList").val()).AccountTypeList;
        //console.log($scope.AccountTypes);
        $scope.SubledgerList = JSON.parse($("#SubledgerList").val());
    }

    $scope.init();

}]);