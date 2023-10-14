using MediatR;
using Microsoft.AspNetCore.Mvc;
using TournamentBracket.BackEnd.V1.API.Controllers.Definitions;
using TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;
using TournamentBracket.BackEnd.V1.Common.Common;

namespace TournamentBracket.BackEnd.V1.API.Controllers;

[ApiController]
public class TeamController : BaseController
{
    private readonly IMediator mediator;

    public TeamController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPut]
    [Route("/seed")]
    public async Task<ActionResult> SeedTeam([FromBody] Dictionary<string, List<SeedDetails>> createSeedRequest)
    {
        await mediator.Send(new CreateTeamsCommand
        {
            SeedDetails = createSeedRequest,

        });
        return Ok(createSeedRequest);
    }

    [HttpPut]
    [Route("/advance")]
    public async Task<ActionResult> AdvanceTeam([FromBody] string team)  //prefer ID here 
    {
        return Ok(team);
    }
}
