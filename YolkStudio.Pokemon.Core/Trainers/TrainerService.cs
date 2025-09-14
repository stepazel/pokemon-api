using YolkStudio.Pokemon.Core.Extensions;
using YolkStudio.Pokemon.Core.Pokemons;

namespace YolkStudio.Pokemon.Core.Trainers;

public class TrainerService : ITrainerService
{
    private readonly ITrainerRepository _repository;

    public TrainerService(ITrainerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<TrainerDto>> CreateTrainerAsync(AddTrainerCommand command)
    {
        var doesExist = await _repository.DoesExistAsync(command.Name);
        if (doesExist)
            return Result<TrainerDto>.Fail(ErrorType.Conflict, $"Trainer with name {command.Name} already exists");

        var trainer = new Trainer(
            null,
            command.Name,
            command.Region,
            command.BirthDate,
            DateTime.UtcNow,
            0,
            0);
        var createdTrainer = await _repository.AddAsync(trainer);
        return Result<TrainerDto>.Ok(new TrainerDto(
            createdTrainer.Id!.Value,
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

    public async Task<Result<TrainerWithPokemonsDto?>> GetTrainerWithPokemonsAsync(GetTrainerWithPokemonsQuery query)
    {
        var trainer = await _repository.GetTrainerWithPokemonsAsync(query.Id);
        if (trainer is null)
            return Result<TrainerWithPokemonsDto?>.Fail(ErrorType.NotFound,"Trainer with the specified id doesn't exist");

        var dto = new TrainerWithPokemonsDto(
            trainer.Id!.Value,
            trainer.Name,
            trainer.Region,
            trainer.BirthDate.ToUtcDateTimeOffset(),
            trainer.CreatedAt.ToUtcDateTimeOffset(),
            trainer.Wins,
            trainer.Losses,
            trainer.Pokemons.Select(p => new PokemonDto(
                p.Id,
                p.Name,
                p.Level,
                new ElementDto(p.Id, p.Type.Name),
                p.Health,
                new TrainerSummaryDto(p.OwnerId!.Value, p.Owner!.Name, p.Owner.Region),
                p.CaughtAt)));
        return Result<TrainerWithPokemonsDto?>.Ok(dto);
    }

    public async Task<Result<TrainerDto?>> UpdateTrainerAsync(UpdateTrainerCommand command)
    {
        var trainer = await _repository.GetAsync(command.Id);
        if (trainer is null)
            return Result<TrainerDto?>.Fail(ErrorType.NotFound, "Trainer with this id doesn't exist");
        
        if (command.Name is not null)
            trainer.Name = command.Name;
        if (command.Region is not null)
            trainer.Region = command.Region;
        if (command.Wins is not null)
            trainer.Wins = command.Wins.Value;
        if (command.Losses is not null)
            trainer.Losses = command.Losses.Value;

        await _repository.SaveChangesAsync();
        var dto = new TrainerDto(
            trainer.Id!.Value,
            trainer.Name,
            trainer.Region,
            trainer.BirthDate.ToUtcDateTimeOffset(),
            trainer.CreatedAt.ToUtcDateTimeOffset(),
            trainer.Wins,
            trainer.Losses);
        return Result<TrainerDto?>.Ok(dto);
    }

    public async Task<Result> DeleteTrainerAsync(DeleteTrainerCommand command)
    {
        var trainer = await _repository.GetTrainerWithPokemonsAsync(command.Id);
        if (trainer is null)
            return Result.Fail(ErrorType.NotFound, "Trainer with this id doesn't exist");

        if (trainer.Pokemons.Any())
            return Result.Fail(ErrorType.Conflict, "Trainer has pokemons assigned to them");

        _repository.Remove(trainer);
        await _repository.SaveChangesAsync();
        return Result.Ok();
    }
}