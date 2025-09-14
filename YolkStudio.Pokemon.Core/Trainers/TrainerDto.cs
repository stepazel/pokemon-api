// ReSharper disable NotAccessedPositionalProperty.Global
namespace YolkStudio.Pokemon.Core.Trainers;

public record TrainerDto(
    int Id,
    string Name,
    string Region,
    DateTimeOffset Birthdate,
    DateTimeOffset CreatedAt,
    int Wins,
    int Losses);
