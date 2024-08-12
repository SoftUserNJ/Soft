$(function () {

    Select2();
    ActiveMenu();
    EditDashboard();
    $('.customizer-links').addClass('d-none');

});

function EditDashboard() {
    var path = window.location.pathname;

    if (path === "/" || path === "/Home/Index") {
        $('body').attr('oncontextmenu', 'EditMenu(event)');
    }
}

function Select2() {

    $(".select2").each(function () {
        $(this)
            .wrap("<div class=\"position-relative w-100\"></div>")
            .select2({
                placeholder: "Select value",
                dropdownParent: $(this).parent()
            });

    })

}

function ActiveMenu() {

    var url = window.location.pathname;

    if (url != "/Home/Index") {

        $(".submenu").each(function () {

            $(this).find('li').each(function () {

                var subMenu = $(this).find('a').attr('href');
                if (subMenu.toLowerCase() == url.toLowerCase()) {
                    $(this).find('a').addClass('active');

                    $(this).closest('.submenu').children('a').addClass('active subdrop');
                }

            });

        });
    }
    else {
        $(".sidebar-menu").find('li').each(function () {
            if (url == $(this).find('a').attr('href')) {
                $(this).addClass('active');
            }
        });
    }

}

function ToolTip(title) {

    var name = "";
    if (title.length > 20) {
        name = title.substring(0, 20) + "...";
    } else {
        name = title;
    }

    return name;
}

function ToolTipAuto(title, length) {

    var name = "";
    if (title.length > length) {
        name = title.substring(0, length) + "...";
    } else {
        name = title;
    }

    return name;
}

function ToolTip50(title) {

    var name = "";
    if (title.length > 50) {
        name = title.substring(0, 50) + "...";
    } else {
        name = title;
    }

    return name;
}

function CheckValidation(classname) {

    var submission = true;

    $("." + classname).find('input,select').each(function () {

        var id = $(this).attr('id');

        var skinid = $(this).attr('skinid');

        var type = $(this).attr('type');

        var value = $.trim($("#" + id).val());

        if (type != "hidden" && type != "file" && type != "checkbox") {

            if (skinid != "" && skinid != undefined) {

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

//function CheckDuplicateEntry(name, tableid, classname) {

//    var Okay = true;

//    $(tableid + " tr").each(function () {
//        var x = $.trim($(this).find(classname).text()).toLowerCase();
//        if (x == name.toLowerCase()) {
//            Okay = false;
//        }

//    });

//    return Okay;
//}


function CheckDuplicateEntry(name, tableid, classname, ignoreRowId, ignoreRowClass) {
    var Okay = true;

    $(tableid + " tr").each(function () {
        var objectId = $(this).find(ignoreRowClass).text().trim();
        if (objectId === ignoreRowId) {
            return; // Ignore this row and continue to the next row.
        }

        var x = $.trim($(this).find(classname).text()).toLowerCase();
        if (x === name.toLowerCase()) {
            Okay = false;
        }
    });

    return Okay;
}


//==================== Export to Word ====================

function HTMLtoWORD(tableid, fileName) {
    var header = "<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
        "xmlns:w='urn:schemas-microsoft-com:office:word' " +
        "xmlns='http://www.w3.org/TR/REC-html40'>" +
        "<head><meta charset='utf-8'><title>Export HTML to Word Document with JavaScript</title>" +
        "<style>table { border-collapse: collapse; width: 100%; border: 1px solid black; } " +
        "th, td { border: 1px solid black; padding: 8px; text-align: left; }</style></head><body>";
    var footer = "</body></html>";

    // Get the outerHTML of the table element to preserve its structure
    var tableHTML = document.getElementById(tableid).outerHTML;
    var cloneTable = tableHTML;

    // Create a temporary div to manipulate the cloned table
    var tempDiv = document.createElement('div');
    tempDiv.innerHTML = cloneTable;

    // Remove elements with the 'd-none' class
    var elementsToRemoved = tempDiv.querySelectorAll('.d-none');
    var elementsToRemove = tempDiv.querySelectorAll('.notPrintCol');
    elementsToRemove.forEach(function (element) {
        element.parentNode.removeChild(element);
    });

    elementsToRemoved.forEach(function (element) {
        element.parentNode.removeChild(element);
    });

    // Get the updated HTML content from the temporary div
    cloneTable = tempDiv.innerHTML;

    var sourceHTML = header + cloneTable + footer;

    var source = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHTML);
    var fileDownload = document.createElement("a");
    document.body.appendChild(fileDownload);
    fileDownload.href = source;
    fileDownload.download = fileName + '.doc';
    fileDownload.click();
    document.body.removeChild(fileDownload);
}


$("body").on("click", ".incLocationView", function () {

    var Lat = $(this).closest('tr').find('.incLati').text();
    var Long = $(this).closest('tr').find('.incLongi').text();

    initialize(Lat, Long);
    $("#LocationModal").modal('show');

    return false;
});

function initialize(x, y) {

    var latlng = new google.maps.LatLng(x, y);
    var myOptions = {
        zoom: 18,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map"), myOptions);

    marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: "Your current location!"
    });
    return false;
}

