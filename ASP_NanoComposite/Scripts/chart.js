﻿var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
var config = {
    type: 'line',
    data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [{
            label: 'My First dataset',
            backgroundColor: 'rgba(105, 0, 132, .2)',
            borderColor: 'rgba(200, 99, 132, .7)',
            data: [
                65, 59, 80, 81, 56, 55, 40
            ],
            fill: false,
        }, {
            label: 'My Second dataset',
            fill: true,
            backgroundColor: 'rgba(0, 137, 132, .2)',
            borderColor: 'rgba(0, 10, 130, .7)',
            data: [
                28, 48, 40, 19, 86, 27, 90
            ],
        }]
    },
    options: {
        responsive: true,
        title: {
            display: true,
            text: 'Chart.js Line Chart'
        },
        tooltips: {
            mode: 'index',
            intersect: false,
        },
        hover: {
            mode: 'nearest',
            intersect: true
        }
    }
};

window.onload = function () {
    var ctx = document.getElementById('lineChart').getContext('2d');
    window.myLine = new Chart(ctx, config);
};