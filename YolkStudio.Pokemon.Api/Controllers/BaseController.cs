using Microsoft.AspNetCore.Mvc;

namespace YolkStudio.Pokemon.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected const string BaseUrl = "api/";

    protected BaseController() {}
}