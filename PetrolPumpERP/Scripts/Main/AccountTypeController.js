PetroliumApp.controller("AccountTypeController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.MainAccountTypeList = [];

    $scope.AccountTypeList = [];
    $scope.SearchAccountTypeList = [];
    $scope.SubLedgers = [];
    $scope.AddedSubledgers = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsSelectSubledger: false, IsSelectAccountType: false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.AccountTypeId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.AccountTypeModel = { AccountTypeId: 0, AccountType: "", SubledgerId: 0 };

    $scope.Prefix = "";

    function GetAccountTypeDetails()
    {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/AccountType/GetAccountTypes";
        $http.get(url)
        .then(function (response) {
            $scope.AccountTypeList = response.data.AccountTypeList;
            $scope.MainAccountTypeList = response.data.AccountTypeList;
            $scope.SearchAccountTypeList = $filter('limitTo')($scope.AccountTypeList, $scope.Paging, $scope.CurruntIndex);
            $scope.SubLedgers = response.data.Subledgers.SubledgerList;
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

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }

    $scope.AddToList = function (model)
    {
        $scope.AddedSubledgers.push(model)
    }

    $scope.RemoveModel = function (item) {
        var index = $scope.AddedSubledgers.indexOf(item);
        $scope.AddedSubledgers.splice(index, 1);
        $("#" + item.SubLedgerId).attr("checked",false);
    }

    $scope.FilterList = function () {
        //$scope.AccountTypeList = $filter('filter')(JSON.parse($("#accounttypelist").val()), { AccountType: $scope.Prefix })
        var reg = new RegExp($scope.Prefix.toLowerCase());
        $scope.AccountTypeList = $scope.MainAccountTypeList.filter(function (actype) {
            return (reg.test(actype.AccountType.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.AccountTypeList = $scope.MainAccountTypeList;
        $scope.SearchAccountTypeList = $scope.AccountTypeList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $scope.SearchSubledger = "";
        angular.forEach($scope.AccountTypeList, function (value, key) {
            $("#" + value.SubledgerId).attr("checked", false);
        });
        $scope.AddedSubledgers = [];
        $("#AccountType").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (SubledgerModel) {
        //$("#SubLedgerId").val(SubledgerModel.MainLedgerId);
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + '/AccountType/GetAccountTypeDetails?AcId=' + SubledgerModel.AccountTypeId;
        $http({
            method: 'GET',
            url: url,
        }).then(function successCallback(response) {
            $("#AccountType").val(SubledgerModel.AccountType);
            $scope.AccountTypeId = SubledgerModel.AccountTypeId;
            $scope.Details = false;
            $scope.Add = false;
            $scope.Edit = true;
            $scope.AddedSubledgers = [];
            angular.forEach(response.data, function (value, key) {
                $("#" + value.SubledgerId).attr("checked", true);
                $scope.AddedSubledgers.push({ SubLedgerId: value.SubledgerId, SubLedgerName: value.SubLedgerName, SRNo: value.SRNo });
            });
            document.getElementById("mainbody").removeChild(spinner.el);
        }, function errorCallback(response) {
            var objShowCustomAlert = new ShowCustomAlert({
                Title: "",
                Message: response.responseText,
                Type: "alert",
            });
            objShowCustomAlert.ShowCustomAlertBox();
            document.getElementById("mainbody").removeChild(spinner.el);
        });
        
    }

    $scope.Save = function (isEdit) {
        if ($("#AccountType").val() == "") {
            $scope.ErrorModel.IsSelectSubledger = true;
            $scope.ErrorMessage = "Account type should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsSelectSubledger = false;
        }
        
        if ($scope.AddedSubledgers.length==0) {
            $scope.ErrorModel.IsSelectAccountType = true;
            $scope.ErrorMessage = "Please select subledgers.";
            return false;
        }
        else {
            $scope.ErrorModel.IsSelectAccountType = false;
        }

        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);

        var model = [];
        if (isEdit==false) {
            angular.forEach($scope.AddedSubledgers, function (value, key) {
                model.push({ SubLedgerId: value.SubLedgerId, AccountType: $("#AccountType").val(),AccountTypeId:$scope.AccountTypeId });
            });
        }
        else {
            angular.forEach($scope.AddedSubledgers, function (value, key) {
                model.push({ SubLedgerId: value.SubLedgerId, AccountType: $("#AccountType").val() });
            });
        }
        var url = GetVirtualDirectory() + '/AccountType/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/AccountType/Update';
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
                if (isEdit==true) {
                    model = { AccountType: $("#AccountType").val(), AccountTypeId: response.data.Id };
                    $scope.AccountTypeList.push(model);
                }
                
                setTimeout(function () {
                    $scope.$apply(function () {
                        $scope.MainAccountTypeList = $scope.AccountTypeList;
                        //$("#accounttypelist").val(JSON.stringify($scope.AccountTypeList));
                        $scope.SearchAccountTypeList = $scope.AccountTypeList;
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
        $scope.SearchAccountTypeList = $filter('limitTo')($scope.AccountTypeList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchAccountTypeList = $filter('limitTo')($scope.AccountTypeList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.AccountTypeList.length) {
            $scope.SearchAccountTypeList = $filter('limitTo')($scope.AccountTypeList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.AccountTypeList.length;
        var rem = parseInt($scope.AccountTypeList.length) % parseInt($scope.Paging);
        var position = $scope.AccountTypeList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.AccountTypeList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchAccountTypeList = $filter('limitTo')($scope.AccountTypeList, $scope.Paging, position);
    }

    $scope.init = function () {
        GetAccountTypeDetails();
        
    }

    $scope.init();

}]);