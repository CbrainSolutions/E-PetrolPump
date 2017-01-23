PetroliumApp.controller("DeptController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.DepartmentList = [];
    $scope.SearchDepartmentList = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsDeptName: false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.DepartmentId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { DepartmentName: "", DepartmentId: 0 };

    $scope.Prefix = "";

    $scope.AddNewUI = function (isedit) {
        $scope.Details = false;
        $scope.Add = true;
        $scope.Edit = false;
    }

    $scope.FilterList = function () {
        $scope.DepartmentList = $filter('filter')(JSON.parse($("#DepartmentList").val()), { DepartmentName: $scope.Prefix })
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.DepartmentList = JSON.parse($("#DepartmentList").val());
        $scope.SearchDepartmentList = $scope.DepartmentList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#DepartmentName").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (SubledgerModel) {
        $("#DepartmentName").val(SubledgerModel.SubLedgerName);
        $scope.SubLedgerId = SubledgerModel.SubLedgerId;
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {
        
        if ($("#DepartmentName").val() == "") {
            $scope.ErrorModel.IsDeptName = true;
            $scope.ErrorMessage = "Department name should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsSelectsubledgerName = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var model = { DepartmentName: $("#DepartmentName").val() };
        if (isEdit == false) {
            model = { DepartmentName: $("#DepartmentName").val(), DepartmentId: $scope.DepartmentId };
        }

        var url = GetVirtualDirectory() + '/Department/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/Department/Update';
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
                    angular.forEach($scope.DepartmentList, function (value, key) {
                        if (value.DepartmentId == model.DepartmentId) {
                            $scope.DepartmentList[key].DepartmentName = model.DepartmentName;
                        }
                    });
                }
                else {
                    model = { DepartmentName: model.DepartmentName, DepartmentId: response.data.Id};
                    $scope.DepartmentList.push(model);
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        $("#DepartmentList").val(JSON.stringify($scope.DepartmentList));
                        $scope.SearchDepartmentList = $scope.DepartmentList;
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
        $scope.SearchDepartmentList = $filter('limitTo')($scope.DepartmentList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchDepartmentList = $filter('limitTo')($scope.DepartmentList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.DepartmentList.length) {
            $scope.SearchDepartmentList = $filter('limitTo')($scope.DepartmentList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.DepartmentList.length;
        var rem = parseInt($scope.DepartmentList.length) % parseInt($scope.Paging);
        var position = $scope.DepartmentList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.DepartmentList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchDepartmentList = $filter('limitTo')($scope.DepartmentList, $scope.Paging, position);
    }

    $scope.init = function () {
        $scope.DepartmentList = JSON.parse($("#DepartmentList").val());
        $scope.SearchDepartmentList = $filter('limitTo')($scope.DepartmentList, $scope.Paging, $scope.CurruntIndex);
        
    }

    $scope.init();

}]);