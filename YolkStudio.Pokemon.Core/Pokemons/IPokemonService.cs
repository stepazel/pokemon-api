namespace YolkStudio.Pokemon.Core.Pokemons;

public interface IPokemonService
{
    public Task<IEnumerable<PokemonDto>> GetAllAsync(GetAllPokemonsQuery query);
    public Task<Result<PokemonDto?>> AssignTrainerToPokemon(AssignTrainerToPokemonCommand command);
}

public record AssignTrainerToPokemonCommand(int PokemonId, int TrainerId);
public record GetAllPokemonsQuery();