
//function createBarChart(urlComplete, divId, title, beginDate, period, goal) {
//    $("#chart-" + divId).kendoChart({
//        dataSource: {
//            transport: {
//                read: {
//                    url: urlComplete,
//                    dataType: "json",
//                    type: "get",
//                    data: {
//                        beginDate: beginDate,
//                        period: period
//                    },
//                }
//            },
//            sort: {
//                field: "day",
//                dir: "asc"
//            },
//        },
//        title: {
//            text: title
//        },
//        legend: {
//            position: "top"
//        },
//        seriesDefaults: {
//            type: "column"
//        },
//        series: [{
//            field: "value",
//            categoryField: "day",
//            name: title
//        }],
//        categoryAxis: {
//            labels: {
//                rotation: -90
//            },
//            majorGridLines: {
//                visible: false
//            }
//        },
//        valueAxis: {
//            labels: {
//            format: "N0"
//        },
//        majorUnit: 1000,
//        plotBands: [{
//            from: 0,
//            to: goal, 
//            color: "#c00",
//            opacity: 0.3
//        }],
//        line: {
//            visible: false
//        }
//    },
//    tooltip: {
//        visible: true,
//        format: "N0"
//        }
//    });
//}

function createLineChart(urlComplete, divId, title, beginDate, period, dataTitle, interval) {
    $("#chart-" + divId).kendoChart({
        dataSource: {
            transport: {
                read: {
                    url: urlComplete,
                    dataType: "json",
                    type: "get",
                    data: {
                        beginDate: beginDate,
                        period: period
                    }
                }
            },
            sort: {
                field: "day",
                dir: "asc"
            }
        },
        title: {
            text: title
        },
        legend: {
            position: "top"
        },
        series: [{
            type: "line",
            field: "value",
            categoryField: "day",
            name: dataTitle
        }, {
            type: "area",
            field: "goal",
            categoryField: "day",
            name: "Goal",
            color: "#ff8080"
        }],
        categoryAxis: {
            labels: {
                rotation: -90
            }
        },
        valueAxis: {
            labels: {
                format: "N0"
            },
            majorUnit: interval
        },
        tooltip: {
            visible: true,
            shared: true,
            format: "N0"
        }
    });
}

$(window).bind("resize", function () {
    $(".resize").data("kendoChart").refresh();
});