// -------------------------------------------------------------------------------------------------------------------------------------------
// Dashboard 2 : Chart Init Js
// -------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    "use strict";

    // -----------------------------------------------------------------------
    // Expance Charts
    // -----------------------------------------------------------------------

    var data = [],
        totalPoints = 300;

    function getRandomData() {
        if (data.length > 0) data = data.slice(1);
        // Do a random walk
        while (data.length < totalPoints) {
            var prev = data.length > 0 ? data[data.length - 1] : 50,
                y = prev + Math.random() * 10 - 5;
            if (y < 0) {
                y = 0;
            } else if (y > 100) {
                y = 100;
            }
            data.push(y);
        }
        // Zip the generated y values with the x values 
        var res = [];
        for (var i = 0; i < data.length; ++i) {
            res.push([i, data[i]])
        }
        return res;
    }

    var updateInterval = 30;
    $("#updateInterval").val(updateInterval).change(function () {
        var v = $(this).val();
        if (v && !isNaN(+v)) {
            updateInterval = +v;
            if (updateInterval < 1) {
                updateInterval = 1;
            } else if (updateInterval > 3000) {
                updateInterval = 3000;
            }
            $(this).val("" + updateInterval);
        }
    });

    var plot = $.plot("#expance", [getRandomData()], {
        series: {
            shadowSize: 0 // Drawing is faster without shadows
        },
        yaxis: {
            min: 0,
            max: 100
        },
        xaxis: {
            show: false
        },
        colors: ["#fff"],
        grid: {
            color: "rgba(255, 255, 255, 0.0)",
            hoverable: true,
            borderWidth: 0
        },
        tooltip: true,
        tooltipOpts: {
            content: "Y: %y",
            defaultTheme: false
        }
    });

    function update() {
        plot.setData([getRandomData()]);
        // Since the axes don't change, we don't need to call plot.setupGrid()
        plot.draw();
        setTimeout(update, updateInterval);
    }

    $(window).resize(function () {
        $.plot($('#expance'), [getRandomData()]);
    });

    update();

    // -----------------------------------------------------------------------
    // Sales Charts
    // -----------------------------------------------------------------------

    var option_Sales = {
        series: [35, 15, 15, 105],
        labels: ["Mar", "Feb", "Jan", "Apr"],
        chart: {
            type: 'donut',
            height: 270,
            fontFamily: '"Nunito Sans",sans-serif',
        },
        dataLabels: {
            enabled: false,
        },
        fill: {
            opacity: 1
        },
        plotOptions: {
            pie: {
                expandOnClick: true,
                startAngle: 40,
                donut: {
                    size: '70',
                    labels: {
                        show: true,
                        name: {
                            show: true,
                            offsetY: -10,
                            fontSize: '30px',
                            fontFamily: 'Arial',
                            fontWeight: 500,
                            color: '#8898aa',
                        },
                        value: {
                            show: true,
                            fontSize: '18px',
                            fontFamily: 'Arial',
                            fontWeight: 400,
                            color: '#8898aa',
                        },
                        total: {
                            show: true,
                            label: 'Apr',
                            fontSize: '30px',
                            fontFamily: 'Arial',
                            fontWeight: 500,
                            color: '#8898aa',
                        }
                    },
                },
            },
        },
        colors: ['#53e69d', '#2cabe3', '#ff7676', '#7bcef3'],
        tooltip: {
            show: true,
            theme: "dark",
            fillSeriesColor: false 
        },
        legend: {
            show: false
        },
        responsive: [{
            breakpoint: 426,
            options: {
                chart: {
                    height: 250,
                    width: 250,
                    offsetX: -20
                },
            }
        }]
    };

    var chart_pie_donut = new ApexCharts(document.querySelector("#sales"), option_Sales);
    chart_pie_donut.render();

    // -----------------------------------------------------------------------
    // Guage Chart
    // -----------------------------------------------------------------------

    var options_Finance = {
        series: [45, 53, 80, 90, 95],
        chart: {
            height: 300,
            type: 'radialBar',
        },
        colors: ['#edebee', '#bedbe9', '#88b8e6', '#ff7676', '#53e69d'],
        plotOptions: {
            radialBar: {
                dataLabels: {
                    name: {
                        offsetY: 9,
                        fontSize: '22px',
                    },
                    value: {
                        fontSize: '16px',
                    },
                    total: {
                        show: true,
                        label: 'Sales',
                        color: '#313131',
                        fontSize: '20px',
                        fontFamily: 'Nunito Sans,sans-serif',
                        fontWeight: 400,
                        formatter: function () {
                        }
                    }
                },
                track: {
                    show: true,
                    startAngle: 270,
                    background: 'transparent',
                    strokeWidth: '0',
                    opacity: 1,
                    margin: 0,
                },
                hollow: {
                }
            }
        },
        labels: ['Dec', 'Nov', 'Oct', 'Sep', 'Aug'],
        tooltip: {
            enabled: true,
            theme: "dark",
            fillSeriesColor: false,
        },
        responsive: [{
            breakpoint: 426,
            options: {
                chart: {
                    height: 250,
                    width: 250,
                    offsetX: -20
                },
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#finance"), options_Finance);
    chart.render();


    // -----------------------------------------------------------------------
    // ct-main-balance-chart Charts
    // -----------------------------------------------------------------------

    // total sales
    var option_Total_Income = {
        series: [{
            name: '',
            data: [6, 9, 6, 9, 0.5, 6, 12]
        }
        ],
        chart: {
            type: 'bar',
            height: 55,
            offsetX: 7,
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        colors: ["#2cabe3"],
        grid: {
            show: false,
        },
        plotOptions: {
            bar: {
                horizontal: false,
                startingShape: 'flat',
                endingShape: 'flat',
                columnWidth: '80%',
                barHeight: '100%',
            },
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 4,
            colors: ['transparent']
        },
        xaxis: {
            axisBorder: {
                show: false,
            },
            axisTicks: {
                show: false,
            },
            labels: {
                show: false,
            },
        },
        yaxis: {
            labels: {
                show: false,
            },
        },
        axisBorder: {
            show: false,
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            theme: "dark",
            style: {
                fontSize: '12px',
                fontFamily: '"Nunito Sans", sans- serif',
            },
            x: {
                show: false,
            },
            y: {
                formatter: undefined,
            }
        }
    };

    var chart_column_basic = new ApexCharts(document.querySelector("#total-income"), option_Total_Income);
    chart_column_basic.render();


    // -----------------------------------------------------------------------
    // monthly sales Charts
    // -----------------------------------------------------------------------

    // yearly sales
    var options_Yearly_Sales = {
        series: [{
            name: 'January',
            data: [1, 2, 5, 3, 4, 2.5, 5, 3, 1]
        }, {
            name: 'February',
            data: [1, 4, 2, 5, 2, 5.5, 3, 4, 1]
        }],
        chart: {
            height: 50,
            type: 'area',
            sparkline: {
                enabled: true
            },
        },
        colors: ['rgba(44, 171, 227, 0.2)', 'rgba(44, 171, 227, 0.5)'],
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 1,
            dashArray: 3,
        },
        fill: {
            type: 'solid',
            opacity: 1,
        },
        xaxis: {
            show: false
        },
        tooltip: {
            x: {
                show: false
            },
            theme: 'dark',
            style: {
                fontSize: '12px',
                fontFamily: '"Nunito Sans", sans- serif',
            },
        },
        legend: {
            show: false
        }
    };

    var chart = new ApexCharts(document.querySelector("#yearly-sales"), options_Yearly_Sales);
    chart.render();

    // monthly sales
    var options_Monthly_Sales = {
        series: [{
            name: "",
            data: [1, -2, 5, 3, 0, 2.5]
        }],
        chart: {
            height: 60,
            type: 'line',
            zoom: {
                enabled: false
            },
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            }
        },
        dataLabels: {
            enabled: false
        },
        colors: ["#2cabe3"],
        stroke: {
            curve: 'smooth',
            width: 4,
            dashArray: 3,
        },
        markers: {
            size: 4,
            strokeColors: 'transparent',
        },
        grid: {
            show: true,
            borderColor: 'rgba(0,0,0,0.1)',
            strokeDashArray: 3,
            xaxis: {
                lines: {
                    show: false
                },
            },
            yaxis: {
                lines: {
                    show: true
                }
            },
        },
        xaxis: {
            axisBorder: {
                show: true,

            }
        },
        tooltip: {
            theme: "dark"
        }
    };

    var chart = new ApexCharts(document.querySelector("#monthly-sales"), options_Monthly_Sales);
    chart.render();

    // weeksales-bar
    var options_Weekly_Usage = {
        series: [{
            name: "",
            data: [50, 40, 30, 70, 50, 20, 30]
        }],
        chart: {
            type: 'bar',
            height: 350,
            fontFamily: 'Nunito Sans,sans-serif',
            toolbar: {
                show: false,
            },
        },
        grid: {
            show: false,
        },
        colors: ['#2cabe3'],
        dataLabels: {
            enabled: false
        },
        fill: {
            opacity: 1
        },
        plotOptions: {
            bar: {
                columnWidth: '30%',
                barHeight: '100%',
            },
        },
        stroke: {
            show: true,
            colors: 'transparent',
            width: 8,
        },
        xaxis: {
            type: 'category',
            categories: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
            labels: {
                show: true,
                fontFamily: 'Nunito Sans,sans-serif',
                fontSize: '18px',
                style: {
                    colors: '#a1aab2',
                },
                opacity: 1,
                fontWeight: 800,
                offsetX: 0,
            },
            axisBorder: {
                show: false,
            },
            axisTicks: {
                show: false,
            }
        },
        yaxis: {
            show: false,
        },
        tooltip: {
            theme: "dark"
        },
        responsive: [{
            breakpoint: 426,
            options: {
                stroke: {
                    width: 3,
                },
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#weekly-usage"), options_Weekly_Usage);
    chart.render();

    // meter chart
    var option_Mothly_Usage = {
        chart: {
            height: 400,
            type: "radialBar",
            fontFamily: 'Nunito Sans,sans-serif',
            sparkline: {
                enabled: true
            }
        },
        series: [60],
        colors: ["#2cd07e"],
        plotOptions: {
            radialBar: {
                startAngle: -90,
                endAngle: 90,
                track: {
                    background: '#E0E0E0',
                    startAngle: -90,
                    endAngle: 90,
                },
                hollow: {
                    size: '50%',
                },
                dataLabels: {
                    name: {
                        show: false,
                    },
                    value: {
                        show: false
                    }
                }
            }
        },
        fill: {
            type: "solid",
        },
        stroke: {
            lineCap: "butt"
        },
        labels: ["Progress"],
        tooltip: {
            enabled: true,
            theme: "dark",
            fillSeriesColor: false,

        }
    };

    new ApexCharts(document.querySelector("#monthly-usage"), option_Mothly_Usage).render();

});