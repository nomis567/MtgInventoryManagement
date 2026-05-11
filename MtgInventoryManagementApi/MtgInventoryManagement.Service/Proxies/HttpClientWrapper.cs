using System.Net.Http;
using System.Net.Http.Json;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies;

public interface IHttpClientWrapper
{
    Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken = default);
    Task<T?> GetFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken = default);
}

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync(requestUri, cancellationToken);
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }

    public async Task<T?> GetFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<T>(requestUri, cancellationToken);
    }
}
