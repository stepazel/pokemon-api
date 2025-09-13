using System.Net;
using YolkStudio.Pokemon.Api.Shared;
using YolkStudio.Pokemon.Core.Trainers;

namespace YolkStudio.Pokemon.Api.Trainers;

public record TrainerResponse(HttpStatusCode StatusCode, string Message)
    : ApiResponse<Trainer>(StatusCode, Message);