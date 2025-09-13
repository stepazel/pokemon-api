namespace YolkStudio.Pokemon.Core.Pokemons;

public interface IPokemonRepository
{
    Task<IEnumerable<Pokemon>> GetAllAsync();
    Task<Pokemon?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}