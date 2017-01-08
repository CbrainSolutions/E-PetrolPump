PetroliumApp.controller("LoginController", ['$scope', '$http', '$filter', function ($scope, $http, $filter) {

    $scope.LoginModel = {
        UserName: '',
        Password: ''
    };

    $scope.ValidateAndLogin = function () {
        var spinner = new Spinner().spin();
        document.getElementById("mainbody").appendChild(spinner.el);
        var model = { Email: $scope.LoginModel.UserName, Password: $scope.LoginModel.Password };
        if ($scope.LoginModel.$invalid) {
            var objShowCustomAlert = new ShowCustomAlert({
                Title: "",
                Message: "Login name and password should be filled.",
                Type: "alert",
            });
            objShowCustomAlert.ShowCustomAlertBox();
        }
        else {
            var url = GetVirtualDirectory();
            var req = {
                method: 'POST',
                url: url + '/home/login',
                headers: {
                    'Content-Type': "application/json"
                },
                data: model,
            }

            $http(req).then(function (response) {
                if (response.data.Status == true) {
                    window.location = url + "/Subledger";
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
    }

    $scope.lnkForgotPasswordClick = function () {
        $("#divLogin").hide();
        $("#forGotPassword").show();
    }


    $scope.lnkLoginClick = function () {
        $("#divLogin").show();
        $("#forGotPassword").hide();
    }

    $scope.ForgotPassword = function () {
        var userid = $("#UserLoginId").val();
        if (userid == "") {
            var objShowCustomAlert = new ShowCustomAlert({
                Title: "",
                Message: "Login name should be filled",
                Type: "alert",
            });
            objShowCustomAlert.ShowCustomAlertBox();
            $("#usernammelogin").focus();
            return false;
        }

        var spinner = new Spinner().spin();
        document.getElementById("contentdiv").appendChild(spinner.el);
        var model = { UserName: userid, Password: "" };
        $.ajax({
            cache: false,
            type: "POST",
            async: false,
            data: model,
            url: GetVirtualDirectory() + "/Account/ForgotPassword",
            dataType: "json",
            success: function (students) {
                if (students.Status == true) {
                    var objShowCustomAlert = new ShowCustomAlert({
                        Title: "",
                        Message: "Your Password has been send on your registered mail id.",
                        Type: "alert",
                    });
                    objShowCustomAlert.ShowCustomAlertBox();
                }
                else {
                    var objShowCustomAlert = new ShowCustomAlert({
                        Title: "",
                        Message: students.ErrorMessage,
                        Type: "alert",
                    });
                    objShowCustomAlert.ShowCustomAlertBox();
                }
                document.getElementById("contentdiv").removeChild(spinner.el);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error during process: \n' + xhr.responseText);
                document.getElementById("contentdiv").removeChild(spinner.el);
            }
        });
    }

    $scope.init = function () {
        //$scope.BindVendorTypes();
    }

    $scope.init();
}]);