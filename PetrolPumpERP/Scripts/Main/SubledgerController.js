PetroliumApp.controller("SubledgerController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {
    $scope.MainSubledgerList = [];
    $scope.SubledgerList = [];
    $scope.SearchSubledgerList = [];
    $scope.MainLedgers = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsSelectMainledger: false, IsSelectsubledgerName: false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.SubLedgerId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { SubLedgerId: 0, SubLedgerName: "", MainLedgerId: 0 };

    $scope.Prefix = "";

    function GetAllSubledgers()
    {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/Subledger/GetAllSubledgers";
        $http.get(url)
        .then(function (response) {
            $scope.SubledgerList = response.data.SubledgerList;
            $scope.MainSubledgerList = response.data.SubledgerList;
            $scope.SearchSubledgerList = $filter('limitTo')($scope.SubledgerList, $scope.Paging, $scope.CurruntIndex);
            $scope.MainLedgers = response.data.MainledgerList;
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

    $scope.FilterList = function ()
    {
        //$scope.SubledgerList = $filter('filter')(JSON.parse($("#subledgerlist").val()), { SubLedgerName: $scope.Prefix });
        var reg = new RegExp($scope.Prefix.toLowerCase());
        $scope.SubledgerList = $scope.MainSubledgerList.filter(function (customer) {
            return (reg.test(customer.SubLedgerName.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function ()
    {
        $scope.SubledgerList = $scope.MainSubledgerList;// JSON.parse($("#subledgerlist").val());
        $scope.SearchSubledgerList = $scope.SubledgerList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#MainLedgerId").val("0");
        $("#SubledgerName").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (SubledgerModel) {
        $("#MainLedgerId").val(SubledgerModel.MainLedgerId);
        $("#SubledgerName").val(SubledgerModel.SubLedgerName);
        $scope.SubLedgerId = SubledgerModel.SubLedgerId;
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

        var url = GetVirtualDirectory() + '/Subledger/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/Subledger/Update';
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
                if (isEdit==false) {
                    angular.forEach($scope.SubledgerList, function (value, key) {
                        if (value.SubLedgerId == model.SubLedgerId) {
                            $scope.SubledgerList[key].MainLedgerId = $("#MainLedgerId").val();
                            $scope.SubledgerList[key].SubLedgerName = model.SubLedgerName;
                        }
                    });
                }
                else {
                    model = { MainLedgerId: $("#MainLedgerId").val(), SubLedgerName: model.SubLedgerName, SubLedgerId: response.data.Id, MainLedgerName: $("#MainLedgerId option:selected").text() };
                    $scope.SubledgerList.push(model);
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        //$("#subledgerlist").val(JSON.stringify($scope.SubledgerList));
                        $scope.MainSubledgerList = $scope.SubledgerList;
                        $scope.SearchSubledgerList = $scope.SubledgerList;
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
        $scope.SearchSubledgerList = $filter('limitTo')($scope.SubledgerList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex>=0) {
            $scope.SearchSubledgerList = $filter('limitTo')($scope.SubledgerList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.SubledgerList.length) {
            $scope.SearchSubledgerList = $filter('limitTo')($scope.SubledgerList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.SubledgerList.length;
        var rem = parseInt($scope.SubledgerList.length) % parseInt($scope.Paging);
        var position = $scope.SubledgerList.length-$scope.Paging;
        if (rem>0) {
            position = $scope.SubledgerList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchSubledgerList = $filter('limitTo')($scope.SubledgerList, $scope.Paging, position);
    }

    $scope.init = function () {
        GetAllSubledgers();
        //$scope.SubledgerList = JSON.parse($("#subledgerlist").val());
        //$scope.SearchSubledgerList = $filter('limitTo')($scope.SubledgerList, $scope.Paging, $scope.CurruntIndex);
        //$scope.MainLedgers = JSON.parse($("#MainLedgerlist").val());
    }

    $scope.init();

}]);