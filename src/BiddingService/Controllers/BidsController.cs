using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly GrpcAuctionClient grpcClient;

    public BidsController(IMapper mapper,IPublishEndpoint publishEndpoint,GrpcAuctionClient grpcClient)
    {
        this.mapper = mapper;
        this.publishEndpoint = publishEndpoint;
        this.grpcClient = grpcClient;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BidDTO>> PlaceBid(string auctionId, int amount)
    {
        var auction = await DB.Find<Auction>().OneAsync(auctionId);
        if (auction == null)
        {
            auction = grpcClient.GetAuction(auctionId);
            if (auction == null) return BadRequest("Cannot accept bids on this auction at this time");
        }
        if (auction.Seller == User.Identity.Name)
        {
            return BadRequest("You cannot bid on your own auction");
        }
        var bid = new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = User.Identity.Name
        };
        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>()
                .Match(a => a.AuctionId == auctionId)
                .Sort(b => b.Descending(x => x.Amount))
                .ExecuteFirstAsync();
            if (highBid != null && amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = amount > auction.ReservePrice ?
                BidStatus.Accepted :
                BidStatus.AcceptedBelowReserve;
            }
            if (highBid != null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }
        await DB.SaveAsync(bid);
        await publishEndpoint.Publish(mapper.Map<BidPlaced>(bid));
        return Ok(mapper.Map<BidDTO>(bid));
    }
    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDTO>>> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(b => b.AuctionId == auctionId)
            .Sort(b => b.Descending(x => x.BidTime))
            .ExecuteAsync();
        return bids.Select(mapper.Map<BidDTO>).ToList();
    }
}