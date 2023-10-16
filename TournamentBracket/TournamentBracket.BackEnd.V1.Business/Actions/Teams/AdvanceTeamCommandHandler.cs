using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Business.Actions.Matches;
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
    {
        public string WinnerName { get; set; }
    }

    public class AdvanceTeamCommandHandler : BackEndGenericHandler, IRequestHandler<AdvanceTeamCommand, AdvanceTeamCommandResult>
    {
        private readonly IMediator mediator;

        public AdvanceTeamCommandHandler(ITournamentBracketDbContext dbContext, IMediator mediator) : base(dbContext)
        {
            this.mediator = mediator;
        }

        public async Task<AdvanceTeamCommandResult> Handle(AdvanceTeamCommand request, CancellationToken cancellationToken)
        {
            #region Initiate win for R16 matches

            var roundOf16WinningTeamsTeamIDs = new List<Guid>();

            var teams = await teamRepository.GetAllTeams() ?? throw new Exception(ExceptionMessages.TeamNotFoundException);

            var teamIDSeedMap = teams.ToDictionary(team => team.TeamID, team => team.Seed);

            var roundOf16WinningTeamsSeedList = new List<string>();


            var roundOf16Winners = new HashSet<string>(request.AdvanceTeamRequest.Events);
            roundOf16WinningTeamsTeamIDs.AddRange(teams.Where(team => roundOf16Winners.Contains(team.Name)).Select(team => team.TeamID));

            foreach (var team in roundOf16WinningTeamsTeamIDs)
            {
                roundOf16WinningTeamsSeedList.Add(teamIDSeedMap[team]);
            }

            var quaterFinalmatches = await matchRepository.GetAllMatches(roundOf16WinningTeamsTeamIDs);


            var tournamentID = quaterFinalmatches.Select(m => m.TournamentID).FirstOrDefault();

            if (quaterFinalmatches == null)
                throw new Exception(ExceptionMessages.MatchNotFoundException);

            foreach (var match in quaterFinalmatches)
            {
                match.IsMatchCompleted = true;
                match.WinningTeamID = roundOf16WinningTeamsTeamIDs.Contains(match.AwayTeamID) ? match.AwayTeamID : match.HomeTeamID;
            }


            #endregion

            #region Create Quater Final fixtures and matches          

            await mediator.Send(new CreateQuaterFinalMatchesCommand
            {
                TournamentID = tournamentID,
                Seeds = roundOf16WinningTeamsSeedList,
                TeamIDSeedMap = teamIDSeedMap,
            });

            #endregion            

            #region Initiate win for quater finals

            var quaterFinalsWinningTeamsTeamIDs = new List<Guid>();
            var quaterFinalsWinningTeamsSeedList = new List<string>();

            var quaterFinalWinners = request.AdvanceTeamRequest.Events
               .GroupBy(e => e) // Group the elements by their value
               .Where(el => el.Count() > 1) // Filter groups with count more than 1
               .Select(el => el.Key);


            quaterFinalsWinningTeamsTeamIDs.AddRange(teams.Where(team => quaterFinalWinners.Contains(team.Name)).Select(team => team.TeamID));

            foreach (var team in quaterFinalsWinningTeamsTeamIDs)
            {
                quaterFinalsWinningTeamsSeedList.Add(teamIDSeedMap[team]);
            }

            var matches = await matchRepository.GetAllMatches(quaterFinalsWinningTeamsTeamIDs) ?? throw new Exception(ExceptionMessages.MatchNotFoundException);

            foreach (var match in matches)
            {
                match.IsMatchCompleted = true;
                match.WinningTeamID = quaterFinalsWinningTeamsTeamIDs.Contains(match.AwayTeamID) ? match.AwayTeamID : match.HomeTeamID;
            }
            #endregion

            #region Create Semi Final Fixtures and matches

            await mediator.Send(new CreateSemiFinalMatchesCommand
            {
                TournamentID = tournamentID,
                Seeds = quaterFinalsWinningTeamsSeedList,
                TeamIDSeedMap = teamIDSeedMap,
            });
            #endregion

            #region Initiate win for Semi Finals

            var semiFinalsWinningTeamsTeamIDs = new List<Guid>();
            var semiFinalsWinningTeamsSeedList = new List<string>();

            var semiFinalWinners = request.AdvanceTeamRequest.Events
               .GroupBy(e => e) // Group the elements by their value
               .Where(el => el.Count() > 2) // Filter groups with count more than 1
               .Select(el => el.Key);


            semiFinalsWinningTeamsTeamIDs.AddRange(teams.Where(team => semiFinalWinners.Contains(team.Name)).Select(team => team.TeamID));

            foreach (var team in semiFinalsWinningTeamsTeamIDs)
            {
                semiFinalsWinningTeamsSeedList.Add(teamIDSeedMap[team]);
            }

            var semiFinalMatches = await matchRepository.GetAllMatches(semiFinalsWinningTeamsTeamIDs) ?? throw new Exception(ExceptionMessages.MatchNotFoundException);

            foreach (var match in semiFinalMatches)
            {
                match.IsMatchCompleted = true;
                match.WinningTeamID = semiFinalsWinningTeamsTeamIDs.Contains(match.AwayTeamID) ? match.AwayTeamID : match.HomeTeamID;
            }

            #endregion

            #region Create Final fixture and Match

            await mediator.Send(new CreateFinalMatchFixtureCommand
            {
                TournamentID = tournamentID,
            });

            #endregion

            #region Initiate win for Finals                     

            var finalWinner = request.AdvanceTeamRequest.Winner;

            var tournamentWinnerID = teams.Where(team => team.Name == finalWinner).Select(team => team.TeamID).FirstOrDefault();

            foreach (var team in semiFinalsWinningTeamsTeamIDs)
            {
                semiFinalsWinningTeamsSeedList.Add(teamIDSeedMap[team]);
            }

            var finalMatch = await matchRepository.GetCurrentMatchByTeam(tournamentWinnerID) ?? throw new Exception(ExceptionMessages.MatchNotFoundException);

            finalMatch.IsMatchCompleted = true;
            finalMatch.WinningTeamID = tournamentWinnerID;

            var secondPlace = finalMatch.WinningTeamID == finalMatch.HomeTeamID ? finalMatch.AwayTeamID : finalMatch.HomeTeamID;

            var tournament = await tournamentRepository.GetTournament(tournamentID);

            tournament.Winner = tournamentWinnerID;
            tournament.SecondPlace = secondPlace;

            #endregion

            await unitOfWork.SaveChangesAsync();

            return new AdvanceTeamCommandResult
            {
                WinnerName = finalWinner,
            };
        }


    }

}
