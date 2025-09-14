namespace YolkStudio.Pokemon.Core.Trainers;

public interface ITrainerService
{
    Task<Result<TrainerDto>> CreateAsync(AddTrainerCommand command);
    Task<IEnumerable<TrainerDto>> GetTrainersAsync(GetAllTrainersQuery query);
    Task<Result<TrainerWithPokemonsDto?>> GetTrainerWithPokemonsAsync(GetTrainerWithPokemonsQuery query);
    Task<Result<TrainerDto?>> UpdateTrainerAsync(UpdateTrainerCommand command);
    Task<Result> DeleteTrainerAsync(DeleteTrainerCommand command);
}

public record AddTrainerCommand(string Name, string Region, DateTime BirthDate);
public record DeleteTrainerCommand(int Id);
public record GetAllTrainersQuery();
public record GetTrainerWithPokemonsQuery(int Id);
public record UpdateTrainerCommand(int Id, string? Name, string? Region, int? Wins, int? Losses);