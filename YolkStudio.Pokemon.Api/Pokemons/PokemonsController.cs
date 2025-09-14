using System.Net;
using Microsoft.AspNetCore.Mvc;
using YolkStudio.Pokemon.Api.Shared;
using YolkStudio.Pokemon.Core;
using YolkStudio.Pokemon.Core.Pokemons;
using YolkStudio.Pokemon.Core.Shared;

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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<PokemonDto>>>> GetAsync([FromQuery] GetAllPokemonsQuery query) 
    // A request record should be used here to be technically correct, but I'm lazy and this works.
    // But it would be good to have an abstract Pagination/Sortable request to have custom validation for them without the need to repeat myself.
    {
        var result = await _pokemonService.GetAsync(query);
        return Ok(new ApiResponse<PagedResult<PokemonDto>>(
            HttpStatusCode.OK,
            "Pokemons retrieved successfully",
            result));
    }

    [HttpPut("{id:int}/trainer")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<PokemonDto>>> AssignTrainerAsync(int id, AssignTrainerRequest request)
    {
        var command = new AssignTrainerToPokemonCommand(id, request.TrainerId);
        var result = await _pokemonService.AssignTrainerToPokemon(command);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ErrorResponse(HttpStatusCode.NotFound, result.Message!)),
                ErrorType.Conflict => Conflict(new ErrorResponse(HttpStatusCode.Conflict, result.Message!)),
                _ => BadRequest(result.Message),
            };
        }

        return Ok(new ApiResponse<PokemonDto>(HttpStatusCode.OK, "Trainer was assigned to the pokemon", result.Value));
    }
}