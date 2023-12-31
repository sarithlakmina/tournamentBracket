﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using TournamentBracket.BackEnd.V1.API.Controllers.Definitions;
using TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;

namespace TournamentBracket.BackEnd.V1.API.Controllers;

public class TournamentsController : BaseController
{
    private readonly IMediator mediator;

    public TournamentsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [Route("winner")]
    public async Task<ActionResult> Winner([FromQuery] Guid TournamentID)
    {
        var tournamentWinner = await mediator.Send(new GetTournamentWinnerQuery
        {
            TournamentID = TournamentID

        });
        return Ok(tournamentWinner.WinnerName);
    }

    [HttpGet]
    [Route("pathtovictory")]
    public async Task<ActionResult> PathToVictory([FromQuery] Guid TournamentID)
    {
        var result = await mediator.Send(new GetPathToVictoryQuery
        {
            TournamentID = TournamentID
        });
        return Ok(result.PathToVictoryDto);
    }
}
