
// ======================== gas Loten Chart =====================
var ctx = document.getElementById("gasLotenChart").getContext("2d");
var data = {
  labels: [" ", " ", " ", " ", " ", " ", " "],
  datasets: [{
      label: "Nieuw",
      backgroundColor: "#01a2fd",
      data: [2, 4, 8, 12, 16, 22, 28],
    },
    {
      label: "Gesloten",
      backgroundColor: "#62d73b",
      data: [3, 5, 10, 15, 20, 25, 30],
    }
  ]
};
var gasLotenChart = new Chart(ctx, {
  type: "horizontalBar",
  data: data,
  options: {
    barValueSpacing: 10,
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      yAxes: [{
        gridLines: {
          display: false
        }
      }]
    },
  }
});

// ======================== gas Loten Chart End =====================
