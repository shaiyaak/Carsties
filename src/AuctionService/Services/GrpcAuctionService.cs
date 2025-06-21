using AuctionService.Data;
using Grpc.Core;
using AuctionService;

namespace AuctionService.Services;

public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext dbContext;

    public GrpcAuctionService(AuctionDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request,
        ServerCallContext context)
    {
        Console.WriteLine("==>Received Grpc request for auction");

        var auction = await dbContext.Auctions.FindAsync(Guid.Parse(request.Id))
         ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));
        var reponse = new GrpcAuctionResponse
        {
            Auction = new GrpcAuctionModel
            {
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id = auction.Id.ToString(),
                ReservePrice = auction.ReservePrice,
                Seller = auction.Seller
            }
        };
        return reponse;
    }
}