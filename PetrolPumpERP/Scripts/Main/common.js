$(document).ready(function () {
    $("#status_message").keypress(function (event) {
        if (event.which == 13) {
            event.preventDefault();
            SendMessage();
        }
    });
});
function Logout() {
    window.location = GetVirtualDirectory() + '/Account/LogOff';
}

function getUrlParameter(param, dummyPath) {
    var sPageURL = dummyPath || window.location.search.substring(1),
        sURLVariables = sPageURL.split(/[&||?]/),
        res;

    for (var i = 0; i < sURLVariables.length; i += 1) {
        var paramName = sURLVariables[i],
            sParameterName = (paramName || '').split('=');

        if (sParameterName[0] === param) {
            res = sParameterName[1];
        }
    }

    return res;
}

function baseUrl() {
    var pathname = window.location.href;
    return pathname;
}

function SendMessage()
{
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: GetVirtualDirectory() + "/Message/SendMessage?fromUserId=" + $("#ActiveUserId").val() + "&toUserId=" + $("#touserid").attr("data") + "&msg=" + $("#status_message").val(),
        dataType: "json",
        success: function (students) {
            GetAllMessgaes($("#ActiveUserId").val(), $("#touserid").attr("data"));
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error during process: \n' + xhr.responseText);
        }
    });
}

function ReadMessgesByUser() {
    setInterval(function () {
        if (document.getElementById("ActiveUserId") != null) {
            $.ajax({
                cache: false,
                type: "GET",
                async: false,
                url: GetVirtualDirectory() + "/Home/ReadMessage?toUserId=" + $("#ActiveUserId").val(),
                dataType: "json",
                success: function (students) {
                    bindUserMessage(students, $("#ActiveUserId").val());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    
                }
            });
        }
    }, 15000);
}

function GetAllMessgaes(userid, touserid)
{
    setInterval(function () {
        $.ajax({
            cache: false,
            type: "GET",
            async: false,
            url: GetVirtualDirectory() + "/Home/GetAllMessgaes?UserId=" + userid + "&ToUserId=" + touserid,
            dataType: "json",
            success: function (students) {
                bindUserMessges(students);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    }, 15000);
}

function bindUserMessges(students)
{
    var htmlin = "";
    $("#msguser").html(htmlin);
    for (var i = 0; i < students.length; i++) {
        var date = new Date(parseInt(students[i].MessageDate.substr(6)));
        htmlin += '<div class="chat-box-single-line"><abbr class="timestamp">' + date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear() + '</abbr></div>';
        htmlin += '<div class="direct-chat-msg doted-border"><div class="direct-chat-info clearfix"><span class="direct-chat-name pull-left">';
        if (students[i].FromUserId == $("#ActiveUserId").val()) {
            htmlin += 'me';
        }
        else {
            htmlin += $("#" + students[i].FromUserId + ">strong").html();
        }
        htmlin += '</span></div><div class="direct-chat-text">' + students[i].MessageText + '</div></div>';
    }
    $("#msguser").html(htmlin);
}

function bindUserMessage(respnse,userid)
{
    $("#inmsg").html("");
    var htmlin = "";
    for (var i = 0; i < respnse.length; i++) {
        htmlin += '<li class="dropdown-menu-header text-center" id="' + respnse[i].Id + '" onclick="ShowChat(this)"><strong>' + respnse[i].FirstName + " " + respnse[i].LName + '</strong></li>';
    }
    $("#inmsg").html(htmlin);
}

function ShowChat(liuser)
{
    $('#qnimate').addClass('popup-box-on');
    var touserid = $(liuser).attr("id");
    $("#touserid").attr("data", touserid);
    var userid = $("#ActiveUserId").val();
    $("#chatuser").html(liuser.fir);
    GetAllMessgaes(userid, touserid);
}


function GetVirtualDirectory() {

    var result = "";
    var url = window.location.href;

    var url_parts = url.split('/');
    var index = 0;
    for (var i = 0; i < url_parts.length; i++) {
        if (url_parts[i] != "") {
            if (i > 2) {
                break;
            }
            result = result + url_parts[i];
        }
        if (i == 1) {
            result = result + "//";
        }
    }

    return result + "/PetrolPumpERP";
}
function SetActiveTab(tabname) {
    switch (tabname) {
        case "register":
            $("#lihome").removeClass("active");
            $("#liSearch").removeClass("active")
            $("#liMarriageHall").removeClass("active");
            $("#liOther").removeClass("active");
            $("#licontact").removeClass("contact");
            $("#lireg").addClass("active");
            break
        case "search":
            $("#lihome").removeClass("active");
            $("#liSearch").addClass("active")
            $("#liMarriageHall").removeClass("active");
            $("#liOther").removeClass("active");
            $("#licontact").removeClass("contact");
            $("#lireg").removeClass("active");
            break
        case "contact":
            $("#lihome").removeClass("active");
            $("#liSearch").removeClass("active")
            $("#liMarriageHall").removeClass("active");
            $("#liOther").removeClass("active");
            $("#lireg").removeClass("active");
            $("#licontact").addClass("active");
            break
        default:
            $("#lihome").addClass("active");
            $("#liSearch").removeClass("active")
            $("#liMarriageHall").removeClass("active");
            $("#liOther").removeClass("active");
            $("#lireg").removeClass("active");
            break;

    }
}