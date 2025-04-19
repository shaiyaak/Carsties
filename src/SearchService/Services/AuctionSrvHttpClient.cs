using System;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionSrvHttpClient
{
    public HttpClient HttpClient { get; }
    public IConfiguration Config { get; }
    public AuctionSrvHttpClient(HttpClient httpClient,IConfiguration config)
    {
        HttpClient = httpClient;
        Config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item,string>()
            .Sort(x=>x.Descending(x=>x.UpdatedAt))
            .Project(x=>x.UpdatedAt.ToString())
            .ExecuteFirstAsync();
            //Console.WriteLine("Url:"+Config["AuctionServiceUrl"]);
        return await HttpClient.GetFromJsonAsync<List<Item>> (Config["AuctionServiceUrl"]+
            "/api/auctions?date="+lastUpdated);
    }
}
