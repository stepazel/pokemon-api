using Microsoft.AspNetCore.Mvc;
using YolkStudio.Pokemon.Api.Shared;

namespace YolkStudio.Pokemon.Api.Pokemons;

[ApiController]
[Route(BaseUrl +"pokemons")]
public class PokemonController : BaseController
{
    public PokemonController()
    {
        throw new NotImplementedException("This controller is not implemented yet.");
    }
}