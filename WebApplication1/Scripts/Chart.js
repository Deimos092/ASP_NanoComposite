var config = {
    type: 'line',
    data: {
        labels: [],

        datasets: [{
            label: 'Abc 90% + Qwe 10%',
            backgroundColor: 'rgba(105, 0, 132, .2)',
            borderColor: 'rgba(200, 99, 132, .7)',
            data: [

            ],
            fill: false,
        }]
    },
    options: {
        responsive: true,
        title: {
            display: true,
            text: 'Прочность'
        },
        tooltips: {
            mode: 'index',
            intersect: false,
        },
        hover: {
            mode: 'nearest',
            intersect: true
        },
        scales: {
            xAxes: [{
                ticks: {
                    display: false //this will remove only the label
                }
            }]
        }
    }
};

function randomInteger(min, max) {
    var rand = min - 0.5 + Math.random() * (max - min + 1)
    rand = Math.round(rand);
    return rand;
}

for (var i = 0; i < 100; i++) {
    config.data.labels.push(i);
    config.data.datasets[0].data.push(randomInteger(0, 100));
}

window.onload = function () {
    var ctx = document.getElementById('lineChart').getContext('2d');
    window.myLine = new Chart(ctx, config);
};