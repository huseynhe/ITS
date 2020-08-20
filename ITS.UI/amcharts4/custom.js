var initChartSerial = function () {

    $.ajax({
        type: "GET",
        url: "/AmReport/GetMachines/",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(result) {
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            var chart = am4core.create("chartdivMachine", am4charts.XYChart);
            chart.padding(40, 40, 40, 40);

            var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.dataFields.category = "name";
            categoryAxis.renderer.minGridDistance = 1;
            categoryAxis.renderer.inversed = true;
            categoryAxis.renderer.grid.template.disabled = true;

            var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
            valueAxis.min = 0;

            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.categoryY = "name";
            series.dataFields.valueX = "count";
            series.tooltipText = "{valueX.value}"
            series.columns.template.strokeOpacity = 0;
            series.columns.template.column.cornerRadiusBottomRight = 5;
            series.columns.template.column.cornerRadiusTopRight = 5;

            var labelBullet = series.bullets.push(new am4charts.LabelBullet())
            labelBullet.label.horizontalCenter = "left";
            labelBullet.label.dx = 10;
            labelBullet.label.text = "{values.valueX.workingValue.formatNumber('#.0as')}";
            labelBullet.locationX = 1;

            // as by default columns of the same series are of the same color, we add adapter which takes colors from chart.colors color set
            series.columns.template.adapter.add("fill", function (fill, target) {
                return chart.colors.getIndex(target.dataItem.index);
            });

            categoryAxis.sortBySeries = series;
            chart.data = result.data;



        }); // end am4core.ready()
    }

    function errorFunc() {

    }

};
var initPieChart3D = function () {

    $.ajax({
        type: "GET",
        url: "/AmReport/PieChart3D/",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(result) {

        am4core.useTheme(am4themes_animated);

        var chart = am4core.create("chartdiv", am4charts.PieChart);

        chart.data = result.data;

     

        var series = chart.series.push(new am4charts.PieSeries());
        series.dataFields.value = "count";
        series.dataFields.category = "name";

        // this creates initial animation
        series.hiddenState.properties.opacity = 1;
        series.hiddenState.properties.endAngle = -90;
        series.hiddenState.properties.startAngle = -90;

        chart.legend = new am4charts.Legend();

    }

    function errorFunc() {

    }

}
var variableheight3DPieChart = function () {
    $.ajax({
        type: "GET",
        url: "/AmReport/Variableheight3DPieChart/",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(result) {

        am4core.useTheme(am4themes_animated);

        var chart = am4core.create("chartdivGender", am4charts.PieChart3D);


        chart.data = result.data;

        chart.innerRadius = am4core.percent(40);
        chart.depth = 90;

        chart.legend = new am4charts.Legend();
        chart.legend.position = "right";

        var series = chart.series.push(new am4charts.PieSeries3D());
        series.dataFields.value = "count";
        series.dataFields.depthValue = "count";
        series.dataFields.category = "name";

    }

    function errorFunc() {

    }
}