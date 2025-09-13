using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Core.Pokemons;
using YolkStudio.Pokemon.Infrastructure.Data;

namespace YolkStudio.Pokemon.Infrastructure.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly PokemonDbContext _context;

    public PokemonRepository(PokemonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Core.Pokemons.Pokemon>> GetAllAsync()
    {
        return await _context.Pokemons
            .Include(p => p.Owner)
            .Include(p => p.Type)
            .ToListAsync();
    }

    public async Task<Core.Pokemons.Pokemon?> GetByIdAsync(int id)
    {
        return await _context.Pokemons
            .Include(p => p.Type)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    { 
        await _context.SaveChangesAsync();
    }

    public async Task<Core.Pokemons.Pokemon> AddOwnerToPokemonAsync(int id, int trainerId)
    {
        var pokemon = await _context.Pokemons
            .Include(p => p.Type)
            .Include(p => p.Owner)
            .FirstAsync(p => p.Id == id);

        pokemon.OwnerId = trainerId;

        await _context.SaveChangesAsync();
        return pokemon;
    }
}