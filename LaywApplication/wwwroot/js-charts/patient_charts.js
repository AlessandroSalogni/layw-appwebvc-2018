
function createBarChart(urlComplete, divId, title, beginDate, period, goal) {
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
                    },
                }
            },
            sort: {
                field: "day",
                dir: "asc"
            },
        },
        title: {
            text: title
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "column"
        },
        series:
        [{
            field: "value",
            categoryField: "day",
            name: title
        }],
        categoryAxis: {
            labels: {
                rotation: -90
            },
                majorGridLines: {
                visible: false
            }
        },
        valueAxis: {
            labels: {
            format: "N0"
        },
        majorUnit: 1000,
        plotBands: [{
            from: 0,
            to: goal, 
            color: "#c00",
            opacity: 0.3
        }],
        line: {
            visible: false
        }
    },
    tooltip: {
        visible: true,
        format: "N0"
        }
    });
}

function createLineChart(urlComplete, divId, title, beginDate, period, goal) {
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
        seriesDefaults: {
            type: "line"
        },
        series: [{
            field: "value",
            categoryField: "day",
            name: title
        }],
        categoryAxis: {
            labels: {
                rotation: -90
            },
            crosshair: {
                visible: true
            }
        },
        valueAxis: {
            labels: {
                format: "N0"
            },
            majorUnit: 220,
            plotBands: [
                {
                    from: 0,
                    to: goal,
                    color: "#00FFFF",
                    opacity: 0.3
                }],
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

$(window).bind("resize", function () {
    $(".resize").data("kendoChart").refresh();
});