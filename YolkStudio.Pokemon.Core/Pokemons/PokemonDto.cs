// ReSharper disable NotAccessedPositionalProperty.Global
namespace YolkStudio.Pokemon.Core.Pokemons;

public record PokemonDto(
    int Id,
    string Name,
    int Level,
    ElementDto Type,
    int Health,
    TrainerSummaryDto? Owner = null,
    DateTimeOffset? CaughtAt = null);
    
public record ElementDto(int Id, string Name);

public record TrainerSummaryDto(int Id, string Name, string Region);