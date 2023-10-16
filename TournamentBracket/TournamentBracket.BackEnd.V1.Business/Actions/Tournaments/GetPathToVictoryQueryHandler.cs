using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.DTO;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Tournaments
{

    public class GetPathToVictoryQuery : IRequest<GetPathToVictoryQueryResult>
    {
        public Guid TournamentID { get; set; }
    }

    public class GetPathToVictoryQueryResult
    {
        public List<PathToVictoryDto> PathToVictoryDto { get; set; }
    }

    public class GetPathToVictoryQueryHandler : BackEndGenericHandler, IRequestHandler<GetPathToVictoryQuery, GetPathToVictoryQueryResult>
    {
        private readonly IMediator mediator;

        public GetPathToVictoryQueryHandler(ITournamentBracketDbContext dbContext, IMediator mediator) : base(dbContext)
        {
            this.mediator = mediator;
        }

        public async Task<GetPathToVictoryQueryResult> Handle(GetPathToVictoryQuery request, CancellationToken cancellationToken)
        {
            var winningTeamIDs = await tournamentRepository.GetTournamentWinnerIDs(request.TournamentID);

            if (winningTeamIDs == null)
                throw new Exception(ExceptionMessages.TournamentHasNoWinnerFoundException);

            var result = await tournamentRepository.GetPathToVictory(request.TournamentID, winningTeamIDs.Value);

            return new GetPathToVictoryQueryResult
            {
                PathToVictoryDto = result
            };
        }
    }

}
