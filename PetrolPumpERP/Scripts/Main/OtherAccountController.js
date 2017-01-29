PetroliumApp.controller("OtherAccountController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.MainOtherAccountList = [];
    $scope.OtherAccountList = [];
    $scope.SearchOtherAccountList = [];
    $scope.AccountTypes = [];
    $scope.SubledgerList = [];

    $scope.SelectedSubledgerList = [];
    $scope.Details = true;
    $scope.ErrorModel = {
        IsOtherAccountName: false,
        IsAccountType: false,
        IsSubledger: false,
    };
    $scope.ErrorMessage = ""
    $scope.IsRoundOff = false;
    $scope.IsPercentWise = false;
    $scope.Add = false;
    $scope.Edit = false;
    $scope.SubLedgerId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { SubLedgerId: 0, SubLedgerName: "", MainLedgerId: 0 };
    $scope.ddlvalue = 0;
    $scope.OtherAccountId = 0;

    $scope.Prefix = "";

    function GetOtherAccountDetails()
    {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/OtherAccount/GetOtherAccountDetails";
        $http.get(url)
        .then(function (response) {
            $scope.OtherAccountList = response.data.OtherAccountList;
            $scope.MainOtherAccountList = response.data.OtherAccountList;
            $scope.SearchOtherAccountList = $filter('limitTo')($scope.OtherAccountList, $scope.Paging, $scope.CurruntIndex);
            $scope.AccountTypes = response.data.AccountTypes.AccountTypeList;
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
        if ($("#AccountName").val() == "") {
            $scope.ErrorMessage = "Account Name should be filled.";
            $scope.ErrorModel.IsBankName = true;
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
        $scope.OtherAccountList = $scope.MainOtherAccountList.filter(function (other) {
            return (reg.test(other.AccountName.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.OtherAccountList = $scope.MainOtherAccountList;
        $scope.SearchOtherAccountList = $scope.OtherAccountList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#AccountName").val("");
        $("#AccountType").val(0);
        $scope.AcTypeChanged();
        $("#ddlsubledger").val("0");
        $("#PercentOrFixedAmount").val("")
        $scope.IsPercent = false;
        $scope.IsRoundOff = false;
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (OtherAccountModel) {
        $scope.OtherAccountId = OtherAccountModel.OtherAccountId;
        $("#AccountType").val(OtherAccountModel.AccountTypeId);
        $scope.AcTypeChanged();
        $("#AccountName").val(OtherAccountModel.AccountName);
        $("#ddlsubledger").val(OtherAccountModel.SubledgerId);
        if (OtherAccountModel.RoundOff == null) {
            $scope.IsRoundOff = false;
        }
        else {
            $scope.IsRoundOff = OtherAccountModel.RoundOff;
        }
        //$scope.SetRoundOff();
        if (OtherAccountModel.IsPercent==null) {
            $scope.IsPercentWise = false;
        }
        else {
            $scope.IsPercentWise = OtherAccountModel.IsPercent;
            if ($scope.IsPercentWise) {
                $("#PercentOrFixedAmount").val(OtherAccountModel.PercentOrFixedAmount);
            }
        }
        
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
                    OtherAccountId: isEdit == false ? $scope.OtherAccountId : 0,
                    AccountName: $("#AccountName").val(),
                    SubledgerId: $("#ddlsubledger").val(),
                    AccountTypeId: $("#AccountType").val(),
                    RoundOff: $scope.IsRoundOff,
                    IsPercent: $scope.IsPercent,
                    PercentOrFixedAmount: $("#PercentOrFixedAmount").val()==""?"0":$("#PercentOrFixedAmount").val(),
                };

            var url = GetVirtualDirectory() + '/OtherAccount/Save';
            if (isEdit == false) {
                url = GetVirtualDirectory() + '/OtherAccount/Update';
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
                        angular.forEach($scope.OtherAccountList, function (value, key) {
                            if (value.OtherAccountId == model.OtherAccountId) {
                                $scope.OtherAccountList[key].AccountName = model.AccountName;
                                $scope.OtherAccountList[key].RoundOff = model.IsRoundOff;
                                $scope.OtherAccountList[key].IsPercent = model.IsPercent;
                                $scope.OtherAccountList[key].PercentOrFixedAmount = model.PercentOrFixedAmount;
                            }
                        });
                    }
                    else {
                        model.OtherAccountId = response.data.Id;
                        $scope.OtherAccountList.push(model);
                    }
                    setTimeout(function () {
                        $scope.$apply(function () {
                            $scope.MainOtherAccountList = $scope.OtherAccountList;
                            //$("#otherlist").val(JSON.stringify($scope.OtherAccountList));
                            $scope.SearchOtherAccountList = $scope.OtherAccountList;
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
        $scope.SearchOtherAccountList = $filter('limitTo')($scope.OtherAccountList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchOtherAccountList = $filter('limitTo')($scope.OtherAccountList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.OtherAccountList.length) {
            $scope.SearchOtherAccountList = $filter('limitTo')($scope.OtherAccountList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.OtherAccountList.length;
        var rem = parseInt($scope.OtherAccountList.length) % parseInt($scope.Paging);
        var position = $scope.OtherAccountList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.OtherAccountList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchOtherAccountList = $filter('limitTo')($scope.OtherAccountList, $scope.Paging, position);
    }

    $scope.init = function () {
        GetOtherAccountDetails();
        //$scope.OtherAccountList = JSON.parse($("#otherlist").val());
        //$scope.SearchOtherAccountList = $filter('limitTo')($scope.OtherAccountList, $scope.Paging, $scope.CurruntIndex);
        //$scope.AccountTypes = JSON.parse($("#AccontTypeList").val()).AccountTypeList;
        //$scope.SubledgerList = JSON.parse($("#SubledgerList").val());
    }

    $scope.init();

}]);