using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Constants;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Providers;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Scryfall;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Services;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Service.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddProxies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>(client =>
        {
            client.DefaultRequestHeaders.Add(ScryfallConstants.UserAgentHeader, ScryfallConstants.UserAgentValue);
            client.DefaultRequestHeaders.Add(ScryfallConstants.AcceptHeader, ScryfallConstants.AcceptHeaderValue);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });

        services.AddScoped<IScryfallProxy, ScryfallProxy>();

        services.AddScoped<IDatabaseDownloader, DatabaseDownloader>();

        services.AddScoped<IScryfallService, ScryfallService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
