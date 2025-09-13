namespace YolkStudio.Pokemon.Core.Trainers;

public record Trainer(
    int? Id,
    string Name,
    string Region,
    DateTime BirthDate,
    DateTime CreatedAt,
    int Wins,
    int Losses
);

