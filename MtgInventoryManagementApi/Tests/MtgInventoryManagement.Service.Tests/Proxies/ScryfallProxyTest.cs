using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using NSubstitute;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies.Models;
using Shouldly;
using ScryfallData = MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies.Models.Data;

namespace MtgInventoryManagement.Service.Tests.Proxies;

public class ScryfallProxyTest
{
    private readonly IHttpClientWrapper _mockHttpClient;
    private readonly ScryfallProxy _proxy;

    public ScryfallProxyTest()
    {
        _mockHttpClient = Substitute.For<IHttpClientWrapper>();
        _proxy = new ScryfallProxy(_mockHttpClient);
    }

    [Fact]
    public async Task GetBulkDataAsync_ShouldReturnBulkData()
    {
        var bulkData = new ScryfallBulkData(
        [
            new ScryfallData(
                Id: "922288cb-4bef-45e1-bb30-0c2bd3d3534f",
                Type: "all_cards",
                UpdatedAt: DateTime.Parse("2026-05-11T09:31:33.091+00:00"),
                Uri: "https://api.scryfall.com/bulk-data/922288cb-4bef-45e1-bb30-0c2bd3d3534f",
                Name: "All Cards",
                Description: "A JSON file containing every card object on Scryfall in every language.",
                Size: 2509460730,
                Download_Uri: "https://data.scryfall.io/all-cards/all-cards-20260511093133.json",
                Content_Type: "application/json",
                Content_Encoding: "gzip"
            )
        ]);
        _mockHttpClient.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(bulkData)
            });

        var result = await _proxy.GetBulkDataAsync();

        result.ShouldSatisfyAllConditions(
            r => r.ShouldNotBeNull(),
            r => r.Data.Count.ShouldBe(1),
            r => r.Data[0].Name.ShouldBe("All Cards"),
            r => r.Data[0].Download_Uri.ShouldContain("data.scryfall.io")
        );
        await _mockHttpClient.Received(1).GetAsync("https://api.scryfall.com/bulk-data", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBulkDataAsync_ShouldThrowWhenResponseIsNull()
    {
        _mockHttpClient.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null")
            });

        await Assert.ThrowsAsync<InvalidOperationException>(() => _proxy.GetBulkDataAsync());
    }

    [Fact]
    public async Task GetBulkDataAsync_ShouldThrowWhenResponseFails()
    {
        _mockHttpClient.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.BadRequest));

        await Assert.ThrowsAsync<HttpRequestException>(() => _proxy.GetBulkDataAsync());
    }
}
