using Microsoft.EntityFrameworkCore;
using MtgInventoryManagementApi.MtgInventoryManagement.Data.Models;
using MtgInventoryManagementApi.MtgInventoryManagement.Data;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetAllAsync();
    Task<Card?> GetByIdAsync(Guid id);
    Task<Card?> GetByNameAsync(string name);
    Task AddAsync(Card card);
    Task UpdateAsync(Card card);
    Task DeleteAsync(Guid id);
    Task<bool> SaveChangesAsync();
}

public class CardRepository : ICardRepository
{
    private readonly MyDbContext _context;

    public CardRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Card>> GetAllAsync()
    {
        return await _context.Cards
            .Include(c => c.Editions)
            .Include(c => c.ForeignNames)
            .Include(c => c.Legalities)
            .ToListAsync();
    }

    public async Task<Card?> GetByIdAsync(Guid id)
    {
        return await _context.Cards
            .Include(c => c.Editions)
            .Include(c => c.ForeignNames)
            .Include(c => c.Legalities)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Card?> GetByNameAsync(string name)
    {
        return await _context.Cards
            .Include(c => c.Editions)
            .Include(c => c.ForeignNames)
            .Include(c => c.Legalities)
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task AddAsync(Card card)
    {
        await _context.Cards.AddAsync(card);
    }

    public async Task UpdateAsync(Card card)
    {
        _context.Cards.Update(card);
    }

    public async Task DeleteAsync(Guid id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card != null) _context.Cards.Remove(card);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
