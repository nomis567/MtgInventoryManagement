using NSubstitute;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Scryfall;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Services;
using Shouldly;

namespace MtgInventoryManagement.Service.Tests.Services;

public class ScryfallServiceTest
{
    private readonly IDatabaseDownloader _databaseDownloader;
    private readonly IScryfallService _scryfallService;

    public ScryfallServiceTest()
    {
        _databaseDownloader = Substitute.For<IDatabaseDownloader>();
        _scryfallService = new ScryfallService(_databaseDownloader);
    }

    [Fact]
    public async Task UpdateCardsFromScryfallAsync_ShouldCallDatabaseDownloader()
    {
        _databaseDownloader.IsDataFileStale().Returns(true);

        await _scryfallService.UpdateCardsFromScryfallAsync();

        await _databaseDownloader.Received(1).UpdateCardsFromScryfall(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateCardsFromScryfallAsync_ShouldSkipDownloadWhenDataFileIsFresh()
    {
        _databaseDownloader.IsDataFileStale().Returns(false);

        await _scryfallService.UpdateCardsFromScryfallAsync();

        await _databaseDownloader.DidNotReceive().UpdateCardsFromScryfall(Arg.Any<CancellationToken>());
    }
}
