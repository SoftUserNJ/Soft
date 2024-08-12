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

// ======================== Move Drag Grids =====================

const sortableList = document.querySelector(".sortable-list");
const items = sortableList.querySelectorAll(".item");

items.forEach(item => {
  item.addEventListener("dragstart", () => {
    // Adding dragging class to item after a delay
    setTimeout(() => item.classList.add("dragging"), 0);
  });
  // Removing dragging class from item on dragend event
  item.addEventListener("dragend", () => item.classList.remove("dragging"));
});

const initSortableList = (e) => {
  e.preventDefault();

  const draggingItem = document.querySelector(".dragging");
  // Getting all items except currently dragging and making array of them
  let siblings = [...sortableList.querySelectorAll(".item:not(.dragging)")];

  // Finding the sibling after which the dragging item should be placed
  let nextSibling = siblings.find(sibling => {
    return e.clientY <= sibling.offsetTop + sibling.offsetHeight / 2;
  });

  const divElements = document.querySelectorAll('div');

  // Convert the NodeList to an array using the spread operator
  const divArray = [...divElements];
  
  // Map over the array to extract the attributes
  const divAttributes = divArray.map(div => div.attributes);
  
  // Find the first div with draggable attribute
  const draggableDiv = divAttributes.find(attrs => attrs.draggable !== undefined);
  
  if(draggableDiv.draggable.value === "true"){
    // Inserting the dragging item before the found sibling
    sortableList.insertBefore(draggingItem, nextSibling);
  }
}

sortableList.addEventListener("dragover", initSortableList);
sortableList.addEventListener("dragenter", e => e.preventDefault());

// ======================== Move Drag Grids End =====================

// ======================== Mouse Btn Hold Options =====================

function WidgetRemove(event) {
  // Your JavaScript function code goes here
  event.preventDefault();
  // var elements = document.getElementsByClassName("removeWidget");
  //     while (elements.length > 0) {
  //       elements[0].parentNode.removeChild(elements[0]);
  //     }
}
// ======================== Mouse Btn Hold Options End =====================

// ======================== On Screen Edit Menu =====================

function EditMenu(event) {

  var editWidgetClass = event.target.closest(".btn-widgetEdit");

  event.preventDefault(); // Prevent the default context menu from showing

  if (editWidgetClass && editWidgetClass.classList.contains("btn-widgetEdit")) {
    RemoveScreenMenu()

    var menu = document.createElement("ul");
    menu.className = "widget-menu";
    menu.style.left = event.clientX + "px";
    menu.style.top = event.clientY + "px";
    var options = ["Edit", "Remove", "Close"];
    options.forEach(function (option, index) {
      var li = document.createElement("li");
      li.id = "WidgetId" + option; // Set a unique id name for each <li> element
      li.textContent = option; // Set the text content directly on the <li> element
      menu.appendChild(li);
    });

    document.body.appendChild(menu);
  } else {

    RemoveScreenMenu()
    var menu = document.createElement("ul");
    menu.className = "widget-menu";
    menu.style.left = event.clientX + "px";
    menu.style.top = event.clientY + "px";
    var options = ["Enable Drag", "Disable Drag", "Close"];
    options.forEach(function (option, index) {
      var li = document.createElement("li");
      if (option === "Enable Drag") {
        li.id = "EnableDragId";
      } else if (option === "Disable Drag") {
        li.id = "DisableDragId";
      } else if (option === "Close") {
        li.id = "DragIdClose";
      }
      li.textContent = option;
      menu.appendChild(li);
    });

    document.body.appendChild(menu);
  }
}

function RemoveScreenMenu() {
  var widgetMenu = document.querySelector('ul.widget-menu');
  if (widgetMenu) {
    widgetMenu.parentNode.removeChild(widgetMenu);
  }
}

// ======================== On Screen Edit Menu End =====================

document.body.addEventListener('click', function (event) {
  var target = event.target;
  if (target.matches('#WidgetIdClose') || target.matches('#DragIdClose')) {
    RemoveScreenMenu()
  }
});

document.body.addEventListener('click', function (event) {
  var target = event.target;
  if (target.matches('#EnableDragId')) {
    var draggableElements = document.querySelectorAll('[draggable="false"]');
    draggableElements.forEach(function (element) {
      element.setAttribute('draggable', 'true');
      element.style.border = '2px solid white'; // Apply CSS border using JavaScript
      element.style.borderRadius = '20px';
    });
    RemoveScreenMenu();
  }


  if (target.matches('#DisableDragId')) {
    var draggableElements = document.querySelectorAll('[draggable="true"]');
    draggableElements.forEach(function (element) {
      element.setAttribute('draggable', 'false');
      element.style.border = ''; // Apply CSS border using JavaScript
      element.style.borderRadius = '';
    });
    RemoveScreenMenu()
  }
});


document.body.addEventListener('click', function (event) {
  var target = event.target;
  if (target.matches('#WidgetIdRemove')) {
    var widgetMenu = document.querySelector('ul.widget-menu');
    RemoveScreenMenu()
  }
});


document.body.addEventListener('click', function (event) {
  var target = event.target;
  if (target.matches('#WidgetIdEdit')) {
    var widgetMenu = document.querySelector('ul.widget-menu');
    RemoveScreenMenu()
  }
});

// $(document).ready(function() {
//   $(".resize-box").mouseenter(function() {
//     $(this).addClass("editWidgetData");
//   }).mouseleave(function() {
//     $(this).removeClass("editWidgetData");
//   });
// });

