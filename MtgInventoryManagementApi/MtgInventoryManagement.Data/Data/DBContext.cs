using Microsoft.EntityFrameworkCore;
using MtgInventoryManagementApi.MtgInventoryManagement.Data.Models;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Data;

public class MyDbContext : DbContext 
{
	public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

	public DbSet<MtgInventoryManagement.Data.Models.Version> Version {get; set;}
	public DbSet<Card> Cards {get;set;}
	public DbSet<Edition> Editions {get;set;}
	public DbSet<CardEdition> CardEditions {get;set;}
	public DbSet<ForeignName> ForeignNames {get;set;}
	public DbSet<Legality> Legalities {get;set;}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CardEdition>()
			.HasKey(ce => new { ce.CardId, ce.EditionId });

		modelBuilder.Entity<CardEdition>()
			.HasOne(ce => ce.Card)
			.WithMany(c => c.Editions)
			.HasForeignKey(ce => ce.CardId);

		modelBuilder.Entity<CardEdition>()
			.HasOne(ce => ce.Edition)
			.WithMany(e => e.CardEditions)
			.HasForeignKey(ce => ce.EditionId);
	}
}
