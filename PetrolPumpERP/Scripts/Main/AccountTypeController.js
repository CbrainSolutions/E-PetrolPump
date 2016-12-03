﻿PetroliumApp.controller("AccountTypeController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.AccountTypeList = [];
    $scope.SearchAccountTypeList = [];
    $scope.SubLedgers = [];
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

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }

    $scope.FilterList = function () {
        $scope.AccountTypeList = $filter('filter')(JSON.parse($("#accounttypelist").val()), { SubLedgerName: $scope.Prefix })
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.AccountTypeList = $("#accounttypelist").val();
        $scope.SearchAccountTypeList = $scope.AccountTypeList;
    }

    $scope.CancelClick = function () {
        $("#SubLedgerId").val("0");
        $("#AccountType").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (SubledgerModel) {
        $("#SubLedgerId").val(SubledgerModel.MainLedgerId);
        $("#AccountType").val(SubledgerModel.AccountType);
        $scope.AccountTypeId = SubledgerModel.AccountTypeId;
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {
        if ($("#SubLedgerId").val() == "0") {
            $scope.ErrorModel.IsSelectMainledger = true;
            $scope.ErrorMessage = "Main ledger should be selected.";
            return false;
        }
        else {
            $scope.ErrorModel.IsSelectMainledger = false;
        }
        if ($("#AccountType").val() == "") {
            $scope.ErrorModel.IsSelectsubledgerName = true;
            $scope.ErrorMessage = "Subledger name should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsSelectsubledgerName = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var model = { SubLedgerId: $("#SubLedgerId").val(), AccountType: $("#AccountType").val() };
        if (isEdit == false) {
            model = { SubLedgerId: $("#SubLedgerId").val(), AccountType: $("#AccountType").val(), AccountTypeId: $scope.AccountTypeId };
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
                model = { SubLedgerId: $("#SubLedgerId").val(), AccountType: $("#AccountType").val(), AccountTypeId: response.data.Id, SubLedgerName: $("#SubLedgerId option:selected").text() };
                $scope.AccountTypeList.push(model);
                setTimeout(function () {
                    $scope.$apply(function () {
                        $("#accounttypelist").val(JSON.stringify($scope.AccountTypeList));
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
        $scope.AccountTypeList = JSON.parse($("#accounttypelist").val());
        $scope.SearchAccountTypeList = $filter('limitTo')($scope.AccountTypeList, $scope.Paging, $scope.CurruntIndex);
        $scope.SubLedgers = JSON.parse($("#SubledgerList").val()).SubledgerList;
    }

    $scope.init();

}]);