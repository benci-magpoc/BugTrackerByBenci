// -------------------------------------------------------------------------------------------------------------------------------------------
// Dashboard 4 : Chart Init Js
// -------------------------------------------------------------------------------------------------------------------------------------------

// -----------------------------------------------------------------------
// Minimal Demo Dashboard Init Js
// -----------------------------------------------------------------------
$(function () {
    "use strict";

    // -----------------------------------------------------------------------
    // PRODUCTS YEARLY SALES Charts
    // -----------------------------------------------------------------------

    var option_Products_Yearly_Sales = {
        series: [
            {
                type: "area",
                name: "Mac",
                data: [5, 2, 7, 4, 5, 3, 5, 4],
            },
            {
                type: "area",
                name: "Windows",
                data: [2, 5, 2, 6, 2, 5, 2, 4],
            },
        ],
        chart: {
            fontFamily: 'Nunito Sans,sans-serif',
            height: 360,
            type: "line",
            toolbar: {
                show: false,
            },
        },
        grid: {
            show: true,
            borderColor: "rgba(0,0,0,.1)",
            strokeDashArray: 3,
            xaxis: {
                lines: {
                    show: true,
                },
            },
            yaxis: {
                lines: {
                    show: true,
                },
            },
        },
        colors: ["#2cabe3", "#8898aa"],
        fill: {
            type: 'gradient',
            opacity: 0.5,
            gradient: {
                shade: 'light',
                type: "vertical",
                shadeIntensity: 0.5,
                gradientToColors: undefined, // optional, if not defined - uses the shades of same color in series
                inverseColors: true,
                opacityFrom: 0.5,
                opacityTo: 0.3,
                stops: [0, 50, 100],
                colorStops: []
              }
        },
        dataLabels: {
            enabled: false,
        },
        stroke: {
            curve: "smooth",
            width: 2,
        },
        markers: {
            size: 5,
            strokeColors: "transparent",
        },
        xaxis: {
            categories: ['2008', '2009', '2010', '2011', '2012', '2013', '2014', '2015'],
            labels: {
                style: {
                    colors: "#a1aab2",
                },
            },
        },
        yaxis: {
            seriesName: ['1k'],
            labels: {
                style: {
                    colors: "#a1aab2",
                },
            },
        },
        tooltip: {
            x: {
                format: "dd/MM/yy HH:mm",
            },
            theme: "dark",
        },
        legend: {
            show: false,
        },
    };

    var chart_area_spline = new ApexCharts(document.querySelector("#products-yearly-sales"), option_Products_Yearly_Sales);
    chart_area_spline.render();


    // 1) First 4 Card Charts
    var option_Total_Visit = {
        series: [{
            name: '',
            data: [5, 6, 10, 9, 12, 4, 9]
        }
        ],
        chart: {
            type: 'bar',
            height: 30,
            offsetX: -10,
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        colors: ["#53e69d"],
        grid: {
            show: false,
        },
        plotOptions: {
            bar: {
                horizontal: false,
                startingShape: 'flat',
                endingShape: 'flat',
                columnWidth: '95%',
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
                fontFamily: '"Nunito Sans",sans-serif',
            },
            x: {
                show: false,
            },
            y: {
                formatter: undefined,
            }
        }
    };

    var chart_column_basic = new ApexCharts(document.querySelector("#total-visit"), option_Total_Visit);
    chart_column_basic.render();

    // second chart 
    var option_Total_Page_Views = {
        series: [{
            name: '',
            data: [5, 6, 10, 9, 12, 4, 9]
        }
        ],
        chart: {
            type: 'bar',
            height: 30,
            offsetX: -10,
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        colors: ["#7460ee"],
        grid: {
            show: false,
        },
        plotOptions: {
            bar: {
                horizontal: false,
                startingShape: 'flat',
                endingShape: 'flat',
                columnWidth: '95%',
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
                fontFamily: '"Nunito Sans",sans-serif',
            },
            x: {
                show: false,
            },
            y: {
                formatter: undefined,
            }
        }
    };

    var chart_column_basic = new ApexCharts(document.querySelector("#total-page-views"), option_Total_Page_Views);
    chart_column_basic.render();

    // 3rd chart
    var option_Unique_Visitor = {
        series: [{
            name: '',
            data: [5, 6, 10, 9, 12, 4, 9]
        }
        ],
        chart: {
            type: 'bar',
            height: 30,
            offsetX: -10,
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        colors: ["#11a0f8"],
        grid: {
            show: false,
        },
        plotOptions: {
            bar: {
                horizontal: false,
                startingShape: 'flat',
                endingShape: 'flat',
                columnWidth: '95%',
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
                fontFamily: '"Nunito Sans",sans-serif',
            },
            x: {
                show: false,
            },
            y: {
                formatter: undefined,
            }
        }
    };

    var chart_column_basic = new ApexCharts(document.querySelector("#unique-visitor"), option_Unique_Visitor);
    chart_column_basic.render();


    // 4th chart
    var option_Bounce_Rate = {
        series: [{
            name: '',
            data: [5, 6, 10, 9, 12, 4, 9]
        }
        ],
        chart: {
            type: 'bar',
            height: 30,
            offsetX: -10,
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        colors: ["#ff7676"],
        grid: {
            show: false,
        },
        plotOptions: {
            bar: {
                horizontal: false,
                startingShape: 'flat',
                endingShape: 'flat',
                columnWidth: '95%',
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
                fontFamily: '"Nunito Sans",sans-serif',
            },
            x: {
                show: false,
            },
            y: {
                formatter: undefined,
            }
        }
    };

    var chart_column_basic = new ApexCharts(document.querySelector("#bounce-rate"), option_Bounce_Rate);
    chart_column_basic.render();

});