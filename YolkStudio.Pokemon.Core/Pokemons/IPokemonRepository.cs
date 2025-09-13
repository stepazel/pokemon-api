namespace YolkStudio.Pokemon.Core.Pokemons;

public interface IPokemonRepository
{
    Task<IEnumerable<Pokemon>> GetAllAsync();
}