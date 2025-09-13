using YolkStudio.Pokemon.Core.Pokemons;

namespace YolkStudio.Pokemon.Core.Trainers;

public record TrainerWithPokemonsDto(
    int Id,
    string Name,
    string Region,
    DateTimeOffset Birthdate,
    DateTimeOffset CreatedAt,
    int Wins,
    int Losses,
    IEnumerable<PokemonDto> Pokemons);