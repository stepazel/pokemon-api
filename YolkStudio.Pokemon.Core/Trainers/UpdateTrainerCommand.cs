namespace YolkStudio.Pokemon.Core.Trainers;

public record UpdateTrainerCommand(int Id, string? Name, string? Region, int? Wins, int? Losses);