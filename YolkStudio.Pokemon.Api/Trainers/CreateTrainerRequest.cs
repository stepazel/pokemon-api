namespace YolkStudio.Pokemon.Api.Trainers;

public record CreateTrainerRequest(
    string Name,
    string Region,
    DateTimeOffset BirthDate);
