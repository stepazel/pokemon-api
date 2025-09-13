namespace YolkStudio.Pokemon.Core.Trainer;

public interface ITrainerService
{
    Task<Result<TrainerCreatedDto>> CreateTrainerAsync(AddTrainerCommand command);
    // Task<IEnumerable<Trainer>> GetAllTrainersAsync();
}