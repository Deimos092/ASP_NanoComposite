//var config = {
//    type: 'line',
//    data: {
//        labels: [],

//        datasets: [{
//            label: 'Abc 90% + Qwe 10%',
//            backgroundColor: 'rgba(105, 0, 132, .2)',
//            borderColor: 'rgba(200, 99, 132, .7)',
//            data: [

//            ],
//            fill: false,
//        }]
//    },
//    options: {
//        responsive: true,
//        title: {
//            display: true,
//            text: 'Прочность'
//        },
//        tooltips: {
//            mode: 'index',
//            intersect: false,
//        },
//        hover: {
//            mode: 'nearest',
//            intersect: true
//        },
//        scales: {
//            xAxes: [{
//                ticks: {
//                    display: false //this will remove only the label
//                }
//            }]
//        }
//    }
//};

//function randomInteger(min, max) {
//    var rand = min - 0.5 + Math.random() * (max - min + 1)
//    rand = Math.round(rand);
//    return rand;
//}

//for (var i = 0; i < 100; i++) {
//    config.data.labels.push(i);
//    config.data.datasets[0].data.push(randomInteger(0, 100));
//}

//window.onload = function () {
//    var ctx = document.getElementById('lineChart').getContext('2d');
//    window.myLine = new Chart(ctx, config);
//};

//function DisplayChartFromData(data, dstElement) {
//    var config = {
//        type: 'line',
//        data: {
//            labels: [],

//            datasets: [{
//                label: data[0].Value,
//                backgroundColor: 'rgba(105, 0, 132, .2)',
//                borderColor: 'rgba(200, 99, 132, .7)',
//                data: [

//                ],
//                fill: false,
//            }]
//        },
//        options: {
//            animation: {
//                duration: 0 // general animation time
//            },
//            hover: {
//                animationDuration: 0 // duration of animations when hovering an item
//            },
//            responsiveAnimationDuration: 0,
//            responsive: true,
//            tooltips: {
//                mode: 'index',
//                intersect: false,
//            },
//            hover: {
//                mode: 'nearest',
//                intersect: true
//            },
//            scales: {
//                xAxes: [{
//                    ticks: {
//                        display: false //this will remove only the label
//                    }
//                }]
//            }
//        }
//    };
//    for (var i = 0; i < data[1].Value.length; i++) {
//        config.data.labels.push(data[1].Value[i]);
//        config.data.datasets[0].data.push(data[2].Value[i]);
//    }
    
//    //config.animation.duration = 0;
//    var ctx = dstElement.getContext('2d');
//    new Chart(ctx, config);
//}

//function DisplayChartFromData(data, dstElement) {
//    var chart = new CanvasJS.Chart(dstElement, {
//        data: [
//            {
//                type: "line",
//                dataPoints: [

//                ]
//            }
//        ]
//    });

    
//    for (var i = 0; i < data[1].Value.length; i++) {
//        var d = new Object();
//        d.x = data[1].Value[i];
//        d.y = data[2].Value[i];
//        chart.data.push(d)
//    }

//    //config.animation.duration = 0;
    
//    chart.render();
//}