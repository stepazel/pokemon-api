namespace YolkStudio.Pokemon.Core.Pokemons;

public class PokemonService : IPokemonService
{
    private readonly IPokemonRepository _repository;

    public PokemonService(IPokemonRepository repository)
    {
        _repository = repository;
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
}