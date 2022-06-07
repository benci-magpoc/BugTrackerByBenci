// -------------------------------------------------------------------------------------------------------------------------------------------
// Dashboard 1 : Chart Init Js
// -------------------------------------------------------------------------------------------------------------------------------------------
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
            height: 300,
            type: "line",
            toolbar: {
                show: false,
            },
        },
        colors: ["#ff5050", "#2cabe3"],
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
            axisBorder: {
                show: false
            },
            categories: ['2008', '2009', '2010', '2011', '2012', '2013', '2014', '2015'],
            labels: {
                style: {
                    colors: "#a1aab2",
                },
            },
        },
        yaxis: {
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


    // // -----------------------------------------------------------------------
    // // Week Sales Chart
    // // -----------------------------------------------------------------------
    var option_Week_Sales = {
        series: [{
            name: '',
            data: [5, 4, 3, 6, 5, 2, 3]
        }
        ],
        chart: {
            type: 'bar',
            width: 300,
            height: 255,
            fontFamily: 'Nunito Sans,sans-serif',
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
            offsetY: 49
        },
        colors: ["rgba(255,255,255,0.7)"],
        grid: {
            show: false,
        },
        plotOptions: {
            bar: {
                horizontal: false,
                startingShape: 'flat',
                endingShape: 'flat',
                columnWidth: '35%',
                barHeight: '100%',
            },
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 0,
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

    var chart_column_crypto2 = new ApexCharts(document.querySelector("#week-sales"), option_Week_Sales);
    chart_column_crypto2.render();



    // // -----------------------------------------------------------------------
    // // Sunday
    // // -----------------------------------------------------------------------

    var option_Wallet_Balance = {
        series: [{
            name: "Site A ",
            type: 'area',
            data: [50, 160, 110, 60, 130, 200, 100]
        }, {
            name: "Site B ",
            type: 'area',
            data: [0, 100, 60, 200, 150, 90, 150]
        }],
        chart: {
            type: 'line',
            height: 70,
            fontFamily: 'Nunito Sans,sans-serif',
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
        colors: ['#79e580', '#2cabe3'],
        stroke: {
            curve: 'smooth',
            lineCap: 'round',
            width: 2,
            colors: ['#79e580', '#2cabe3']
        },
        xaxis: {
            categories: ['2010', '2011', '2012', '2013', '2014', '2015', '2016'],
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
        markers: {
            size: 0
        },
        fill: {
            type: 'solid',
            colors: ['#79e580', '#2cabe3'],
            opacity: 0.1
        },
        tooltip: {
            theme: "dark",
            style: {
                fontSize: '13px',
                fontFamily: 'Nunito Sans,sans-serif',
            },
            colors: ['#79e580', '#2cabe3'],
            x: {
                show: true,
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

    var chart_area_basic = new ApexCharts(document.querySelector("#wallet-balance"), option_Wallet_Balance);
    chart_area_basic.render();


    // // -----------------------------------------------------------------------
    // // Minimal Demo Dashboard Init Js
    // // -----------------------------------------------------------------------



});