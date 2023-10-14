using MediatR;
using TournamentBracket.BackEnd.V1.API.Controllers.Definitions;

namespace TournamentBracket.BackEnd.V1.API.Controllers;

public class TournamentController : BaseController
{
    private readonly IMediator mediator;

    public TournamentController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    //[HttpPut]
    //[Route("/seed")]
    //public async Task<ActionResult> SeedTeam([FromBody] CreateSeedRequest createSeedRequest)
    //{
    //    var createSeedResponse = await mediator.Send(new CreateTournamentsCommand
    //    {
    //        Seed = createSeedRequest.Seed,
    //        Team = createSeedRequest.Team,

    //    });
    //    return Ok();
    //}
}
