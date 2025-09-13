using System.Net;
using Microsoft.AspNetCore.Mvc;
using YolkStudio.Pokemon.Api.Shared;
using YolkStudio.Pokemon.Core.Pokemons;

namespace YolkStudio.Pokemon.Api.Pokemons;

[ApiController]
[Route(BaseUrl + "pokemons")]
public class PokemonController : BaseController
{
    private readonly IPokemonService _pokemonService;

    public PokemonController(IPokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _pokemonService.GetAllAsync(new GetAllPokemonsQuery());
        return Ok(new ApiResponse<IEnumerable<PokemonDto>>(
            HttpStatusCode.OK,
            "Pokemons retrieved successfully",
            result));
    }
}