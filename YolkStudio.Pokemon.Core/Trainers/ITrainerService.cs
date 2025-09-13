namespace YolkStudio.Pokemon.Core.Trainers;

public interface ITrainerService
{
    Task<Result<TrainerCreatedDto>> CreateTrainerAsync(AddTrainerCommand command);
    Task<IEnumerable<TrainerDto>> GetAllTrainersAsync(GetAllTrainersQuery query);
    Task<Result<TrainerWithPokemonsDto?>> GetTrainerWithPokemonsAsync(GetTrainerWithPokemonsQuery query);
}