using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Data.DependencyInjection;

public static class DatabaseMigration
{
    public static void MigrateDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<MyDbContext>();
                
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
                throw;
            }
        }
    }
}
