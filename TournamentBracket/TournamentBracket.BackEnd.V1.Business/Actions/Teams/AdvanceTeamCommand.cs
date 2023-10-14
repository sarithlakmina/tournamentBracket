using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Model;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Teams
{

    public class AdvanceTeamCommand : IRequest<AdvanceTeamCommandResult>
    {
        public AdvanceTeamRequestModel AdvanceTeamRequest { get; set; }
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
            //var team = await teamRepository.GetTeamByName(request.Team);

            //if (team == null)
            //    throw new ArgumentException(ExceptionMessages.TeamNotFoundException);

            //var teamId = team.TeamID;

            //var status = await matchRepository.GetCurrentMatchByTeam(teamId);
            //if (status == null)
            //    throw new ArgumentException(ExceptionMessages.MatchNotFoundException);

            //status.IsMatchCompleted = true;

            ////Other logic goes here

            //await unitOfWork.SaveChangesAsync();


            var teamNameSeedMap = new Dictionary<string, Guid>();

            var winningTeamsTeamIDs = new List<Guid>();

            var teams = await teamRepository.GetAllTeams();

            if (teams == null)
                throw new Exception(ExceptionMessages.TeamNotFoundException);

            var roundOf16Winners = new HashSet<string>(request.AdvanceTeamRequest.Events);

            foreach (var team in teams)
            {
                if (roundOf16Winners.Contains(team.Name))
                {
                    winningTeamsTeamIDs.Add(team.TeamID);
                }
            }

            var matches = await matchRepository.GetAllMatches(winningTeamsTeamIDs);

            if (matches == null)
                throw new Exception(ExceptionMessages.MatchNotFoundException);

            foreach (var match in matches)
            {
                match.IsMatchCompleted = true;
                match.WinningTeamID = winningTeamsTeamIDs.Contains(match.AwayTeamID) ? match.AwayTeamID : match.HomeTeamID;
            }

            await matchRepository.SaveChangesAsync();








            return new AdvanceTeamCommandResult();
        }
    }

}
