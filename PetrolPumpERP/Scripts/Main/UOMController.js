PetroliumApp.controller("UOMController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.MainUOMList = [];
    $scope.UOMList = [];
    $scope.SearchUOMList = [];
    //$scope.SubLedgers = [];
    //$scope.AddedSubledgers = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsUOMDesc: false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.UnitCode = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.AccountTypeModel = { AccountTypeId: 0, AccountType: "", SubledgerId: 0 };

    $scope.Prefix = "";

    function GetUOMList() {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/UOM/GetUOMList";
        $http.get(url)
        .then(function (response) {
            $scope.MainUOMList = response.data.UOMList;
            $scope.UOMList = response.data.UOMList;
            $scope.SearchUOMList = $filter('limitTo')($scope.UOMList, $scope.Paging, $scope.CurruntIndex);
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

    $scope.FilterList = function () {
        //$scope.UOMList = $filter('filter')(JSON.parse($("#uomlist").val()), { UnitDesc: $scope.Prefix })
        var reg = new RegExp($scope.Prefix.toLowerCase());
        $scope.UOMList = $scope.MainUOMList.filter(function (customer) {
            return (reg.test(customer.UnitDesc.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.UOMList = $scope.MainUOMList; //JSON.parse($("#uomlist").val());
        $scope.SearchUOMList = $scope.UOMList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#UnitDesc").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (UOMModel) {
        $("#UnitDesc").val(UOMModel.UnitDesc);
        $scope.UnitCode = UOMModel.UnitCode;
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {
        if ($("#UnitDesc").val() == "") {
            $scope.ErrorModel.IsUOMDesc = true;
            $scope.ErrorMessage = "Unit description should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsUOMDesc = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);

        var model = [];
        if (isEdit == false) {
            model = { UnitCode: $scope.UnitCode, UnitDesc: $("#UnitDesc").val() };
        }
        else {
            model = { UnitDesc: $("#UnitDesc").val() };
        }
        var url = GetVirtualDirectory() + '/UOM/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/UOM/Update';
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
                if (isEdit == true) {
                    model = { UnitDesc: $("#UnitDesc").val(), UnitCode: response.data.Id };
                    $scope.UOMList.push(model);
                }
                else {
                    angular.forEach($scope.UOMList, function (value, key) {
                        if (value.UnitCode == model.UnitCode) {
                            $scope.UOMList[key].UnitDesc = model.UnitDesc;
                        }
                    });
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        //$("#uomlist").val(JSON.stringify($scope.UOMList));
                        $scope.MainUOMList = $scope.UOMList;
                        $scope.SearchUOMList = $scope.UOMList;
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
        $scope.SearchUOMList = $filter('limitTo')($scope.UOMList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchUOMList = $filter('limitTo')($scope.UOMList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.UOMList.length) {
            $scope.SearchUOMList = $filter('limitTo')($scope.UOMList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.UOMList.length;
        var rem = parseInt($scope.UOMList.length) % parseInt($scope.Paging);
        var position = $scope.UOMList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.UOMList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchUOMList = $filter('limitTo')($scope.UOMList, $scope.Paging, position);
    }

    $scope.init = function () {
        GetUOMList();
        //$scope.UOMList = JSON.parse($("#uomlist").val());
        //$scope.SearchUOMList = $filter('limitTo')($scope.UOMList, $scope.Paging, $scope.CurruntIndex);
    }

    $scope.init();

}]);