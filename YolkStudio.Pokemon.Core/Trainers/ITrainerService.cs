namespace YolkStudio.Pokemon.Core.Trainers;

public interface ITrainerService
{
    Task<Result<TrainerCreatedDto>> CreateTrainerAsync(AddTrainerCommand command);
    // Task<IEnumerable<Trainer>> GetAllTrainersAsync();
}