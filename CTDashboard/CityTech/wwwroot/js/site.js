$(function () {

    Select2();
    ActiveMenu();
    EditDashboard();
    $('.customizer-links').addClass('d-none');
    

});

$(document).ready(function () {
    // Initialize Select2 for elements with the class "multi_search_filter"
    $('.multi_search_filter').select2();
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

    //$(tableid + " tr").each(function () {
    //    var objectId = $(this).find(ignoreRowClass).text().trim();
    //    if (objectId === ignoreRowId) {
    //        return; // Ignore this row and continue to the next row.
    //    }

    //    var x = $.trim($(this).find(classname).text()).toLowerCase();
    //    if (x === name.toLowerCase()) {
    //        Okay = false;
    //    }
    //});

    return Okay;
}



$("#btnNewIncident").click(function () {
    NewIncidentGetData();
});



$(".btnradioclick").click(function () {


    const labelValue = $(this).attr("label");
    if (labelValue == "BRIGHTNESS") {
        $("#div1").empty();
        $("#div1").append(` <img src="/Images/CHART1.png" alt="" >  `);
    }
    if (labelValue == "SENSOR COUNT") {
        $("#div2").empty();
        $("#div2").append(`  <img src="/Images/CHART2.png" alt="" > `);
    }
    if (labelValue == "MAINTENANCE HISTORY") {
        $("#div3").empty();
        $("#div3").append(`  <img src="/Images/CHART3.png" alt="" > `);
    }
});

$("#btndate").click(function () {
    $("#reportrange").click();
    var daterangepickerDiv = $(".daterangepicker");
    const customRangeElement = document.querySelector('li[data-range-key="Custom Range"]');
    customRangeElement.click();
});


function BeepSound() {
    const incidentAlertSound = document.getElementById("incidentAlertSound");
    incidentAlertSound.play();
}

function SheduledModalOpen() {

    $("#NewIncident").show();
    $("#NewIncident").addClass('show');
    $("#NewIncident").css('backdrop-filter', 'blur(10px)');
}

function AfterSheduled() {

    var divsWithClassDivInc = document.querySelectorAll(".DivInc");
    var count = divsWithClassDivInc.length;
    if (count == 0) {
        $("#NewIncident").hide();
        $("#NewIncident").removeClass('show');
        $("#NewIncident").css('backdrop-filter', 'none');
    }

}


//=============== Modal Loop Start ===============

function NewIncidentGetData() {
    try {
        $.ajax({
            type: "Get",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            url: "/Incident/IncidentGetData/",
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Incident.length == 0) {
                    return false;
                }

                SheduledModalOpen();
                BeepSound();
                $("#NewIncident").empty();

                var select = "";

                $.each(data.Mechanic, function (i, item) {
                    select += `<option incident="${item.INCIDENTNO}" value="${item.USERID}">${item.MECHANIC}</option>`;
                });

                $.each(data.Incident, function (i, item) {

                    var response = item.SLARESPONSE;
                    const currentDateTimeR = moment();
                    const newDateTimeR = currentDateTimeR.add(response, "hours");
                    var finalResponse = newDateTimeR.format("HH:mm D-M-YYYY");

                    var secure = item.SLASECURE;
                    const currentDateTimeS = moment();
                    const newDateTimeS = currentDateTimeS.add(secure, "hours");
                    var finalSecure = newDateTimeS.format("HH:mm D-M-YYYY")
                    var secureDateVal = newDateTimeS.format("YYYY-MM-DDTHH:mm");

                    var fixed = item.SLAFIXED;
                    const currentDateTimeF = moment();
                    const newDateTimeF = currentDateTimeF.add(fixed, "hours");
                    var finalFixed = newDateTimeF.format("HH:mm D-M-YYYY")
                    var fixedDateVal = newDateTimeF.format("YYYY-MM-DDTHH:mm");


                    $("#NewIncident").append(`

                        <div class="modal-dialog" style="max-width:634px;">
                      
                            <div class="modal-content modal-content-cs" style="border: 2px solid #b01d16 !important;">
                                <div class="modal-body modal-body-cs">
                                    <input type="hidden" value="${item.INCIDENTNO}" class="iNo" />
                                    <div class="container1">
                                        <div><h2 class="main-head1">PRIO <span class="incidentPrio">${item.PRIOTYPE}</span></h2></div>
                                        <div><p class="main-head2">NEW INCIDENT<br>ACTION REQUIRED</p></div>
                                        <div><i class="fa-solid fa-triangle-exclamation main-head3" style="color: #f53d6a;"></i></div>
                                    </div>
                                    <div class="container2">
                                        <div class="box-1">
                                            <p>Incident:  <span class="incidentName">${item.INCIDENT}</span></p>
                                            <p>Location: <span class="incidentLocation">${item.LOCATION}</span></p>
                                            <p>Object:   <span class="incidentObject">${item.OBJECT}</span></p>
                                        </div>
                                        <div class="box-2">
                                            <h3>SLA</h3>
                                            <table>
                                            <tr><td class="pb-2">Response: </td> <td>&nbsp; before &nbsp;<span class="incidentResponse">${finalResponse}</span></td></tr>
                                            <tr><td class="pb-2">Secure: </td> <td>&nbsp; before &nbsp;<span class="incidentSecure">${finalSecure}</span></td></tr>
                                            <tr><td class="pb-2">Fixed: </td> <td>&nbsp; before &nbsp;<span class="incidentFixed">${finalFixed}</span></td></tr>
                                            </table>
                                        </div>
                                    </div>
                        
                                    <div class="container3 pb-1">
                                        <div class="SchBox1">
                                            <p class="sch-head">Schedule now</p>
                                            <div>
                                                <h3>Name mechanic:</h3>
                                                <select class="drpMechanic form-control rounded-pill login-input pt-1 pb-1 mt-0 mb-0" skinid="Select Mechanic....!">
                                                    ${select}
                                                </select>
                                            </div>
                        
                                            <h3>Preparations:</h3>
                                            <input type="text" value="${item.PREPRATION}" class="incidentPrepration form-control rounded-pill login-input-c" style="width:100%;" skinid="Enter Preparations....!" />
                                            <h3>Requirements:</h3>
                                            <input type="text" value="${item.REQUIREMENTS}" class="incidentRequirements form-control rounded-pill login-input-c" style="width:100%;" skinid="Enter Preparations....!" />
                                        </div>
                                        <div class="SchBox2 pt-3">
                                            <h3 class="mb-1">Secure date:</h3>
                                            <input type="datetime-local" value="${secureDateVal}" class="txtSecureDate form-control rounded-pill login-input-c" style="width:210px;" skinid="Select Secure Date....!">
                                            <h3 class="mb-1 mt-2">Fixed date:</h3>
                                            <input type="datetime-local" value="${fixedDateVal}" class="txtFixedDate form-control rounded-pill login-input-c" style="width:210px;" skinid="Select Fixed Date....!">
                        
                                            <button class="btnIncidentSchedule rounded-pill schedule-btn login-button p-0">Schedule</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `);
                });


                //var iscureDate = $("#incidentSecure").text();
                //var iscureDateFormat = moment(iscureDate, "HH:mm DD-M-YYYY");
                //$("#txtSecureDate").val(iscureDateFormat.format("YYYY-MM-DDTHH:mm"));

                //var iFixedDate = $("#incidentFixed").text();
                //var iFixedDateFormat = moment(iFixedDate, "HH:mm DD-M-YYYY");
                //$("#txtFixedDate").val(iFixedDateFormat.format("YYYY-MM-DDTHH:mm"));


            },
            error: function (data) {
            }
        });
    }
    catch (err) {
        alert(err)
    }
}
document.querySelectorAll('.modal-header button.close').forEach(function (button) {
    button.addEventListener('click', function () {
        $(this).closest('.modal').modal('hide');
    });
});

function checkModalExistence(ModelId) {
    return !!document.getElementById(ModelId);
}



function NewIncidentGetData1(jsonData) {

    try {
        var data = JSON.parse(jsonData);
        if (data.Incident.length == 0) {
            AfterSheduled();
            $("#NewIncident").empty();
            return false;
        }

        var divsWithClassDivInc = document.querySelectorAll(".DivInc");
        divsWithClassDivInc.forEach(function (element) {
            var idToCheck = element.id;
            var found = data.Incident.some(function (incident) {

                return "DivInc" + incident.INCIDENTNO === idToCheck;
            });

            if (found) {
            } else {

                $("#" + idToCheck).remove();
                AfterSheduled();


            }
        });

        $.each(data.Incident, function (i, item) {

            //      debugger;

            var select = "";
            var Ownformselect = "";
            var filteredMechanics = data.Mechanic.filter(function (mechanicItem) {
                return mechanicItem.INCIDENTNO === item.INCIDENTNO;
            });

            $.each(filteredMechanics, function (i, mechanicItem) {
                select += `<option incident="${mechanicItem.INCIDENTNO}" value="${mechanicItem.USERID}">${mechanicItem.MECHANIC}</option>`;
            });



            $.each(data.Ownform, function (i, item) {
                Ownformselect += `<option  value="${item.FORMID}">${item.FORMNAME}</option>`;
            });











            var ModelId = "DivInc" + item.INCIDENTNO;
            if ($('#' + ModelId).length > 0 || $("#lblincidentno").val() == ModelId) {

            }
            else {


                var response = item.SLARESPONSE;
                const currentDateTimeR = moment();
                const newDateTimeR = currentDateTimeR.add(response, "hours");
                var finalResponse = newDateTimeR.format("HH:mm D-M-YYYY");

                var secure = item.SLASECURE;
                const currentDateTimeS = moment();
                const newDateTimeS = currentDateTimeS.add(secure, "hours");
                var finalSecure = newDateTimeS.format("HH:mm D-M-YYYY")
                var secureDateVal = newDateTimeS.format("YYYY-MM-DDTHH:mm");

                var fixed = item.SLAFIXED;
                const currentDateTimeF = moment();
                const newDateTimeF = currentDateTimeF.add(fixed, "hours");
                var finalFixed = newDateTimeF.format("HH:mm D-M-YYYY")
                var fixedDateVal = newDateTimeF.format("YYYY-MM-DDTHH:mm");
                SheduledModalOpen();
                BeepSound();
                var iddrpMechanic = "drpMechanic" + item.INCIDENTNO;
                var ReasonHideShow = "";
                if (item.STATUS != "REJECTED") {
                    ReasonHideShow = "display :none;";
                }
                var settingsTime = parseInt($("#NextCallerWaitingTimeLayout").val());
                var cDateTime = new Date();
                cDateTime.setMinutes(cDateTime.getMinutes() + settingsTime);
                $("#NewIncident").append(`

                        <div class="modal-dialog DivInc" style="max-width:634px; " id="${ModelId}">
                     
                            <div class="modal-content modal-content-cs" style="border: 2px solid #b01d16 !important;">
                           
                                <div class="modal-body modal-body-cs">

                                    <input type="hidden" value="${item.INCIDENTNO}" class="iNo" />
                                    <div class="container1">
                                        <div><h2 class="main-head1">PRIO <span class="incidentPrio">${item.PRIOTYPE}</span></h2></div>
                                        <div><p class="main-head2">${item.STATUS} INCIDENT<br>ACTION REQUIRED</p></div>
                                        <div><i class="fa-solid fa-triangle-exclamation main-head3" style="color: #f53d6a;"></i></div>
                                      
                                    </div>
                                    <div class="container2">
                                        <div class="box-1">
                                           <p>IncidentNo:  <span class="incidentName">${item.INCIDENTNO}</span></p>
                                            <p>Incident:  <span class="incidentName">${item.INCIDENT}</span></p>
                                            <p>Location: <span class="incidentLocation">${item.LOCATION}</span></p>
                                            <p>Object:   <span class="incidentObject">${item.OBJECT}</span></p>
                                        </div>
                                        <div class="box-2">
                                            <h3>SLA</h3>
                                            <table>
                                            <tr><td class="pb-2">Response: </td> <td>&nbsp; before &nbsp;<span class="incidentResponse">${finalResponse}</span></td></tr>
                                            <tr><td class="pb-2">Secure: </td> <td>&nbsp; before &nbsp;<span class="incidentSecure">${finalSecure}</span></td></tr>
                                            <tr><td class="pb-2">Fixed: </td> <td>&nbsp; before &nbsp;<span class="incidentFixed">${finalFixed}</span></td></tr>
                                            </table>
                                        </div>
                                    </div>
                        
                                    <div class="container3 pb-1">
                                        <div class="SchBox1">
                                            <p class="sch-head">Schedule now</p>
                                            <div>
                                                <h3>Name mechanic:</h3>
                                                <select id=${iddrpMechanic} class="drp-arrow drpMechanic form-control rounded-pill login-input pt-1 pb-1 mt-0 mb-0" skinid="Select Mechanic....!">
                                                    ${select}
                                                </select>
                                            </div>



                        
                                            <h3>Preparations:</h3>
                                            <input type="text" value="${item.PREPRATION}" class="incidentPrepration form-control rounded-pill login-input-c" style="width:100%;" skinid="Enter Preparations....!" />
                                            <h3>Requirements:</h3>
                                            <input type="text" value="${item.REQUIREMENTS}" class="incidentRequirements form-control rounded-pill login-input-c" style="width:100%;" skinid="Enter Preparations....!" />

                                            <h3>Attach Forms:</h3>
                                              <select name="multi_search_filter" style="width:200px;"  multiple="multiple" class="multi_search_filter form-control ">
                                               ${Ownformselect}
                                              </select>
                                        </div>
                                        <div class="SchBox2 pt-3">
                                       
                                            <h3 class="mb-1">Work Start:</h3>
                                            <input type="datetime-local" value="${secureDateVal}" class="date24 txtSecureDate form-control rounded-pill login-input-c" style="width:210px;" skinid="Select Secure Date....!">
                                            <h3 class="mb-1 mt-2">Work End:</h3>
                                            <input type="datetime-local" value="${fixedDateVal}" class="date24 txtFixedDate form-control rounded-pill login-input-c" style="width:210px;" skinid="Select Fixed Date....!">
                                     
                                             <div style=" display: flex; justify-content: space-between;">
                                            <button class="btnIncidentSchedule  rounded-pill schedule-btn login-button p-0" style="    margin-right: 8px;"  Modelid="${ModelId}" >Schedule</button>

                                                           <div class="countdown-timer circle" style="margin-top: 22px;margin-right: 5px; display:none;" onclick="toggleCountdown('${ModelId}')">
                                                            <span class="timer">5</span>
                                                              </div>
                                                              </div>
                                             <div class="d-none text-center mt-2" style="width: 200px !important;"><h3 class="callerUserName"></h3>
                                                <button class="user-btn-okay-alert ct-btns p-3 pt-0 pb-0 text-white border-0" style="box-shadow:0 2px 4px rgba(0, 0, 0, 0.2);">Ok</button>
                                                <input type="hidden" value="${cDateTime}" class="user-btn-okay-alert-time">
                                            </div>
                                       </div>
                                    </div>
                                 <div>   <p style="color: #cf2730;margin-left: 15px;font-weight: bold; ${ReasonHideShow} ">     Reason: ${item.WORKDES}</p></div>
                              </div>
                            </div>
                        </div>
                    `);



                $('.multi_search_filter').select2();




                createCountdownTimer(ModelId);


                //      debugger;
                if (item.STATUS == "REJECTED") {
                    $("#drpMechanic" + item.INCIDENTNO).val(item.MECHANICID);
                }

            }
        });



        DataInputListener();

    }
    catch (err) {
        alert(err)
    }
}



setInterval(OkayTimeOver, 500);



// Function to create and manage a countdown timer for an incident
function createCountdownTimer(ModelId) {
    const timerElement = $(`#${ModelId} .timer`);
    const countdownCircle = $(`#${ModelId} .countdown-timer`);

    let seconds = parseInt(timerElement.text());
    let countdownInterval;
    let isPaused = false;

    function updateTimer() {
        return false;
        timerElement.text(seconds);

        if (seconds < 0) {
            clearInterval(countdownInterval);
            timerElement.text("Countdown complete!");
            countdownCircle.addClass("stopped"); // Add the "stopped" class
            const scheduleButton = $(`#${ModelId} .btnIncidentSchedule`);
            scheduleButton.click();
        } else if (!isPaused) {
            seconds--;
        }
    }

    countdownInterval = setInterval(updateTimer, 1000);

    // Function to toggle pause/resume when clicking on the countdown timer
    function toggleCountdown() {
        return false;
        if (isPaused) {
            countdownInterval = setInterval(updateTimer, 1000);
            isPaused = false;
            countdownCircle.removeClass("stopped"); // Remove the "stopped" class
        } else {
            clearInterval(countdownInterval);
            isPaused = true;
            countdownCircle.addClass("stopped"); // Add the "stopped" class
        }
    }

    // Bind the toggleCountdown function to the click event of the countdown timer
    countdownCircle.on("click", toggleCountdown);
}

// Call createCountdownTimer for each incident div
$(".DivInc").each(function () {
    const ModelId = $(this).attr("id");
    createCountdownTimer(ModelId);
});




// Start the countdown timer for the new incident
function OkayTimeOver() {

    var targetTime = $(".user-btn-okay-alert-time").val();
    console.log(targetTime)

    if (targetTime != '') {

        var cDateTime = new Date().toString();

        if (cDateTime == targetTime) {

            $(".user-btn-okay-alert-time").each(function () {

                if ($(this).val() == targetTime) {

                    $(this).closest('.SchBox2').find('button.btnIncidentSchedule').addClass('d-none');
                    $(this).closest('.SchBox2').find('div').removeClass('d-none');


                    if ($(this).closest('.SchBox2').find('div').is(':visible')) {

                        var nextUser = $(this).closest('.SchBox2').find('.callerUserName').text();

                        //ajax  start
                        try {
                            $.ajax({
                                type: "POST",
                                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                                url: "/Incident/GetEmContactUser/",
                                data: { username: nextUser },
                                async: false,
                                dataType: "json",
                                success: function (data) {
                                    nextUser = data;
                                },
                                error: function () {
                                }
                            });
                        }
                        catch (err) {
                            alert(err)
                        }
                        //ajax  end

                        $(this).closest('.SchBox2').find('.callerUserName').text(nextUser);

                        var settingsTime = parseInt($("#NextCallerWaitingTimeLayout").val());
                        var DateTimeNow = new Date();
                        DateTimeNow.setMinutes(DateTimeNow.getMinutes() + settingsTime);
                        $(this).val(DateTimeNow);
                    }
                }
            });
        }
    }
}

$('body').on('click', ".user-btn-okay-alert", function () {
    $(this).closest('.SchBox2').find('button.btnIncidentSchedule').removeClass('d-none');
    $(this).closest('div').remove();
});
//==================== Modal Loop End ====================




$('body').on('click', ".btnIncidentSchedule", function () {

    var modelId = $(this).attr("Modelid");
    $("#lblincidentno").val(modelId);





    var obj = {};
    obj.incidentNo = $(this).closest('.modal-body-cs').find('.iNo').val();
    obj.scheduleDate = moment().format('YYYY-MM-DD HH:mm:ss');
    obj.secureDate = $(this).closest('.modal-body-cs').find('.txtSecureDate').val();
    obj.fixDate = $(this).closest('.modal-body-cs').find('.txtFixedDate').val();
    obj.mechanic = $(this).closest('.modal-body-cs').find('.drpMechanic').val();
    obj.prepration = $(this).closest('.modal-body-cs').find('.incidentPrepration').val();
    obj.requirement = $(this).closest('.modal-body-cs').find('.incidentRequirements').val();
    obj.selectedForms = $(this).closest('.modal-body-cs').find('.multi_search_filter').val();

    try {
        $.ajax({
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            url: "/Incident/ScheduleIncidentSave/",
            data: obj,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data != false) {
                    $("#" + modelId).remove();
                    AfterSheduled();

                    toastr.success("Scheduled Successfully", "", { closeButton: !0, tapToDismiss: !1, ltl: o });
                }
                else {
                    $("#lblincidentno").val("");
                    toastr.error("Please Schedule Again", "", { closeButton: !0, tapToDismiss: !1, ltl: o });
                }
            },
            error: function () {
                $("#lblincidentno").val("");
            }
        });
    }
    catch (err) {
        $("#lblincidentno").val("");
        alert(err)
    }
});

//==================== Modal Date Limit Validation Start ====================

$('body').on('change', ".txtSecureDate", function () {

    var scureDate = $(this).closest('.modal-body-cs').find('.container2').find('.incidentSecure').text()
    var secureMoment = moment(scureDate, "HH:mm D-M-YYYY");
    var desireSecureFormat = secureMoment.format("YYYY-MM-DDTHH:mm");
    var maxDateTime = new Date(desireSecureFormat).getTime();

    var selectedDateTime = new Date($(this).val()).getTime();

    if (selectedDateTime > maxDateTime) {
        var iscureDate = $(this).closest('.modal-body-cs').find('.container2').find('.incidentSecure').text()
        var iscureDateFormat = moment(iscureDate, "HH:mm DD-M-YYYY");
        $(this).val(iscureDateFormat.format("YYYY-MM-DDTHH:mm"));
        toastr.warning("Datetime exceeds the maximum allowed date and time", "", { closeButton: !0, tapToDismiss: !1, ltl: o });
    }
});

$('body').on('change', ".txtFixedDate", function () {

    var fixedDate = $(this).closest('.modal-body-cs').find('.container2').find('.incidentFixed').text();
    var fixedMoment = moment(fixedDate, "HH:mm D-M-YYYY");
    var desireFixedFormat = fixedMoment.format("YYYY-MM-DDTHH:mm");
    var maxDateTime = new Date(desireFixedFormat).getTime();

    var selectedDateTime = new Date($(this).val()).getTime();

    if (selectedDateTime > maxDateTime) {

        var iFixedDate = $(this).closest('.modal-body-cs').find('.container2').find('.incidentFixed').text();
        var iFixedDateFormat = moment(iFixedDate, "HH:mm DD-M-YYYY");
        $(this).val(iFixedDateFormat.format("YYYY-MM-DDTHH:mm"));
        toastr.warning("Datetime exceeds the maximum allowed date and time", "", { closeButton: !0, tapToDismiss: !1, ltl: o });
    }
});

//==================== Modal Date Limit Validation End ====================

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




function DataInputListener() {
    //// Get all elements with the class 'date24'
    //const date24Elements = document.querySelectorAll('.date24');

    //date24Elements.forEach(function (element) {
    //    element.addEventListener('input', function () {
    //        const datetimeValue = element.value;
    //        const date = new Date(datetimeValue);
    //        const hours = date.getHours();
    //        const minutes = date.getMinutes();
    //        const ampm = hours >= 12 ? 'PM' : 'AM';
    //        const formattedTimeString = `${(hours % 12).toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')} ${ampm}`;

    //        // Check if the date is the 24th day of the month
    //        //if (date.getDate() === 24) {
    //        //    // Add your event handling code here
    //        //    console.log('Event on date 24');
    //        //}
    //    });
    //});
}

// Call the function to set up the event listener



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

const SaveLoader = async () => {
    try {
        await new Promise((resolve) => {
            $('#LoaderModal').modal({
                backdrop: 'static',
                keyboard: false,
                show: false
            });

            $("#LoaderModal").modal('show');
            resolve();
        });

    } catch (err) {
        console.log(err);
        return false;
    }
};



//Hide Loader 


const SaveLoaderHide = async () => {
    try {
        await new Promise(resolve => {
            setTimeout(() => {
                $("#LoaderModal").modal('hide');
                resolve();
            }, 1000); // Add a delay of 1000 milliseconds before hiding the modal
        });
    } catch (err) {
        console.log(err);
        return false;
    }
};
