using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using YolkStudio.Pokemon.Api.Shared;
using YolkStudio.Pokemon.Core.Trainers;

namespace YolkStudio.Pokemon.Api.Trainers;

[ApiController]
[Route(BaseUrl + "trainers")]
public class TrainersController : BaseController
{
    private readonly ITrainerService _trainerService;
    private readonly IValidator<CreateTrainerRequest> _validator;

    public TrainersController(ITrainerService trainerService, IValidator<CreateTrainerRequest> validator)
    {
        _trainerService = trainerService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrainer([FromBody] CreateTrainerRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(new ErrorResponse(
                HttpStatusCode.UnprocessableEntity,
                "Some of the fields have incompatible values",
                validationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")));
        }

        var addTrainerCommand = new AddTrainerCommand(
            request.Name,
            request.Region,
            request.BirthDate.UtcDateTime);

        var result = await _trainerService.CreateTrainerAsync(addTrainerCommand);
        if (result.Success)
        {
            return CreatedAtAction(nameof(GetTrainer), new { id = result.Value!.Id }, result.Value);
        }

        return Conflict(new ErrorResponse(
            HttpStatusCode.Conflict,
            result.Error!,
            []));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTrainers()
    {
        var result = await _trainerService.GetAllTrainersAsync(new GetAllTrainersQuery());

        return Ok(new ApiResponse<IEnumerable<TrainerDto>>(
             HttpStatusCode.OK, "Trainers retrieved successfully", result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTrainer(int id)
    {
        return Ok();
    }
    
    // TODO
    // Retrieve a specific trainer along with their Pokemons
    // Update trainer information
    // Delete a trainer
}