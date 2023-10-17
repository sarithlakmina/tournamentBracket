using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;

public class GetTournamentWinnerQuery : IRequest<GetTournamentWinnerQueryResult>
{
    public Guid TournamentID { get; set; }
}

public class GetTournamentWinnerQueryResult
{
    public string WinnerName { get; set; }
}

public class GetTournamentWinnerQueryHandler : BackEndGenericHandler, IRequestHandler<GetTournamentWinnerQuery, GetTournamentWinnerQueryResult>
{
    public GetTournamentWinnerQueryHandler(ITournamentBracketDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<GetTournamentWinnerQueryResult> Handle(GetTournamentWinnerQuery request, CancellationToken cancellationToken)
    {
        var tournament = await dbContext.GetTournament(request.TournamentID) ?? throw new Exception(ExceptionMessages.TournamentNotFoundException);

        if (tournament.Winner == null)
            throw new Exception(ExceptionMessages.TournamentHasNoWinnerFoundException);

        var winner = await tournamentRepository.GetTournamentWinner(request.TournamentID);

        return new GetTournamentWinnerQueryResult
        {
            WinnerName = winner.FirstOrDefault()
        };
    }
}
