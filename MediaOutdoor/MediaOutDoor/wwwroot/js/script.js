
var visitorId = getCookie("visitorId");

$(document).ready(function () {

    visitorId = getCookie("visitorId");
    updateCartRate(visitorId);


});



    function SlideNext() {
        $('.owl-next').click();
        }

    setInterval(SlideNext, 5000);

    function calculateScreenCounts(stationId, screensize) {
            const stationData = window.productData.filter(product => product.stationid === stationId);

            const singleCount = stationData.reduce((count, product) => {
                return count + (product.screensize === 'Single' ? product.TotalScreen : 0);
            }, 0);

            const doubleCount = stationData.reduce((count, product) => {
                return count + (product.screensize === 'Double' ? product.TotalScreen : 0);
            }, 0);

            const tripleCount = stationData.reduce((count, product) => {
                return count + (product.screensize === 'Triple' ? product.TotalScreen : 0);
            }, 0);

    return {
        single: singleCount,
    double: doubleCount,
    triple: tripleCount,
            };
        }






function loadProducts() {
    
        $.ajax({
            url: '/MOD/GetStations',
            method: 'GET',
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            dataType: 'json',
            async: true,
            success: function (data) {
                console.log(data);
                // Store the product data in a variable
                window.productData = data.Stations;
                initMap(data);
                filterProducts(""); // Initially, show all products
            },
            error: function (error) {
                console.error('Error loading products:', error);
            },
        });
}



    function filterProducts(searchText) {
        // Clear the product container
        $('#product-container').empty();

    // Create a Set to store unique station names
    const uniqueStationNames = new Set();

    $.each(window.productData, function (index, product) {
                // Check if the StationName contains the search text
                if (product.StationName.toLowerCase().includes(searchText.toLowerCase())) {
                    // Check if the station name is not in the Set
                    if (!uniqueStationNames.has(product.StationName)) {
        uniqueStationNames.add(product.StationName);
    var screenCounts = calculateScreenCounts(product.stationid, product.screensize);
    var productHtml = `
    <div class="col-lg-4 col-md-6 col-sm-6 mix" style="cursor:pointer">
        <div class="product__item">
            <div class="product__item__pic set-bg" style="background-image: url('${product.StationImage}');border-radius:14px;">
                <!-- Add any additional elements you need here -->
                <ul class="product__hover">
                    <li><a href="#"><img src="img/icon/QuickView.png" alt=""></a></li>
                </ul>
            </div>

            <div class="product__item__text">
                <center><h6>${product.StationName}</h6></center>
                <center>
                    <h6 style="display:inline-block;color: #735bc0;">Single: ${screenCounts.single} </h6>
                    <h6 style="display:inline-block;color: #735bc0;"> | Double: ${screenCounts.double}</h6>
                    <h6 style="display:inline-block;color: #735bc0;"> | Triple: ${screenCounts.triple}</h6>
                    <h6>Daily Passer: 23900</h6>
                </center>
                <a href="#" class="add-cart book">+ Book Now</a>

                <!-- Add any additional product information here -->
            </div>
        </div>
    </div>
    `;
    $('#product-container').append(productHtml);
                    }
                }
            });
        }


function initMap(data) {
    const labelSet = new Set(); // Use a Set to store unique labels
    const mapCenter = { lat: 52.3676, lng: 4.9041 };

    const map = new google.maps.Map(document.getElementById('map'), {
        center: mapCenter,
        zoom: 11
    });

    const markerData = [];
    const screensData = data.Screens;

    data.Stations.forEach(station => {
        const stationData = {
            position: { lat: parseFloat(station.Lat), lng: parseFloat(station.Long) },
            imageUrl: station.StationImage,
            label: station.StationName,
            stationid: station.stationid,
            lat: station.Lat,
            long: station.Long
        };
        markerData.push(stationData);

        // Check if the label is not already in the Set, then add it
        if (!labelSet.has(stationData.label)) {
            labelSet.add(stationData.label);

            const searchMapLocations = document.getElementById('SearchMapLocations');
            if (searchMapLocations) {
                const labelListHTML = `<div><a style="font-size:20px;color:white;" href="#" data-label="${stationData.label}" data-lat="${stationData.lat}" data-long="${stationData.long}">${stationData.label}</a></div>`;
                searchMapLocations.innerHTML += labelListHTML;
            }
        }
    });

    const infoWindow = new google.maps.InfoWindow();

    markerData.forEach(datastations => {
        const marker = new google.maps.Marker({
            position: datastations.position,
            map: map,
            icon: '/img/icon/train.png'
        });

        datastations.marker = marker; // Assign the marker to the stationData object

        marker.addListener('click', () => { 
            console.log(JSON.stringify(screensData));

            const content = `<div><img src="/${datastations.imageUrl}" stationid="${datastations.stationid}" rate="${datastations.rate}" ScreenName="${datastations.ScreenName}" label="${datastations.label}" onclick="stationimgview('/${datastations.imageUrl}', ${datastations.stationid})" alt="Marker Image" width="350" height="200"><br><br>${datastations.label}<br><br>Daily Passers: 33242</div>`;

            infoWindow.setContent(content);
            infoWindow.open(map, marker);
        });

        marker.addListener('mouseover', () => {
            infoWindow.setContent(datastations.label); // Show label on mouseover
            infoWindow.open(map, marker);
        });
    });
    
    // Add an event listener to the search input
    const searchInput = document.getElementById('searchMapsBtn');
    if (searchInput) {
        searchInput.addEventListener('input', () => {
            const searchValue = searchInput.value.trim().toLowerCase();
            const labelLinks = document.querySelectorAll('#SearchMapLocations a');

            labelLinks.forEach(link => {
                const label = link.getAttribute('data-label').toLowerCase();
                if (label.includes(searchValue)) {
                    link.style.display = 'block';
                } else {
                    link.style.display = 'none';
                }
            });
        });
    }

    // Update your click event listener for the links:

    const labelLinks = document.querySelectorAll('#SearchMapLocations a');  

    labelLinks.forEach(link => {
        link.addEventListener('click', (event) => {
            event.preventDefault(); // Prevent the default link behavior
            console.log('Link clicked!');
            // Get the latitude and longitude from the clicked <a> tag's data attributes
            const lat = parseFloat(link.getAttribute('data-lat'));
            const lng = parseFloat(link.getAttribute('data-long'));
            console.log('Lat:', lat);
            console.log('Lng:', lng);
            // Find the corresponding marker and open its info window
            markerData.forEach(datastations => {
                // Use a small epsilon value for comparison
                const epsilon = 0.000001;
                if (Math.abs(datastations.lat - lat) < epsilon && Math.abs(datastations.long - lng) < epsilon) {
                    google.maps.event.trigger(datastations.marker, 'click');
                }
            });
        });
    });

}











        var mapoverlay; // Declare a global variable to hold the map instance
        var screenIdsArray = [];
        function stationimgview(imageUrl, stationid) { 
            var selectedLinks = [];
        $("#sideBarBody a").each(function () {
                var stationId = $(this).attr("stationid");
        var screenId = $(this).attr("screenid");
        if (stationId !== undefined && screenId !== undefined) {
            selectedLinks.push({ stationId: stationId, screenId: screenId });
                }
        });

        var imageBounds = [[0, 0], [400, 600]];
        if (mapoverlay) {
            mapoverlay.eachLayer(function (layer) {
                mapoverlay.removeLayer(layer);
            });
            } else {

            mapoverlay = L.map('stationPopup', { attributionControl: false, center: [200, 300], zoom: 1, maxBounds: imageBounds });
            }









        L.imageOverlay(imageUrl, imageBounds).addTo(mapoverlay);
        var svgElement = document.createElementNS("http://www.w3.org/2000/svg", "svg");

        svgElement.setAttribute('xmlns', "http://www.w3.org/2000/svg");
        svgElement.setAttribute('viewBox', "0 0 400 600");
        svgElement.setAttribute('onclick', "openNav()");
        svgElement.setAttribute('preserveAspectRatio', "none");
        svgElement.style.width = '100%';
        svgElement.style.height = '100%';

            var ButtonGroup = document.createElementNS("http://www.w3.org/2000/svg", "g");
            ButtonGroup.style.width = '100%';
            ButtonGroup.style.height = '100%';


        $.ajax({

            type: "Get",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        url: '/MOD/GetScreens?stationid=' + stationid,
        async: false,
        dataType: "json",
        success: function (data) {
            $.each(data, function (i, item) {

           



                var foreignObject = document.createElementNS("http://www.w3.org/2000/svg", "foreignObject");
                foreignObject.setAttribute("x", item.XPosition); // Adjust X and Y as needed
                foreignObject.setAttribute("y", item.YPosition); // Adjust X and Y as needed
                foreignObject.setAttribute("width", item.width);
                foreignObject.setAttribute("height", item.height);

                foreignObject.classList.add("rect-bounce");
                foreignObject.setAttribute("objectid", "");
                foreignObject.setAttribute("stationid", item.StationId);
                foreignObject.setAttribute("id", item.ScreenId);
                foreignObject.setAttribute("screenName", item.ScreenName);
                foreignObject.setAttribute("StationName", item.StationName);
                foreignObject.setAttribute("Rate", item.Rate);
                foreignObject.classList.add("add-to-cart-btn");


               





                var combinationExists = selectedLinks.some(function (link) {
                    return link.stationId == item.StationId && link.screenId == item.ScreenId;
                });

                

                var div = document.createElement("div");
                div.setAttribute("xmlns", "http://www.w3.org/1999/xhtml");


             
              


                if (combinationExists) {
                    div.innerHTML = '<i class="fas fa-long-arrow-alt-down arrow-clicked"  style="color:green" ></i>';
                } else {
                    div.innerHTML = '<i class="fas fa-long-arrow-alt-down"  style="color:white" ></i>';
                }





           


              

                foreignObject.appendChild(div);

                ButtonGroup.appendChild(foreignObject);

            });





                },
        error: function (error) {
            console.error('Error:', error);
            console.error('Error:', error.statusCode);
                }
            });

            svgElement.appendChild(ButtonGroup);
  
        L.svgOverlay(svgElement, imageBounds).addTo(mapoverlay);


        $(document).ready(function () {
            $(".add-to-cart-btn").click(function () {
                var StationId = $(this).attr("stationid");
                var ScreenId = $(this).attr("id");
                var ScreenName = $(this).attr("screenName");
                var StationName = $(this).attr("StationName");

                


                var OrderRate = getCookie("OrderRate");
                var rate = 0;
                if (OrderRate != null) {
                    rate = parseFloat(OrderRate);
                }
                else {
                  rate =  parseFloat($(this).attr("Rate"));
                }
                displayScreenName(StationId, StationName, ScreenId, ScreenName, rate);
                updateTotalRate();
                $(this).toggleClass("arrow-clicked");
            });
            });


        $(".add-to-cart-btn").css("pointer-events", "auto");



        // Show the modal
        $("#largeModal").modal("show");

        // Wait for the modal to be fully shown before resizing the map
        $("#largeModal").on('shown.bs.modal', function () {
            mapoverlay.invalidateSize();
            });

            



        }

        var stationScreenGroups = { };

console.log("Visitor:" + visitorId);
function displayScreenName(StationId, StationName, ScreenId, ScreenName, rate) {
    debugger

    if (!stationScreenGroups[StationName]) {
        var head = `<h3 style="font-size: 16px; padding-top:16px; padding-left:16px;">${StationName}</h3>`;
        $("#sideBarBody").append(head);
        stationScreenGroups[StationName] = [];
    }
    var screenIndex = stationScreenGroups[StationName].indexOf(ScreenName);
    if (screenIndex === -1) {
        var newLink = `<a href="#mapSection" stationid='${StationId}' screenid='${ScreenId}' rate='${rate}' style="padding-bottom: 0;" >Screen: ${ScreenName} - Rate: ${rate}</a>`;
        $("#sideBarBody").append(newLink);
        stationScreenGroups[StationName].push(ScreenName);

        // Add the ScreenId to the array
        screenIdsArray.push(ScreenId);

        // Add a click event handler to the new link to call the controller method
        
        var screenId = $(newLink).attr('screenid');
        var stationId = $(newLink).attr('stationid');
        var rate = $(newLink).attr('rate');
        saveCart(screenId, visitorId, stationId, rate);
        
        
    } else {
        stationScreenGroups[StationName].splice(screenIndex, 1);
        $("#sideBarBody").find(`a:contains('Screen: ${ScreenName}')`).remove();

        // Remove the ScreenId from the array
        var screenIdIndex = screenIdsArray.indexOf(ScreenId);
        if (screenIdIndex !== -1) {
            screenIdsArray.splice(screenIdIndex, 1);
            removeScreenFromCart(ScreenId, visitorId);
            
        }

        if (stationScreenGroups[StationName].length === 0) {
            delete stationScreenGroups[StationName];
            $("#sideBarBody").find(`h3:contains('${StationName}')`).remove();
        }
    }
}

function navigateToCart() {
    // Replace the URL with the desired controller and action
    window.location.href = '/Cart/Cart';
}


function saveCart(screenId, visitorId, stationId, rate) {
    var orderType = getCookie("OrderType");
        $.ajax({
            url: '/Cart/SaveCart',
            type: 'POST',
            data: {
                screenId: screenId,
                visitorId: visitorId,
                stationId: stationId,
                rate: rate,
                orderType: orderType


                     

            },
            success: function (data) {
                // Handle the response from the controller if needed
                updateCartRate(visitorId);
            },
            error: function () {
                // Handle errors if the request fails
            }
        });
}


function removeScreenFromCart(screenId, visitorId) {
    
    $.ajax({
        url: '/Cart/RemoveScreenFromCart',
        type: 'POST',
        data: {
            screenId: screenId,
            visitorId: visitorId
        },
        success: function (data) {
            // Handle the response from the controller if needed
            updateCartRate(visitorId);
            toastr.success("Cart Saved Successfully", "", { closeButton: !0, tapToDismiss: !1, ltl: o });
        },
        error: function () {
            toastr.error("Please Try Again", "", { closeButton: !0, tapToDismiss: !1, ltl: o });
        }
    });
}


       


function navigateToCart() {
    window.location.href = '/Cart/Cart';
}

function navigateToDesign() {

    window.location.href = '/Design/UploadYourDesign';
}

function navigateToDesignMV() {
   
    window.location.href = '/Design/DesignMV';
}


        function openNav() {
            document.getElementById("mySidebar").style.width = "250px";
        }

        function closeNav() {
            document.getElementById("mySidebar").style.width = "0";
        }

