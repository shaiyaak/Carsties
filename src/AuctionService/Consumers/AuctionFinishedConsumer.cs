using System;
using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext dbContext;

    public AuctionFinishedConsumer(AuctionDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("--> consuming auction finished");
        var auction = await dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }
        auction.Status = auction.SoldAmount > auction.ReservePrice 
            ? Status.Finished : Status.ReserveNotMet;

        await dbContext.SaveChangesAsync();

    }
}
