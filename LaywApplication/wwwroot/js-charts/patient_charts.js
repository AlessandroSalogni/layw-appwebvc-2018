
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

function createDonutChart(completeUrl, divId) {
    $("#chart-" + divId).kendoChart({
        dataSource: {
            transport: {
                read: {
                    url: completeUrl,
                    type: "get",
                    dataType: "json"
                }
            },
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "donut"
        },
        series: [{
            field: "amount",
            categoryField: "category",
            padding: 15
        }],
        tooltip: {
            visible: true,
            format: "N0",
            template: "#= category # - #= kendo.format('{0:P}', percentage)#"
        },
        chartArea: {
            margin: 1,
            height: 300
        },
    });
}

function createGrid(completeUrl, divId, title) {
    $("#grid-" + divId).kendoGrid({
        dataSource: {
            transport: {
                read: {
                    dataType: "json",
                    url: completeUrl,
                    type: "get"
                }
            },
            pageSize: 20,
            schema: {
                model: {
                    fields: {
                        name: { type: "string" }
                    }
                }
            }
        },
        height: 350,
        sortable: true,
        filterable: true,
        pageable: true,
        columns: [{
            field: "name",
            title: title
        }]
    });
}

function createWindow(divId, title) {
    var myWindow = $("#window-" + divId),
        undo = $("#undo-" + divId);

    undo.click(function () {
        myWindow.data("kendoWindow").open();
        undo.fadeOut();
    });

    function onClose() {
        undo.fadeIn();
    }

    myWindow.kendoWindow({
        width: "750px",
        title: title,
        visible: false,
        actions: [
            "Pin",
            "Minimize",
            "Close"
        ],
        close: onClose
    }).data("kendoWindow").center();
}

function createTabStrip(divId) {
    $("#tabstrip-" + divId).kendoTabStrip({
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });
}

function createHeartBeatChart(divId, source, down, up) {
    var s = JSON.parse(JSON.stringify(source));
    $("#chart-" + divId).kendoChart({
        dataSource: {
            data: s
        },
        title: {
            text: ""
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "line"
        },
        series: [{
            field: "value",
            categoryField: "time",
            title: "ac"
        }],
        valueAxis: {
            labels: {
                format: "{0}"
            },
            line: {
                visible: false
            },
            plotBands: [{
                from: down,
                to: up,
                color: "green",
                opacity: 0.3
            },
            {
                from: up,
                to: 220,
                color: "red",
                opacity: 0.3
            },
            {
                from: 0,
                to: down,
                color: "red",
                opacity: 0.3
            }
            ],
            axisCrossingValue: -10
        },
        categoryAxis: {
            majorGridLines: {
                visible: true
            },
            labels: {
                rotation: "auto"
            },
            line: {
                visibile: false
            }
        },
        tooltip: {
            visible: true,
            template: "#= category # #= value #"
        }
    });
}

function createLineZonesChart(divId, source) {
    var s = JSON.parse(JSON.stringify(source));
    $("#line-chart-" + divId).kendoChart({
        dataSource: {
            data: s
        },
        title: {
            //align: "left",
            //text: "Comments per day"
        },
        legend: {
            visible: false
        },
        seriesDefaults: {
            type: "column",
            labels: {
                visible: true,
                background: "transparent"
            }
        },
        series: [{
            field: "minutes",
            categoryField: "name"
        }],
        valueAxis: {
            max: 28,
            majorGridLines: {
                visible: false
            },
            visible: false
        },
        categoryAxis: {
            majorGridLines: {
                visible: false
            },
            line: {
                visible: false
            }
        }
    });
}