using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MtgInventoryManagementApi.MtgInventoryManagement.Data;
using MtgInventoryManagementApi.MtgInventoryManagement.Data.Models;
using Shouldly;
using Xunit;

namespace MtgInventoryManagement.Data.Tests;

public class CardRepositoryTest : IAsyncLifetime
{
    private readonly MyDbContext _context;
    private readonly CardRepository _repository;

    public CardRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MyDbContext(options);
        _repository = new CardRepository(_context);
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCards()
    {
        var card1 = new Card { Id = Guid.NewGuid(), Name = "Black Lotus" };
        var card2 = new Card { Id = Guid.NewGuid(), Name = "Mox Pearl" };
        await _context.Cards.AddRangeAsync(card1, card2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        result.Count().ShouldBe(2);
        result.Any(c => c.Name == "Black Lotus").ShouldBeTrue();
        result.Any(c => c.Name == "Mox Pearl").ShouldBeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCardWhenExists()
    {
        var card = new Card { Id = Guid.NewGuid(), Name = "Ancestral Recall" };
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(card.Id);

        result.ShouldNotBeNull();
        result!.Name.ShouldBe("Ancestral Recall");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenNotFound()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldIncludeRelatedEntities()
    {
        var cardId = Guid.NewGuid();
        var card = new Card { Id = cardId, Name = "Lightning Bolt" };
        card.Editions.Add(new CardEdition { CardId = cardId, EditionId = "LEA" });
        card.ForeignNames.Add(new ForeignName { CardId = cardId, Language = "French", Name = "Éclair" });
        card.Legalities.Add(new Legality { CardId = cardId, Format = "Modern", FormatLegality = "Legal" });
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(cardId);

        result.ShouldNotBeNull();
        result!.Editions.Count.ShouldBe(1);
        result.ForeignNames.Count.ShouldBe(1);
        result.Legalities.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnCardWhenExists()
    {
        await _context.Cards.AddAsync(new Card { Id = Guid.NewGuid(), Name = "Counterspell" });
        await _context.SaveChangesAsync();

        var result = await _repository.GetByNameAsync("Counterspell");

        result.ShouldNotBeNull();
        result!.Name.ShouldBe("Counterspell");
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNullWhenNotFound()
    {
        var result = await _repository.GetByNameAsync("Nonexistent Card");

        result.ShouldBeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddCardToDatabase()
    {
        var card = new Card { Id = Guid.NewGuid(), Name = "Dark Ritual" };
        await _repository.AddAsync(card);
        await _repository.SaveChangesAsync();

        _context.Cards.Single(c => c.Name == "Dark Ritual").ShouldBeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingCard()
    {
        var card = new Card { Id = Guid.NewGuid(), Name = "Giant Growth", Artist = "Old Artist" };
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();

        card.Artist = "New Artist";
        await _repository.UpdateAsync(card);
        await _repository.SaveChangesAsync();

        var updated = await _context.Cards.FindAsync(card.Id);
        updated!.Artist.ShouldBe("New Artist");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCardFromDatabase()
    {
        var card = new Card { Id = Guid.NewGuid(), Name = "Swords to Plowshares" };
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(card.Id);
        await _repository.SaveChangesAsync();

        _context.Cards.Any(c => c.Id == card.Id).ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDoNothingWhenCardNotFound()
    {
        var initialCount = _context.Cards.Count();

        await _repository.DeleteAsync(Guid.NewGuid());
        await _repository.SaveChangesAsync();

        _context.Cards.Count().ShouldBe(initialCount);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldReturnTrueWhenChangesMade()
    {
        await _repository.AddAsync(new Card { Id = Guid.NewGuid(), Name = "Healing Salve" });

        var result = await _repository.SaveChangesAsync();

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldReturnFalseWhenNoChanges()
    {
        var result = await _repository.SaveChangesAsync();

        result.ShouldBeFalse();
    }
}
