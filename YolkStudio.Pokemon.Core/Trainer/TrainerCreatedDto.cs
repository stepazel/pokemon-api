namespace YolkStudio.Pokemon.Core.Trainer;

public record TrainerCreatedDto(
    int Id,
    string Name,
    string Region,
    DateTimeOffset BirthDate,
    DateTimeOffset CreatedAt,
    int Wins,
    int Losses);