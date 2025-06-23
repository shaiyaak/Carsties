using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NoticationService.Hubs;

namespace NoticationService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly IHubContext<NoticationHub> hubContext;

    public AuctionFinishedConsumer(IHubContext<NoticationHub> hubContext)
    {
        this.hubContext = hubContext;
    }
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("--> auction finished message received");
        await hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
    }
}