namespace YolkStudio.Pokemon.Core.Trainers;

public record TrainerCreatedDto(
    int Id,
    string Name,
    string Region,
    DateTimeOffset BirthDate,
    DateTimeOffset CreatedAt,
    int Wins,
    int Losses);