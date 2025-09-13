using YolkStudio.Pokemon.Core.Extensions;

namespace YolkStudio.Pokemon.Core.Trainers;

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
            return Result<TrainerCreatedDto>.Fail(ErrorType.Conflict,
                $"Trainer with name {command.Name} already exists");
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
            createdTrainer.BirthDate.ToUtcDateTimeOffset(),
            createdTrainer.CreatedAt.ToUtcDateTimeOffset(),
            createdTrainer.Wins,
            createdTrainer.Losses));
    }

    public async Task<IEnumerable<TrainerDto>> GetAllTrainersAsync(GetAllTrainersQuery query)
    {
        var allTrainers = await _repository.GetAllAsync();
        return allTrainers.Select(t => new TrainerDto(
            t.Id!.Value,
            t.Name,
            t.Region,
            t.BirthDate.ToUtcDateTimeOffset(),
            t.CreatedAt.ToUtcDateTimeOffset(),
            t.Wins,
            t.Losses));
    }
}