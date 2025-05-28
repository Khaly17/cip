"use strict";
$(document).ready(
    function() {
        function draw_pareto(div) {
            // Create chart instance
            var chart = am4core.create(div, am4charts.XYChart);

            // Add data
            chart.data = [{
                "Name": "Lyon",
                "Value": 8
            }, {
                "Name": "Marseille",
                "Value": 6
            }, {
                "Name": "Nantes",
                "Value": 3
            }, {
                "Name": "Rennes",
                "Value": 1
            }];
            chart.colors.list = [
                am4core.color("#0B2850"),
                am4core.color("#0e3366"),
                am4core.color("#113e7d"),
                am4core.color("#144a93")
            ];

            prepareParetoData();

            function prepareParetoData() {
                var total = 0;

                for (var i = 0; i < chart.data.length; i++) {
                    total += chart.data[i].Value;
                }

                var sum = 0;
                for (var i = 0; i < chart.data.length; i++) {
                    sum += chart.data[i].Value;
                    chart.data[i].pareto = sum / total * 100;
                }
            }

            // Create axes
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "Name";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 60;
            categoryAxis.tooltip.disabled = true;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.renderer.minWidth = 50;
            valueAxis.min = 0;
            valueAxis.cursorTooltipEnabled = false;

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.sequencedInterpolation = true;
            series.dataFields.valueY = "Value";
            series.dataFields.categoryX = "Name";
            series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
            series.columns.template.strokeWidth = 0;

            series.tooltip.pointerOrientation = "vertical";

            series.columns.template.column.cornerRadiusTopLeft = 10;
            series.columns.template.column.cornerRadiusTopRight = 10;
            series.columns.template.column.fillOpacity = 1;

            // on hover, make corner radiuses bigger
            var hoverState = series.columns.template.column.states.create("hover");
            hoverState.properties.cornerRadiusTopLeft = 0;
            hoverState.properties.cornerRadiusTopRight = 0;
            hoverState.properties.fillOpacity = 1;

            series.columns.template.adapter.add("fill", (fill, target) => {
                return chart.colors.getIndex(target.dataItem.index);
            })


            var paretoValueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            paretoValueAxis.renderer.opposite = true;
            paretoValueAxis.min = 0;
            paretoValueAxis.max = 100;
            paretoValueAxis.strictMinMax = true;
            paretoValueAxis.renderer.grid.template.disabled = true;
            paretoValueAxis.numberFormatter = new am4core.NumberFormatter();
            paretoValueAxis.numberFormatter.numberFormat = "#'%'"
            paretoValueAxis.cursorTooltipEnabled = false;

            var paretoSeries = chart.series.push(new am4charts.LineSeries())
            paretoSeries.dataFields.valueY = "pareto";
            paretoSeries.dataFields.categoryX = "Name";
            paretoSeries.yAxis = paretoValueAxis;
            paretoSeries.tooltipText = "pareto: {valueY.formatNumber('#.0')}%[/]";
            paretoSeries.bullets.push(new am4charts.CircleBullet());
            paretoSeries.strokeWidth = 2;
            paretoSeries.stroke = new am4core.InterfaceColorSet().getFor("alternativeBackground");
            paretoSeries.strokeOpacity = 0.5;

            // Cursor
            chart.cursor = new am4charts.XYCursor();
            chart.cursor.behavior = "panX";

        }
        draw_pareto("sec-ecommerce-chart-bar1");
        draw_pareto("sec-ecommerce-chart-bar2");
        draw_pareto("sec-ecommerce-chart-bar3");
        draw_pareto("sec-ecommerce-chart-bar4");
    }
);
