using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using YolkStudio.Pokemon.Api.Shared;
using YolkStudio.Pokemon.Core;
using YolkStudio.Pokemon.Core.Trainers;

namespace YolkStudio.Pokemon.Api.Trainers;

[ApiController]
[Route(BaseUrl + "trainers")]
public class TrainersController : BaseController
{
    private readonly ITrainerService _trainerService;
    private readonly IValidator<CreateTrainerRequest> _createTrainerValidator;
    private readonly IValidator<UpdateTrainerRequest> _updateTrainerValidator;

    public TrainersController(
        ITrainerService trainerService,
        IValidator<CreateTrainerRequest> createTrainerValidator,
        IValidator<UpdateTrainerRequest> updateTrainerValidator)
    {
        _trainerService = trainerService;
        _createTrainerValidator = createTrainerValidator;
        _updateTrainerValidator = updateTrainerValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrainer([FromBody] CreateTrainerRequest request)
    {
        var validationResult = await _createTrainerValidator.ValidateAsync(request);

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
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetTrainer), new { id = result.Value!.Id }, result.Value);
        }

        return Conflict(new ErrorResponse(
            HttpStatusCode.Conflict,
            result.Message!,
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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TrainerWithPokemonsDto>>> GetTrainer(int id)
    {
        var result = await _trainerService.GetTrainerWithPokemonsAsync(new GetTrainerWithPokemonsQuery(id));

        if (result.IsSuccess is false)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ErrorResponse(HttpStatusCode.NotFound, result.Message!)),
                _ => BadRequest()
            };
        }

        return Ok(new ApiResponse<TrainerWithPokemonsDto>(
            HttpStatusCode.OK,
            "Trainer retrieved successfully",
            result.Value));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse<TrainerDto>>> UpdateTrainer(int id, [FromBody] UpdateTrainerRequest request)
    {
        var validationResult = await _updateTrainerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(new ErrorResponse(
                HttpStatusCode.UnprocessableEntity,
                "Some of the fields have incompatible values",
                validationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")));
        }
        
        var command = new UpdateTrainerCommand(id, request.Name, request.Region, request.Wins, request.Losses);
        var result = await _trainerService.UpdateTrainerAsync(command);
        if (result.IsError)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ErrorResponse(HttpStatusCode.NotFound, result.Message!)),
                _ => BadRequest()
            };
        }

        return Ok(new ApiResponse<TrainerDto>(HttpStatusCode.OK, "Trainer updated successfully", result.Value));
    }

    // TODO
    // Delete a trainer
}