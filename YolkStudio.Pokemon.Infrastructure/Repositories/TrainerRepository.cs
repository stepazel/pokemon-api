using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Core.Trainers;
using YolkStudio.Pokemon.Infrastructure.Data;

namespace YolkStudio.Pokemon.Infrastructure.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private readonly PokemonDbContext _context;

    public TrainerRepository(PokemonDbContext context)
    {
        _context = context;
    }

    public async Task<Trainer> AddAsync(Trainer trainer)
    {
        _context.Trainers.Add(trainer);
        await _context.SaveChangesAsync();
        return trainer;
    }

    public async Task<Trainer?> GetAsync(int id)
    {
        return await _context.Trainers.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> DoesExistAsync(string name)
    {
        return await _context.Trainers.AnyAsync(x => x.Name == name);
    }

    public async Task<bool> DoesExistAsync(int id)
    {
        return await _context.Trainers.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Trainer>> GetAllAsync()
    {
        return await _context.Trainers.ToListAsync();
    }

    public async Task<Trainer?> GetTrainerWithPokemonsAsync(int id)
    {
        return await _context.Trainers
            .Include(t => t.Pokemons)
            .ThenInclude(p => p.Type)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public void Remove(Trainer trainer)
    {
        _context.Trainers.Remove(trainer);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}