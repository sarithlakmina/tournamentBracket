using Microsoft.AspNetCore.Mvc;

namespace TournamentBracket.BackEnd.V1.API.Controllers.Definitions;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
    public BaseController()
    {

    }
}
