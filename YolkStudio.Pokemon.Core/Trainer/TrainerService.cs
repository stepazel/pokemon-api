namespace YolkStudio.Pokemon.Core.Trainer;

public class TrainerService : ITrainerService
{
    private readonly ITrainerRepository _repository;

    public TrainerService(ITrainerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<TrainerCreatedDto>> CreateTrainerAsync(AddTrainerCommand command)
    {
        var doesExist = await _repository.DoesExistAsync(command.Name);
        if (doesExist)
        {
            return Result<TrainerCreatedDto>.Fail($"Trainer with name {command.Name} already exists");
        }
        
        var trainer = new Trainer(
            null,
            command.Name,
            command.Region,
            command.BirthDate,
            DateTime.UtcNow, 
            0,
            0);
        var createdTrainer = await _repository.AddAsync(trainer);
        return Result<TrainerCreatedDto>.Ok(new TrainerCreatedDto(
            createdTrainer.Id!.Value, // TODO jak vyresit ten vykricnik?
            createdTrainer.Name,
            createdTrainer.Region,
            createdTrainer.BirthDate,
            createdTrainer.CreatedAt,
            createdTrainer.Wins,
            createdTrainer.Losses));
    }

    // public Task<IEnumerable<Trainer>> GetAllTrainersAsync()
    // {
    //     return Task.CompletedTask;
    // }
}