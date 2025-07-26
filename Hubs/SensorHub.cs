using Microsoft.AspNetCore.SignalR;

public class SensorHub : Hub
{
    public async Task SendSensorData(SensorData data)
    {
        await Clients.All.SendAsync("ReceiveSensorData", data);
    }
}
