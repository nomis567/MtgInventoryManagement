using Microsoft.EntityFrameworkCore;
using MtgInventoryManagementApi.MtgInventoryManagement.Data.Models;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Data;

public class MyDbContext : DbContext 
{
	public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

	public DbSet<MtgInventoryManagement.Data.Models.Version> Version {get; set;}
}
