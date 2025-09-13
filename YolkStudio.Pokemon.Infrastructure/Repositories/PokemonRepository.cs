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
}