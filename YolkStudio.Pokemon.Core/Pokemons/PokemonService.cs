using YolkStudio.Pokemon.Core.Trainers;

namespace YolkStudio.Pokemon.Core.Pokemons;

public class PokemonService : IPokemonService
{
    private readonly IPokemonRepository _repository;
    private readonly ITrainerRepository _trainerRepository;

    public PokemonService(IPokemonRepository repository, ITrainerRepository trainerRepository)
    {
        _repository = repository;
        _trainerRepository = trainerRepository;
    }

    public async Task<IEnumerable<PokemonDto>> GetAllAsync(GetAllPokemonsQuery query)
    {
        var allPokemons = await _repository.GetAllAsync();
        return allPokemons.Select(p => new PokemonDto(
            p.Id,
            p.Name,
            p.Level,
            new ElementDto(p.Type.Id, p.Type.Name),
            p.Health,
            p.Owner is null ? null : new TrainerSummaryDto(p.Owner.Id!.Value, p.Owner.Name, p.Owner.Region),
            p.CaughtAt));
    }

    public async Task<Result<PokemonDto?>> AssignTrainerToPokemon(AssignTrainerToPokemonCommand command)
    {
        var pokemon = await _repository.GetByIdAsync(command.PokemonId);
        if (pokemon is null)
        {
            return Result<PokemonDto?>.Fail(ErrorType.NotFound, "The pokemon does not exist");
        }

        if (pokemon.Owner is not null && pokemon.Owner.Id != command.TrainerId)
        {
            return Result<PokemonDto?>.Fail(ErrorType.Conflict,"The pokemon already has an owner");
        }

        var trainer = await _trainerRepository.GetAsync(command.TrainerId);
        if (trainer is null)
        {
            return Result<PokemonDto?>.Fail(ErrorType.NotFound, "The trainer does not exist");
        }

        pokemon.Owner = trainer;
        await _repository.SaveChangesAsync();
        
        var pokemonDto = new PokemonDto(
            pokemon.Id,
            pokemon.Name,
            pokemon.Level,
            new ElementDto(pokemon.Type.Id, pokemon.Type.Name),
            pokemon.Health,
            new TrainerSummaryDto(pokemon.Owner.Id!.Value, pokemon.Owner.Name,pokemon.Owner.Region),
            pokemon.CaughtAt);
        return Result<PokemonDto?>.Ok(pokemonDto);
    }
}