using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NoticationService.Hubs;

namespace NoticationService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IHubContext<NoticationHub> hubContext;

    public AuctionCreatedConsumer(IHubContext<NoticationHub> hubContext)
    {
        this.hubContext = hubContext;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> auction created message received");
        await hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
    }
}