PetroliumApp.controller("DesignationController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.MainDesignationList = [];
    $scope.DesignationList = [];
    $scope.SearchDesignationList = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsDesigName: false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.DesignationCode = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SubledgerModel = { DesignationDesc: "", DesignationCode: 0 };

    $scope.Prefix = "";
    function GetDesignations() {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/Designation/GetAllDesignations";
        $http.get(url)
        .then(function (response) {
            $scope.MainDesignationList = response.data.DesignationList;
            $scope.DesignationList = response.data.DesignationList;
            $scope.SearchDesignationList = $filter('limitTo')($scope.DesignationList, $scope.Paging, $scope.CurruntIndex);
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
        //$scope.DesignationList = $filter('filter')(JSON.parse($("#DesignationList").val()), { DesignationDesc: $scope.Prefix })
        var reg = new RegExp($scope.Prefix.toLowerCase());
        $scope.DesignationList = $scope.MainDesignationList.filter(function (dept) {
            return (reg.test(dept.DesignationDesc.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.DesignationList = $scope.MainDesignationList;
        $scope.SearchDesignationList = $scope.DesignationList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#DesignationDesc").val("");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (SubledgerModel) {
        $("#DesignationDesc").val(SubledgerModel.DesignationDesc);
        $scope.DesignationCode = SubledgerModel.DesignationCode;
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {

        if ($("#DesignationDesc").val() == "") {
            $scope.ErrorModel.IsDesigName = true;
            $scope.ErrorMessage = "Designation name should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsDesigName = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var model = { DesignationDesc: $("#DesignationDesc").val() };
        if (isEdit == false) {
            model = { DesignationDesc: $("#DesignationDesc").val(), DesignationCode: $scope.DesignationCode };
        }

        var url = GetVirtualDirectory() + '/Designation/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/Designation/Update';
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
                    angular.forEach($scope.DesignationList, function (value, key) {
                        if (value.DesignationCode == model.DesignationCode) {
                            $scope.DesignationList[key].DesignationDesc = model.DesignationDesc;
                        }
                    });
                }
                else {
                    model = { DesignationDesc: model.DesignationDesc, DesignationCode: response.data.Id };
                    $scope.DesignationList.push(model);
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        $scope.MainDesignationList=$scope.DesignationList;
                        $scope.SearchDesignationList = $scope.DesignationList;
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
        $scope.SearchDesignationList = $filter('limitTo')($scope.DesignationList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchDesignationList = $filter('limitTo')($scope.DesignationList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.DesignationList.length) {
            $scope.SearchDesignationList = $filter('limitTo')($scope.DesignationList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.DesignationList.length;
        var rem = parseInt($scope.DesignationList.length) % parseInt($scope.Paging);
        var position = $scope.DesignationList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.DesignationList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchDesignationList = $filter('limitTo')($scope.DesignationList, $scope.Paging, position);
    }

    $scope.init = function () {
        GetDesignations();
    }

    $scope.init();

}]);