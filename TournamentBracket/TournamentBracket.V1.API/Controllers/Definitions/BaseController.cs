using Microsoft.AspNetCore.Mvc;

namespace TournamentBracket.BackEnd.V1.API.Controllers.Definitions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    public BaseController()
    {

    }
}
