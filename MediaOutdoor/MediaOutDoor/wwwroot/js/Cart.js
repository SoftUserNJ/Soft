$(function () {
    GetCheckOut();
});


function GetCheckOut() {
    var visitorId = getCookie("visitorId");
    $.ajax({
        url: '/Cart/GetCartbyVisitor',
        type: 'POST',
        data: { visitorId: visitorId },
        success: function (data) {
            console.log(data);
            if (typeof data === 'string') {
                data = JSON.parse(data);
            }
            $('#CheckoutList').empty();

            // Group the data by StationId
            var groupedData = {};
            data.forEach(function (item) {
                var screenId = item.ScreenId;
                if (!groupedData[item.StationId]) {
                    groupedData[item.StationId] = {
                        StationName: item.StationName,
                        StationImage: item.StationImage,
                        Screens: []
                    };
                }
                groupedData[item.StationId].Screens.push({
                    ScreenId: screenId,
                    ScreenName: item.ScreenName,
                    Rate: item.Rate,
                    slotFrom: item.slotfrom,
                    slotTo: item.slotto,
                    playDate: item.playdate
                });
            });

            // Initialize the total price
            var totalPrice = 0;
            var uniqueCombinations = [];
            // Loop through the grouped data and create rows
            for (var stationId in groupedData) {
                var stationData = groupedData[stationId];

                // Calculate the total rate for this station
                var totalRate = stationData.Screens.reduce(function (acc, screen) {
                    return acc + screen.Rate;
                }, 0);

                totalPrice += totalRate; // Add the station's total rate to the total price

                var row = `
        <hr class="my-4">

        <div class="row mb-4 d-flex align-items-center items-row">
            <div class="col-md-2 col-lg-2 col-xl-2">
                <img id="stationImg" src="/${stationData.StationImage}" class="img-fluid rounded-3" alt="${stationData.StationName}">
            </div>
            <div class="col-md-2 col-lg-2 col-xl-2">
                <h6 class="text-black mb-0">${stationData.StationName}</h6>
            </div>
            <div class="col-md-3 col-lg-3 col-xl-3">
                <ul>
                    ${stationData.Screens.map(screen => `
                        <li style="color:white">
                            Screen Name: ${screen.ScreenName}
                            <i style="margin-left:10px; cursor:pointer;" class="text-muted fa fa-times remove-screen" data-screenid="${screen.ScreenId}" aria-hidden="true"></i>
                        </li>
                    `).join('')}
                </ul>
            </div>

            <div class="col-md-2 col-lg-2 col-xl-2">
                <ul>
                    ${stationData.Screens.map(screen => `
                        <li style="color:white">€ ${screen.Rate}</li>
                    `).join('')}
                </ul>
            </div>
            <div class="col-md-2 col-lg-2 col-xl-2">
                <h5 style="padding:12px;"> € ${totalRate}</h5>
            </div>
            <div class="col-md-1 col-lg-1 col-xl-1">
                <h6 class="text-black mb-0"><i style="margin-left:10px;cursor:pointer;" class="fa fa-times text-muted remove-station" data-stationid="${stationId}" aria-hidden="true"></i></h6>
            </div>
        </div>
        
       <div class="row mb-4 d-flex align-items-center items-row">
           <div class="col-md-12 col-lg-12 col-xl-12">
            <ul>
          ${stationData.Screens.map(screen => {
              // Create a unique combination string for slotFrom, slotTo, and playDate
              const combination = `${screen.slotFrom}-${screen.slotTo}-${screen.playDate}`;

              // Check if the combination has been displayed before
              if (!uniqueCombinations.includes(combination)) {
                  // Add the combination to the list of unique combinations
                  uniqueCombinations.push(combination);
                  return `
            <div class="row mb-4 d-flex align-items-center items-row">
                <div class="col-md-2 col-lg-2 col-xl-3" style="color: white;">
                    Slot From: ${screen.slotFrom ? screen.slotFrom : '<span style="color:red;">Please Select Slot</span>'}
                </div>
                <div class="col-md-2 col-lg-2 col-xl-3" style="color: white;">
                    Slot To: ${screen.slotTo ? screen.slotTo : '<span style="color:red;">Please Select Slot</span>'}
                </div>
                <div class="col-md-2 col-lg-2 col-xl-3" style="color: white;">
                    Play Date: ${screen.playDate ? formatDate(screen.playDate) : '<span style="color:red;">Please Select Date</span>'}
                </div>

            </div>
        `;
              } else {
                  return ''; // Return an empty string for duplicate combinations
              }
          }).join('')}




            </ul>
        </div>
        </div>
    `;
                $('#CheckoutList').append(row);
            }

            // Update the total price in the HTML element with id "subTotal"
            $('#subTotal').text('€ ' + totalPrice);

        },
        error: function () {
            // Handle errors if the request fails
        }
    });
}


$('#CheckoutList').on('click', '.remove-screen', function () {


    var screenId = $(this).data('screenid');

    removeScreenFromCart(screenId, visitorId);
  
});

function formatDate(dateString) {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}


$('#CheckoutList').on('click', '.remove-station', function () {


    var stationId = $(this).data('stationid');

    removeStationFromCart(stationId, visitorId);
 
});

var visitorId = getCookie("visitorId");



function removeScreenFromCart(screenId, visitorId) { 

    $.ajax({
        url: '/Cart/RemoveScreenFromCart',
        type: 'POST',
        data: {
            screenId: screenId,
            visitorId: visitorId
        },
        success: function (data) {
            GetCheckOut();
            updateCartRate(visitorId)
        },
        error: function () {
            // Handle errors if the request fails
        }
    });
}


function removeStationFromCart(stationId, visitorId) {

    $.ajax({
        url: '/Cart/RemoveStationFromCart',
        type: 'POST',
        data: {
            stationId: stationId,
            visitorId: visitorId
        },
        success: function (data) {
            GetCheckOut();
            updateCartRate(visitorId)
        },
        error: function () {
            // Handle errors if the request fails
        }
    });
}

$("#PaymentPage").click(function () { debugger

    var visitorId = getCookie("visitorId");
    $.ajax({
        url: '/Cart/CheckDesign',
        type: 'POST',
        data: { visitorId: visitorId },
        success: function (data) {
            if (data === true) {
                window.location.href = "/Payment/Payment";
            } else {
                window.location.href = "/Design/UploadYourDesign";
            }
        },
        error: function () {
            // Handle errors if the request fails
        }
    });

});