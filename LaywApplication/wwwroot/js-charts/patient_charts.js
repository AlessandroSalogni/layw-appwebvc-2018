function createLineChart(urlComplete, divId, beginDate, period, dataTitle, interval) {
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
            }
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
        }
    });
}

function createGridTotalGoal(completeUrl, divId, title) {
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

function createHeartBeatChart(divId, source, down, up, stepX) {
    $("#chart-" + divId).kendoChart({
        dataSource: {
            data: JSON.parse(source)
        },
        title: {
            text: "Heartbeats history"
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "line"
        },
        series: [{
            field: "value",
            categoryField: "heartRateTime"
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
                visible: false
            },
            labels: {
                rotation: "auto",
                step: stepX
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
    $("#line-chart-" + divId).kendoChart({
        dataSource: {
            data: JSON.parse(source)
        },
        title: {
            text: "Heartrate zones"
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
            categoryField: "name",
            color: "yellow" //todo magari mettere colore 
        }],
        valueAxis: {
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
        },
        chartArea: {
            height: 300
        }
    });
}

function createPieChart(divId, source) {
    $("#pie-chart-" + divId).kendoChart({
        dataSource: {
            data: JSON.parse(source)
        },
        title: {
            text: "Heartrate Zones"
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "donut",
            labels: {
                template: "#= category # - #= kendo.format('{0:P}', percentage)#",
                position: "outsideEnd",
                visible: true,
                background: "transparent"
            }
        },
        series: [{
            field: "minutes",
            categoryField: "name"
        }],
        seriesColors: ["#03a9f4", "#ff9800", "#fad84a", "#4caf50"]
    });
}

function createGridTrainingDietDataSource(crudServiceBaseUrl, pageSize) {
    return new kendo.data.DataSource({
        transport: {
            read: {
                url: crudServiceBaseUrl,
                dataType: "json",
                type: "get"
            },
            update: {
                url: crudServiceBaseUrl + "update",
                type: "post",
                dataType: "json",
                contentType: "application/json"
            },
            parameterMap: function (model, operation) {
                if (operation !== "read" && model) {
                    return kendo.stringify(model);
                }
            }
        },
        batch: false,
        pageSize: pageSize,
        schema: {
            model: {
                id: "name",
                fields: {
                    name: { type: "string", editable: false, nullable: false },
                    option1: { type: "string" },
                    option2: { type: "string" },
                    option3: { type: "string" }
                }
            }
        }
    });
}

function createGridGoalsDataSource(crudServiceBaseUrl, pageSize) {
    return dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: crudServiceBaseUrl,
                dataType: "json",
                type: "get"
            },
            update: {
                url: crudServiceBaseUrl + "/update",
                type: "post",
                dataType: "json",
                contentType: "application/json"
            },
            parameterMap: function (model, operation) {
                if (operation !== "read" && model) {
                    return kendo.stringify(model);
                }
            }
        },
        batch: false,
        pageSize: pageSize,
        schema: {
            model: {
                id: "date",
                fields: {
                    goal: { type: "number", editable: true, nullable: false },
                    date: { type: "date", editable: true, nullable: false }
                }
            }
        }
    });
}

function createGrid(divId, scrollable, editableType, dataSource, columns) {
    var tempSavedRecordsTraining = null;

    $("#grid-" + divId).kendoGrid({
        dataSource: dataSource,
        scrollable: scrollable,
        columns: JSON.parse(columns),
        editable: editableType,
        save: function (e) {
            updateTempRecordsTraining('#grid-' + divId);
        },
        cancel: function (e) {
            if (tempSavedRecordsTraining !== null) {
                $('#grid-' + divId).data('kendoGrid').dataSource.data(tempSavedRecordsTraining);
            }
            else {
                $('#grid-' + divId).data('kendoGrid').dataSource.cancelChanges();
            }
        },
        remove: function (e) {
            $('#grid-' + divId).data('kendoGrid').dataSource.remove(e.model);
            updateTempRecordsTraining('#grid-' + divId);
        }
    });
}

function updateTempRecordsTraining(id) {
    tempSavedRecordsTraining = $(id).data('kendoGrid').dataSource.data();
    tempSavedRecordsTraining = tempSavedRecordsTraining.toJSON();
}

function counter(divId) {
    $(divId).each(function () {
        var $this = $(this), countTo = $this.attr('data-count');
        $({
            countNum: $this.text()
        }).animate({
            countNum: countTo
        }, {
                duration: 2500,
                easing: 'swing',
                step: function () {
                    $this.text(Math.floor(this.countNum));
                },
                complete: function () {
                    $this.text(this.countNum);
                }
            });
    });
}