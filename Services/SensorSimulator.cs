
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

public class SensorSimulator : BackgroundService
{
    private readonly IHubContext<SensorHub> _hub;
    private readonly Random _rand = new();

    public SensorSimulator(IHubContext<SensorHub> hub)
    {
        _hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var data = new SensorData
            {
                Timestamp = DateTime.Now,
                Temperature = Math.Round(20 + _rand.NextDouble() * 10, 2),
                Humidity = Math.Round(40 + _rand.NextDouble() * 20, 2)
            };

            await _hub.Clients.All.SendAsync("ReceiveSensorData", data);

            await Task.Delay(1000);
        }
    }
}
