using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        this.mapper = mapper;
    }
    public  async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine("-->Consuming auction updated: "+context.Message.Id);
        var item = mapper.Map<Item>(context.Message);
        
        await DB.Update<Item>()
        .MatchID(context.Message.Id)
        .ModifyOnly(b => new { b.Make, b.Model, b.Year, b.Color, b.Mileage }, item)
        .ExecuteAsync();  
    }
}
