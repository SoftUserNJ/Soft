
$(document).ready(function () {

    // Check if the visitor ID cookie already exists
    var visitorId = getCookie("visitorId");

    // If the cookie doesn't exist, generate a new visitor ID
    if (!visitorId) {
        visitorId = generateVisitorId();

        // Set the visitor ID in a cookie that expires in 1 day
        setCookie("visitorId", visitorId, 1);
    }

    updateCartRate(visitorId) 
   
    // Add an event listener to the search input
    $('#searchInput').on('input', function () {
        filterProducts($(this).val());
    });
});


// Function to generate a unique visitor ID
function generateVisitorId() {
    var timestamp = new Date().getTime();
    var randomNum = Math.floor(Math.random() * 1000000);
    return timestamp + "-" + randomNum;
}


// Function to set a cookie with a given name, value, and expiration in days
function setCookie(name, value, days) {
    days = 365;
    var expires = "";
    if (days) {

        var date = new Date();
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}







// Function to get a cookie's value by name
function getCookie(name) {
    var nameEQ = name + "=";
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        while (cookie.charAt(0) === " ") {
            cookie = cookie.substring(1, cookie.length);
        }
        if (cookie.indexOf(nameEQ) === 0) {
            return cookie.substring(nameEQ.length, cookie.length);
        }
    }
    return null;
}


function updateCartRate(visitorId) {
    
    var displayedStationIds = [];
    $.ajax({
        type: "GET",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        url: '/Cart/GetCartbyVisitor',
        data: { VisitorId: visitorId },
        dataType: "json",
        success: function (data) {
            var subtotal = 0;
            $("#sideBarBody").empty();
            $.each(data, function (i, item) {
                var stationId = item.StationId;

                if (!displayedStationIds.includes(stationId)) {
                    var head = `<h3 style="font-size: 16px; padding-top:16px; padding-left:16px;">${item.StationName}</h3>`;
                    $("#sideBarBody").append(head);
                    displayedStationIds.push(stationId); // Mark this stationId as displayed
                }

                var newLink = `<a href="#" stationid='${item.StationId}' screenid='${item.ScreenId}' rate='${item.Rate}' style="padding-bottom: 0;" >Screen: ${item.ScreenName} - Rate: ${item.Rate}</a>`;
                $("#sideBarBody").append(newLink);


                var rate = item.Rate;
                if (!isNaN(rate)) {
                    subtotal += parseFloat(rate);
                }
            });
            
            var formattedSubtotal = '€' + subtotal.toFixed(2);
            $(".price").html(formattedSubtotal);


            updateTotalRate();
        },
        error: function () {
            console.log("Error fetching data from the server.");
        }
    });
}


function updateTotalRate() {
    var totalRate = 0;

    $("#sideBarBody a").each(function () {
        var rate = parseFloat($(this).attr("rate"));
        if (!isNaN(rate)) {
            totalRate += rate;
        }
    });

    $("#sidebarFooter").html('<h4 style=" padding-bottom: 10px; ">________________________</h4><h4 id="totalCart" style="padding:16px;">Total: €' + totalRate.toFixed(2) + '</h4>');
}


function CheckValidation(classname) {

    var submission = true;

    $("." + classname).find('input,select').each(function () {

        var id = $(this).attr('id');

        var skinid = $(this).attr('skinid');

        var type = $(this).attr('type');

        var value = $.trim($("#" + id).val());

        if (type != "hidden" && type != "file" && type != "checkbox") {

            if (skinid != "") {

                if (value == "" || value == "0") {

                    toastr.warning(skinid, { closeButton: !0, tapToDismiss: !1, rtl: o });

                    $("#" + id).focus();

                    submission = false;
                    return false;
                }
            }
        }
    });

    return submission;
}

function o(c) {
    var d = v();
    return b || i(d), c && 0 === a(":focus", c).length ? void w(c) : void (b.children().length && b.remove());
}