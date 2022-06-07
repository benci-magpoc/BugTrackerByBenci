// -------------------------------------------------------------------------------------------------------------------------------------------
// Dashboard 6 : Chart Init Js
// -------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    "use strict";

    // -----------------------------------------------------------------------
    // ct-weather
    // -----------------------------------------------------------------------

    var options_Top_Selling = {
        series: [{
            name: "Item 1 ",
            data: [2666, 2778, 4912, 3767, 6810, 5670, 4820, 15073, 10687, 8432]
        }],
        chart: {
            height: 390,
            fontFamily: '"Nunito Sans",sans-serif',
            type: 'line',
            zoom: {
                enabled: false
            },
            toolbar: {
                show: false
            }
        },
        colors: ['#2962ff'],
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 1
        },
        markers: {
            size: 3,
            colors: '#2962ff',
            strokeColors: 'transparent',
            strokeWidth: 0,
        },
        grid: {
            show: true,
            borderColor: 'rgba(0,0,0,0.1)',
            xaxis: {
                lines: {
                    show: false
                }
            },
            yaxis: {
                lines: {
                    show: true,
                    width: 1,
                    opacity: 0.2
                }
            },
        },
        xaxis: {
            axisBorder: {
                show: false
            },
            axisTicks: {
                color: "rgba(0,0,0,0.5)"
            },
            categories: ['2012', '2013' ,'2014', '2015', '2016', '2017', '2018', '2019', '2020', '2021'],
            labels: {
                show: true,
                style: {
                    colors: '#a1aab2',
                }
            },
        },
        yaxis: {
            labels: {
                style: {
                    colors: '#a1aab2',
                }
            }
        },
        tooltip: {
            theme: 'dark',
            x: {
                show: false
            }
        }
    };

    var chart = new ApexCharts(document.querySelector("#top-selling"), options_Top_Selling);
    chart.render();

    //Top earnings

    var options_Top_Earning = {
        series: [
            {
                name: "Max temp",
                data: [5, 15, 11, 15, 12, 13, 10]
            },
            {
                name: "Min temp",
                data: [1, -2, 2, 5, 3, 2, 0]
            }
        ],
        chart: {
            height: 350,
            fontFamily: 'Nunito Sans,sans-serif',
            type: 'line',
            toolbar: {
                show: false
            }
        },
        colors: ['#2962FF', '#ff7676'],
        dataLabels: {
            enabled: false,
        },
        stroke: {
            curve: 'straight',
            width: 3
        },
        grid: {
            borderColor: 'rgba(0,0,0,0.2)',
            
        },
        markers: {
            size: 2,
            colors: 'transparent',
            strokeColors: ['#2962FF', '#ff7676'],
            strokeWidth: 2,
        },
        xaxis: {
            categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            labels: {
                style: {
                    colors: '#a1aab2',
                }
            }
        },
        yaxis: {
            tickAmount: 4,
            floating: false,

            labels: {
                style: {
                    colors: '#a1aab2',
                },
            },
            axisBorder: {
                show: false,
            },
            axisTicks: {
                show: false
            }
        },
        legend: {
            position: 'top',
            horizontalAlign: 'center',
            floating: true,
            labels: {
                  colors: '#a1aab2',
              },
        },
        tooltip: {
            theme: "dark"
        }
    };

    var chart = new ApexCharts(document.querySelector("#top-earning"), options_Top_Earning);
    chart.render();

    // -----------------------------------------------------------------------
    // Badnwidth usage
    // -----------------------------------------------------------------------
    var option_Bandwidth_Usage = {
        series: [{
            name: "",
            labels: [],
            data: [5, 0, 12, 1, 8, 3, 12, 15]
        }],
        chart: {
            height: 100,
            type: 'line',
            toolbar: {
                show: false
            },
            sparkline: {
                enabled: true
            },
        },
        fill: {
            type: 'solid',
            opacity: 1,
            colors: ['#fff']
        },
        colors: ["#fff"],
        grid: {
            show: false,
        },
        stroke: {
            curve: 'smooth',
            lineCap: 'square',
            colors: ['#fff'],
            width: 4,
        },
        markers: {
            size: 0
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

    var chart_line_basic = new ApexCharts(document.querySelector("#bandwidth-usage"), option_Bandwidth_Usage);
    chart_line_basic.render();


    // -----------------------------------------------------------------------
    // Download count
    // -----------------------------------------------------------------------

    var option_Download_Count = {
        series: [{
            name: '',
            data: [4, 5, 0.3, 10, 9, 12, 4, 9, 4, 5, 3, 10, 9, 12, 10, 9, 12, 4, 9]
        }
        ],
        chart: {
            type: 'bar',
            height: 100,
            fontFamily: 'Nunito Sans,sans-serif',
            toolbar: {
                show: false,
            },
            sparkline: {
                enabled: true
            },
        },
        colors: ["rgba(255,255,255,0.4)"],
        grid: {
            show: false,
        },
        plotOptions: {
            bar: {
                horizontal: false,
                startingShape: 'flat',
                endingShape: 'flat',
                columnWidth: '90%',
                barHeight: '100%',
            },
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 3,
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

    var chart_column_crypto2 = new ApexCharts(document.querySelector("#download-count"), option_Download_Count);
    chart_column_crypto2.render();

});