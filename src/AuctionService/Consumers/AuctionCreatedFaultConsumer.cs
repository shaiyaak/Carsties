using System;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("--> Consuming faulty creation");
        var execption = context.Message.Exceptions.First();
        if (execption.ExceptionType == "System.ArgumentException")
        {
            context.Message.Message.Model = "FooBar";
            await context.Publish(context.Message.Message);
        }
        else Console.WriteLine("Not an argument exception - update an error dashboad somewhere");
    }
}
