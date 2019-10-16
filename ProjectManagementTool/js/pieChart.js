if (typeof (google) != "undefined") {
    google.load("visualization", "1", { packages: ["corechart"] });
    //  google.setOnLoadCallback(drawChart);
}

var data;
var options;
var chart;

function drawChart(pieChartArray) {
    //var data = google.visualization.arrayToDataTable([
    //    ['Task', 'Hours per Day'],
    //    ['Open', 11],
    //    ['Pending', 2],
    //    ['Closed', 2]
    //]);
    data = google.visualization.arrayToDataTable(pieChartArray);
    options = {
        title: 'My Daily Activities',
        width: '100%',
        height: '100%'
    };

    chart = new google.visualization.PieChart(document.getElementById('piechart'));
    if (chart !== undefined) {
        chart.draw(data, options);
    }

}
$(window).resize(function () { resize(); });
$(window).on("load", function () { resize(); });
function resize() {
    if (chart !== undefined) {
        setTimeout(function () {
            chart.draw(data, options);
        }, 500);
    }
}



