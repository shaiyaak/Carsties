using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NoticationService.Hubs;

namespace NoticationService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly IHubContext<NoticationHub> hubContext;

    public BidPlacedConsumer(IHubContext<NoticationHub> hubContext)
    {
        this.hubContext = hubContext;
    }
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> bid placed message received");
        await hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}