using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Teams
{

    public class AdvanceTeamCommand : IRequest<AdvanceTeamCommandResult>
    {
        public string Team { get; set; }
    }

    public class AdvanceTeamCommandResult
    { }

    public class AdvanceTeamCommandHandler : BackEndGenericHandler, IRequestHandler<AdvanceTeamCommand, AdvanceTeamCommandResult>
    {
        public AdvanceTeamCommandHandler(ITournamentBracketDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<AdvanceTeamCommandResult> Handle(AdvanceTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await teamRepository.GetTeamByName(request.Team);

            if (team == null)
                throw new ArgumentException(ExceptionMessages.TeamNotFoundException);

            var teamId = team.TeamID;

            var status = await matchRepository.GetCurrentMatchByTeam(teamId);
            if (status == null)
                throw new ArgumentException(ExceptionMessages.MatchNotFoundException);

            status.IsMatchCompleted = true;

            //Other logic goes here

            await unitOfWork.SaveChangesAsync();

            return new AdvanceTeamCommandResult();
        }
    }

}
