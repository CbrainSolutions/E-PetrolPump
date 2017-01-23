PetroliumApp.controller("SwipeController", ['$scope', '$http', '$filter', '$rootScope', function ($scope, $http, $filter, $rootScope) {

    $scope.SwipeList = [];
    $scope.SearchSwipeList = [];
    $scope.SwipeMachineList = [];
    $scope.BankList = [];
    $scope.Details = true;
    $scope.ErrorModel = { IsMachineNo: false, IsBankName:false,IsCardType:false };
    $scope.ErrorMessage = ""
    $scope.Add = false;
    $scope.Edit = false;
    $scope.MachineId = 0;
    $scope.Paging = 10;
    $scope.CurruntIndex = 0;
    $scope.SwipeMachineModel = { MachineNo: "", MachineId: 0, BankId: 0, BankName: "", CardType:"CC" };

    $scope.Prefix = "";

    function GetMachineDetails() {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var url = GetVirtualDirectory() + "/swipe/GetData";
        $http.get(url)
        .then(function (response) {
            $scope.SwipeList = response.data.SwipeMachineList;
            $scope.SwipeMachineList = response.data.SwipeMachineList;
            $scope.SearchSwipeList = $filter('limitTo')($scope.SwipeList, $scope.Paging, $scope.CurruntIndex);
            $scope.BankList = response.data.BankDetails.BankList;
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
        var reg = new RegExp($scope.Prefix.toLowerCase());
        $scope.SwipeList = $scope.SwipeMachineList.filter(function (swipe) {
            return (reg.test(swipe.MachineNo.toLowerCase()) || reg.test(swipe.BankName.toLowerCase()));
        });
        $scope.First();
    }

    $scope.Reset = function () {
        $scope.SwipeList = JSON.parse($("#SwipeList").val());
        $scope.SearchSwipeList = $scope.SwipeList;
        $scope.First();
    }

    $scope.CancelClick = function () {
        $("#MachineNo").val("");
        $("#BankId").val("0");
        $scope.Details = true;
        $scope.Add = false;
        $scope.Edit = false;
    }

    $scope.EditClick = function (SwipeModel) {
        $("#MachineNo").val(SwipeModel.MachineNo);
        $scope.MachineId = SwipeModel.MachineId;
        $("#BankId").val(SwipeModel.BankId);
        $scope.SwipeMachineModel.CardType = SwipeModel.CardType;
        $scope.Details = false;
        $scope.Add = false;
        $scope.Edit = true;
    }

    $scope.Save = function (isEdit) {

        if ($("#MachineNo").val() == "") {
            $scope.ErrorModel.IsMachineNo = true;
            $scope.ErrorMessage = "Machine no should be filled.";
            return false;
        }
        else {
            $scope.ErrorModel.IsMachineNo = false;
        }
        if ($("#BankId").val() == "0") {
            $scope.ErrorModel.IsBankName = true;
            $scope.ErrorMessage = "Bank name should be selected.";
            return false;
        }
        else {
            $scope.ErrorModel.IsBankName = false;
        }
        if ($scope.SwipeMachineModel.CardType == "") {
            $scope.ErrorModel.IsCardType = true;
            $scope.ErrorMessage = "Card type should be selected.";
            return false;
        }
        else {
            $scope.ErrorModel.IsCardType = false;
        }
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var model = { MachineNo: $("#MachineNo").val(), BankId: $("#BankId").val(), CardType: $scope.SwipeMachineModel.CardType };
        if (isEdit == false) {
            model = { MachineNo: $("#MachineNo").val(), BankId: $("#BankId").val(), CardType: $scope.SwipeMachineModel.CardType, MachineId: $scope.MachineId };
        }

        var url = GetVirtualDirectory() + '/Swipe/Save';
        if (isEdit == false) {
            url = GetVirtualDirectory() + '/Swipe/Update';
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
                var arr = $("#BankId option:selected").text().split("_AC_");
                var bankname = arr[0];
                var acno;
                try {
                    acno = arr[1];
                } catch (e) {

                }

                if (isEdit == false) {
                    angular.forEach($scope.SwipeList, function (value, key) {
                        if (value.MachineId == model.MachineId) {
                            $scope.SwipeList[key].MachineNo = model.MachineNo;
                            $scope.SwipeList[key].BankName = bankname;
                            $scope.SwipeList[key].AccountNo = acno;
                            $scope.SwipeList[key].CardType = model.CardType;
                            $scope.SwipeList[key].MachineNo = model.MachineNo;
                        }
                    });
                }
                else {
                    model = { MachineNo: model.MachineNo, MachineId: response.data.Id, BankName: bankname,CardType:$scope.SwipeMachineModel.CardType,BankId:$("#BankId").val(),AccountNo:acno };
                    $scope.SwipeList.push(model);
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        $scope.SwipeMachineList=$scope.SwipeList;
                        $scope.SearchSwipeList = $scope.SwipeList;
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
        $scope.SearchSwipeList = $filter('limitTo')($scope.SwipeList, $scope.Paging, 0);
    }

    $scope.Prev = function () {
        $scope.CurruntIndex = $scope.CurruntIndex - $scope.Paging;
        if ($scope.CurruntIndex >= 0) {
            $scope.SearchSwipeList = $filter('limitTo')($scope.SwipeList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.CurruntIndex = 0;
        }
    }

    $scope.Next = function () {
        $scope.CurruntIndex = $scope.CurruntIndex + $scope.Paging;
        if ($scope.CurruntIndex <= $scope.SwipeList.length) {
            $scope.SearchSwipeList = $filter('limitTo')($scope.SwipeList, $scope.Paging, $scope.CurruntIndex);
        }
        else {
            $scope.Last();
        }
    }

    $scope.Last = function () {
        var total = $scope.SwipeList.length;
        var rem = parseInt($scope.SwipeList.length) % parseInt($scope.Paging);
        var position = $scope.SwipeList.length - $scope.Paging;
        if (rem > 0) {
            position = $scope.SwipeList.length - rem;
        }
        $scope.CurruntIndex = position;
        $scope.SearchSwipeList = $filter('limitTo')($scope.SwipeList, $scope.Paging, position);
    }

    $scope.init = function () {
        GetMachineDetails();
    }

    $scope.init();

}]);