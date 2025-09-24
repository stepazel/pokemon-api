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
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TrainerDto>> CreateAsync([FromBody] CreateTrainerRequest request)
    {
        var validationResult = await _createTrainerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(new ValidationErrorResponse("Some of the fields have incompatible values",
                validationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")));
        }

        var addTrainerCommand = new AddTrainerCommand(request.Name, request.Region, request.BirthDate.UtcDateTime);
        var result = await _trainerService.CreateAsync(addTrainerCommand);
        if (result.IsError)
        {
            return Conflict(new ErrorResponse(result.Message!));
        }

        return CreatedAtAction("GetById", new { id = result.Value!.Id }, result.Value);
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<TrainerDto>>>> GetAsync()
    {
        var result = await _trainerService.GetTrainersAsync(new GetAllTrainersQuery());
        return Ok(new ApiResponse<IEnumerable<TrainerDto>>("Trainers retrieved successfully", result));
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TrainerWithPokemonsDto>>> GetByIdAsync(int id)
    {
        var result = await _trainerService.GetTrainerWithPokemonsAsync(new GetTrainerWithPokemonsQuery(id));
        if (result.IsSuccess is false)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ValidationErrorResponse(result.Message!)),
                _ => BadRequest(),
            };
        }

        return Ok(new ApiResponse<TrainerWithPokemonsDto>("Trainer retrieved successfully",
            result.Value));
    }

    [HttpPatch("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TrainerDto>>> UpdateAsync(int id, [FromBody] UpdateTrainerRequest request)
    {
        var validationResult = await _updateTrainerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(new ValidationErrorResponse("Some of the fields have incompatible values",
                validationResult.Errors.Select(e => $"{e.PropertyName} - {e.ErrorMessage}")));
        }
        
        var command = new UpdateTrainerCommand(id, request.Name, request.Region, request.Wins, request.Losses);
        var result = await _trainerService.UpdateTrainerAsync(command);
        if (result.IsError)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ValidationErrorResponse(result.Message!)),
                _ => BadRequest(),
            };
        }

        return Ok(new ApiResponse<TrainerDto>("Trainer updated successfully", result.Value));
    }

    [HttpDelete("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command  = new DeleteTrainerCommand(id);
        var result = await _trainerService.DeleteTrainerAsync(command);
        if (result.IsError)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ValidationErrorResponse(result.Message!)),
                ErrorType.Conflict => Conflict(new ValidationErrorResponse(result.Message!)),
                _ => BadRequest(),
            };
        }

        return Ok(new ApiResponse<object>("Trainer deleted successfully"));
    }
}