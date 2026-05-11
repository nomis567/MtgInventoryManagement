using MtgInventoryManagementApi.MtgInventoryManagement.Service.Constants;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Scryfall;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Service.Services;

public interface IScryfallService
{
    Task UpdateCardsFromScryfallAsync(CancellationToken cancellationToken = default);
}

public class ScryfallService : IScryfallService
{
    private readonly IDatabaseDownloader _databaseDownloader;

    public ScryfallService(IDatabaseDownloader databaseDownloader)
    {
        _databaseDownloader = databaseDownloader;
    }

    public async Task UpdateCardsFromScryfallAsync(CancellationToken cancellationToken = default)
    {
		if (_databaseDownloader.IsDataFileStale())
			await _databaseDownloader.UpdateCardsFromScryfall(cancellationToken);

        //string filePath = Path.Combine(Environment.CurrentDirectory, ScryfallConstants.DataFileName);
        //string json = await File.ReadAllTextAsync(filePath, cancellationToken);

        await Task.CompletedTask;
    }
}
