namespace YolkStudio.Pokemon.Core.Pokemons;

public interface IPokemonService
{
    public Task<IEnumerable<PokemonDto>> GetAllAsync(GetAllPokemonsQuery query);
}