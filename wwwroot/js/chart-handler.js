let chart;
const data = {
    labels: [],
    datasets: [{
        label: 'Temperature (°C)',
        borderColor: 'red',
        data: [],
    }, {
        label: 'Humidity (%)',
        borderColor: 'blue',
        data: [],
    }]
};

function initChart() {
    const ctx = document.getElementById('sensorChart').getContext('2d');
    chart = new Chart(ctx, {
        type: 'line',
        data: data,
        options: {
            scales: {
                x: { type: 'time', time: { unit: 'second' } },
                y: { beginAtZero: true }
            }
        }
    });

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/sensorhub")
        .build();

    connection.on("ReceiveSensorData", function (sensor) {
        if (data.labels.length > 20) {
            data.labels.shift();
            data.datasets[0].data.shift();
            data.datasets[1].data.shift();
        }
        data.labels.push(sensor.timestamp);
        data.datasets[0].data.push(sensor.temperature);
        data.datasets[1].data.push(sensor.humidity);
        chart.update();
    });

    connection.start().catch(err => console.error(err));
}
