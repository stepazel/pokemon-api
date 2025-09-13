using Microsoft.AspNetCore.Mvc;

namespace YolkStudio.Pokemon.Api.Shared;

public abstract class BaseController : ControllerBase
{
    protected const string BaseUrl = "api/";

    protected BaseController() {}
}