using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Core.Pokemons;
using YolkStudio.Pokemon.Core.Shared;
using YolkStudio.Pokemon.Infrastructure.Data;
using FuzzySharp;

namespace YolkStudio.Pokemon.Infrastructure.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly PokemonDbContext _context;

    public PokemonRepository(PokemonDbContext context)
    {
        _context = context;
    }

    public async Task<List<Core.Pokemons.Pokemon>> GetAsync(GetAllPokemonsQuery query)
    {
        var pokemonsQuery = _context.Pokemons
            .Include(p => p.Owner)
            .Include(p => p.Type)
            .AsQueryable();

        if (string.IsNullOrWhiteSpace(query.Element) is false)
        {
            pokemonsQuery = pokemonsQuery.Where(p => p.Type.Name.ToLower().Contains(query.Element.ToLower()));
        }

        if (query.MinLevel.HasValue)
        {
            pokemonsQuery = pokemonsQuery.Where(p => p.Level >= query.MinLevel.Value);
        }

        if (query.MaxLevel.HasValue)
        {
            pokemonsQuery = pokemonsQuery.Where(p => p.Level <= query.MaxLevel.Value);
        }

        if (query.SortBy.HasValue)
        {
            pokemonsQuery = query.SortBy switch
            {
                PokemonSortableProps.Name => query.SortDirection is SortDirection.Asc
                    ? pokemonsQuery.OrderBy(p => p.Name)
                    : pokemonsQuery.OrderByDescending(p => p.Name),
                PokemonSortableProps.Level => query.SortDirection is SortDirection.Asc
                    ? pokemonsQuery.OrderBy(p => p.Level)
                    : pokemonsQuery.OrderByDescending(p => p.Level),
                PokemonSortableProps.CaughtAt => query.SortDirection is SortDirection.Asc
                    ? pokemonsQuery.OrderBy(p => p.CaughtAt)
                    : pokemonsQuery.OrderByDescending(p => p.CaughtAt),
                _ => throw new ArgumentOutOfRangeException(), // Can't happen since the enum is in the API.
            };
        }
        var pokemons = await pokemonsQuery.ToListAsync();

        var filtered = string.IsNullOrWhiteSpace(query.Name)
            ? pokemons
            : pokemons.Where(p => p.Name.Contains(query.Name, StringComparison.CurrentCultureIgnoreCase) ||
                Fuzz.Ratio(p.Name, query.Name) > 80).ToList();

        return filtered
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }

    public async Task<Core.Pokemons.Pokemon?> GetByIdAsync(int id)
    {
        return await _context.Pokemons
            .Include(p => p.Type)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Pokemons.CountAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}