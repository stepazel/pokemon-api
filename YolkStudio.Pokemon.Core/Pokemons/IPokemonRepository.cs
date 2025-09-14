namespace YolkStudio.Pokemon.Core.Pokemons;

public interface IPokemonRepository
{
    Task<List<Pokemon>> GetAsync(GetAllPokemonsQuery query);
    Task<Pokemon?> GetByIdAsync(int id);
    Task<int> GetCountAsync();
    Task SaveChangesAsync();
}