using Microsoft.AspNetCore.SignalR;

namespace Winter.Backend.Hubs;

public class MetricsHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("connected");
        return base.OnConnectedAsync();
    }
}
