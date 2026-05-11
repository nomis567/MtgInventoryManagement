using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Data.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyDbContext>(options =>
            options
                .UseNpgsql(configuration.GetConnectionString("DatabaseConnection"))
                .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

        return services;
    }

	public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
	{
		services.AddScoped<ICardRepository, CardRepository>();

		return services;
	}
}
