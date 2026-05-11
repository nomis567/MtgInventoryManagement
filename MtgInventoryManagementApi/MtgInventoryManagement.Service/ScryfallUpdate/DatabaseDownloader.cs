using MtgInventoryManagementApi.MtgInventoryManagement.Service.Constants;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Providers;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Service.Scryfall;

public interface IDatabaseDownloader
{
    public Task UpdateCardsFromScryfall(CancellationToken cancellationToken = default);

    public bool IsDataFileStale();
}

public class DatabaseDownloader : IDatabaseDownloader
{
    private readonly IScryfallProxy _scryfallProxy;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DatabaseDownloader(IScryfallProxy scryfallProxy, IDateTimeProvider dateTimeProvider)
    {
        _scryfallProxy = scryfallProxy;
        _dateTimeProvider = dateTimeProvider;
    }

    public bool IsDataFileStale()
    {
        string filePath = GetFilePath();
        if (!File.Exists(filePath))
            return true;

        DateTime lastWrite = File.GetLastWriteTimeUtc(filePath);
        return lastWrite < _dateTimeProvider.UtcNow.AddDays(-1);
    }

    public async Task UpdateCardsFromScryfall(CancellationToken cancellationToken = default)
    {   
        string filePath = GetFilePath();
        using FileStream outputFileStream = File.Create(filePath);
        var bulkDatas = await _scryfallProxy.GetBulkDataAsync(cancellationToken);
        var allCards = bulkDatas.Data.ToList().Where(s => s.Type == ScryfallConstants.AllCardsType).Single();
        using var stream = await _scryfallProxy.DownloadBulkDataAllCards(allCards.Download_Uri, cancellationToken);
        stream.CopyTo(outputFileStream);
    }

    private static string GetFilePath()
    {
        return Path.Combine(Environment.CurrentDirectory, ScryfallConstants.DataFileName);
    }

}
