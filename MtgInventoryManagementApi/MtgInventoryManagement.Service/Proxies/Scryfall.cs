using System.Net.Http.Json;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Constants;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies.Models;


namespace MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies;

public interface IScryfallProxy
{
    Task<ScryfallBulkData> GetBulkDataAsync(CancellationToken cancellationToken = default);
    Task<Stream> DownloadBulkDataAllCards(string downloadUri, CancellationToken cancellationToken = default);
}

public class ScryfallProxy : IScryfallProxy
{
    private readonly IHttpClientWrapper _httpClient;

    public ScryfallProxy(IHttpClientWrapper httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ScryfallBulkData> GetBulkDataAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(ScryfallConstants.BulkDataEndpoint, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ScryfallBulkData>(cancellationToken: cancellationToken);

        return content ?? throw new InvalidOperationException("Received null response from Scryfall bulk-data endpoint");
    }

    public async Task<Stream> DownloadBulkDataAllCards(string downloadUri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var encoding = response.Content.Headers.ContentEncoding.FirstOrDefault();

        return await response.Content.ReadAsStreamAsync();
    }
}
