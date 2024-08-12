// ======================== Left Active Link Menu =====================

$(document).ready(function () {
    // Get the current page URL
    var currentPageURL = window.location.href;
    var urlSegments = currentPageURL.split("/");

    // Get the last two segments of the URL
    var lastTwoSegments = "/" + urlSegments.slice(-2).join("/");

    // 'lastTwoSegments' now contains "User/UserAdd"

    // Iterate through each anchor tag within the menu
    $(".menu-list li a").each(function () {
        // Get the href attribute of the anchor tag
        var anchorURL = $(this).attr("href");

        // Compare the current page URL with the anchor URL
        if (lastTwoSegments === anchorURL) {
            // Change the color of the anchor to green
            $(this).addClass("active-color");
        }
    });
});


// ======================== Left Active Link Menu End =====================