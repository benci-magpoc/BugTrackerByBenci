// -------------------------------------------------------------------------------------------------------------------------------------------
// Dashboard 3 : Chart Init Js
// -------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    "use strict";

    // -----------------------------------------------------------------------
    // Expance Charts
    // -----------------------------------------------------------------------

    var option_Monthly_Site_Traffic = {
        series: [{
            name: "",
            data: [2, 4, 4, 6, 8, 5, 6, 4, 8, 6, 6, 2]
        }],
        chart: {
            type: 'area',
            height: 50,
            fontFamily: '"Nunito Sans",sans-serif',
            zoom: {
                enabled: false
            },
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        grid: {
            show: false,
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'straight',
            width: 1,
            colors: ["rgba(255,255,255,.2)"]
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
        colors: ["#ff7676"],
        markers: {
            size: 1,
            strokeColors: 'transparent',
            strokeWidth: 2,
            shape: "circle",
            colors: ["#ff7676"],
        },
        fill: {
            type: 'solid',
            colors: ['#ff7676'],
            opacity: 1
        },
        tooltip: {
            theme: "dark",
            style: {
                fontSize: '8px',
                fontFamily: '"Nunito Sans",sans-serif',
            },
            x: {
                show: false,
            },
            y: {
                formatter: undefined,
            },
            marker: {
                show: true,
            },
            followCursor: true,
        },
        legend: {
            show: false
        }
    };

    var chart_area_basic = new ApexCharts(document.querySelector("#monthly-site-traffic"), option_Monthly_Site_Traffic);
    chart_area_basic.render();

    var option_Weekly_Site_Traffic = {
        series: [{
            name: "",
            data: [0, 2, 8, 6, 8, 5, 6, 4, 8, 6, 6, 2]
        }],
        chart: {
            type: 'area',
            height: 50,
            zoom: {
                enabled: false
            },
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        grid: {
            show: false,
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'straight',
            width: 1,
            colors: ["rgba(255,255,255,.2)"]
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
        colors: ["#2cabe3"],
        markers: {
            size: 1,
            strokeColors: 'transparent',
            strokeWidth: 2,
            shape: "circle",
            colors: ["#2cabe3"],
        },
        fill: {
            type: 'solid',
            colors: ['#2cabe3'],
            opacity: 1
        },
        tooltip: {
            theme: "dark",
            style: {
                fontSize: '8px',
                fontFamily: '"Nunito Sans",sans-serif',
            },
            x: {
                show: false,
            },
            y: {
                formatter: undefined,
            },
            marker: {
                show: true,
            },
            followCursor: true,
        },
        legend: {
            show: false
        }
    };

    var chart_area_basic = new ApexCharts(document.querySelector("#weekly-site-traffic"), option_Weekly_Site_Traffic);
    chart_area_basic.render();


    // -----------------------------------------------------------------------
    // ct-weather
    // -----------------------------------------------------------------------

    var option_Weather_Report = {
        series: [{
            name: "",
            labels: [],
            data: [5, 2, 7, 4, 5, 3, 5, 4]
        }],
        chart: {
            height: 200,
            type: 'line',
            toolbar: {
                show: false
            },
            sparkline: {
                enabled: false
            },
        },
        fill: {
            type: 'solid',
            opacity: 0.5,
            colors: ['rgba(255,255,255,0.2)']
        },
        colors: ["rgba(255,255,255,0.2)"],
        grid: {
            show: false,
        },
        stroke: {
            curve: 'smooth',
            lineCap: 'square',
            colors: ['rgba(255,255,255,0.2)'],
            width: 4,
        },
        markers: {
            size: 5,
            colors: ['rgba(255,255,255,0.2)'],
            strokeColors: 'transparent',
            shape: 'square',
        },
        xaxis: {
            categories: ['12AM', '2AM', '6AM', '9AM', '12AM', '3PM', '6PM', '9PM'],
            axisBorder: {
                show: false,
            },
            axisTicks: {
                show: false,
            },
            labels: {
                show: true,
                offsetY: 5,
                style: {
                    colors: "#a1aab2",
                },
            },
        },
        fill: {
            type: 'solid',
            colors: ['#FDD835'],
        },
        yaxis: {
            labels: {
                show: false,
            },
        },
        tooltip: {
            theme: "dark",
            style: {
                fontSize: '10px',
                fontFamily: '"Nunito Sans",sans-serif',
            },
            x: {
                show: false
            },
            y: {
                formatter: undefined,
            },
            marker: {
                show: true,
            },
            followCursor: true,
        },
    };

    var chart_line_basic = new ApexCharts(document.querySelector("#weather-report"), option_Weather_Report);
    chart_line_basic.render();

    // country visit
    $('#country-visit').vectorMap({
          map : 'us_aea_en',
          backgroundColor : 'transparent',
          zoomOnScroll: false,
          regionStyle : {
              initial : {
                  fill : '#2cabe3'
              }
          }
        });

});