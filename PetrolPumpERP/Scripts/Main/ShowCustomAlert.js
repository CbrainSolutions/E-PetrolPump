var ShowCustomAlert = function () {

    // Define option defaults
    var defaults = {
        Title: '',
        Message: '', // message which you want to show
        Type: 'alert',//alert or confirm
        OKButtonText: "OK",
        CancelButtonText: "Cancel",
        HideAfterInterval: false, // if you want to hide the alert box after given time
        HideTime: 0, //in seconds
        OnHide: function () { },
        OnOKClick: function () { },
        OnCancelClick: function () { }
    }

    // Create options by extending defaults with the passed in arugments
    if (arguments[0] && typeof arguments[0] === "object") {
        this.options = extendDefaults(defaults, arguments[0]);
    }

    this.ShowCustomAlertBox = function () {
        document.getElementById("alertDecription").innerHTML = defaults.Message;
        switch (defaults.Type) {
            case "alert":
                var listContainer = document.getElementById("alertBoxList");
                if (listContainer.firstChild.getAttribute("id") != "liBtnOk" && listContainer.firstChild.getAttribute("id") == "liBtnCancel") {
                    listContainer.removeChild(listContainer.firstChild);
                }
                break;
            case "confirm":
                var listContainer = document.getElementById("alertBoxList");
                if (listContainer.firstChild.getAttribute("id") == "liBtnOk") {
                    var btnCancel = document.createElement("li");
                    btnCancel.setAttribute("id", "liBtnCancel");
                    btnCancel.innerHTML = "<input value=\"" + defaults.CancelButtonText + "\" type=\"button\" id=\"btn_Cancel\" class=\"btn btn-primary\" />";
                    listContainer.insertBefore(btnCancel, listContainer.firstChild);

                    document.getElementById("btn_Cancel").addEventListener("click", function () {
                        closeThis('CANCEL');
                    });
                }
                break;
        }
        document.getElementById("alertBox").style.display = "block";
        document.getElementById("btn_Ok").focus();

        if (defaults.HideAfterInterval) {
            SetTimer(defaults.HideTime);
        }

    }

    this.addAlertPopup = function () {
        var alertBoxContainer = document.createElement("div");
        alertBoxContainer.setAttribute("id", "alertBox");
        alertBoxContainer.setAttribute("class", "submitPopup");
        alertBoxContainer.style.display = "none";
        var alertBox = "";
        alertBox += "<div class=\"overlay\">";
        alertBox += "</div>";
        alertBox += "<div class=\"alert alert-info submitPopupPanel\">";
        alertBox += "<div id=\"alertDecription\" class=\"popupInfo\"></div>";
        alertBox += "<div class=\"legendBtn\">";
        alertBox += "<ul id=\"alertBoxList\">";
        alertBox += "<li id=\"liBtnOk\">";
        alertBox += "<input value=\"" + defaults.OKButtonText + "\" type=\"button\" id=\"btn_Ok\" class=\"btn btn-danger\" />";
        alertBox += "</li>";
        alertBox += "</ul>";
        alertBox += "</div>";
        alertBox += "</div>";

        if (document.body) {
            if (document.getElementById("alertBox") == null) {
                document.body.appendChild(alertBoxContainer);
            }
        }

        if (document.getElementById("alertBox") != null) {
            document.getElementById("alertBox").innerHTML = alertBox;
        }
    }

    function closeThis(target) {
        document.getElementById("alertBox").style.display = "none";
        switch (target) {
            case "OK":
                if (defaults.OnOKClick)
                    defaults.OnOKClick();
                break;
            case "CANCEL":
                defaults.OnCancelClick();
                break;
            case "HIDE":
                defaults.OnHide();
                break;
        }

    }

    function extendDefaults(source, properties) {
        var property;
        for (property in properties) {
            if (properties.hasOwnProperty(property)) {
                source[property] = properties[property];
            }
        }
        return source;
    }

    function SetTimer(sec) {
        setInterval(function () {
            sec--;
            if (sec == 00) {
                closeThis("HIDE");
            }
        }, 1000);
    }

    this.init = function () {
        this.addAlertPopup();

        document.getElementById("btn_Ok").addEventListener("click", function () {
            closeThis("OK");
        });
    }



    this.init();
}


//for backward compatibility
function showCustomAlertBox(title, description, type, callbackFunct, callbackobject) {

    var objShowCustomAlert = new ShowCustomAlert({
        Title: title,
        Message: description,
        Type: type,
        OnOKClick: callbackFunct
    });

    objShowCustomAlert.ShowCustomAlertBox();
}