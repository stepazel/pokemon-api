using System.Text.Json.Serialization;
using YolkStudio.Pokemon.Core.Shared;

namespace YolkStudio.Pokemon.Core.Pokemons;

public interface IPokemonService
{
    public Task<PagedResult<PokemonDto>> GetAsync(GetAllPokemonsQuery query);
    public Task<Result<PokemonDto?>> AssignTrainerToPokemon(AssignTrainerToPokemonCommand command);
}

public record AssignTrainerToPokemonCommand(int PokemonId, int TrainerId);

public record GetAllPokemonsQuery(
    string? Name,
    string? Element,
    int? MinLevel,
    int? MaxLevel,
    PokemonSortableProps? SortBy) : SortedQuery;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PokemonSortableProps
{
    Name,
    Level,
    CaughtAt,
}