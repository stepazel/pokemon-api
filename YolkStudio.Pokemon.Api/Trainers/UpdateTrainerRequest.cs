namespace YolkStudio.Pokemon.Api.Trainers;

public record UpdateTrainerRequest(string? Name, string? Region, int? Wins, int? Losses);