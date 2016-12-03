PetroliumApp.controller("CustomerController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.CustomerList = [];
    $scope.SearchCustomerList = [];
    $scope.MainLedgers = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsFirstNameFilled: false, IsLastNameFilled: false, IsAddressFilled:false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.SubLedgerId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { SubLedgerId: 0, SubLedgerName: "", MainLedgerId: 0 };

    $scope.Prefix = "";

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }

    $scope.FilterList = function () {
        $scope.CustomerList = $filter('filter')(JSON.parse($("#customerlist").val()), { SubLedgerName: $scope.Prefix })
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.CustomerList = $("#customerlist").val();
        $scope.SearchSubledgerList = $scope.CustomerList;
    }

    $scope.CancelClick = function () {
        //$("#MainLedgerId").val("0");
        //$("#SubledgerName").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (CustomerModel) {
        //$("#MainLedgerId").val(SubledgerModel.MainLedgerId);
        //$("#SubledgerName").val(SubledgerModel.SubLedgerName);
        $scope.CustomerId = CustomerModel.CustomerId;
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {
        if ($("#MainLedgerId").val() == "0") {
            $scope.ErrorModel.IsSelectMainledger = true;
            $scope.ErrorMessage = "Main ledger should be selected.";
            return false;
        }
        else {
            $scope.ErrorModel.IsSelectMainledger = false;
        }
        if ($("#SubledgerName").val() == "") {
            $scope.ErrorModel.IsSelectsubledgerName = true;
            $scope.ErrorMessage = "Subledger name should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsSelectsubledgerName = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var model = { MainLedgerId: $("#MainLedgerId").val(), SubLedgerName: $("#SubledgerName").val() };
        if (isEdit == false) {
            model = { MainLedgerId: $("#MainLedgerId").val(), SubLedgerName: $("#SubledgerName").val(), SubLedgerId: $scope.SubLedgerId };
        }

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
            if (response.data.Status == true) {
                model = { MainLedgerId: $("#MainLedgerId").val(), SubLedgerName: model.SubLedgerName, SubLedgerId: response.data.Id, MainLedgerName: $("#MainLedgerId option:selected").text() };
                $scope.CustomerList.push(model);
                setTimeout(function () {
                    $scope.$apply(function () {
                        $("#customerlist").val(JSON.stringify($scope.CustomerList));
                        $scope.CustomerList = $scope.CustomerList;
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
        $scope.CustomerTypes = JSON.parse($("#MainLedgerlist").val());
    }

    $scope.init();

}]);