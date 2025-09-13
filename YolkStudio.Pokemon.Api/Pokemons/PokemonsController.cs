using System.Net;
using Microsoft.AspNetCore.Mvc;
using YolkStudio.Pokemon.Api.Shared;
using YolkStudio.Pokemon.Core;
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

    [HttpPut("{id:int}/trainer")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<PokemonDto>>> AssignTrainer(int id, AssignTrainerRequest request)
    {
        var command = new AssignTrainerToPokemonCommand(id, request.TrainerId);
        var result = await _pokemonService.AssignTrainerToPokemon(command);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ErrorResponse(HttpStatusCode.NotFound, result.Message!)),
                ErrorType.Conflict => Conflict(new ErrorResponse(HttpStatusCode.Conflict, result.Message!)),
                _ => BadRequest(result.Message)
            };
        }

        return Ok(new ApiResponse<PokemonDto>(HttpStatusCode.OK, "Trained was assigned to the pokemon", result.Value));
    }

}