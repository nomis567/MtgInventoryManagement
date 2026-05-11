using System.Text;
using NSubstitute;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies.Models;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Providers;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Scryfall;
using Shouldly;
using ScryfallData = MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies.Models.Data;

namespace MtgInventoryManagement.Service.Tests.ScryfallUpdate;

public class DatabaseDownloaderTest : IDisposable
{
    private readonly IScryfallProxy _scryfallProxy;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDatabaseDownloader _databaseDownloader;
    private readonly string _filePath;

    public DatabaseDownloaderTest()
    {
        _scryfallProxy = Substitute.For<IScryfallProxy>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _databaseDownloader = new DatabaseDownloader(_scryfallProxy, _dateTimeProvider);
        _filePath = Path.Combine(Environment.CurrentDirectory, "data-scryfall.json");
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    [Fact]
    public async Task UpdateCardsFromScryfall_ShouldDownloadAndSaveAllCards()
    {
        var allCardsUri = "https://data.scryfall.io/all-cards/all-cards.json";
        var bulkData = new ScryfallBulkData(
        [
            new ScryfallData(
                Id: "922288cb-4bef-45e1-bb30-0c2bd3d3534f",
                Type: "other_type",
                UpdatedAt: DateTime.Parse("2026-05-11T09:31:33.091+00:00"),
                Uri: "https://api.scryfall.com/bulk-data/other",
                Name: "Other Data",
                Description: "Not the all_cards type",
                Size: 100,
                Download_Uri: "https://data.scryfall.io/other/other.json",
                Content_Type: "application/json",
                Content_Encoding: "gzip"
            ),
            new ScryfallData(
                Id: "a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
                Type: "all_cards",
                UpdatedAt: DateTime.Parse("2026-05-11T09:31:33.091+00:00"),
                Uri: "https://api.scryfall.com/bulk-data/all-cards",
                Name: "All Cards",
                Description: "Every card object on Scryfall.",
                Size: 2509460730,
                Download_Uri: allCardsUri,
                Content_Type: "application/json",
                Content_Encoding: "gzip"
            )
        ]);

        var expectedContent = "{\"cards\":[]}";
        _scryfallProxy.GetBulkDataAsync(Arg.Any<CancellationToken>()).Returns(bulkData);
        _scryfallProxy.DownloadBulkDataAllCards(allCardsUri, Arg.Any<CancellationToken>())
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes(expectedContent)));

        await _databaseDownloader.UpdateCardsFromScryfall();

        File.Exists(_filePath).ShouldBeTrue();
        var actualContent = await File.ReadAllTextAsync(_filePath);
        actualContent.ShouldBe(expectedContent);

        await _scryfallProxy.Received(1).GetBulkDataAsync(Arg.Any<CancellationToken>());
        await _scryfallProxy.Received(1).DownloadBulkDataAllCards(allCardsUri, Arg.Any<CancellationToken>());
    }
}
