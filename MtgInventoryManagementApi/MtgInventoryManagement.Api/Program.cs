using MtgInventoryManagementApi.MtgInventoryManagement.Data.DependencyInjection;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddProxies(builder.Configuration);
builder.Services.AddDependencyInjection();

builder.Services.AddControllers();

var app = builder.Build();
app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
